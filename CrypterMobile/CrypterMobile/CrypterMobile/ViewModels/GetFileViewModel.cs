using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CrypterMobile.Services;
using Xamarin.Forms;

namespace CrypterMobile.ViewModels
{
    public class GetFileViewModel : BaseViewModel
    {
        private string fileName;
        private int extensionIndex = 1;
        private bool canSave = true;

        public ObservableCollection<string> Extensions { get; } = new ObservableCollection<string> { ".docx", ".txt"};

        public string FileName
        {
            get => fileName;
            set => SetProperty(ref fileName, value, nameof(FileName), () =>
            {
                CanSave = !fileName.Equals("");
            });
        }

        public int ExtensionIndex
        {
            get => extensionIndex;
            set => SetProperty(ref extensionIndex, value, nameof(ExtensionIndex));
        }

        public bool CanSave
        {
            get => canSave;
            set => SetProperty(ref canSave, value, nameof(CanSave));
        }

        public string Extension => Extensions[ExtensionIndex];

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public GetFileViewModel()
        {
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            ExtensionIndex = 0;
            FileName = string.Empty;
        }

        public async void OnSave()
        {
            await Shell.Current.GoToAsync("..");
            DependencyService.Get<IFileManager>().FileNameChoosed(FileName + Extension);
        }

        public async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
            DependencyService.Get<IFileManager>().FileNameChoosed(null);
        }
    }
}
