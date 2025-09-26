// Path: Assets/West/Runtime/Hud/HudPresenter.cs
// Assembly: West.Runtime
// Namespace: West.Runtime.Hud
// Summary: Minimal HUD binder for M1.2. Presents Pause/1x/2x/3x/5x speed buttons and a clock label.

#nullable enable
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using West.Core;
using West.Core.Time;

namespace West.Runtime.Hud
{
    /// <summary>Simple HUD controls to drive TimeService and display the clock.</summary>
    public sealed class HudPresenter : MonoBehaviour
    {
        [Header("Bindings")]
        [SerializeField] private Button _btnPause;
        [SerializeField] private Button _btn1x;
        [SerializeField] private Button _btn2x;
        [SerializeField] private Button _btn3x;
        [SerializeField] private Button _btn5x;
        [SerializeField] private TMP_Text _clockLabel;

        private EventBus _bus;
        private TimeService _time;

        private void Awake()
        {
            _bus = ServiceRegistry.Get<EventBus>();
            _time = ServiceRegistry.Get<TimeService>();

            _btnPause.onClick.AddListener(() => SetSpeed(0));
            _btn1x.onClick.AddListener(() => SetSpeed(1));
            _btn2x.onClick.AddListener(() => SetSpeed(2));
            _btn3x.onClick.AddListener(() => SetSpeed(3));
            _btn5x.onClick.AddListener(() => SetSpeed(4)); // index 4 maps to 5x in default config

            // Start label
            RefreshClock();
            // Update clock on LateSimTick
            _bus.Subscribe<LateSimTickEvent>(_ => RefreshClock());
        }

        private void OnDestroy()
        {
            if (_bus != null)
            {
                _bus.Unsubscribe<LateSimTickEvent>(_ => RefreshClock()); // harmless if not subscribed
            }
        }

        private void SetSpeed(int idx)
        {
            _time.SetSpeedIndex(idx);
            _bus.Publish(new GameSpeedChangedEvent(idx));
        }

        private void RefreshClock()
        {
            if (_clockLabel != null)
                _clockLabel.text = _time.GetClockString();
        }
    }
}
