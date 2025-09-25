using UnityEngine;
using Sirenix.OdinInspector;

namespace West.Data
{
    /// <summary>
    /// Example job definition (expand later: skill reqs, work rate, tool tags, etc.).
    /// </summary>
    [CreateAssetMenu(menuName = "West/Data/JobDef", fileName = "JobDef")]
    public class JobDef : ScriptableObject
    {
        [LabelText("ID (unique key)")]
        public string Id = "ChopWood";

        [LabelText("Display Name")]
        public string DisplayName = "Chop Wood";

        [LabelText("Base Duration (sec)")]
        [MinValue(0)]
        public float BaseDuration = 5f;
    }
}
