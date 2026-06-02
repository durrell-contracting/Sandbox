namespace Problem._3093;

public class SuffixComparer
{
    private readonly string[] _wordsContainer;

    public SuffixComparer(string[]? wordsContainer)
    {
        _wordsContainer = wordsContainer ?? throw new ArgumentNullException(nameof(wordsContainer));
    }

    /// <summary>
    /// For each query string, find the index in wordsContainer that:
    /// 1) has the longest common suffix with the query;
    /// 2) if tied on suffix length, has the smallest length;
    /// 3) if still tied, appeared earlier in wordsContainer.
    /// </summary>
    public int[] LongestCommonSuffixIndices(string[]? wordsQuery)
    {
        if (wordsQuery == null) throw new ArgumentNullException(nameof(wordsQuery));

        SuffixComparerQuery processor = new SuffixComparerQuery(_wordsContainer);

        int n = wordsQuery.Length;
        int[] result = new int[n];

        for (int i = 0; i < n; i++)
        {
            result[i] = processor.Process(wordsQuery[i]);
        }

        return result;
    }

    private class SuffixComparerQuery
    {
        private readonly string[] _wordsContainer;
        private int _bestIdx = -1;
        private int _bestSuffixLen = -1;
        private int _bestContainerLen = int.MaxValue;

        public SuffixComparerQuery(string[] wordsContainer)
        {
            _wordsContainer = wordsContainer ?? throw new ArgumentNullException(nameof(wordsContainer));
        }

        public int Process(string query)
        {
            int m = _wordsContainer.Length;

            InitializeInstanceVariables(m);

            for (int j = 0; j < m; j++)
            {
                EvaluateContainerEntry(query, j, _wordsContainer[j] ?? string.Empty);
            }
            return _bestIdx;
        }

        private void EvaluateContainerEntry(string query, int currentIndex, string containerWord)
        {
            int ia = containerWord.Length - 1;
            int ib = query.Length - 1;
            int common = 0;

            while (ia >= 0 && ib >= 0 && containerWord[ia] == query[ib])
            {
                common++;
                ia--;
                ib--;
            }

            UpdateIfBetterCandidate(currentIndex, containerWord, common);
        }

        /// <summary>
        /// For each query string, find the index in wordsContainer that:
        /// 1) has the longest common suffix with the query;
        /// 2) if tied on suffix length, has the smallest length;
        /// 3) if still tied, appeared earlier in wordsContainer.
        /// </summary>
        private void UpdateIfBetterCandidate(int currentIndex, string containerWord, int common)
        {
            // Choose if this container string is better per rules.
            if (common > _bestSuffixLen
                || (common == _bestSuffixLen && containerWord.Length < _bestContainerLen)
                || (common == _bestSuffixLen && containerWord.Length == _bestContainerLen && currentIndex < _bestIdx))
            {
                _bestSuffixLen = common;
                _bestContainerLen = containerWord.Length;
                _bestIdx = currentIndex;
            }
        }

        private void InitializeInstanceVariables(int m)
        {
            _bestIdx = m > 0 ? 0 : -1;
            _bestSuffixLen = -1;
            _bestContainerLen = int.MaxValue;
        }
    }
}
