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

        public bool IsJitCiphering => 
            CheckBox_JitCiphering.IsChecked != null && (bool) CheckBox_JitCiphering.IsChecked;


        public MainWindow()
        {
            InitializeComponent();
            IsEncrypting = false;

            TextBox_Input.TextChanged += (o, a) =>
            {
                if (IsJitCiphering)
                {
                    Button_Run_OnClick(null, null);
                }
            };

            TextBox_Key.TextChanged += (o, a) =>
            {
                if (IsJitCiphering)
                {
                    Button_Run_OnClick(null, null);
                }
                else
                {
                    ValidateKey();
                }
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
                    $"При чтении файла возникла ошибка: {e.GetType()}\nВозможно, файл поврежден ил отсутствует");
            }
        }

        private bool ValidateKey()
        {
            var curKey = TextBox_Key.Text;
            try
            {
                if (!curKey.Equals(Cipher.KeyAsString))
                {
                    Cipher = new VigenereCipher(curKey, Alphabets.RUSSIAN);
                }

                TextBox_Key.BorderBrush = Brushes.Green;
                return true;
            }
            catch (ArgumentException)
            { 
                TextBox_Key.BorderBrush = Brushes.Red;
                return false;
            }
        }

        private void Button_Run_OnClick(object sender, RoutedEventArgs e)
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