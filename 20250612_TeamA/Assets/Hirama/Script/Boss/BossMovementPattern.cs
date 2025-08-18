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
    /// 移動パターンの終了処理
    /// </summary>
    public virtual void OnExit(BossController bossController)
    {

    }

    /// <summary>
    /// この移動パターンが完了したかどうかを返す
    /// </summary>
    public abstract bool IsFinished();
}