// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
    }
}
