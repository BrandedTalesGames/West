using UnityEngine;
using West.Data;
using West.Runtime;
using West.Systems;

namespace West
{
    /// <summary>
    /// MonoBehaviour that wires Systems + Runtime + Config and drives the tick loop.
    /// Place one in a bootstrap scene, assign GlobalConfig, and hit Play.
    /// </summary>
    public class GameDirector : MonoBehaviour
    {
        [Header("Config & Persistence")]
        public GlobalConfig Config;

        [Header("Runtime (ES3-saved)")]
        [TextArea] public string EasySaveKey = "GameModel_v1";
        public GameModel Model = new GameModel();

        private float _tickAccumulator;
        private float _tickInterval;

        void Awake()
        {
            if (Config == null)
            {
                Debug.LogError("[GameDirector] GlobalConfig is missing.");
                enabled = false; return;
            }

            // Hook up debug
            DebugLog.Init(Config);

            // Tick interval from TPS
            _tickInterval = 1f / Mathf.Max(1, Config.TargetTicksPerSecond);

            // Load or initialize model
            if (ES3.KeyExists(EasySaveKey))
            {
                Model = ES3.Load(EasySaveKey, Model);
                DebugLog.Log(DebugChannel.Time, $"Loaded Model from ES3 key '{EasySaveKey}'.");
            }
            else
            {
                // Seed new model
                Model.Characters.Clear();
                Model.Characters.Add(new CharacterState { Id = "c_1", Name = "Ada", Position = new Vector2(0, 0), TargetPosition = new Vector2(4, 0) });
                Model.Characters.Add(new CharacterState { Id = "c_2", Name = "Noah", Position = new Vector2(-2, -1), TargetPosition = new Vector2(-2, 3) });

                TimeSystem.Initialize(Model, Config);
                ES3.Save(EasySaveKey, Model);
                DebugLog.Log(DebugChannel.Time, $"Initialized new Model -> ES3 '{EasySaveKey}'.");
            }
        }

        void Update()
        {
            if (Config.IsPaused) return;

            // Per-frame systems (real-time dependent)
            MovementSystem.Update(Model, Config, Time.deltaTime);

            // Fixed-rate tick systems (minute-based)
            _tickAccumulator += Time.deltaTime;
            while (_tickAccumulator >= _tickInterval)
            {
                _tickAccumulator -= _tickInterval;
                TimeSystem.Tick(Model, Config);
            }

            // Persist occasionally; here we do a very light auto-save (okay for prototype).
            // For production, do it on checkpoints/intervals/menu actions.
            if (Application.isEditor) ES3.Save(EasySaveKey, Model);
        }

        [ContextMenu("Save Now")]
        public void SaveNow()
        {
            ES3.Save(EasySaveKey, Model);
            DebugLog.Warn($"Manual save -> ES3 '{EasySaveKey}'.");
        }

        [ContextMenu("Toggle Pause")]
        public void TogglePause()
        {
            Config.IsPaused = !Config.IsPaused;
            DebugLog.Warn($"Pause={Config.IsPaused}");
        }
    }
}
