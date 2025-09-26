// Path: Assets/West/Core/Grid/IGridService.cs
// Assembly: West.Core
// Namespace: West.Core.Grid
// Summary: Facade for grid/world conversions and bounds queries. Simulation and systems consume this interface only.

#nullable enable
using UnityEngine;

namespace West.Core.Grid
{
    /// <summary>Grid facade per AD; Square grid, 32 PPU default, no corner cutting implied for pathing (later).</summary>
    public interface IGridService
    {
        /// <summary>Pixels per unit for art→world scale (default 32 PPU).</summary>
        int PixelsPerUnit { get; }

        /// <summary>Size of a single cell in world units.</summary>
        float CellSize { get; }

        /// <summary>Grid dimensions in cells, if bounded; 0 means unbounded in that axis.</summary>
        GridSize Dimensions { get; }

        /// <summary>Converts a grid coordinate to world position (cell center by default).</summary>
        Vector3 GridToWorld(GridPos gp, bool center = true);

        /// <summary>Converts a world position to the containing grid cell (floors to int).</summary>
        GridPos WorldToGrid(Vector3 worldPos);

        /// <summary>Returns a world AABB for a cell (min/max corners in world units).</summary>
        Bounds CellBounds(GridPos gp);
    }
}
