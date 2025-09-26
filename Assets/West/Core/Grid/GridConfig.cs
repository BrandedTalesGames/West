// Path: Assets/West/Core/Grid/GridConfig.cs
// Assembly: West.Core
// Namespace: West.Core.Grid
// Summary: Authoring ScriptableObject for grid defaults; ingested to a runtime DTO. Never mutate SOs at runtime.

#nullable enable
using UnityEngine;

namespace West.Core.Grid
{
    [CreateAssetMenu(fileName = "GridConfig", menuName = "West/Configs/Grid Config")]
    public sealed class GridConfig : ScriptableObject
    {
        [Header("Scale")]
        [Tooltip("Pixels Per Unit; art→world scale. Default 32 for this project.")]
        [Min(1)] public int pixelsPerUnit = 32;

        [Tooltip("World size of one grid cell = 1f * (PPU/32) by default; override for non-1:1.")]
        [Min(0.01f)] public float cellSizeWorld = 1f;

        [Header("World Bounds (cells)")]
        [Tooltip("Grid width in cells (0 = unbounded).")]
        [Min(0)] public int width;
        [Tooltip("Grid height in cells (0 = unbounded).")]
        [Min(0)] public int height;
    }

    /// <summary>Runtime copy (plain DTO) used by GridService.</summary>
    public sealed class GridConfigRuntime
    {
        public readonly int PixelsPerUnit;
        public readonly float CellSizeWorld;
        public readonly GridSize Dimensions;

        public GridConfigRuntime(GridConfig src)
        {
            PixelsPerUnit = Mathf.Max(1, src.pixelsPerUnit);
            CellSizeWorld = Mathf.Max(0.01f, src.cellSizeWorld);
            Dimensions = new GridSize(src.width, src.height);
        }
    }
}
