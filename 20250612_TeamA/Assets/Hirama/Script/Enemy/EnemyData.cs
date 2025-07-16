using UnityEngine;

/// <summary>
/// 雑魚敵の基本設定
/// </summary>
[CreateAssetMenu(fileName = "EnemyData", menuName = "Barrage/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("見た目の画像")]
    public Sprite enemySprite;

    [Header("雑魚敵表示名")]
    public string EnemydisplayName;

    [Header("雑魚敵のPrefab")]
    public GameObject EnemyPrefab;

    [Header("弾性能データ")]
    public BulletBase bulletBase;

    [Header("見た目の大きさ。(1, 1)が等倍")]
    public Vector2 scale = Vector2.one;

    [Header("見た目の色")]
    public Color enemyColor = Color.white;

    [Header("最大HP")]
    public int maxHP = 10;

    [Header("移動速度")]
    public float moveSpeed = 2.0f;

    [Header("基本出現位置")]
    public Vector2 spawnPosition = Vector2.zero;

    [Header("攻撃パターン")]
    public EnemyAttackPattern attackPattern;

    [Header("移動パターン")]
    public EnemyMovementPattern movementPattern;

    [Header("撃破時に加算するスコア")]
    public int score = 0;
}