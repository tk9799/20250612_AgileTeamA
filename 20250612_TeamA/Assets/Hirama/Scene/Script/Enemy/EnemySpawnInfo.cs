using UnityEngine;

/// <summary>
/// Waveの雑魚敵出現設定
/// </summary>
[System.Serializable]
public class EnemySpawnInfo
{
    [Header("出現させる敵のデータ")]
    public EnemyData enemyData;

    [Header("出現の基準位置（中心）")]
    public Vector2 spawnCenter = new Vector2(0, 0);

    [Header("出現までの待機時間（秒）")]
    public float delayBeforeSpawn = 0f;

    [Header("出現数")]
    public int spawnCount = 1;

    [Header("連続出現時のXずらし量")]
    public float offsetX = 0f;

    [Header("連続出現時のYずらし量")]
    public float offsetY = 0f;

    [Header("連続出現の間隔(秒)")]
    public float interval = 0.5f;

    [Header("グリッド配置を使用するか")]
    public bool useGridLayout = false;

    [Header("X方向の列数(グリッド使用時のみ)")]
    public int gridColumns = 1;

    [Header("Y方向の列数(グリッド使用時のみ)")]
    public int gridRows = 1;
}