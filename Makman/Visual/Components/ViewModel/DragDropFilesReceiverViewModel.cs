using Makman.Visual.Utilities; 

namespace Makman.Visual.Components.ViewModel
{
    public class DragDropFilesReceiverViewModel : Core.ViewModel, IFileDragDropTarget
    {
        public required Action<string[]> ReturnFilenamesAction { get; set; }

        public void OnFileDrop(string[] filepaths)
        {
            ReturnFilenamesAction(filepaths);
        }
    }
}
