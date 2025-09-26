// Path: Assets/West/Core/Config/DebugConfigService.cs
// Assembly: West.Core
// Namespace: West.Core.Config
#nullable enable
using UnityEngine;
using West.Data;

namespace West.Core.Config
{
    /// <summary>Runtime provider for debug config; reads a ScriptableObject asset created by designers.</summary>
    public sealed class DebugConfigService : IDebugConfig
    {
        private readonly Data.DebugConfig _asset;
        public DebugConfigService(Data.DebugConfig asset) { _asset = asset; }

        public bool EnableDebug => _asset.enableDebug;
        public bool OverrideTime => _asset.overrideTime;
        public int  TickRateHzOverride => _asset.tickRateHzOverride;
        public int  MaxCatchUpPerFrameOverride => _asset.maxCatchUpPerFrameOverride;
        public int  StartSpeedIndex => _asset.startSpeedIndex;
        public bool PauseOnPlay => _asset.pauseOnPlay;
        public int  FastForwardTicksOnPlay => _asset.fastForwardTicksOnPlay;
    }
}
