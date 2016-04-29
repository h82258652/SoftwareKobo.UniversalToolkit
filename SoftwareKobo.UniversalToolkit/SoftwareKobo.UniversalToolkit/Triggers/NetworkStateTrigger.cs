using System;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;

namespace SoftwareKobo.UniversalToolkit.Triggers
{
    /// <summary>
    /// 网络状态触发器。
    /// </summary>
    public class NetworkStateTrigger : StateTriggerBase
    {
        public static readonly DependencyProperty NetworkStateProperty = DependencyProperty.Register(nameof(NetworkState), typeof(NetworkState), typeof(NetworkStateTrigger), new PropertyMetadata(NetworkState.Unavaliable, NetworkStateChanged));

        public NetworkStateTrigger()
        {
            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
        }

        public NetworkState NetworkState
        {
            get
            {
                return (NetworkState)GetValue(NetworkStateProperty);
            }
            set
            {
                SetValue(NetworkStateProperty, value);
            }
        }

        private static void NetworkStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (NetworkStateTrigger)d;

            obj.UpdateState();
        }

        private async void NetworkInformation_NetworkStatusChanged(object sender)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, UpdateState);
        }

        private void UpdateState()
        {
            var isAvailable = false;
            var profile = NetworkInformation.GetInternetConnectionProfile();
            if (profile != null)
            {
                isAvailable = profile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            }

            if (isAvailable)
            {
                SetActive(NetworkState == NetworkState.Avaliable);
            }
            else
            {
                SetActive(NetworkState == NetworkState.Unavaliable);
            }
        }
    }
}