// Path: Assets/West/Tools/Editor/Grid/GridGizmo.cs
// Assembly: West.Tools.Editor
// Namespace: West.Tools.Editor.Grid
// Summary: Lightweight scene view overlay to visualize the grid in the editor.

#if UNITY_EDITOR
#nullable enable
using UnityEditor;
using UnityEngine;
using West.Core;
using West.Core.Grid;

namespace West.Tools.Editor.Grid
{
    [InitializeOnLoad]
    public static class GridGizmo
    {
        static GridGizmo()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private static void OnSceneGUI(SceneView sv)
        {
            var grid = ServiceRegistry.TryGet<IGridService>();
            if (grid == null) return;

            Handles.color = new Color(1f, 1f, 1f, 0.08f);
            int w = grid.Dimensions.width <= 0 ? 100 : grid.Dimensions.width;
            int h = grid.Dimensions.height <= 0 ? 100 : grid.Dimensions.height;

            float cs = grid.CellSize;
            // Vertical lines
            for (int x = 0; x <= w; x++)
            {
                var a = new Vector3(x * cs, 0, 0);
                var b = new Vector3(x * cs, h * cs, 0);
                Handles.DrawLine(a, b);
            }
            // Horizontal lines
            for (int y = 0; y <= h; y++)
            {
                var a = new Vector3(0, y * cs, 0);
                var b = new Vector3(w * cs, y * cs, 0);
                Handles.DrawLine(a, b);
            }
        }
    }
}
#endif
