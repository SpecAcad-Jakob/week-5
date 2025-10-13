using NUnit.Framework;
using System.Data;
using Week_4_PDF_downloader;

namespace PdfDownloader.Tests {

    internal class IntegrationTests {
        [SetUp]
        public void Setup() {
        }

        [TearDown]
        public void Teardown() {
        }

        [Test]
        public void Test1() {
            FileHandler fileHandler = new FileHandler("../../../../CSV files");
            fileHandler.readTableFromCsvFileWithHeaders(0, ';');
            DataTable dataFromFile = fileHandler.getTable();
        }
    }
}
