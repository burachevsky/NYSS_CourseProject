using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CrypterCore;

namespace CrypterDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public VigenereCipher Cipher = new VigenereCipher("Скорпион", Alphabets.RUSSIAN);

        public IAlphabet CurrentAlphabet => ComboBox_Language.SelectedItem as IAlphabet;

        public bool IsEncrypting
        {
            get => RadioButton_Encrypt.IsChecked != null && (bool) RadioButton_Encrypt.IsChecked;
            set
            {
                if (value)
                {
                    RadioButton_Encrypt.IsChecked = true;
                    RadioButton_Decrypt.IsChecked = false;
                }
                else
                {
                    RadioButton_Encrypt.IsChecked = false;
                    RadioButton_Decrypt.IsChecked = true;
                }
            }
        }

        public bool IsJitCiphering { get; private set; }


        public MainWindow()
        {
            InitializeComponent();
            IsEncrypting = false;

            TextBox_Input.TextChanged += (o, a) =>
            {
                if (IsJitCiphering)
                {
                    Update();
                }
            };

            TextBox_Key.TextChanged += (o, a) =>
            {
                if (IsJitCiphering)
                {
                    Update();
                }
                else
                {
                    ValidateKey();
                }
            };

            var alphabets = Alphabets.AlphabetList();
            ComboBox_Language.ItemsSource = alphabets;
            ComboBox_Language.SelectedItem = alphabets[0];
            Cipher = new VigenereCipher("Скорпион", alphabets[0]);

            ComboBox_Language.SelectionChanged += Button_Run_OnClick;
            RadioButton_Encrypt.Checked += Button_Run_OnClick;
            RadioButton_Decrypt.Checked += Button_Run_OnClick;

            CheckBox_JitCiphering.Checked += (e, a) =>
            {
                IsJitCiphering = true;
                Button_Run.Visibility =  Visibility.Hidden;
                Update();
            };

            CheckBox_JitCiphering.Unchecked += (e, a) =>
            {
                IsJitCiphering = false;
                Button_Run.Visibility = Visibility.Visible;
                Update();
            };
        }

        public void DecryptCommandLineArgFile(string path)
        {
            try
            {
                TextBox_Input.Text = File.ReadAllText(path);
                Button_Run.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    $"При чтении файла возникла ошибка: {e.GetType()}\nВозможно, файл поврежден или отсутствует");
            }
        }

        private bool ValidateKey()
        {
            var curKey = TextBox_Key.Text;
            try
            {
                if (!curKey.Equals(Cipher.KeyAsString) || !CurrentAlphabet.Equals(Cipher.Alphabet))
                {
                    Cipher = new VigenereCipher(curKey, CurrentAlphabet);
                }

                Colors.InitTextBoxBorderColor(TextBox_Key, true);
                return true;
            }
            catch (ArgumentException)
            { 
                Colors.InitTextBoxBorderColor(TextBox_Key, false);
                TextBox_Output.Text = "";
                return false;
            }
        }

        private void Button_Run_OnClick(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void Update()
        {
            if (ValidateKey())
            {
                var input = TextBox_Input.Text;
                TextBox_Output.Text = IsEncrypting 
                    ? Cipher.Encrypt(input) 
                    : Cipher.Decrypt(input);
            }
        }
    }
}