using UnityEngine;

public abstract class MovementPattern : ScriptableObject
{
    /// <summary>
    /// ‰Šú‰»(EnemyController‚ÌQÆ)
    /// </summary>
    public abstract void Initialize(EnemyController controller);

    /// <summary>
    /// –ˆƒtƒŒ[ƒ€ŒÄ‚Î‚ê‚éˆÚ“®ˆ—
    /// </summary>
    public abstract void UpdateMovement(EnemyController controller);
}