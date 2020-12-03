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
    public partial class GetFilePage : ContentPage
    {
        public GetFilePage()
        {
            InitializeComponent();
            BindingContext = new GetFileViewModel();
            Picker_Extension.SelectedIndex = 0;
            Entry_FileName.Text = string.Empty;
        }
    }
}