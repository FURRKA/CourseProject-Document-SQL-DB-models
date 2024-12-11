namespace BLL.Services
{
    public static class TableService
    {
        public static void Show<T>(List<T> data, string[] columnNames, params Func<T, object>[] selectors)
        {
            if (columnNames == null || columnNames.Length == 0)
                throw new ArgumentException("Column names cannot be null or empty.");
            if (selectors == null || selectors.Length == 0)
                throw new ArgumentException("Selectors cannot be null or empty.");
            if (columnNames.Length != selectors.Length)
                throw new ArgumentException("Number of column names must match the number of selectors.");

            if (data == null)
                throw new ArgumentException("Data cannot be null.");

            var rows = data.Select(obj => selectors.Select(s => s(obj)?.ToString() ?? "").ToList()).ToList();

            var columnWidths = new List<int>();
            for (int i = 0; i < columnNames.Length; i++)
            {
                int maxWidth = columnNames[i].Length;
                foreach (var row in rows)
                {
                    if (row[i].Length > maxWidth)
                        maxWidth = row[i].Length;
                }
                columnWidths.Add(maxWidth);
            }

            DrawHorizontalLine(columnWidths);
            DrawRow(columnNames.ToList(), columnWidths);
            DrawHorizontalLine(columnWidths);

            foreach (var row in rows)
            {
                DrawRow(row, columnWidths);
            }

            DrawHorizontalLine(columnWidths);
            Console.WriteLine();
        }

        private static void DrawHorizontalLine(List<int> columnWidths)
        {
            Console.Write("+");
            foreach (var width in columnWidths)
            {
                Console.Write(new string('-', width + 2));
                Console.Write("+");
            }
            Console.WriteLine();
        }

        private static void DrawRow(List<string> cells, List<int> columnWidths)
        {
            Console.Write("|");
            for (int i = 0; i < cells.Count; i++)
            {
                Console.Write($" {cells[i].PadRight(columnWidths[i])} |");
            }
            Console.WriteLine();
        }
    }
}
