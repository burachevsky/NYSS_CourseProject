using System.Windows;
using System.Windows.Controls;
using CrypterDesktop.ViewModels;


namespace CrypterDesktop.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            DataContext = viewModel;

            viewModel.AlphabetIndex = 0;
            viewModel.IsJitCiphering = true;
            viewModel.KeyText = "Скорпион";
            viewModel.IsDecrypting = true;
        }

        public void OnKeyTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            viewModel.KeyText = textBox.Text;
        }

        public void OnInputTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            viewModel.InputText = textBox.Text;
        }
    }
}