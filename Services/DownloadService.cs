using Microsoft.JSInterop;
using static System.Net.WebRequestMethods;

namespace ChatClient.Services
{
    public class DownloadService
    {
        public DownloadService(string _storedFilename, string _originalFileName)
        {
            storedFilename = _storedFilename;
            originalFileName = _originalFileName;
        }

        private string storedFilename { get; set; }
        private string originalFileName { get; set; }

        public async Task DownloadFile()
        {
            //var response = await Http.GetAsync($"/api/File/{storedFilename}");

            //if (!response.IsSuccessStatusCode)
            //{
            // await JS.InvokeVoidAsync("alert", "File not found.");
            // }
            //  else
            //  {
            //    var fileStream = response.Content.ReadAsStream();
            //    using var streamRef = new DotNetStreamReference(stream: fileStream);
            //await JS.InvokeVoidAsync("downloadFileFromStream", originalFileName, streamRef);
            //   }
        }
    }
}