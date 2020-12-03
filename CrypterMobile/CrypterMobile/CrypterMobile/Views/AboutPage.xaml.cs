using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace CrypterMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        [Android.Runtime.Preserve]
        public AboutPage()
        {
            InitializeComponent();
        }
    }
}