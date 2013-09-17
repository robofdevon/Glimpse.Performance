// -----------------------------------------------------------------------
// <copyright file="Stopwatch.cs" company="Rob Walker">
//     Copyright (c) Rob Walker. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Glimpse.Performance.Time
{
    public class Stopwatch : IStopwatch
    {
        private readonly System.Diagnostics.Stopwatch _stopWatch;

        public Stopwatch()
        {
            _stopWatch = new System.Diagnostics.Stopwatch();
        }

        public long ElapsedMilliseconds
        {
            get { return _stopWatch.ElapsedMilliseconds; }
        }

        public void Start()
        {
            _stopWatch.Start();
        }
    }
}
