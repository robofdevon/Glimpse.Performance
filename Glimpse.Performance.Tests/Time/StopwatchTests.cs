// -----------------------------------------------------------------------
// <copyright file="StopwatchTests.cs" company="Rob Walker">
//     Copyright (c) Rob Walker. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Glimpse.Performance.Time
{
    using System.Threading;
    using NUnit.Framework;

    [TestFixture]
    public class StopwatchTests
    {
        [Test]
        public void Stopwatch_CountsElapsedTime_TimeIncreases()
        {
            //Arrange
            var stopwatch = new Stopwatch();

            //Act
            stopwatch.Start();
            var firstCheckpoint = stopwatch.ElapsedMilliseconds;
            Thread.Sleep(1);
            var secondCheckpoint = stopwatch.ElapsedMilliseconds;

            //Assert
            Assert.Greater(secondCheckpoint, firstCheckpoint);
        }
    }
}
