// Path: Assets/West/Core/TimeService.cs
// Assembly: West.Core
// Namespace: West.Core
// Purpose: AD §6 fixed-step sim interface. In M1.1 we do NOT advance sim; we only hold state: pause/speed/day/tick.
// Later (M1.2+) a TickSystem will drive this at 30 Hz, catch up to max 4 ticks/frame, publish FixedSimTick.

#nullable enable
using System;

namespace West.Core
{
    /// <summary>
    /// Holds simulation clock state per AD §6: fixed-step 30 Hz with Pause/1x/2x/3x/5x/10x (dev).
    /// M1.1: storage and setters only; tick advancement happens in a dedicated TickSystem later.
    /// </summary>
    public sealed class TimeService
    {
        /// <summary>Current in-world day (integer, 0-based). M1.1: not advanced.</summary>
        public int Day { get; private set; }

        /// <summary>Current tick within the day. M1.1: not advanced.</summary>
        public int TickOfDay { get; private set; }

        /// <summary>Current speed multiplier index (0=Paused,1=1x,2=2x,3=3x,4=5x,5=10x dev).</summary>
        public int SpeedIndex { get; private set; } = 1;

        /// <summary>Speed multipliers as per AD defaults.</summary>
        public static readonly int[] SpeedMultipliers = { 0, 1, 2, 3, 5, 10 };

        /// <summary>True if simulation is paused.</summary>
        public bool Paused => SpeedIndex == 0;

        /// <summary>Sets the current speed by AD slot (0|1|2|3|4|5). Values out of range clamp.</summary>
        public void SetSpeedIndex(int index)
        {
            if (index < 0) index = 0;
            if (index >= SpeedMultipliers.Length) index = SpeedMultipliers.Length - 1;
            SpeedIndex = index;
        }

        /// <summary>Jumps the clock (dev/console helper). No side effects in M1.1.</summary>
        public void SetTime(int day, int tickOfDay)
        {
            Day = Math.Max(0, day);
            TickOfDay = Math.Max(0, tickOfDay);
        }
    }
}
