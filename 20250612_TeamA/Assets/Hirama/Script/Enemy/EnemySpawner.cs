using UnityEngine;

/// <summary>
/// 試し用
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    public EnemyPool pool;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // スペースキーで敵を出現
            Vector3 spawnPos = new Vector3(Random.Range(-2, 2), 6, 0);
            pool.GetEnemy(spawnPos);
        }
    }
}