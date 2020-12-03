using CrypterMobile.Views;
using Xamarin.Forms;

namespace CrypterMobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(GetFilePage), typeof(GetFilePage));
        }
    }
}
