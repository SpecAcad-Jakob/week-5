using System.Data;

namespace Week_4_PDF_downloader
{
    internal class Program
    {
        static async Task Main(string[] args) {
            Console.WriteLine("PDF Download Application");
            Console.WriteLine("Status: Reading Excel file");

            //  Read excel file
            FileHandler excelFileHandler = new FileHandler("../../../../Excel files");
            try {
                excelFileHandler.readTableFromExcelFileWithHeaders(0);
            } catch (IndexOutOfRangeException exception) {
                Console.WriteLine("Status: No Excel file found in folder " + Path.GetFullPath(excelFileHandler.folderLocation));
            }


            Console.WriteLine("Status: Excel file finished reading");
            Console.WriteLine("Status: Attempting download");

            DownloadManager downloadManager = new DownloadManager();

            //  Download files
            int i = 0;
            foreach (DataRow tableRow in excelFileHandler.getTable().Rows) {
                Console.WriteLine("Download status:");
                Console.WriteLine("".PadRight(8) + tableRow[0]);

                HttpResponseMessage httpResponseMessage = await downloadManager.tryDownloadAsync(tableRow, 0, 37, 38);

                Console.WriteLine("".PadRight(8) + (httpResponseMessage != null ? httpResponseMessage.StatusCode : "NULL"));

                if (i >= 8) {   //  Limit to TEN (10) during on-site test
                    break;
                }
                i++;
            }


            Console.WriteLine("Status: Writing log");

            FileHandler logFileHandler = new FileHandler("../../../../Logs");
            logFileHandler.writeToCsvFileWithHeaders(downloadManager.getStatusList(), new List<int> { 0, 1 }, ';');

            Console.WriteLine("Status: Log written");
        }
    }
}
