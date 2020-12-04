using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CrypterCore;
using CrypterDesktop.Services;

namespace CrypterDesktop.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private string inputText = string.Empty;
        private string outputText = string.Empty;
        private string keyText = string.Empty;
        private Visibility runButtonVisibility;
        private bool isEncrypting;
        private bool isDecrypting;
        private bool isJitCiphering;
        private int alphabetIndex;
        private Brush keyTextBorderBrush = Brushes.Green;
        private bool isKeyNotValid;

        public string InputText
        {
            get => inputText;
            set => SetProperty(ref inputText, value, nameof(InputText), TryUpdate);
        }

        public string OutputText
        {
            get => outputText;
            set => SetProperty(ref outputText, value, nameof(OutputText));
        }

        public string KeyText
        {
            get => keyText;
            set => SetProperty(ref keyText, value, nameof(KeyText), TryUpdate);
        }

        public bool IsEncrypting
        {
            get => isEncrypting;
            set => SetProperty(ref isEncrypting, value, nameof(IsEncrypting), () =>
            {
                if (isEncrypting)
                {
                    IsDecrypting = false;
                }
                else if (!IsDecrypting)
                {
                    IsDecrypting = true;
                }

                TryUpdate();
            });
        }

        public bool IsDecrypting
        {
            get => isDecrypting;
            set => SetProperty(ref isDecrypting, value, nameof(IsDecrypting), () =>
            {
                if (isDecrypting)
                {
                    IsEncrypting = false;
                }
                else if (!IsEncrypting)
                {
                    IsEncrypting = true;
                }

                TryUpdate();
            });
        }

        public bool IsJitCiphering
        {
            get => isJitCiphering;
            set => SetProperty(ref isJitCiphering, value, nameof(IsJitCiphering), () =>
            {
                RunButtonVisibility = isJitCiphering
                    ? Visibility.Hidden
                    : Visibility.Visible;

                TryUpdate();
            });
        }

        public Visibility RunButtonVisibility
        {
            get => runButtonVisibility;
            set => SetProperty(ref runButtonVisibility, value, nameof(RunButtonVisibility));
        }

        public int AlphabetIndex
        {
            get => alphabetIndex;
            set => SetProperty(ref alphabetIndex, value, nameof(AlphabetIndex), TryUpdate);
        }

        public Brush KeyTextBorderBrush
        {
            get => keyTextBorderBrush;
            set => SetProperty(ref keyTextBorderBrush, value, nameof(KeyTextBorderBrush));
        }

        public bool IsKeyNotValid
        {
            get => isKeyNotValid;
            set => SetProperty(ref isKeyNotValid, value, nameof(IsKeyNotValid), () =>
            {
                KeyTextBorderBrush = isKeyNotValid
                    ? Brushes.Red
                    : Brushes.Green;
            });
        }

        public IAlphabet CurrentAlphabet => Alphabets[AlphabetIndex];

        public FileManager FileManager { get; }

        public VigenereCipher Cipher { get; set; }

        public ObservableCollection<IAlphabet> Alphabets { get; }

        public MainViewModel()
        {
            FileManager = new FileManager();
            SaveCommand = new Command(OnSave);
            OpenCommand = new Command(OnOpen);
            RunCommand = new Command(OnRun);
            ExitCommand = new Command(OnExit);
            ReverseCommand = new Command(OnReverse);
            Alphabets = new ObservableCollection<IAlphabet>(CrypterCore.Alphabets.AlphabetList());
        }

        public ICommand SaveCommand { get; }
        public ICommand OpenCommand { get; }
        public ICommand RunCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand ReverseCommand { get; }

        private void Validate()
        {
            var alphabet = CurrentAlphabet;
            IsKeyNotValid = !keyText.Any(alphabet.Contains);
            if (IsKeyNotValid)
            {
                OutputText = string.Empty;
            }
        }

        private void Update()
        {
            if (IsKeyNotValid)
            {
                return;
            }

            try
            {
                if (Cipher == null || Cipher.Alphabet != CurrentAlphabet || !Cipher.KeyAsString.Equals(keyText))
                {
                    Cipher = new VigenereCipher(KeyText, CurrentAlphabet);
                }

                OutputText = IsEncrypting ? Cipher.Encrypt(InputText) : Cipher.Decrypt(InputText);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.StackTrace); //never happens
            }
        }

        private void TryUpdate()
        {
            Validate();

            if (IsJitCiphering)
            {
                Update();
            }
            else
            {
                OutputText = string.Empty;
            }
        }

        private void OnRun()
        {
            Update();
        }

        private async void OnSave()
        {
            await FileManager.OpenSaveFileDialog(OutputText);
        }

        private async void OnOpen()
        {
            var text = await FileManager.OpenReadFileDialog();
            if (text != null)
            {
                InputText = text;
            }
        }

        private void OnReverse()
        {
            if (isKeyNotValid)
            {
                return;
            }

            if (OutputText.Equals(string.Empty))
            {
                Update();
            }

            var input = OutputText;
            InputText = string.Empty;

            IsEncrypting = !IsEncrypting;

            InputText = input;

            Update();
        }

        private void OnExit()
        {
            Application.Current.Shutdown();
        }
    }
}