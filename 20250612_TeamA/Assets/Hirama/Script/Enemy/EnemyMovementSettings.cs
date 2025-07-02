using UnityEngine;

/// <summary>
/// 敵の移動設定のベースクラス（ScriptableObject）
/// </summary>
public abstract class EnemyMovementSettings : ScriptableObject
{
    [Header("移動速度")]
    public float moveSpeed = 3f;
}