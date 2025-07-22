using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボスの基本設定データ
/// </summary>
[CreateAssetMenu(fileName = "BossData", menuName = "Barrage/BossData")]
public class BossData : ScriptableObject
{
    [Header("ボス表示名")]
    public string bossDisplayName;

    [Header("ボスの見た目の画像")]
    public Sprite bossSprite;

    [Header("ボスのPrefab")]
    public GameObject bossPrefab;

    [Header("最大HP")]
    public int maxHP = 100;

    [Header("移動速度")]
    public float moveSpeed = 2f;

    [Header("開始時の出現位置")]
    public Vector2 startPosition = Vector2.zero;

    [Header("攻撃パターンのフェーズリスト")]
    public List<BossPhase> attackPhases = new List<BossPhase>();

    [Header("移動パターンのフェーズリスト")]
    public List<BossPhase> movementPhases = new List<BossPhase>();

    [Header("専用BGM")]
    public AudioClip bossBGM;

    [Header("撃破時に加算するスコア")]
    public int score = 0;
}