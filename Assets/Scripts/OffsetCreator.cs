using MatrixFinder.HelpTools;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixFinder
{
    public class OffsetCreator
    {
        private const int OFFSET_QUEUE_LIMIT = 1000;

        private readonly PrimitiveType _primitiveType;
        private readonly Color _primitiveColor;

        private Queue<GameObject> _offsetObjects;

        public OffsetCreator(PrimitiveType primitiveType = PrimitiveType.Cube, Color primitiveColor = default)
        {
            _offsetObjects ??= new();

            _primitiveType = primitiveType;
            _primitiveColor = primitiveColor;
        }

        public void Create(Matrix4x4 offset)
        {
            if (_offsetObjects.Count > OFFSET_QUEUE_LIMIT)
            {
                var primitive = _offsetObjects.Dequeue();
                Object.Destroy(primitive);
            }

            _offsetObjects.Enqueue(PrimitiveHelper.CreatePrimitiveFromMatrix(offset, _primitiveType, _primitiveColor));
        }
    }
}