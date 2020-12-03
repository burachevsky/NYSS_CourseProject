using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrypterMobile.Droid.Services;

[assembly: Xamarin.Forms.Dependency(typeof(MessageAndroid))]
namespace CrypterMobile.Droid.Services
{
    public class MessageAndroid : CrypterMobile.Services.IMessage
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long)?.Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short)?.Show();
        }
    }
}