


using Makman.Middle.Entities;
using Makman.Middle.Entities.Settings;

namespace Makman.Visual.Components.ViewModel
{
    public class UnitsAdderMainViewModel : Core.ViewModel
    {
        public required Settings Settings { get; set; }

        public Action<string[], CollectionDirectory?> ReceivedFilenamesAction { get; set; }

        private Core.ViewModel? dragDropControl;
        public Core.ViewModel? DragDropControl
        {
            get => dragDropControl;
            private set
            {
                dragDropControl = value;
                OnPropertyChanged(nameof(DragDropControl));
            }
        }

        public void StartTryingUnitCreationAction(Action<string[], CollectionDirectory?> receivedFilenamesAction)
        {
            ReceivedFilenamesAction = receivedFilenamesAction;
            {
                string? targetFolderPath;

                if (Settings.DefaultTryMoveFilesByDirectoryTagcategoryNameOnAdding)
                {

                }

                targetFolderPath = "C:\\Users\\Akatsuki\\Desktop\\MakmanTestTarget";

                DragDropControl = new DragDropFilesReceiverViewModel()
                {
                    ReturnFilenamesAction = returnedFilenames => ReceivedFilenamesAction(returnedFilenames, new CollectionDirectory()
                    {
                        Path = targetFolderPath,
                        SynchronizingWithCloud = false
                    })
                };
            }
        }

        public UnitsAdderMainViewModel()
        {

        }
    }
}
