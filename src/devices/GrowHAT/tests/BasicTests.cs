// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;

namespace Iot.Device.GrowHat.Tests
{
    /// <summary>
    /// Tests for the GrowHat itself
    /// </summary>
    public class GrowHatTests
    {
        /// <summary>
        /// Can the growHat be created?
        /// </summary>
        [Fact]
        public void CanCreateGrowHat()
        {
            var growHat = new GrowHAT();
            Assert.NotNull(growHat);
        }
    }
}
