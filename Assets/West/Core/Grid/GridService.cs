// Path: Assets/West/Core/Grid/GridService.cs
// Assembly: West.Core
// Namespace: West.Core.Grid
// Summary: Deterministic, allocation-free grid/world conversion service. Square cells; orthographic camera assumed.

#nullable enable
using UnityEngine;

namespace West.Core.Grid
{
    /// <summary>Runtime grid service per AD Grid & Coordinates.</summary>
    public sealed class GridService : IGridService
    {
        private readonly GridConfigRuntime _cfg;

        public GridService(GridConfigRuntime cfg) => _cfg = cfg;

        public int PixelsPerUnit => _cfg.PixelsPerUnit;
        public float CellSize => _cfg.CellSizeWorld;
        public GridSize Dimensions => _cfg.Dimensions;

        public Vector3 GridToWorld(GridPos gp, bool center = true)
        {
            // Cell origin at bottom-left corner; +center recenters to cell middle.
            float offset = center ? 0.5f : 0f;
            float x = (gp.x + offset) * _cfg.CellSizeWorld;
            float y = (gp.y + offset) * _cfg.CellSizeWorld;
            return new Vector3(x, y, 0f);
        }

        public GridPos WorldToGrid(Vector3 worldPos)
        {
            // Floors to containing cell; negative positions handled correctly.
            int gx = Mathf.FloorToInt(worldPos.x / _cfg.CellSizeWorld);
            int gy = Mathf.FloorToInt(worldPos.y / _cfg.CellSizeWorld);
            return new GridPos(gx, gy);
        }

        public Bounds CellBounds(GridPos gp)
        {
            var center = GridToWorld(gp, center: true);
            var size = new Vector3(_cfg.CellSizeWorld, _cfg.CellSizeWorld, 0.01f);
            return new Bounds(center, size);
        }
    }
}
