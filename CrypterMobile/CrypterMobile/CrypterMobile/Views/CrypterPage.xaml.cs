using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrypterMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CrypterMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrypterPage : ContentPage
    {
        public CrypterPage()
        {
            InitializeComponent();
            BindingContext = new CrypterViewModel();
            Picker_Language.SelectedIndex = 0;
        }
    }
}