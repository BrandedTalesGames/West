// Path: Assets/West/Core/Grid/GridTypes.cs
// Assembly: West.Core
// Namespace: West.Core.Grid
// Summary: Immutable value types for grid coordinates and sizes. Used across GridService and callers.

#nullable enable
using System;
using UnityEngine;

namespace West.Core.Grid
{
    /// <summary>Integer grid position (x,y). Immutable struct for dictionary keys and events.</summary>
    [Serializable]
    public readonly struct GridPos : IEquatable<GridPos>
    {
        public readonly int x;
        public readonly int y;
        public GridPos(int x, int y) { this.x = x; this.y = y; }
        public bool Equals(GridPos other) => x == other.x && y == other.y;
        public override bool Equals(object? obj) => obj is GridPos o && Equals(o);
        public override int GetHashCode() => unchecked((x * 397) ^ y);
        public override string ToString() => $"({x},{y})";

        public static bool operator ==(GridPos a, GridPos b) => a.Equals(b);
        public static bool operator !=(GridPos a, GridPos b) => !a.Equals(b);
    }

    /// <summary>Grid size in cells.</summary>
    [Serializable]
    public readonly struct GridSize
    {
        public readonly int width;
        public readonly int height;
        public GridSize(int w, int h) { width = Mathf.Max(1, w); height = Mathf.Max(1, h); }
        public override string ToString() => $"{width}x{height}";
    }
}
