// Path: Assets/West/Core/Unity/AsyncOperationExtensions.cs
// Assembly: West.Core
// Namespace: West.Core.Unity
// Purpose: Make Unity AsyncOperation awaitable via Task so we can `await` scene loads safely.

#nullable enable
using System.Threading.Tasks;
using UnityEngine;

namespace West.Core.Unity
{
    /// <summary>Awaitable wrapper for Unity's AsyncOperation.</summary>
    public static class AsyncOperationExtensions
    {
        /// <summary>Converts an AsyncOperation to a Task that completes when the operation completes.</summary>
        public static Task AsTask(this AsyncOperation op)
        {
            var tcs = new TaskCompletionSource<bool>();
            // If operation could be already done (rare), complete immediately
            if (op.isDone) { tcs.SetResult(true); return tcs.Task; }
            op.completed += _ => tcs.TrySetResult(true);
            return tcs.Task;
        }
    }
}
