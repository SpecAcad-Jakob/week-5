# README 
## PDF Downloader  
  
### Function 
On run, program will find first file in "Excel files" folder, if any, and assuming that it is an Excel-file 
(.XLSX extension), and will read through list, and run a download for each entry in file (unless limited).  
Download is run up to twice, using column "AL" (index 37) as the main URL-source, and column "AM" (index 38) 
as the fallback URL-source.  
Status of each download will appear in console, but a log will also be written to "Logs" folder, containing 
status of each file attempted to download. Log files are made in CSV format (.CSV extension), readable by 
Excel, using semicolon (;) as the separator character. 
  
## Limitations 
While program is generally made to be expandable, the DownloadManager class currently only accepts the 
download of PDF files (.PDF extension). 
