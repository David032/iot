// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Device.I2c;

namespace Iot.Device.GrowHat
{
    /// <summary>
    /// GrowHAT Mini
    /// </summary>
    public class GrowHAT : IDisposable
    {
        /// <summary>
        /// Default I2C bus id
        /// </summary>
        public const int DefaultI2cBusId = 1;

        private I2cBus? _i2cBus;
        private bool _shouldDispose;

        #region Soil Sensors

        /// <summary>
        /// Sensor labeled S1 on the HAT
        /// </summary>
        public GrowHatSoilSensor? SoilSensor1 { get; private set; }

        /// <summary>
        /// Sensor labeled S2 on the HAT
        /// </summary>
        public GrowHatSoilSensor? SoilSensor2 { get; private set; }

        /// <summary>
        /// Sensor labeled S3 on the HAT
        /// </summary>
        public GrowHatSoilSensor? SoilSensor3 { get; private set; }
        #endregion

        #region LV Devices

        /// <summary>
        /// Device labeled P1 on the HAT
        /// </summary>
        public GrowHatDevice? Device1 { get; private set; }

        /// <summary>
        /// Device labeled P2 on the HAT
        /// </summary>
        public GrowHatDevice? Device2 { get; private set; }

        /// <summary>
        /// Device labeled P3 on the HAT
        /// </summary>
        public GrowHatDevice? Device3 { get; private set; }

        #endregion

        #region Onboard Devices

        /// <summary>
        /// The onboard light and proximity sensor
        /// </summary>
        public GrowHatLightSensor? LightSensor { get; private set; }

        /// <summary>
        /// The onboard buzzer
        /// </summary>
        public GrowHatBuzzer? Buzzer { get; private set; }

        // Todo buttons and screen
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="GrowHAT"/> class.
        /// </summary>
        /// <param name="i2cBus">?</param>
        /// <param name="shouldDispose">?</param>
        /// <param name="onboardDevices">Should the onboard light and buzzer be enabled?</param>
        /// <param name="s1">Soil Sensor 1</param>
        /// <param name="s2">Soil Sensor 2</param>
        /// <param name="s3">Soil Sensor 3</param>
        /// <param name="d1">LV device 1</param>
        /// <param name="d2">LV device 1</param>
        /// <param name="d3">LV device 1</param>
        public GrowHAT(I2cBus? i2cBus = null, bool shouldDispose = false, bool onboardDevices = false,
                        GrowHatSoilSensor? s1 = null, GrowHatSoilSensor? s2 = null, GrowHatSoilSensor? s3 = null,
                        GrowHatDevice? d1 = null, GrowHatDevice? d2 = null, GrowHatDevice? d3 = null)
        {
            _shouldDispose = shouldDispose || i2cBus == null;
            _i2cBus = i2cBus ?? I2cBus.Create(DefaultI2cBusId);
            if (onboardDevices)
            {
                LightSensor = new();
                Buzzer = new();
            }

            SoilSensor1 = s1;
            SoilSensor2 = s2;
            SoilSensor3 = s3;
            Device1 = d1;
            Device2 = d2;
            Device3 = d3;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (_shouldDispose)
            {
                _i2cBus?.Dispose();
            }
            else
            {
                LightSensor?.Dispose();
                LightSensor = null!;
                Buzzer?.Dispose();
                Buzzer = null!;

                SoilSensor1?.Dispose();
                SoilSensor1 = null!;
                SoilSensor2?.Dispose();
                SoilSensor2 = null!;
                SoilSensor3?.Dispose();
                SoilSensor3 = null!;

                Device1?.Dispose();
                Device1 = null!;
                Device2?.Dispose();
                Device2 = null!;
                Device3?.Dispose();
                Device3 = null!;
            }

            _i2cBus = null!;
        }
    }
}
