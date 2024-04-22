using ChatClient.Infrastructure;
using ChatClient.Models;
using ChatClient.Services;
using ChatClient.Util;
using Microsoft.AspNetCore.Components;
using SqliteWasmHelper;
using System.Text.Json;
using System.Text;
using System.Windows.Input;
using ChatClient.Handlers;
using Microsoft.JSInterop;
using Blazored.Modal;
using System.Security.Principal;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ChatClient.Infrastructure.Repositories.Abstraction;

namespace ChatClient.ViewModel
{
    public class VideoRecordViewModel : BaseViewModel
    {
        public string _videoSrc;

        public string videoSrc
        {
            get => _videoSrc;
            set { _videoSrc = value; OnPropertyChanged(); }
        }

        public Action<UploadResult> OnRecordingCompleted { get; set; }

        public ICommand StartRecording { get; set; }
        public ICommand PauseRecording { get; set; }
        public ICommand StopRecording { get; set; }
        public ICommand StopRecordingAndLoadVideo { get; set; }

        private IJSObjectReference _module;

        private BlazoredModalInstance BlazoredModal;
        public IJSRuntime JSRuntime { get; set; }
        public IRepositoryWrapper Repos { get; set; }

        public VideoRecordViewModel(IJSRuntime jSRuntime, IRepositoryWrapper repos)
        {
            JSRuntime = jSRuntime;
            StartRecording = new RelayCommand(async () => await StartRecordingAsync());
            PauseRecording = new RelayCommand(async () => await PauseRecordingAsync());
            StopRecording = new RelayCommand(async () => await StopRecordingAsync());
            StopRecordingAndLoadVideo = new RelayCommand(async () => await StopRecordingAndLoadVideoAsync());
            Repos = repos;
        }

        public async Task SetUpAsync(BlazoredModalInstance blazoredModal)
        {
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/VideoRecordScripts.js");
            BlazoredModal = blazoredModal;
        }

        public async Task StartRecordingAsync()
        {
            try
            {
                await _module.InvokeVoidAsync("startRecording");
            }
            catch (Exception ex)
            {
            }
        }

        public async Task PauseRecordingAsync()
        {
            try
            {
                await _module.InvokeVoidAsync("toggleRecording");
            }
            catch (Exception ex)
            {
            }
        }

        private List<string> fileNames = new();
        private List<UploadResult> uploadResults = new();

        public async Task StopRecordingAsync()
        {
            UploadResult uploadResult = default(UploadResult);

            try
            {
                var result = await _module.InvokeAsync<RecordingResult>("stopRecording");
                if (!string.IsNullOrEmpty(result.Base64Content) && result.Base64Content.Length > 0)
                {
                    var bytes = Convert.FromBase64String(result.Base64Content);
                    using var content = new MultipartFormDataContent();

                    var stream = new MemoryStream(bytes);
                    var fileContent = new StreamContent(stream);

                    // Extract only the base MIME type
                    string baseMimeType = result.MimeType.Split(';')[0]; // Split and take the first part before any ';'
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(baseMimeType);
                    var filename = Guid.NewGuid().ToString() + "." + baseMimeType.Split('/')[1]; // Append proper file extension

                    content.Add(
                        content: fileContent,
                        name: "\"files\"",
                        fileName: filename);

                    var httpClient = new HttpClient();

                    HttpResponseMessage response = await httpClient.PostAsync($"{Constants.ChatServerUrl}/File/UploadFile", content);
                    var newUploadResults = await response.Content.ReadFromJsonAsync<List<UploadResult>>();

                    if (newUploadResults != null)
                    {
                        uploadResults = uploadResults.Concat(newUploadResults).ToList();
                    }
                    uploadResult = newUploadResults[newUploadResults.Count - 1];
                    //store file
                    ChatFile chatFile = new ChatFile
                    {
                        FileContentArray = bytes,
                        UploadResultContentType = uploadResult.ContentType,
                        UploadResultFileName = uploadResult.FileName,
                        UploadResultId = uploadResult.Id,
                        UploadResultStoredFileName = uploadResult.StoredFileName,
                    };
                    await Repos.ChatFile.CreateAsync(chatFile);
                    //await Task.Delay(2000);
                }
                else
                {
                    Console.WriteLine("No video data received or recording was not started");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                if (uploadResult is not null)
                    OnRecordingCompleted.Invoke(uploadResult);
            }
        }

        private async Task StopRecordingAndLoadVideoAsync()
        {
            try
            {
                byte[] videoData = await _module.InvokeAsync<byte[]>("stopRecording");
                if (videoData != null && videoData.Length > 0)
                {
                    videoSrc = ConvertToBase64(videoData);
                }
                else
                {
                    Console.WriteLine("No video data received or recording was not started");
                }
            }
            catch (Exception ex)
            {
            }
        }

        public string ConvertToBase64(byte[] videoBytes)
        {
            return $"data:video/webm;base64,{Convert.ToBase64String(videoBytes)}";
        }

        public async ValueTask DisposeAsync()
        {
            if (_module != null)
            {
                await _module.DisposeAsync();
            }
        }
    }
}