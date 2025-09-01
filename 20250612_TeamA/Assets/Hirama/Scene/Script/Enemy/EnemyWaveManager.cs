using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 複数のWaveを順に実行して雑魚敵を出現させる
/// </summary>
public class EnemyWaveManager : MonoBehaviour
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

    public bool isWaveRunning = false;
    private bool skipToBoss = false;
    public bool isAnnihilation = false;
    public bool isBossDead = false;

    private void Awake()
    {
        //初期化
        ResetManager();
        //isWaveRunning = false;
        //skipToBoss = false;
        //isAnnihilation = false;
        //isBossDead = false;

        //シーンに入った瞬間から雑魚戦BGMを鳴らす
        if (bgmSource != null && stageBGM != null)
        {
            if (!bgmSource.isPlaying)
            {
                bgmSource.clip = stageBGM;
                bgmSource.loop = true;
                bgmSource.Play();
                Debug.Log("[EnemyWaveManager] Stage BGM 再生開始");
            }
        }

        if (autoStart)
        {
            StartCoroutine(SpawnAllWaves());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Waveスキップ
            SkipToBoss();
        }


        if (bossBase != null && bossBase.IsDead)
        {
            Debug.Log("ボス撃破確認");
            isBossDead = true;

            //ボスBGMを停止
            if (bgmSource != null && bgmSource.isPlaying)
            {
                bgmSource.Stop();
                Debug.Log("[EnemyWaveManager] Boss BGM 停止");
            }
        }
    }

    /// <summary>
    /// すべてのWaveを順に実行する
    /// </summary>
    private IEnumerator SpawnAllWaves()
    {
        isWaveRunning = true;
        skipToBoss = false;

        foreach (var wave in waveList)
        {
            if (skipToBoss)
            {
                yield break;
            }

            yield return StartCoroutine(SpawnWave(wave));
        }

        // 全Wave終了後に敵全滅を待つ
        yield return StartCoroutine(WaitUntilAllEnemiesCleared());

        // ボス戦前に少し待機（例：3秒）
        yield return new WaitForSeconds(3f);

        // ボス戦開始
        StartBossBattle();

        isWaveRunning = false;

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

        // Wave開始前の待機時間
        yield return new WaitForSeconds(wave.delayBeforeStart);

        //Wave開始前に前Waveの敵が全滅するまで待つ
        yield return StartCoroutine(WaitUntilAllEnemiesCleared());

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

    /// <summary>
    /// 敵のグループを出現させる
    /// </summary>
    private IEnumerator SpawnEnemyGroup(EnemySpawnInfo info)
    {
        if (skipToBoss)
        {
            yield break;
        }

        yield return new WaitForSeconds(info.delayBeforeSpawn);

        if (info.useGridLayout)
        {
            yield return StartCoroutine(SpawnGridEnemies(info));
        }
        else
        {
            yield return StartCoroutine(SpawnLinearEnemies(info));
        }
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
        while (EnemyPoolManager.Instance.GetAllActiveEnemies().Count > 0)
        {
            yield return null;
        }
        //全滅確認
        isAnnihilation = true;
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
    private void StartBossBattle()
    {
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
            GameObject BossEnemy = Instantiate(bossPrefab, bossAppearancePosition, Quaternion.identity);
            bossBase = BossEnemy.GetComponent<BossBase>();
            if (bossBase == null)
            {
                Debug.LogError("BossBaseがnull");
            }

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