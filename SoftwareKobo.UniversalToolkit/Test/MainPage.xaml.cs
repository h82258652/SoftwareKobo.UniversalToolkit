﻿using SoftwareKobo.UniversalToolkit.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Test
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            GV.ItemsSource = Enumerable.Range(0, 200).ToList();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async void BtnNewWindow_Click(object sender, RoutedEventArgs e)
        {
            await App.Current.ShowNewWindowAsync<MainPage>();
        }

        private async void BtnNewWindowWithParameter_Click(object sender, RoutedEventArgs e)
        {
            await App.Current.ShowNewWindowAsync<MainPage>("带参数打开新窗口");
        }

        private async void ProtocolLaunch_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("softwarekobo:fromprotocol"));
        }

        private async void ProtocolLaunchWithoutExtendedSplashScreen_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("softwarekobo:nosplash"));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GV.ScrollIntoViewSmoothly(GV.Items[new Random().Next(GV.Items.Count)]);

            //if (SoftwareKobo.UniversalToolkit.Storage.ApplicationLocalSettings.Exists("Test"))
            //{
            //    var dt = SoftwareKobo.UniversalToolkit.Storage.ApplicationLocalSettings.Read<DateTime>("Test");
            //    await new MessageDialog(dt.ToString()).ShowAsync();
            //}
            //else
            //{
            //    await new MessageDialog("no").ShowAsync();
            //}
        }
    }
}
