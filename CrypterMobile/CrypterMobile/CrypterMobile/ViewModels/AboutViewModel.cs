using System;
using System.Windows.Input;
using CrypterMobile.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CrypterMobile.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "О программе";
            OpenWikipediaCommand = new Command(async () => await Browser.OpenAsync("https://ru.wikipedia.org/wiki/%D0%A8%D0%B8%D1%84%D1%80_%D0%92%D0%B8%D0%B6%D0%B5%D0%BD%D0%B5%D1%80%D0%B0"));
            ShowMessageCommand = new Command(() => Alert.ShortAlert("Приветики ^^"));
            OpenGithubCommand = new Command(async () => await Browser.OpenAsync("https://github.com/burachevsky/NYSS_CourseProject"));
            OpenTelegramCommand = new Command(async () => await Browser.OpenAsync("https://t.me/Burachevsky"));
        }

        public ICommand OpenWikipediaCommand { get; }
        public ICommand ShowMessageCommand { get; }
        public ICommand OpenGithubCommand { get; }
        public ICommand OpenTelegramCommand { get; }
    }
}