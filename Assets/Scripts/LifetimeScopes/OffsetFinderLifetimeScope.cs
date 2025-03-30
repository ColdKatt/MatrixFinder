using MatrixFinder;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class OffsetFinderLifetimeScope : LifetimeScope
{
    [Header("Params")]
    [SerializeField] private SearchMode _searchMode;
    [SerializeField] private bool _enableParallelCreation;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<OffsetFinder>(Lifetime.Singleton)
               .AsSelf()
               .WithParameter(_searchMode)
               .WithParameter(_enableParallelCreation);
    }
}
