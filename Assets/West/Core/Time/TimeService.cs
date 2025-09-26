// Path: Assets/West/Core/Time/TimeService.cs
// Assembly: West.Core
// Namespace: West.Core.Time

#nullable enable
using System;

namespace West.Core.Time
{
    /// <summary>Simulation clock state per fixed-tick model.</summary>
    public sealed class TimeService
    {
        private readonly int[] _speedMultipliers;
        private readonly int _ticksPerDay;

        public TimeService(int[] speedMultipliers, int ticksPerDay)
        {
            _speedMultipliers = speedMultipliers;
            _ticksPerDay = Math.Max(1, ticksPerDay);
        }

        public long TotalTicks { get; private set; }
        public int Day { get; private set; }
        public int TickOfDay { get; private set; }
        public int SpeedIndex { get; private set; } = 1;

        public int SpeedMultiplier => (_speedMultipliers.Length == 0)
            ? 0
            : _speedMultipliers[Math.Clamp(SpeedIndex, 0, _speedMultipliers.Length - 1)];

        public void SetSpeedIndex(int index)
        {
            SpeedIndex = Math.Clamp(index, 0, Math.Max(0, _speedMultipliers.Length - 1));
        }

        public void AdvanceOneTick()
        {
            TotalTicks++;
            TickOfDay++;
            if (TickOfDay >= _ticksPerDay)
            {
                TickOfDay = 0;
                Day++;
            }
        }

        public void GetClockHhMm(out int hours, out int minutes)
        {
            var ticksPerHour = _ticksPerDay / 24f;
            var h = (int)(TickOfDay / ticksPerHour);
            var m = (int)((TickOfDay % ticksPerHour) * (60f / ticksPerHour));
            hours = Math.Clamp(h, 0, 23);
            minutes = Math.Clamp(m, 0, 59);
        }

        public string GetClockString()
        {
            GetClockHhMm(out var h, out var m);
            return $"{h:00}:{m:00}";
        }
    }
}
