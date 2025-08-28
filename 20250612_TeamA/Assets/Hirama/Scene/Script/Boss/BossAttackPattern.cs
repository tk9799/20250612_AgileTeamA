using UnityEngine;

/// <summary>
/// ボス専用の攻撃パターン底辺クラス
/// 複雑な弾幕やフェーズ切り替えに対応できる
/// </summary>
public abstract class BossAttackPattern : ScriptableObject
{
    /// <summary>
    /// 初期化処理。ボスの制御用コンポーネントを受け取る
    /// </summary>
    public abstract void Initialize(BossController bossController);

    /// <summary>
    /// 毎フレーム呼ばれる攻撃更新処理
    /// </summary>
    public abstract void UpdateAttack(BossController bossController);

    /// <summary>
    /// 攻撃パターンの終了処理
    /// </summary>
    public virtual void OnExit(BossController bossController)
    {

    }

    /// <summary>
    /// この攻撃パターンが完了したかどうか
    /// </summary>
    public virtual bool IsFinished() => false;
}