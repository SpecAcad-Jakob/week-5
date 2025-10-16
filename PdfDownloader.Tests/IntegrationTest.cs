using System.Data;
using Week_4_PDF_downloader;

namespace PdfDownloader.Tests {
    public class IntegrationTest {
        public List<String> filePathsToDeleteInTeardown = new List<String>();

        [SetUp]
        public void Setup() {
        }

        [TearDown]
        public void Teardown() {
            foreach (String path in filePathsToDeleteInTeardown) {
                File.Delete(path);
            }
            filePathsToDeleteInTeardown.Clear();
        }

        [Test]
        public async Task testDownloadAndSaveFromCsvData() {
            FileHandler fileHandler = new FileHandler("../../../../Test CSV files");
            fileHandler.readTableFromCsvFileWithHeaders(0, ';');
            DataTable dataFromFile = fileHandler.getTable();
            DownloadManager downloadManager = new DownloadManager("../../../../Downloads");
            Assert.That(dataFromFile.Rows.Count != 0);
            foreach (DataRow row in dataFromFile.Rows) {
                await downloadManager.tryDownloadAsync(row, 0, 1, 1);
                bool doesFileExist = File.Exists($"../../../../Downloads/{row[0]}.pdf");
                Assert.That(doesFileExist);
                if (doesFileExist) {
                    filePathsToDeleteInTeardown.Add(Path.GetFullPath($"../../../../Downloads/{row[0]}.pdf"));
                }
            }
        }
    }
}