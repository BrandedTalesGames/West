using UnityEngine;
using Sirenix.OdinInspector;

namespace West.Data
{
    /// <summary>
    /// Example weather definition (expand later: temperature, effects, sprites, rates).
    /// </summary>
    [CreateAssetMenu(menuName = "West/Data/WeatherDef", fileName = "WeatherDef")]
    public class WeatherDef : ScriptableObject
    {
        [LabelText("Key (e.g., Clear, Rain)")]
        public string Key = "Clear";

        [LabelText("Move Speed Multiplier")]
        [MinValue(0)]
        public float MoveSpeedMult = 1f;
    }
}
