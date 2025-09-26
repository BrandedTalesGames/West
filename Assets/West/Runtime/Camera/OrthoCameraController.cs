// Path: Assets/West/Runtime/Camera/OrthoCameraController.cs
// Assembly: West.Runtime
// Namespace: West.Runtime.Cameras
// Summary: Ortho cam pan/zoom supporting both Legacy Input and New Input System.
// - Pan: Hold selected mouse button (default Right) OR WASD
// - Zoom: Mouse wheel OR Q/E keys
// - Optional clamp to IGridService bounds.

#nullable enable
using UnityEngine;
using West.Core;
using West.Core.Grid;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem; // New Input System
#endif

namespace West.Runtime.Cameras
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public sealed class OrthoCameraController : MonoBehaviour
    {
        public enum PanButton { Left = 0, Right = 1, Middle = 2 }

        [Header("Pan")]
        [SerializeField] private PanButton panMouseButton = PanButton.Right;
        [SerializeField] private float panSpeed = 10f;      // world units per mouse-delta unit
        [SerializeField] private float keyPanSpeed = 10f;   // world units/sec (WASD)

        [Header("Zoom")]
        [SerializeField] private float zoomStep = 1.0f;     // orthographicSize step per scroll "tick"
        [SerializeField] private float minOrthoSize = 2f;
        [SerializeField] private float maxOrthoSize = 20f;
        [SerializeField] private float keyZoomPerSec = 6f;  // zoom speed with Q/E

        [Header("Bounds (cells)")]
        [SerializeField] private bool clampToGrid = true;
        [SerializeField] private float paddingWorld = 1f;

        private UnityEngine.Camera _cam = null!;
        private IGridService? _grid;

        private int MouseButtonIndex => (int)panMouseButton;

        private void Awake()
        {
            _cam = GetComponent<UnityEngine.Camera>();
            _cam.orthographic = true;

            // Grid is optional for panning/zooming—no hard crash if not present yet
            _grid = ServiceRegistry.TryGet<IGridService>();
        }

        private void Update()
        {
            float dt = UnityEngine.Time.unscaledDeltaTime;

            // --- Scroll Wheel Zoom ---
            float scroll = ReadScroll();
            if (Mathf.Abs(scroll) > 0.001f)
            {
                // New Input System often reports larger values; scale down a bit
                float scaled = scroll * zoomStep * 0.1f;
                _cam.orthographicSize = Mathf.Clamp(_cam.orthographicSize - scaled, minOrthoSize, maxOrthoSize);
            }

            // --- Keyboard Zoom (Q/E) ---
            float keyZoom = ReadKeyZoom();
            if (Mathf.Abs(keyZoom) > 0.001f)
            {
                _cam.orthographicSize = Mathf.Clamp(_cam.orthographicSize - keyZoom * keyZoomPerSec * dt, minOrthoSize, maxOrthoSize);
            }

            // --- Mouse Pan (hold button) ---
            Vector2 mouseDelta = ReadMouseDeltaHeld(MouseButtonIndex);
            if (mouseDelta.sqrMagnitude > 0.000001f)
            {
                // Move opposite of drag to "grab" the world
                var move = new Vector3(-mouseDelta.x, -mouseDelta.y, 0f) * panSpeed * dt;
                transform.position += move;
            }

            // --- Keyboard Pan (WASD) ---
            Vector2 keyPan = ReadKeyPan();
            if (keyPan.sqrMagnitude > 0.000001f)
            {
                var move = new Vector3(keyPan.x, keyPan.y, 0f) * keyPanSpeed * dt;
                transform.position += move;
            }

            if (clampToGrid && _grid != null)
                ClampWithinGrid(_grid);
        }

        private void ClampWithinGrid(IGridService grid)
        {
            var dims = grid.Dimensions;
            if (dims.width == 0 || dims.height == 0) return; // unbounded

            float halfH = _cam.orthographicSize;
            float halfW = halfH * _cam.aspect;

            float worldW = dims.width * grid.CellSize;
            float worldH = dims.height * grid.CellSize;

            float minX = halfW - paddingWorld;
            float maxX = worldW - halfW + paddingWorld;
            float minY = halfH - paddingWorld;
            float maxY = worldH - halfH + paddingWorld;

            var p = transform.position;
            p.x = Mathf.Clamp(p.x, minX, Mathf.Max(minX, maxX));
            p.y = Mathf.Clamp(p.y, minY, Mathf.Max(minY, maxY));
            transform.position = p;
        }

        // -------- Input helpers (dual backend) --------

        private static float ReadScroll()
        {
#if ENABLE_INPUT_SYSTEM
            if (Mouse.current != null)
            {
                // Typically returns pixels per-frame; positive when scrolling up.
                return Mouse.current.scroll.ReadValue().y;
            }
            return 0f;
#else
            return Input.mouseScrollDelta.y;
#endif
        }

        private static Vector2 ReadMouseDeltaHeld(int buttonIndex)
        {
#if ENABLE_INPUT_SYSTEM
            var m = Mouse.current;
            if (m == null) return Vector2.zero;

            bool held = buttonIndex switch
            {
                0 => m.leftButton.isPressed,
                1 => m.rightButton.isPressed,
                2 => m.middleButton.isPressed,
                _ => false
            };
            return held ? m.delta.ReadValue() : Vector2.zero;
#else
            return Input.GetMouseButton(buttonIndex)
                ? new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"))
                : Vector2.zero;
#endif
        }

        private static Vector2 ReadKeyPan()
        {
#if ENABLE_INPUT_SYSTEM
            var k = Keyboard.current;
            if (k == null) return Vector2.zero;
            float x = (k.aKey.isPressed ? -1f : 0f) + (k.dKey.isPressed ? 1f : 0f);
            float y = (k.sKey.isPressed ? -1f : 0f) + (k.wKey.isPressed ? 1f : 0f);
            return new Vector2(x, y);
#else
            float x = (Input.GetKey(KeyCode.A) ? -1f : 0f) + (Input.GetKey(KeyCode.D) ? 1f : 0f);
            float y = (Input.GetKey(KeyCode.S) ? -1f : 0f) + (Input.GetKey(KeyCode.W) ? 1f : 0f);
            return new Vector2(x, y);
#endif
        }

        private static float ReadKeyZoom()
        {
#if ENABLE_INPUT_SYSTEM
            var k = Keyboard.current;
            if (k == null) return 0f;
            // E = zoom in, Q = zoom out
            return (k.eKey.isPressed ? 1f : 0f) + (k.qKey.isPressed ? -1f : 0f);
#else
            return (Input.GetKey(KeyCode.E) ? 1f : 0f) + (Input.GetKey(KeyCode.Q) ? -1f : 0f);
#endif
        }
    }
}
