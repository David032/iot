// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Device.Gpio;
using Xunit;

namespace Iot.Device.GrowHat.Tests
{
    public class BasicTests
    {
        [Fact]
        public void CanCreateGrowHat()
        {
            var hat = new GrowHAT();
            Assert.NotNull(hat);
        }

        [Fact]
        public void CanCreateSimpleController()
        {
            var gpioController = new GpioController();
            var soilSensor = new GrowHatSoilSensor(pin: SoilSensorPin.S1, gpioController);
            var hat = new GrowHAT(s1: soilSensor);
            Assert.NotNull(hat);
            Assert.NotNull(hat.SoilSensor1);
        }

        [Fact]
        public void CanCreateModerateController()
        {
            var gpioController = new GpioController();
            var soilSensor = new GrowHatSoilSensor(pin: SoilSensorPin.S1, gpioController);
            var waterPump = new GrowHatDevice();
            var hat = new GrowHAT(s1: soilSensor, d1: waterPump);

            Assert.NotNull(hat);
            Assert.NotNull(hat.SoilSensor1);
            Assert.NotNull(hat.Device1);
        }

        [Fact]
        public void CanCreateFullController()
        {
            var gpioController = new GpioController();
            var soilSensor1 = new GrowHatSoilSensor(pin: SoilSensorPin.S1, gpioController);
            var waterPump1 = new GrowHatDevice();
            var soilSensor2 = new GrowHatSoilSensor(pin: SoilSensorPin.S2, gpioController);
            var waterPump2 = new GrowHatDevice();
            var soilSensor3 = new GrowHatSoilSensor(pin: SoilSensorPin.S3, gpioController);
            var waterPump3 = new GrowHatDevice();
            var hat = new GrowHAT(onboardDevices: true,
                s1: soilSensor1, s2: soilSensor2, s3: soilSensor3,
                d1: waterPump1, d2: waterPump2, d3: waterPump3);

            Assert.NotNull(hat);
            Assert.NotNull(hat.Buzzer);
            Assert.NotNull(hat.LightSensor);

            foreach (GrowHatSoilSensor item in hat.GetSoilSensors())
            {
                Assert.NotNull(item);
            }

            foreach (GrowHatDevice item in hat.GetDevices())
            {
                Assert.NotNull(item);
            }

        }
    }
}
