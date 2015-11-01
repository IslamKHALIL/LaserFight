using LaserFight.Helpers;
using Microsoft.Azure.Devices.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Devices.Gpio;
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

namespace LaserFight.PlayerDevice
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private LifePointsManager lifePoints;
        private AzureIoTHubHelper azureIoTHelper = new AzureIoTHubHelper("<deviceId>");
        string dashboardId = "<Dashboard device Id>";

        public MainPage()
        {
            this.InitializeComponent();

            // Start listening to coming messages
            azureIoTHelper.Message_Received += StartGame;
        }

        private void StartGame(string playerId)
        {
            // Start the game
            lifePoints = LifePointsManager.GetDefault(
                playerId: playerId,
                sensorPinNumber: 18,
                LedPinNumbers: new[] { 5, 6, 13, 26 }
                );

            lifePoints.Lost += Player_Lost;
        }

        private async void Player_Lost(string playerId)
        {
            // Stop the game for the user
            lifePoints.IsActive = false;

            // Send user's score to the dashboard application
            azureIoTHelper.Send(dashboardId, playerId);
        }
    }
}
