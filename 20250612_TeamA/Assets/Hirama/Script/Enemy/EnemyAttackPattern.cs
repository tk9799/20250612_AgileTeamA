using UnityEngine;

public abstract class EnemyAttackPattern : ScriptableObject
{
    /// <summary>
    /// 初期化(EnemyControllerの参照)
    /// </summary>
    public abstract void Initialize(EnemyController controller);

    /// <summary>
    /// 攻撃処理を毎フレーム更新で呼ぶ(タイミング判定や弾発射など)
    /// </summary>
    public abstract void UpdateAttack(EnemyController controller);
}