using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CrypterMobile.Services
{
    public static class Alert
    {
        public static void LongAlert(string message) =>
            DependencyService.Get<IMessage>().LongAlert(message);

        public static void ShortAlert(string message) =>
            DependencyService.Get<IMessage>().ShortAlert(message);
    }
}
