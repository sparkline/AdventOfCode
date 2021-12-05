namespace AdventOfCode2021.Common
{
    public class SparseBoard<T>
    {
        public Dictionary<int, Dictionary<int, T?>> board { get; private set; } = new Dictionary<int, Dictionary<int, T?>>();

        public T? this[int x, int y]
        {
            get
            {
                if (board.TryGetValue(y, out Dictionary<int, T?>? row))
                {
                    if (row != null && row.TryGetValue(x, out T? result))
                    {
                        return result;
                    }
                }

                return default;
            }
            set
            {
                if (!board.ContainsKey(y))
                {
                    board.Add(y, new Dictionary<int, T?>());
                }
                var row = board[y];
                if (!row.TryAdd(x, value))
                {
                    row[x] = value;
                }
            }
        }
    }
}
