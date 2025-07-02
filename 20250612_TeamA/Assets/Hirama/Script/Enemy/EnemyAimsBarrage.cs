using UnityEngine;
using System.Collections;

/// <summary>
/// 敵の自機狙い弾
/// </summary>
public class EnemyAimsBarrage : MonoBehaviour
{
    public enum FireMode
    {
        //通常ループ発射
        Loop,
        //即時1セット発射（1回限り）
        OneShot,
        //何もしない
        None
    }

    [Header("BulletBase")]
    public BulletBase bulletbase;

    [Header("発射モード")]
    public FireMode fireMode = FireMode.Loop;

    [Header("1セット内の弾数")]
    public int shotsPerSet = 3;

    [Header("1発ごとの間隔（秒）")]
    public float intervalBetweenShots = 0.2f;

    [Header("Loopモードでの待機時間（秒）")]
    public float intervalBetweenSets = 5f;

    [Header("OneShotモードでの発射開始までの待機時間（秒）")]
    public float delayBeforeOneShot = 0f;

    private Transform player;
    private bool hasFiredOnce = false;

    void Start()
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        if (fireMode == FireMode.Loop)
        {
            StartCoroutine(FireLoopCoroutine());
        }
        else if (fireMode == FireMode.OneShot)
        {
            StartCoroutine(FireSetOnceCoroutine());
        }
    }

    private IEnumerator FireLoopCoroutine()
    {
        while (true)
        {
            for (int i = 0; i < shotsPerSet; i++)
            {
                Fire();
                if (i < shotsPerSet - 1)
                {
                    yield return new WaitForSeconds(intervalBetweenShots);
                }
            }
            yield return new WaitForSeconds(intervalBetweenSets);
        }
    }

    private IEnumerator FireSetOnceCoroutine()
    {
        if (hasFiredOnce)
        {
            yield break;
        }

        hasFiredOnce = true;

        if (delayBeforeOneShot > 0f)
        {
            yield return new WaitForSeconds(delayBeforeOneShot);
        }

        for (int i = 0; i < shotsPerSet; i++)
        {
            Fire();
            if (i < shotsPerSet - 1)
            {
                yield return new WaitForSeconds(intervalBetweenShots);
            }
        }
    }

    private void Fire()
    {
        if (player == null)
        {
            return;
        }

        Vector2 dir = (player.position - transform.position).normalized;
        BulletPoolManager.Instance.FireBullet(bulletbase, transform.position, dir);
    }
}