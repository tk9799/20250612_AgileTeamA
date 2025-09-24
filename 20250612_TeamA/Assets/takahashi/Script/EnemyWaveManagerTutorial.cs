using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManagerTutorial : MonoBehaviour
{
    [Header("全Waveの設定リスト")]
    public List<EnemyWave> waveList = new List<EnemyWave>();

    [Header("ボスのプレハブ")]
    public GameObject bossPrefab;
    private BossBase bossBase;

    [Header("ボスの出現座標")]
    public Vector3 bossAppearancePosition = new Vector3(0f, 10f, 0f);

    [Header("Wave開始時に自動実行するか")]
    public bool autoStart = true;

    [Header("BGM設定")]
    [Header("雑魚戦BGMを再生するAudioSource")]
    public AudioSource bgmSource;
    [Header("雑魚戦BGM")]
    public AudioClip stageBGM;
    [Header("ボス戦BGM")]
    public AudioClip bossBGM;

    [SerializeField] TMPText tutorialProgressText;

    public bool isWaveRunning = false;
    private bool skipToBoss = false;
    public bool isAnnihilation = false;
    public bool isBossDead = false;
    private bool bossStarted = false;//ボス戦が開始されたかどうか//追加

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    /// <summary>
    /// すべてのWaveを順に実行する
    /// </summary>
    public IEnumerator SpawnAllWaves()//変更を加えた
    {
        isWaveRunning = true;
        skipToBoss = false;

        for (int i = 0; i < waveList.Count; i++)
        {
            if (skipToBoss) yield break;

            // Wave出現
            yield return StartCoroutine(SpawnWave(waveList[i]));
            if (isWaveRunning == true)
            {
                //yield return StartCoroutine(WaitUntilAllEnemiesCleared());//全滅待機
            }


            // そのWaveの敵が全滅するまで待つ
            yield return StartCoroutine(WaitUntilAllEnemiesCleared());
            // 最後のWave以外なら全滅待機
            if (i < waveList.Count - 1)
            {
                //yield return StartCoroutine(WaitUntilAllEnemiesCleared());//全滅待機
                yield return new WaitForSeconds(3f);
            }
            else
            {
                yield return new WaitForSeconds(3f);
                Debug.Log("全Wave終了 → ボス戦へ");
                StartBossBattle(); // ここだけで呼ぶ
            }
        }
        isWaveRunning = false;
        //Debug.Log("全Wave終了 → ボス戦へ");
        //yield return StartCoroutine(WaitUntilAllEnemiesCleared());//最後のWaveの全滅待機
        yield return new WaitForSeconds(3f);
        //Debug.Log("全Wave終了 → ボス戦へ");
        //StartBossBattle();
        //Debug.Log("Wave開始");
        //if (isAnnihilation)
        //{
        //    yield return new WaitForSeconds(3f);
        //    StartBossBattle();
        //}
        //Debug.Log("全Wave終了 → ボス戦へ");
        //StartBossBattle();
    }

    /// <summary>
    /// 1つのWaveを処理する（同時出現 or 順番出現を切り替え）
    /// </summary>
    private IEnumerator SpawnWave(EnemyWave wave)
    {
        if (skipToBoss)
        {
            yield break;
        }
        //Debug.Log(waveList);
        // Wave開始前の待機時間
        yield return new WaitForSeconds(wave.delayBeforeStart);

        if (skipToBoss)
        {
            yield break;
        }

        //敵出現処理
        if (wave.spawnSimultaneously)
        {
            foreach (var spawnInfo in wave.spawnInfos)
            {
                StartCoroutine(SpawnEnemyGroup(spawnInfo));
            }

            //同時出現の場合、敵の最大出現時間だけ待つ
            float maxWaitTime = 0f;
            foreach (var spawnInfo in wave.spawnInfos)
            {
                float time = spawnInfo.delayBeforeSpawn;
                if (spawnInfo.useGridLayout)
                {
                    time += (spawnInfo.gridColumns * spawnInfo.gridRows - 1) * spawnInfo.interval;
                }
                else
                {
                    time += (spawnInfo.spawnCount - 1) * spawnInfo.interval;
                }
                if (time > maxWaitTime) maxWaitTime = time;
            }
            yield return new WaitForSeconds(maxWaitTime);
        }
        else
        {
            //順番に敵群を出現させる
            foreach (var spawnInfo in wave.spawnInfos)
            {
                if (skipToBoss) yield break;

                yield return StartCoroutine(SpawnEnemyGroup(spawnInfo));
            }
        }
    }

    private IEnumerator SpawnEnemyGroup(EnemySpawnInfo info)
    {
        yield return new WaitForSeconds(info.delayBeforeSpawn);

        if (info.useGridLayout)
            yield return StartCoroutine(SpawnGridEnemies(info));
        else
            yield return StartCoroutine(SpawnLinearEnemies(info));
    }

    /// <summary>
    /// 直線状に敵を連続出現させる
    /// </summary>
    private IEnumerator SpawnLinearEnemies(EnemySpawnInfo info)
    {
        for (int i = 0; i < info.spawnCount; i++)
        {
            Vector2 pos = info.spawnCenter + new Vector2(info.offsetX * i, info.offsetY * i);
            EnemyPoolManager.Instance.SpawnEnemy(info.enemyData, pos);
            yield return new WaitForSeconds(info.interval);
        }
    }

    /// <summary>
    /// グリッド状に敵を出現させる
    /// </summary>
    private IEnumerator SpawnGridEnemies(EnemySpawnInfo info)
    {
        int total = info.gridColumns * info.gridRows;
        int count = 0;

        for (int y = 0; y < info.gridRows; y++)
        {
            for (int x = 0; x < info.gridColumns; x++)
            {
                Vector2 offset = new Vector2(
                    (x - (info.gridColumns - 1) / 2f) * info.offsetX,
                    (-(y - (info.gridRows - 1) / 2f)) * info.offsetY
                );
                Vector2 spawnPos = info.spawnCenter + offset;

                EnemyPoolManager.Instance.SpawnEnemy(info.enemyData, spawnPos);
                count++;

                if (count < total)
                {
                    yield return new WaitForSeconds(info.interval);
                }
            }
        }
    }

    /// <summary>
    /// 生存中の敵がいなくなるまで待機するコルーチン
    /// </summary>
    private IEnumerator WaitUntilAllEnemiesCleared()
    {
        yield return new WaitForSeconds(0.5f);
        //全滅を確認する処理
        while (EnemyPoolManager.Instance.GetAllActiveEnemies().Count > 0)//Listの中身（敵の数）が０になったら
        {
            //Debug.Log("敵が残っている: " + EnemyPoolManager.Instance.GetAllActiveEnemies().Count);

            yield return null;
        }
        isAnnihilation = true;
        Debug.Log("全滅確認 → 次へ進む");
    }

    /// <summary>
    /// 外部からWave処理を開始したいときに呼ぶ
    /// </summary>
    public void StartWaves()
    {
        if (isWaveRunning)
        {
            Debug.LogWarning("Wave処理はすでに実行中です");
            return;
        }

        StartCoroutine(SpawnAllWaves());
    }

    //Waveスキップ処理
    public void SkipToBoss()
    {
        if (!isWaveRunning)
        {
            Debug.Log("Wave進行中でないため、スキップ処理は行われません");
            return;
        }

        Debug.Log("Waveをスキップしてボス戦へ移行します");
        skipToBoss = true;

        //現在生存中の敵をすべてPoolに返す
        foreach (var enemy in EnemyPoolManager.Instance.GetAllActiveEnemies())
        {
            //EnemyBaseなどにある戻す処理を呼ぶ
            enemy.ReturnToPool();
        }
    }

    /// <summary>
    /// ボス戦の開始処理
    /// </summary>
    private void StartBossBattle()//変更を加えた
    {
        if (bossStarted) return; // 二重呼び出し防止
        bossStarted = true;
        Debug.Log("ボス戦開始！");

        //BGMをフェードアウトしてからボスを出す
        if (bgmSource != null && bgmSource.isPlaying)
        {
            StartCoroutine(FadeOutBGMAndSpawnBoss(2f));
        }
        else
        {
            //BGMが無い場合は即ボス出現
            SpawnBoss();
        }
    }

    /// <summary>
    /// BGMを徐々にフェードアウトするコルーチン
    /// </summary>
    private IEnumerator FadeOutBGMAndSpawnBoss(float duration)
    {
        float startVolume = bgmSource.volume;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            yield return null;
        }

        bgmSource.Stop();
        bgmSource.volume = startVolume;

        SpawnBoss();
    }

    private void SpawnBoss()
    {
        if (bossPrefab != null)
        {
            //Instantiate(bossPrefab, bossAppearancePosition, Quaternion.identity);
            GameObject BossEnemy = Instantiate(bossPrefab, bossAppearancePosition, Quaternion.identity);
            bossBase = BossEnemy.GetComponent<BossBase>();
            //if (bossBase == null)
            //{
            //    Debug.LogError("BossBaseがnull");
            //}

            //if (bossBase != null)
            //{
            //    //ボス撃破イベントを購読
            //    bossBase.OnBossDied += HandleBossDead;
            //}

            //ボスBGMを再生
            if (bgmSource != null && bossBGM != null)
            {
                bgmSource.clip = bossBGM;
                bgmSource.loop = true;
                bgmSource.Play();
                Debug.Log("[EnemyWaveManager] Boss BGM 再生開始");
            }
        }
        else
        {
            Debug.LogWarning("ボスのプレハブが設定されていません！");
        }
    }

    private void HandleBossDead()
    {
        Debug.Log("[EnemyWaveManager] ボス撃破 → BGM停止");
        if (bgmSource != null && bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }
        isBossDead = true;

        // 念のため購読解除
        if (bossBase != null)
        {
            bossBase.OnBossDied -= HandleBossDead;
        }
    }

    public void ResetManager()
    {
        //リセット
        isWaveRunning = false;
        skipToBoss = false;
        isAnnihilation = false;
        isBossDead = false;

        //EnemyPoolManager.Instanceがまだ存在しない場合
        if (EnemyPoolManager.Instance != null)
        {
            var enemies = EnemyPoolManager.Instance.GetAllActiveEnemies();
            if (enemies != null)
            {
                foreach (var enemy in enemies)
                {
                    enemy.ReturnToPool();
                }
            }
        }

        if (bossBase != null)
        {
            Destroy(bossBase.gameObject);
            bossBase = null;
        }
    }
}
