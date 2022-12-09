using BlazorFlickity.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorFlickity.Components.Carousel
{
    public partial class Carousel : ComponentBase
    {
        [Parameter] public string? Title { get; set; }
        [Parameter] public List<Items>? Items { get; set; }
        [Parameter] public EventCallback<Items> OnSelectedIndexChanged { get; set; }
        [Parameter] public EventCallback<Items> ValueChanged { get; set; }
        [Inject] IJSRuntime JsRuntime { get; set; }

        private CarouselJsInterop jsInterop;

        private Items _value;
        [Parameter]
        public Items Value
        {
            get => _value;
            set
            {
                if (_value == value)
                    return;

                _value = value;
                GoToSlide(_value.Index);
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                jsInterop = new CarouselJsInterop(JsRuntime);

                jsInterop.OnSelectedIndexChanged += SelectedIndexChanged;

                await jsInterop.Init();
                //await jsInterop.GoToSlide(Value.Index);
            }
        }

        private void SelectedIndexChanged(int selectedIndex)
        {
            var item = Items?.FirstOrDefault(x => x.Index == selectedIndex);
            OnSelectedIndexChanged.InvokeAsync(item);
        }

        private async void GoToSlide(int position)
        {
            if (jsInterop is not null)
                await jsInterop.GoToSlide(position);
        }
    }
}