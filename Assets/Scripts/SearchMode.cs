namespace MatrixFinder
{
    /// <summary>
    /// Determines a search mode for offset.
    /// </summary>
    public enum SearchMode
    {
        /// <summary>
        /// It looks like a special case but complete his task successfully for current json files
        /// </summary>
        Simple,

        /// <summary>
        /// Iterates over all matrices, but takes longer. Performance decreases slightly
        /// </summary>
        SlowFull,

        /// <summary>
        /// Iterates over all matrices, most fastest mode, but performance decreases strongly. Offset object creation is instantly only
        /// </summary>
        FastFull
    }
}