using System.Data;
using Week_4_PDF_downloader;

namespace PdfDownloader.Tests {

    public class FileHandlerTests {
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
        public void TestReadFailingExcelFile() {
            //  Files in folder are read alphabetically; first test file will fail
            FileHandler fileHandler = new FileHandler("../../../../Test Excel files");
            fileHandler.readTableFromExcelFileNoHeaders(0);
            DataTable dataTable = fileHandler.getTable();
            Assert.That(dataTable.Rows.Count != 0);
            /*Assert.That(String.IsNullOrEmpty(
                (String) dataTable
                .Rows[0]
                [0]), Is.False);*/
        }

        [Test]
        public void TestReadSucceedingExcelFile() {
            //  Files in folder are read alphabetically; second test file should succeed
            FileHandler fileHandler = new FileHandler("../../../../Test Excel files");
            fileHandler.readTableFromExcelFileNoHeaders(1);
            DataTable dataTable = fileHandler.getTable();
            Assert.That(dataTable.Rows.Count != 0);
            Assert.That(String.IsNullOrEmpty(
                (String) dataTable
                .Rows[0]
                [0]), Is.False);
        }

        [Test]
        public void TestReadSucceedingCsvFile() {
            //  Files in folder are read alphabetically; second test file should succeed
            FileHandler fileHandler = new FileHandler("../../../../Test CSV files");
            fileHandler.readTableFromCsvFileWithHeaders(0, ';');
            DataTable dataTable = fileHandler.getTable();
            Assert.That(dataTable.Rows.Count != 0);
            Assert.That(String.IsNullOrEmpty(
                (String)dataTable
                .Rows[0]
                [0]), Is.False);
        }

        [Test]
        public void TestDuplicateColumnNameHandling() {
            FileHandler fileHandler = new FileHandler();
            DataTable dataTable = new DataTable();
            int testTable_size = 10;
            for (int i = 0; i < testTable_size; i++) {
                DataColumn dataColumn = new DataColumn();
                dataColumn.ColumnName = "TestName";
                try {
                    dataTable.Columns.Add(dataColumn);
                } catch (DuplicateNameException dupeException) {
                    fileHandler.tryFixDuplicateName(dataColumn);
                }
            }
            Assert.That(dataTable.Columns.Count == testTable_size);
        }
    }
}