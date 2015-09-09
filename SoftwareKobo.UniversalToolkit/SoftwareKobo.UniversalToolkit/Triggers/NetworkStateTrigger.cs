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
                return (NetworkState)this.GetValue(NetworkStateProperty);
            }
            set
            {
                this.SetValue(NetworkStateProperty, value);
            }
        }

        private static void NetworkStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NetworkStateTrigger obj = (NetworkStateTrigger)d;

            obj.UpdateState();
        }

        private async void NetworkInformation_NetworkStatusChanged(object sender)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, UpdateState);
        }

        private void UpdateState()
        {
            bool isAvailable = false;
            var profile = NetworkInformation.GetInternetConnectionProfile();
            if (profile != null)
            {
                isAvailable = profile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            }

            if (isAvailable)
            {
                this.SetActive(this.NetworkState == NetworkState.Avaliable);
            }
            else
            {
                this.SetActive(this.NetworkState == NetworkState.Unavaliable);
            }
        }
    }
}