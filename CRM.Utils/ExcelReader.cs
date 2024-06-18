using OfficeOpenXml;

namespace CRM.Utils
{
    public static class ExcelReader<T>
    {
        public static Dictionary<string, int> GetHeaderIndexes(string filePath)
        {
            var headerIndexes = new Dictionary<string, int>();
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets[0];

                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                {
                    var header = worksheet.Cells[1, col].Text;
                    if (!string.IsNullOrEmpty(header))
                    {
                        headerIndexes[header] = col;
                    }
                }
            }
            return headerIndexes;
        }

        public static List<T> ReadFromExcel(string filePath, Func<ExcelWorksheet, int, T> mapFunction)
        {
            var itemsToSave = new List<T>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets[0];

                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    var item = mapFunction(worksheet, row);
                    itemsToSave.Add(item);
                }
            }

            return itemsToSave;
        }
    }
}
