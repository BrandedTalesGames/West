using West.Data;
using West.Runtime;

namespace West.Systems
{
    /// <summary>
    /// Advances in-game time in minute chunks per tick.
    /// All time-related helpers live here.
    /// </summary>
    public static class TimeSystem
    {
        /// <summary>
        /// Initialize the model's time based on GlobalConfig.
        /// </summary>
        public static void Initialize(GameModel model, GlobalConfig cfg)
        {
            model.Time.MinutesTotal = cfg.StartDay * 1440;
            DebugLog.Log(DebugChannel.Time, $"Initialize -> StartDay={cfg.StartDay}, MinutesTotal={model.Time.MinutesTotal}");
        }

        /// <summary>
        /// Advance time by cfg.MinutesPerTick. Called by the GameDirector tick loop.
        /// </summary>
        public static void Tick(GameModel model, GlobalConfig cfg)
        {
            model.Time.MinutesTotal += cfg.MinutesPerTick;
            DebugLog.Log(DebugChannel.Time, $"Tick -> +{cfg.MinutesPerTick}m, Total={model.Time.MinutesTotal}, Day={model.Time.Day}, MinutesOfDay={model.Time.MinutesOfDay}");
        }

        /// <summary>
        /// Utility: returns a formatted clock string HH:MM from MinutesOfDay.
        /// </summary>
        public static string GetClockHHMM(GameModel model)
        {
            int m = model.Time.MinutesOfDay;
            int hh = m / 60;
            int mm = m % 60;
            return $"{hh:00}:{mm:00}";
        }
    }
}
