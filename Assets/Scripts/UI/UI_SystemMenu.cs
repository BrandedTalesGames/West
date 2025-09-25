using UnityEngine;
using UnityEngine.UI;
using West;

namespace West.UI
{
    /// <summary>
    /// Minimal UI stub that calls GameDirector actions (never touches Runtime directly).
    /// Wire buttons via Inspector.
    /// </summary>
    public class UI_SystemMenu : MonoBehaviour
    {
        public GameDirector Director;
        public Button PauseButton;
        public Button SaveButton;

        void Start()
        {
            if (PauseButton) PauseButton.onClick.AddListener(() => Director?.TogglePause());
            if (SaveButton)  SaveButton.onClick.AddListener(() => Director?.SaveNow());
        }
    }
}
