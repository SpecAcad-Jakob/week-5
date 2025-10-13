using System.Data;

namespace Week_4_PDF_downloader {
    internal class DownloadManager {
        //  Constants
        private static String DEFAULT_DOWNLOAD_LOCATION = "../../../../Downloads";

        //  Fields
        private String downloadLocation;
        private static HttpClient httpClient = new HttpClient();
        private DataTable statusTable = new DataTable();

        //  Properties
        public List<String> AllowedTypes = new List<String> { "application/pdf" };  //  To improve, should be a Map (or Dictionary, whatever C# calls it)

        /// <summary>
        /// Create a new object instance targeting default folder, to which downloaded files will be saved.
        /// </summary>
        public DownloadManager() : this(DEFAULT_DOWNLOAD_LOCATION) {
        }

        /// <summary>
        /// Create a new object instance targeting a set folder, to which downloaded files will be saved.
        /// </summary>
        /// <param name="downloadLocation">Folder location and name. Can be relative or absolute.</param>
        public DownloadManager(String downloadLocation) {
            this.downloadLocation = downloadLocation.Trim();

            //  Prepare table for status output
            statusTable = new DataTable();
            DataColumn dataColumn;

            dataColumn = new DataColumn();
            dataColumn.DataType = typeof(String);
            dataColumn.ColumnName = "ID";
            statusTable.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.DataType = typeof(Boolean);
            dataColumn.ColumnName = "Status";
            statusTable.Columns.Add(dataColumn);
        }

        /// <summary>
        /// Get result of downloads made with this object instance.
        /// </summary>
        /// <returns>DataTable object of status. Each row represents a download which was either successful or unsuccessful.</returns>
        public DataTable getStatusList() {
            return statusTable;
        }

        /// <summary>
        /// Downloads, or tries to download, a file from URL.
        /// </summary>
        /// <param name="tableRow">DataRow object containing URL, fallback-URL and name for file to be saved as.</param>
        /// <param name="fileNameIndex">Index of column from DataRow in which the file name is located.</param>
        /// <param name="urlIndex">Index of column from DataRow in which the URL is located.</param>
        /// <param name="fallbackUrlIndex">Index of column from DataRow in which the fallback-URL is located.</param>
        /// <returns>HttpResponseMessage Task with information of latest attempted download; from first URL source 
        /// if this was successful; from second URL source if first was unsuccessful, regardless of whether 
        /// the download from the second source was successful or not. 
        /// Not required to be handled.</returns>
        public async Task<HttpResponseMessage> tryDownloadAsync(DataRow tableRow, int fileNameIndex, int urlIndex, int fallbackUrlIndex) {
            HttpResponseMessage httpResponseMessage = null;
            Boolean isSuccess = false;
            int contentTypeIndexInAllowedTypes = -1;

            //  Regardless of error type, assume failure and continue.
            //  Last catch of any "Exception" done because error occurred that I didn't have time to
            //  look into. Somehow httpResponseMessage can remain null after GetAsync()?
            try {   //  Try download file
                httpResponseMessage = await httpClient.GetAsync(tableRow[urlIndex].ToString());
                contentTypeIndexInAllowedTypes = allowedTypes.IndexOf(httpResponseMessage.Content.Headers.ContentType.ToString());
                isSuccess = true;
            } catch (InvalidOperationException exception) {
                //  Do nothing
            } catch (HttpRequestException exception) {
                //  Do nothing
            } catch (TaskCanceledException exception) {
                //  Do nothing
            } catch (UriFormatException exception) {
                //  Do nothing
            } catch (NotSupportedException exception) {
                //  Do nothing
            } catch (Exception exception) {
                //  Do nothing.
            }
            if (!isSuccess) {
                try {   //  Try download file via fallback URL
                    httpResponseMessage = await httpClient.GetAsync(tableRow[fallbackUrlIndex].ToString());
                    contentTypeIndexInAllowedTypes = allowedTypes.IndexOf(httpResponseMessage.Content.Headers.ContentType.ToString());
                    isSuccess = true;
                } catch (InvalidOperationException exception) {
                    //  Do nothing
                } catch (HttpRequestException exception) {
                    //  Do nothing
                } catch (TaskCanceledException exception) {
                    //  Do nothing
                } catch (UriFormatException exception) {
                    //  Do nothing
                } catch (NotSupportedException exception) {
                    //  Do nothing
                } catch (Exception exception) {
                    //  Do nothing.
                }
            }

            //  Save to folder
            if (isSuccess && contentTypeIndexInAllowedTypes != -1) {
                //  Save to folder
                Stream contentStream;
                FileStream fileStream;
                try {
                    contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
                    fileStream = new FileStream(downloadLocation + '/' + tableRow[fileNameIndex] + ".pdf", FileMode.Create, FileAccess.Write, FileShare.None);   //  Might download other types. Make data-driven.
                    await contentStream.CopyToAsync(fileStream);
                } catch (Exception exception) {
                    isSuccess = false;
                }
            } else {
                isSuccess = false;
            }

            //  Write to status
            DataRow statusRow = statusTable.NewRow();
            statusRow[0] = tableRow[fileNameIndex];
            statusRow[1] = isSuccess;
            statusTable.Rows.Add(statusRow);

            return httpResponseMessage;
        }

    }
}
