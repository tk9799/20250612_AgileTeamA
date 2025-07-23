using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1Waveで出現させる敵情報のセット
/// </summary>
[System.Serializable]
public class EnemyWave
{
    [Header("このWaveで出現させる敵リスト")]
    public List<EnemySpawnInfo> spawnInfos = new List<EnemySpawnInfo>();

    [Header("Wave内の敵グループを同時に出現させるか")]
    public bool spawnSimultaneously = false;

    [Header("Wave開始前の待機時間（秒）")]
    public float delayBeforeStart = 1f;
}