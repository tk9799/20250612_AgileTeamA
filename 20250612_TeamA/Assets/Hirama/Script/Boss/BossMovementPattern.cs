using UnityEngine;

/// <summary>
/// ボス専用の移動パターンの基底クラス
/// 各フェーズごとの移動に使用する
/// </summary>
public abstract class BossMovementPattern : ScriptableObject
{
    /// <summary>
    /// 初期化。ボスの参照を受け取る
    /// </summary>
    public abstract void Initialize(BossController controller);

    /// <summary>
    /// 毎フレーム呼ばれる移動処理
    /// </summary>
    public abstract void UpdateMovement(BossController controller);

    /// <summary>
    /// この移動パターンが完了したかどうかを返す。次フェーズ遷移用
    /// </summary>
    public abstract bool IsFinished();
}