// -----------------------------------------------------------------------
// <copyright file="IStopwatch.cs" company="Rob Walker">
//     Copyright (c) Rob Walker. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Glimpse.Performance.Time
{
    public interface IStopwatch
    {
        long ElapsedMilliseconds { get; }
        void Start();
    }
}
