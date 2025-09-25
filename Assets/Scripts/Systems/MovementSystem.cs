using UnityEngine;
using West.Data;
using West.Runtime;

namespace West.Systems
{
    /// <summary>
    /// Moves characters toward their TargetPosition using a simple constant speed.
    /// Systems modify the Runtime model; they do not depend on UI.
    /// </summary>
    public static class MovementSystem
    {
        /// <summary>
        /// Moves each character toward its TargetPosition.
        /// deltaTime should be Unity's Time.deltaTime (seconds).
        /// </summary>
        public static void Update(GameModel model, GlobalConfig cfg, float deltaTime)
        {
            float step = cfg.BaseMoveSpeed * deltaTime;

            for (int i = 0; i < model.Characters.Count; i++)
            {
                var c = model.Characters[i];
                Vector2 pos = c.Position;
                Vector2 target = c.TargetPosition;

                if ((target - pos).sqrMagnitude < 0.0001f) continue;

                Vector2 next = Vector2.MoveTowards(pos, target, step);
                if (next != pos)
                {
                    c.Position = next;
                    DebugLog.Log(DebugChannel.Movement, $"{c.Name} -> {c.Position} (-> {c.TargetPosition})");
                }
            }
        }

        /// <summary>
        /// Teleports a character instantly to a position (useful for spawns or dev).
        /// </summary>
        public static void Teleport(ref CharacterState character, Vector2 newPos)
        {
            character.Position = newPos;
            character.TargetPosition = newPos;
            DebugLog.Log(DebugChannel.Movement, $"Teleport {character.Name} -> {newPos}");
        }

        /// <summary>
        /// Assign a new target destination for a character.
        /// </summary>
        public static void SetDestination(ref CharacterState character, Vector2 target)
        {
            character.TargetPosition = target;
            DebugLog.Log(DebugChannel.Movement, $"SetDestination {character.Name} -> {target}");
        }
    }
}
