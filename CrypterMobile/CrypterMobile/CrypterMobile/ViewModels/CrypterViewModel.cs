using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CrypterCore;
using CrypterMobile.Annotations;
using CrypterMobile.Services;
using Xamarin.Forms;

namespace CrypterMobile.ViewModels
{
    public class CrypterViewModel : BaseViewModel
    {
        private string inputText = string.Empty;
        private string keyText = "Скорпион";
        private string outputText = string.Empty;
        private int currentModeIndex;
        private int currentAlphabetIndex;
        private bool isKeyNotValid;
        private bool isKeyPassword;
        private string eyeImageSource;

        public string InputText
        {
            get => inputText;
            set => SetProperty(ref inputText, value, nameof(InputText), () => { OutputText = string.Empty; });
        }

        public string KeyText
        {
            get => keyText;
            set => SetProperty(ref keyText, value, nameof(KeyText), () =>
            {
                ValidateKey();
                OutputText = string.Empty;
            });
        }

        public string OutputText
        {
            get => outputText;
            set => SetProperty(ref outputText, value, nameof(OutputText));
        }

        public bool IsKeyNotValid
        {
            get => isKeyNotValid;
            set => SetProperty(ref isKeyNotValid, value, nameof(IsKeyNotValid));
        }

        public int CurrentModeIndex
        {
            get => currentModeIndex;
            set => SetProperty(ref currentModeIndex, value, nameof(CurrentModeIndex),
                () => OutputText = string.Empty);
        }

        public int CurrentAlphabetIndex
        {
            get => currentAlphabetIndex;
            set => SetProperty(ref currentAlphabetIndex, value, nameof(CurrentAlphabetIndex), ValidateKey);
        }

        public bool IsKeyPassword
        {
            get => isKeyPassword;
            set => SetProperty(ref isKeyPassword, value, nameof(IsKeyPassword), () =>
            {
                if (IsKeyPassword)
                {
                    EyeImageSource = "Resources/drawable/icon_eye_24.png";
                }
                else
                {
                    EyeImageSource = "Resources/drawable/icon_thirdeye_24.png";
                    Alert.ShortAlert("Third eye is watching you -_-");
                }
            });
        }

        public string EyeImageSource
        {
            get => eyeImageSource;
            set => SetProperty(ref eyeImageSource, value, nameof(EyeImageSource));
        }

        public ObservableCollection<IAlphabet> Languages { get; set; } = new ObservableCollection<IAlphabet>
            {Alphabets.RUSSIAN, Alphabets.ENGLISH};


        public VigenereCipher Cipher { get; private set; }

        public ICommand RunCommand { get; }
        public ICommand ReverseCommand { get; }
        public ICommand OpenCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand ShowKeyCommand { get; }

        public bool IsEncrypting
        {
            get => CurrentModeIndex == 0;
            set => CurrentModeIndex = value ? 0 : 1;
        }

        private IAlphabet CurrentAlphabet => CurrentAlphabetIndex >= 0 && CurrentAlphabetIndex < Languages.Count
            ? Languages[CurrentAlphabetIndex]
            : Languages[0];

        private readonly IFileManager fileManager = DependencyService.Get<IFileManager>();

        public CrypterViewModel()
        {
            RunCommand = new Command(Run);
            ReverseCommand = new Command(Reverse);
            OpenCommand = new Command(OpenAsync);
            SaveCommand = new Command(SaveAsync);
            ShowKeyCommand = new Command(ShowKey);
            IsEncrypting = true;
            IsKeyPassword = true;
        }

        private void ValidateKey()
        {
            var alphabet = CurrentAlphabet;
            IsKeyNotValid = !keyText.Any(alphabet.Contains);
        }

        private void Run()
        {
            if (IsKeyNotValid)
            {
                return;
            }

            try
            {
                if (Cipher == null || Cipher.Alphabet != CurrentAlphabet || !Cipher.KeyAsString.Equals(keyText))
                {
                    Cipher = new VigenereCipher(keyText, CurrentAlphabet);
                }

                OutputText = IsEncrypting ? Cipher.Encrypt(InputText) : Cipher.Decrypt(InputText);
            }
            catch (ArgumentException)
            {
                //never happens
            }
        }

        private void Reverse()
        {
            if (OutputText.Equals(string.Empty))
            {
                Run();
            }
            InputText = OutputText;
            IsEncrypting = !IsEncrypting;
            Run();
        }

        public void OpenAsync()
        {
            fileManager.OpenReadFileDialog(text => InputText = text);
        }

        public void SaveAsync()
        {
            fileManager.OpenSaveFileDialog(OutputText);
        }

        public void ShowKey()
        {
            IsKeyPassword = !IsKeyPassword;
        }
    }
}