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

        public ICommand StartRecording { get; set; }
        public ICommand PauseRecording { get; set; }
        public ICommand StopRecording { get; set; }
        public ICommand StopRecordingAndLoadVideo { get; set; }

        private IJSObjectReference _module;

        private BlazoredModalInstance BlazoredModal;
        public IJSRuntime JSRuntime { get; set; }

        public VideoRecordViewModel(IJSRuntime jSRuntime)
        {
            JSRuntime = jSRuntime;
            StartRecording = new RelayCommand(async () => await StartRecordingAsync());
            PauseRecording = new RelayCommand(async () => await PauseRecordingAsync());
            StopRecording = new RelayCommand(async () => await StopRecordingAsync());
            StopRecordingAndLoadVideo = new RelayCommand(async () => await StopRecordingAndLoadVideoAsync());
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

        public async Task StopRecordingAsync()
        {
            try
            {
                byte[] videoData = await _module.InvokeAsync<byte[]>("stopRecording");
                if (videoData != null && videoData.Length > 0)
                {
                    // Now you have the video data as a byte array in Blazor and you can process or send it to the server
                    Console.WriteLine("Video data received with length: " + videoData.Length);
                    // Optional: Send the data to a server method or process it further
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