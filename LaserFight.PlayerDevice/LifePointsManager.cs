using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace LaserFight.PlayerDevice
{
    /// <summary>
    /// This is a singletone class that manages the life points of the player and the hits he gets
    /// </summary>
    public class LifePointsManager
    {
        /// <summary>
        /// The GPIO pin to which is connected the light sensor
        /// </summary>
        private GpioPin _SensorPin { get; set; }

        /// <summary>
        /// List of pins to which is connected the LEDs indicating life points
        /// </summary>
        private List<GpioPin> _LedPins { get; set; } = new List<GpioPin>();

        /// <summary>
        /// Player/Raspberry Pi device identifier
        /// </summary>
        private string _PlayerId { get; set; }

        /// <summary>
        /// Controls the continuity of the game
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Event that fires when all life points are gone indicating that the player has lost the game
        /// </summary>
        public event Action<string> Lost;

        private LifePointsManager(string playerId, int sensorPinNumber, params int[] LedPinNumbers)
        {
            _PlayerId = playerId;

            var controller = GpioController.GetDefault();

            // Initializing LEDs GPIO pins
            foreach (var pinNumber in LedPinNumbers)
            {
                var pin = controller.OpenPin(pinNumber);
                pin.Write(GpioPinValue.High);
                pin.SetDriveMode(GpioPinDriveMode.Output);
                _LedPins.Add(pin);
            }

            // Initializing light sensor GPIO pin
            _SensorPin = controller.OpenPin(sensorPinNumber);
            _SensorPin.SetDriveMode(GpioPinDriveMode.Input);
            _SensorPin.ValueChanged += GotHit;

            IsActive = true;
        }

        /// <summary>
        /// When the pin value changes to rising edge, that means that the player just got hit by his oponents
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void GotHit(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            if (IsActive && args.Edge == GpioPinEdge.RisingEdge)
            {
                // When player gets hit a LED is turned of one at the time
                if (_LedPins.Count > 0)
                {
                    _LedPins[0].Write(GpioPinValue.Low);
                    _LedPins.RemoveAt(0);
                }

                // When all life points are drained, the lost event is fired
                if (_LedPins.Count == 0)
                    Lost?.Invoke(_PlayerId);
            }
        }

        /// <summary>
        /// Initializing method for the singletone class
        /// </summary>
        /// <param name="playerId">Player/Raspberry Pi device identifier</param>
        /// <param name="sensorPinNumber">pin number of the GPIO pin to which the light sensor is connected</param>
        /// <param name="LedPinNumbers">pin numbers to which life points LEDs is connected</param>
        /// <returns></returns>
        public static LifePointsManager GetDefault(string playerId, int sensorPinNumber, params int[] LedPinNumbers)
        {
            return new LifePointsManager(playerId, sensorPinNumber, LedPinNumbers);
        }
    }
}
