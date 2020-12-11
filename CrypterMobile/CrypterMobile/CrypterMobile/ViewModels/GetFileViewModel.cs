using System;
using CrypterMobile.Services;
using CrypterMobile.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace CrypterMobile.ViewModels
{
    public class GetFileViewModel : BaseViewModel
    {
        private string fileName;
        private int extensionIndex = 1;
        private bool canSave = true;
        private ObservableCollection<DirectoryListItem> directoryItems;
        private string directoryPath;
        private int selectedItemIndex;
        private string okButtonText;
        private bool isSaveAsMode;
        private string currentEncodingName;

        public GetFileMode Mode { get; private set; }

        public static GetFileViewModel Current { get; private set; }

        public ObservableCollection<string> Extensions { get; } = new ObservableCollection<string> { ".docx", ".txt" };

        public int SelectedItemIndex
        {
            get => selectedItemIndex;
            set => SetProperty(ref selectedItemIndex, value, nameof(SelectedItemIndex), () =>
            {
                if (value >= 0 && value < DirectoryItems.Count)
                {
                    var item = DirectoryItems[value];
                    if (item.IsDirectory)
                    {
                        DirectoryPath += item.Name;
                        OnRefresh();
                    }
                    else
                    {
                        var dot = item.Name.LastIndexOf('.');
                        var fileName = item.Name.Substring(0, dot);
                        var ext = item.Name.Substring(dot);

                        FileName = fileName;
                        ExtensionIndex = Extensions.IndexOf(ext);

                        if (Mode == GetFileMode.Open)
                        {
                            OnOk();
                        }
                    }
                }
            });
        }

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

        public string DirectoryPath
        {
            get => directoryPath;
            set => SetProperty(ref directoryPath, value, nameof(DirectoryPath));
        }

        public ObservableCollection<DirectoryListItem> DirectoryItems
        {
            get => directoryItems;
            set => SetProperty(ref directoryItems, value, nameof(DirectoryItems));
        }

        public string Extension => Extensions[ExtensionIndex];

        public string OkButtonText
        {
            get => okButtonText;
            set => SetProperty(ref okButtonText, value, nameof(OkButtonText));
        }

        public bool IsSaveAsMode
        {
            get => isSaveAsMode;
            set => SetProperty(ref isSaveAsMode, value, nameof(IsSaveAsMode));
        }

        public string CurrentEncodingName
        {
            get => currentEncodingName;
            set => SetProperty(ref currentEncodingName, value, nameof(CurrentEncodingName));
        }

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand PrevDirCommand { get; }
        public ICommand CreateFolderCommand { get; }
        public ICommand ChangeEncodingCommand { get; }

        public GetFilePage Page { get; set; }

        public GetFileViewModel()
        {
            Current = this;
            OkCommand = new Command(OnOk);
            CancelCommand = new Command(OnCancel);
            RefreshCommand = new Command(OnRefresh);
            PrevDirCommand = new Command(OnPrevDir);
            CreateFolderCommand = new Command(CreateFolder);
            ChangeEncodingCommand = new Command(OnChangeEncoding);
            ExtensionIndex = 0;
            FileName = string.Empty;
            CanSave = false;
            CurrentEncodingName = DependencyService.Get<IFileManager>().CurrentEncodingName;
            var dir = DependencyService.Get<IFileManager>().GetStartDirectory().Result;
            UpdateDir(dir);
        }

        public void UpdateDir((string path, List<DirectoryListItem> items) directory)
        {
            DirectoryPath = directory.path;
            DirectoryItems = new ObservableCollection<DirectoryListItem>
            (
                directory.items.Where
                (
                    it => it.IsDirectory || Extensions.Contains(it.Name.Substring(it.Name.LastIndexOf('.')))
                )
            );
            FileName = string.Empty;
        }

        public void OnPrevDir()
        {
            var fileManager = DependencyService.Get<IFileManager>();
            if (!fileManager.IsRoot(DirectoryPath))
            {
                DirectoryPath = DirectoryPath.Substring(0, DirectoryPath.LastIndexOf('/', DirectoryPath.Length - 2));
                OnRefresh();
                FileName = string.Empty;
            }
        }

        public async void OnRefresh()
        {
            UpdateDir(await DependencyService.Get<IFileManager>().GetDirectory(DirectoryPath));
        }

        public async void OnOk()
        {
            var goBack = Shell.Current.GoToAsync("..");
            var fileWorker = Task.Run(() => DependencyService.Get<IFileManager>().FileNameChoosed(DirectoryPath + FileName + Extension));
            
            await Task.WhenAll(goBack, fileWorker);
            
            var message = fileWorker.Result;
            if (message != null)
            {
                Alert.LongAlert(message);
            }
        }

        public async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
            DependencyService.Get<IFileManager>().FileNameChoosed(null);
        }

        public async void OnChangeEncoding()
        {
            try
            {
                var encoding = await Page.DisplayActionSheet
                (
                    "Выбор кодировки",
                    "Отменить",
                    null,
                    DependencyService.Get<IFileManager>().GetAvailableEncodings()
                );

                if (encoding != null && !encoding.Equals("Отменить"))
                {
                    DependencyService.Get<IFileManager>().CurrentEncodingName = encoding;
                    CurrentEncodingName = encoding;
                    Alert.ShortAlert("Кодировка изменена: " + encoding);
                }
            }

            catch (Exception e)
            {
                Alert.LongAlert(e.GetType() + ":\n" + e.Message);
            }
        }

        public async void CreateFolder()
        {
            var folderName = await Page.DisplayPromptAsync("Создание папки", "Введите название", "Создать", "Отменить");

            if (folderName == null)
            {
                return;
            }

            DependencyService.Get<IFileManager>().CreateFolder(DirectoryPath + folderName);

            OnRefresh();
        }

        public void ApplyMode(GetFileMode mode)
        {
            Mode = mode;

            switch (mode)
            {
                case GetFileMode.SaveAs:
                    Title = "Сохранить как";
                    OkButtonText = "Сохранить";
                    IsSaveAsMode = true;
                    break;

                case GetFileMode.Open:
                    Title = "Открыть";
                    IsSaveAsMode = false;
                    break;
            }
        }

        public enum GetFileMode
        {
            SaveAs, Open
        }
    }
}