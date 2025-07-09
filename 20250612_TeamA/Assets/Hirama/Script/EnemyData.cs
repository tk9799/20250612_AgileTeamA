using UnityEngine;

/// <summary>
/// 雑魚敵の設定をまとめたデータ
/// </summary> 
[CreateAssetMenu(fileName = "NewEnenyData", menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("雑魚敵表示名")]
    public string EnemydisplayName;

    [Header("雑魚敵のPrefab")]
    public GameObject EnemyPrefab;

    [Header("最大HP")]
    public int maxHP = 10;

    [Header("移動速度")]
    public float moveSpeed = 2.0f;

    [Header("使用する攻撃パターン")]
    public AttackPatternType attackPattern = AttackPatternType.None;

    [Header("デフォルトの移動パターン設定")]
    public EnemyMovementSettings EnemydefaultMovement;

    [Header("撃破時に加算するスコア")]
    public int score = 0;
}

/// <summary>
/// 攻撃パターンの種類
/// </summary>
public enum AttackPatternType
{
    None,
    Loop,
    Burst
}