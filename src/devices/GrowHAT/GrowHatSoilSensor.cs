using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Gpio;
using System.Device.Pwm;

namespace Iot.Device.GrowHat
{
    public enum SoilSensorPin
    {
        S1 = 23,
        S2 = 8,
        S3 = 25
    }

    /// <summary>
    /// GrowHAT Mini Soil Sensor
    /// </summary>
    public class GrowHatSoilSensor : IDisposable
    {
        private int _pin;
        private int _count = 0;
        private double _reading = 0;
        private List<double> _history = new();
        private int _historyLength = 200;
        private double _lastPulse;
        private bool _newData = false;
        private double _timeLastReading;
        private double _wetPoint = 0.7f;
        private double _dryPoint = 27.6f;
        private GpioController _controller;

        /// <summary>
        /// Construct a new SoilSensor
        /// </summary>
        /// <param name="pin">Which slot is it plugged into</param>
        /// <param name="controller">GPIO controller</param>
        /// <param name="WetPoint">Override the default wet point</param>
        /// <param name="DryPoint">Override the default dry point</param>
        public GrowHatSoilSensor(SoilSensorPin pin, GpioController controller, double WetPoint = 0.7, double DryPoint = 27.6)
        {
            _wetPoint = WetPoint;
            _dryPoint = DryPoint;
            _controller = controller;
            _pin = (int)pin;

            _timeLastReading = DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond;
            _lastPulse = DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond;

            _controller.SetPinMode(_pin, PinMode.Input);
            _controller.RegisterCallbackForPinValueChangedEvent(_pin, PinEventTypes.Rising, EventHandler);
        }

        private void EventHandler(object sender, PinValueChangedEventArgs e)
        {
            _count++;
            _lastPulse = DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond;

            if (GetTimeElapsed() >= 1.0)
            {
                _reading = _count / GetTimeElapsed();
                _history.Insert(0, _reading);
                _history = _history.Take(_historyLength).ToList();
                _count = 0;
                _timeLastReading = DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond;
                _newData = true;
            }
        }

        /// <summary>
        /// Get the range between the wet and dry points
        /// </summary>
        /// <returns>double</returns>
        public double GetRange()
        {
            return _wetPoint - _dryPoint;
        }

        /// <summary>
        /// Get the elapsed time since the last reading
        /// </summary>
        /// <returns>double</returns>
        public double GetTimeElapsed()
        {
            return (DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond) - _timeLastReading;
        }

        /// <summary>
        /// Get the current moisture value
        /// </summary>
        public double MoistureValue
        {
            get
            {
                _newData = false;
                return _reading;
            }
        }

        /// <summary>
        /// Check if the moisture sensor is producing a valid reading.
        /// </summary>
        public bool Active
        {
            get { return (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - _lastPulse < 1000 && _reading > 0 && _reading < 28; }
        }

        /// <summary>
        /// Check for new reading.
        /// Returns True if moisture value has been updated since last reading moisture or saturation.
        /// </summary>
        public bool NewData
        {
            get { return _newData; }
        }

        /// <summary>
        /// Return the range sensor range (wet - dry points).
        /// </summary>
        public double Range
        {
            get { return _wetPoint - _dryPoint; }
        }

        /// <summary>
        /// Return saturation as a double from 0.0 to 1.0.
        /// This value is calculated using the wet and dry points.
        /// </summary>
        public double Saturation
        {
            get
            {
                var saturation = (MoistureValue - _dryPoint) / Range;
                saturation = Math.Round(saturation, 3);
                return Math.Max(0.0, Math.Min(1.0, saturation));
            }
        }

        public List<double> History
        {
            get
            {
                var history = new List<double>();

                foreach (var moisture in _history)
                {
                    var saturation = (moisture - _dryPoint) / Range;
                    saturation = Math.Round(saturation, 3);
                    history.Add(Math.Max(0.0, Math.Min(1.0, saturation)));
                }

                return history;
            }
        }

        /// <summary>
        /// Set the wet point
        /// </summary>
        /// <param name="value">New wet point</param>
        public void SetWetPoint(double value = double.NaN)
        {
            _wetPoint = double.IsNaN(value) ? _reading : value;
        }

        /// <summary>
        /// Set the dry point
        /// </summary>
        /// <param name="value">New dry point</param>
        public void SetDryPoint(double value = double.NaN)
        {
            _dryPoint = double.IsNaN(value) ? _reading : value;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
