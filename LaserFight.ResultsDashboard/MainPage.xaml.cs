using LaserFight.Helpers;
using LaserFight.ResultsDashboard.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LaserFight.ResultsDashboard
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<Player> Players { get; set; } = new ObservableCollection<Player>();
        AzureIoTHubHelper iotHelper;

        // Assuming that the playerId = deviceId 
        // Since each player has only one device
        // This device Id must be registered first with the AzureIoTHubRegister
        // Each device has unique device Id. if you have more than one device then this should be string[] of devices
        private string deviceId = "<deviceId>";


        public MainPage()
        {
            this.InitializeComponent();
            players.ItemsSource = Players;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).Visibility = Visibility.Collapsed;
            AddPlayerForm.Visibility = Visibility.Visible;
        }

        private void AddPlayer_Click(object sender, RoutedEventArgs e)
        {
            AddPlayerButton.Visibility = Visibility.Visible;
            AddPlayerForm.Visibility = Visibility.Collapsed;
            Players.Add(new Player()
            {
                Id = deviceId,
                Background = "#55007ACC",
                Name = PlayerName.Text,
                Score = 5
            });

            iotHelper = new AzureIoTHubHelper(deviceId);
            // Here the message is the device Id itself as example
            iotHelper.Send(deviceId, deviceId);
            iotHelper.Message_Received += PlayerLost;
        }

        private void PlayerLost(string playerId)
        {
            var player = Players.FirstOrDefault(e => e.Id == playerId);
            if (player != null)
            {
                player.Score = 0;
                player.Background = "#55FF0000";
            }
        }
    }
}
