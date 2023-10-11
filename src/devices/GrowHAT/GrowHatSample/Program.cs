// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using System.Device.Gpio;
using System.Threading;
using Iot.Device.GrowHat;

namespace GrowHatSample
{
    internal class Program
    {
        private enum Mode
        {
            Simple, // Just a soil sensor
            Advanced, // Soil sensor and onboard devices
            Full // Soil Sensor, onboard devices and lv device(assumed pump)
        }

        private static void Main(string[] args)
        {
            Mode programMode = Mode.Simple;
            switch (args[0])
            {
                case "1":
                    programMode = Mode.Advanced;
                    break;
                case "2":
                    programMode = Mode.Full;
                    break;
                default:
                    programMode = Mode.Simple;
                    break;
            }

            var gpioController = new GpioController();
            GrowHAT growHat = new();
            switch (programMode)
            {
                case Mode.Simple:
                    growHat = new GrowHAT(s1: new GrowHatSoilSensor(SoilSensorPin.S1, gpioController));
                    break;
                case Mode.Advanced:
                    growHat = new GrowHAT(s1: new GrowHatSoilSensor(SoilSensorPin.S1, gpioController), onboardDevices: true);
                    break;
                case Mode.Full:
                    growHat = new GrowHAT(
                        s1: new GrowHatSoilSensor(SoilSensorPin.S1, gpioController),
                        d1: new GrowHatDevice(),
                        onboardDevices: true);
                    break;
            }

            while (true)
            {
                var saturationValue = growHat.SoilSensor1 != null ? growHat.SoilSensor1.Saturation : -1;
                Console.WriteLine(DateTime.Now.ToShortTimeString() + ": Soil sensor 1 saturation reads: " + saturationValue);

                if (saturationValue <= 0.25 && growHat.HatBuzzer != null)
                {
                    growHat.HatBuzzer.PlayTone(200, 1000);
                }

                Thread.Sleep(1000);
            }
        }
    }
}
