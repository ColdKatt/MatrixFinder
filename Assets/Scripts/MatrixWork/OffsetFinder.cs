using Cysharp.Threading.Tasks;
using MatrixFinder.HelpTools;
using MatrixFinder.Matrices;
using MatrixFinder.Matrices.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using VContainer.Unity;

namespace MatrixFinder
{
    /// <summary>
    /// Class for finding all offsets complying with the condition: offset * Model == Space
    /// </summary>
    public class OffsetFinder : IAsyncStartable
    {
        private readonly MatrixComparer _matrixComparer;

        private readonly OffsetCreator _redOffsetCreator; // don't comply with the condition
        private readonly OffsetCreator _greenOffsetCreator; // comply with the condition

        private IReadOnlyList<Matrix4x4> _modelMatrices;
        private IReadOnlyList<Matrix4x4> _invertedModelMatrices;

        private IReadOnlyList<Matrix4x4> _spaceMatrices;

        private List<Matrix4x4> _totalOffsets;

        private SearchMode _searchMode;
        private bool _enableParallelCreation;

        public bool EnableParallelCreation
        {
            get => _enableParallelCreation; 
            set => _enableParallelCreation = value; 
        }

        public OffsetFinder(SearchMode searchMode, bool enableParallelCreation)
        {
            _modelMatrices = JsonHelper.ExtractMatricesFromJson("model").ToList();
            _spaceMatrices = JsonHelper.ExtractMatricesFromJson("space").ToList();
            _invertedModelMatrices = _modelMatrices.Select(m => m.inverse).ToList();

            _matrixComparer ??= new();
            _totalOffsets ??= new();

            _redOffsetCreator ??= new(primitiveColor: Color.red);
            _greenOffsetCreator ??= new(primitiveColor: Color.green);

            _searchMode = searchMode;
            _enableParallelCreation = enableParallelCreation;
        }

        public async Awaitable StartAsync(CancellationToken cancellation = default)
        {
            if (_modelMatrices.Count == 0 || _spaceMatrices.Count == 0) return;

            switch (_searchMode)
            {
                case SearchMode.Simple:
                    {
                        var offsetDictionary = await FindOffsetsAsync(_invertedModelMatrices[0], _spaceMatrices, cancellation);

                        _totalOffsets = offsetDictionary[true];

                        VisualizeOffsets(offsetDictionary[false], _redOffsetCreator);
                        VisualizeOffsets(_totalOffsets, _greenOffsetCreator);

                        break;
                    }
                case SearchMode.SlowFull:
                    {
                        var offsetDictionaryArray = await _invertedModelMatrices.Select(m => FindOffsetsAsync(m, _spaceMatrices, cancellation));

                        ConcatOffsets(offsetDictionaryArray);

                        if (!EnableParallelCreation)
                        {
                            VisualizeOffsets(offsetDictionaryArray.LastOrDefault()[false], _redOffsetCreator);
                            VisualizeOffsets(_totalOffsets, _greenOffsetCreator);
                        }

                        break;
                    }
                case SearchMode.FastFull:
                    {
                        var offsetDictionaryArray = await _invertedModelMatrices.Select(model => UniTask.RunOnThreadPool(() => FindOffsets(model, _spaceMatrices), cancellationToken: cancellation));

                        ConcatOffsets(offsetDictionaryArray);

                        VisualizeOffsets(offsetDictionaryArray.LastOrDefault()[false], _redOffsetCreator);
                        VisualizeOffsets(_totalOffsets, _greenOffsetCreator);


                        break;
                    }
            }

            JsonHelper.ToJson(_totalOffsets.Select(o => o.ToMatrixData()).ToArray());
        }

        public async UniTask<Dictionary<bool, List<Matrix4x4>>> FindOffsetsAsync(Matrix4x4 invertedModelMatrix, IEnumerable<Matrix4x4> spaceMatrices, CancellationToken token)
        {
            // true key - the condition is complying | false - is not
            var offsetDict = new Dictionary<bool, List<Matrix4x4>>() { { true, new() }, { false, new() } };

            foreach (var spaceMatrix in spaceMatrices)
            {
                var offset = spaceMatrix * invertedModelMatrix;

                var isConditionComply = _modelMatrices.All(m => _spaceMatrices.Contains(offset * m, _matrixComparer));
                offsetDict[isConditionComply].Add(offset);

                // offset creation
                if (_searchMode != SearchMode.FastFull && EnableParallelCreation)
                {
                    var offsetCreator = isConditionComply ? _greenOffsetCreator : _redOffsetCreator;
                    VisualizeOffsets(offset, offsetCreator);
                }
                //

                await UniTask.Yield(token);
            }

            return offsetDict;
        }

        public Dictionary<bool, List<Matrix4x4>> FindOffsets(Matrix4x4 invertedModelMatrix, IEnumerable<Matrix4x4> spaceMatrices)
        {
            // true key - the condition is complying | false - is not
            var offsetDict = new Dictionary<bool, List<Matrix4x4>>() { { true, new() }, { false, new() } };

            foreach (var spaceMatrix in spaceMatrices)
            {
                var offset = spaceMatrix * invertedModelMatrix;

                var isConditionComply = _modelMatrices.All(m => _spaceMatrices.Contains(offset * m, _matrixComparer));
                offsetDict[isConditionComply].Add(offset);
            }

            return offsetDict;
        }

        private void VisualizeOffsets(IEnumerable<Matrix4x4> offsets, OffsetCreator creator)
        {
            foreach (var offset in offsets)
            {
                creator.Create(offset);
            }
        }

        private void VisualizeOffsets(Matrix4x4 offset, OffsetCreator creator)
        {
            creator.Create(offset);
        }

        private void ConcatOffsets(Dictionary<bool, List<Matrix4x4>>[] offsetDictionaryArray)
        {
            foreach (var dictionary in offsetDictionaryArray)
            {
                var newOffsets = dictionary[true].Where(o => !_totalOffsets.Contains(o, _matrixComparer)).ToList();
                _totalOffsets = _totalOffsets.Concat(newOffsets).ToList();
            }
        }
    }
}