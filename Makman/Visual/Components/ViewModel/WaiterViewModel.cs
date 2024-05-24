

namespace Makman.Visual.Components.ViewModel
{
    public class WaiterViewModel : Core.ViewModel
    {
        private string text;
        public string Text { 
            get => text;
            set 
            { 
                text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        public void StatusUpdate(string text) => Text = text;

    }
}
