﻿using CrypterCore;
using CrypterMobile.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
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
        private bool isCopyButtonVisible;
        private bool isOutputReady;

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
            set => SetProperty(ref outputText, value, nameof(OutputText), () =>
            {
                IsOutputReady = !outputText.Equals(string.Empty);
            });
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
                    if (++secret % 3 == 0)
                    {
                        Alert.ShortAlert("The Third eye is watching -_-");
                    }
                }
            });
        }
        private int secret = 0;

        public string EyeImageSource
        {
            get => eyeImageSource;
            set => SetProperty(ref eyeImageSource, value, nameof(EyeImageSource));
        }

        public bool IsCopyButtonVisible
        {
            get => isCopyButtonVisible;
            set => SetProperty(ref isCopyButtonVisible, value, nameof(IsCopyButtonVisible));
        }

        public bool IsOutputReady
        {
            get => isOutputReady;
            set => SetProperty(ref isOutputReady, value, nameof(IsOutputReady));
        }

        public ObservableCollection<IAlphabet> Languages { get; set; } = new ObservableCollection<IAlphabet>
            {Alphabets.RUSSIAN, Alphabets.ENGLISH};


        public VigenereCipher Cipher { get; private set; }

        public ICommand RunCommand { get; }
        public ICommand ReverseCommand { get; }
        public ICommand OpenCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand ShowKeyCommand { get; }
        public ICommand CopyResultCommand { get; }
        public ICommand ShareResultCommand { get; }

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
            CopyResultCommand = new Command(CopyResult);
            ShareResultCommand = new Command(ShareResult);

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
                IsOutputReady = true;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.StackTrace);//never happens
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
            fileManager.OpenReadFileDialogAsync(text => InputText = text);
        }

        public void SaveAsync()
        {
            fileManager.OpenSaveFileDialogAsync(OutputText);
        }

        public void ShowKey()
        {
            IsKeyPassword = !IsKeyPassword;
        }

        public async void CopyResult()
        {
            await Clipboard.SetTextAsync(OutputText);
            IsCopyButtonVisible = false;
            Alert.LongAlert("Скопировано!");
        }

        public async void ShareResult()
        {
            await Share.RequestAsync(new ShareTextRequest(OutputText));
        }
    }
}