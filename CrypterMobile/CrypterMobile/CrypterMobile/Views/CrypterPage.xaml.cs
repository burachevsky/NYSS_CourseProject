using System;
using CrypterMobile.Services;
using CrypterMobile.ViewModels;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CrypterMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrypterPage : ContentPage
    {
        private readonly CrypterViewModel vm;

        [Android.Runtime.Preserve]
        public CrypterPage()
        {
            InitializeComponent();
            vm = new CrypterViewModel();
            BindingContext = vm;
            Picker_Language.SelectedIndex = 0;
            vm.IsCopyButtonVisible = false;
        }
    }
}