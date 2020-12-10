using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrypterMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Forms.Behaviors;
using XLabs.Forms.Controls;

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
        }
    }
}