using Microsoft.JSInterop;
using System.Text.Json;

namespace Chat_Application.Help_Classes
{
    public interface ILocalStorageService
    {
        Task<string?> GetItem(string key);
        Task SetItem<T>(string key, T value);
        Task RemoveItem(string key);
    }

    public class LocalStorageService : ILocalStorageService
    {
        private IJSRuntime _jsRuntime;

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        //public  Task<T?> GetItem<T>(string? key) =>
        //      Task.Run(() => JsonSerializer.Deserialize<T>(_jsRuntime.InvokeAsync<string>("localStorage.getItem", key).Result));

        public async Task<string> GetItem(string key) 
        {
            return JsonSerializer.Deserialize<string>(await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key));
        }
        public Task SetItem<T>(string key, T value) =>
            Task.Run(() => _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value)));

        public Task RemoveItem(string key) =>
            Task.Run(() => _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key));
    }
}
