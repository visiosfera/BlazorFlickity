using Microsoft.JSInterop;

namespace BlazorFlickity.Components.Carousel
{
    public class CarouselJsInterop : IAsyncDisposable
    {
        public event Action<int> OnSelectedIndexChanged;

        private readonly Lazy<Task<IJSObjectReference>> moduleTask;
        private readonly DotNetObjectReference<CarouselJsInterop> dotNetObjectReference;

        public CarouselJsInterop(IJSRuntime jsRuntime)
        {
            dotNetObjectReference = DotNetObjectReference.Create(this);
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/carousel.js").AsTask());
        }

        public async ValueTask Init()
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("init", dotNetObjectReference);
        }

        public async ValueTask GoToSlide(int position)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("goToSlide", position);
        }

        [JSInvokable]
        public void OnItemSelectedCallback(int selectedIndex)
        {
            OnSelectedIndexChanged?.Invoke(selectedIndex);
        }

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
