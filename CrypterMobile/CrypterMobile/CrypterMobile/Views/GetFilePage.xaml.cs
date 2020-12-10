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
        public GetFileViewModel viewModel;

        public GetFilePage()
        {
            InitializeComponent();
            viewModel = new GetFileViewModel();
            BindingContext = viewModel;
            Picker_Extension.SelectedIndex = 0;
            Entry_FileName.Text = string.Empty;
        }

        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            viewModel.SelectedItemIndex = e.ItemIndex;
            viewModel.SelectedItemIndex = -1;
        }
    }
}