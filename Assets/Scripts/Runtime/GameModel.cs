using System;
using System.Collections.Generic;
using UnityEngine;

namespace West.Runtime
{
    /// <summary>
    /// Root serializable runtime state that ES3 can persist.
    /// Keep fields simple/serializable. 
    /// </summary>
    [Serializable]
    public class GameModel
    {
        public TimeState Time = new TimeState();
        public WorldState World = new WorldState();
        public List<CharacterState> Characters = new List<CharacterState>();
    }

    [Serializable]
    public class TimeState
    {
        /// <summary>Total minutes since game start.</summary>
        public int MinutesTotal;

        /// <summary>Computed day index (MinutesTotal / 1440).</summary>
        public int Day => MinutesTotal / 1440;

        /// <summary>Minutes within the current day (0..1439).</summary>
        public int MinutesOfDay => MinutesTotal % 1440;
    }

    [Serializable]
    public class WorldState
    {
        /// <summary>Example seed or map id for world generation.</summary>
        public int Seed = 12345;
        /// <summary>Example: current weather key ("Clear", "Rain").</summary>
        public string WeatherKey = "Clear";
    }

    [Serializable]
    public class CharacterState
    {
        /// <summary>Designer-visible id or generated guid.</summary>
        public string Id;

        /// <summary>Display name.</summary>
        public string Name;

        /// <summary>2D world position (X,Y in Unity units).</summary>
        public Vector2 Position;

        /// <summary>Target position for movement (optional).</summary>
        public Vector2 TargetPosition;

        /// <summary>0..1 fatigue placeholder.</summary>
        public float Fatigue;
    }
}
