using UnityEngine;

/// <summary>
///  弾幕ゲームにおける敵キャラクターの共通インターフェース
/// </summary>
public interface IDanmakuEnemy
{
    /// <summary>
    /// 現在のHPを取得
    /// </summary>
    int CurrentHP { get; }

    /// <summary>
    /// 最大HPを取得
    /// </summary>
    int GetMaxHP();

    /// <summary>
    /// 敵の座標を取得する
    /// </summary>
    Vector3 GetPosition();

    /// <summary>
    /// 敵の座標を設定する
    /// </summary>
    void SetPosition(Vector3 position);

    /// <summary>
    /// ダメージを受ける処理
    /// </summary>
    void TakeDamage(int damage);

    /// <summary>
    /// 敵が死亡したかどうか
    /// </summary>
    bool IsDead { get; }
}