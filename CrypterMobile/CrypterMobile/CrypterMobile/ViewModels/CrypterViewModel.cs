using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CrypterCore;
using CrypterMobile.Annotations;
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
        
        public string InputText
        {
            get => inputText;
            set => SetProperty(ref inputText, value, nameof(InputText), () =>
            {
                OutputText = string.Empty;
            });
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

        public ObservableCollection<IAlphabet> Languages { get; set; } = new ObservableCollection<IAlphabet>
            {Alphabets.RUSSIAN, Alphabets.ENGLISH};

        public VigenereCipher Cipher { get; private set; }

        public ICommand RunCommand { get; }

        public ICommand ReverseCommand { get; }

        public bool IsEncrypting
        {
            get => CurrentModeIndex == 0;
            set => CurrentModeIndex = value ? 0 : 1;
        }

        private IAlphabet CurrentAlphabet => CurrentAlphabetIndex >= 0 && CurrentAlphabetIndex < Languages.Count 
            ? Languages[CurrentAlphabetIndex] 
            : Languages[0];

        public CrypterViewModel()
        {
            RunCommand = new Command(Run);
            ReverseCommand = new Command(Reverse);
            IsEncrypting = true;
            //Cipher = new VigenereCipher(KeyText, CurrentAlphabet);
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
                if (Cipher == null ||Cipher.Alphabet != CurrentAlphabet || !Cipher.KeyAsString.Equals(keyText))
                {
                    Cipher = new VigenereCipher(keyText, CurrentAlphabet);
                }

                OutputText = IsEncrypting ? Cipher.Encrypt(InputText) : Cipher.Decrypt(InputText);
            }
            catch (ArgumentException e)
            {
                //never happens
            }
        }

        private void Reverse()
        {
            InputText = OutputText;
            IsEncrypting = !IsEncrypting;
            Run();
        }
    }
}