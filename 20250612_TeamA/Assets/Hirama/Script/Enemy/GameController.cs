using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("雑魚フェーズの時間（秒）")]
    public float enemyPhaseDuration = 120f;

    [Header("ボスのプレハブ")]
    public GameObject bossPrefab;

    [Header("ボスの出現位置")]
    public Vector3 bossSpawnPosition = new Vector3(0, 5, 0);

    private float timer = 0f;
    private bool isEnemyPhase = true;
    private bool bossSpawned = false;

    void Update()
    {
        // 雑魚フェーズのカウント
        if (isEnemyPhase)
        {
            timer += Time.deltaTime;
            if (timer >= enemyPhaseDuration)
            {
                isEnemyPhase = false;
                Debug.Log("雑魚フェーズ終了、敵がいなくなったらボス出現");
            }
        }

        // ボス出現判定（雑魚フェーズ終了済み＋画面内に敵がいない）
        if (!isEnemyPhase && !bossSpawned)
        {
            if (!IsAnyEnemyActive())
            {
                SpawnBoss();
            }
        }
    }

    /// <summary>
    /// 現在アクティブな雑魚敵が残っているかどうかチェック
    /// </summary>
    bool IsAnyEnemyActive()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy e in enemies)
        {
            if (e.gameObject.activeInHierarchy)
                return true;
        }
        return false;
    }

    void SpawnBoss()
    {
        Instantiate(bossPrefab, bossSpawnPosition, Quaternion.identity);
        bossSpawned = true;
        Debug.Log("ボス出現！");
    }
}