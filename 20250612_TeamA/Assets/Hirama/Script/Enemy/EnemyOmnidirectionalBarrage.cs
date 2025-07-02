using UnityEngine;
using System.Collections;

/// <summary>
/// 敵の全方位弾幕
/// </summary>
public class EnemyOmnidirectionalBarrage : MonoBehaviour
{
    [Header("使用する弾の設定（BulletBase）")]
    public BulletBase bulletbase;

    [Header("全方位弾に出す弾の数")]
    public int bulletsPerCircle = 8;

    [Header("全方位弾のセット数（連続発射回数）")]
    public int shotsPerSet = 3;

    [Header("1セット内の全方位弾の間隔（秒）")]
    public float intervalBetweenShots = 0.1f;

    [Header("セット間の待機時間（秒）")]
    public float intervalBetweenSets = 3f;

    [Header("開始時に自動で発射する")]
    public bool autoStart = true;

    void Start()
    {
        if (autoStart)
        {
            StartCoroutine(FireLoop());
        }
    }

    IEnumerator FireLoop()
    {
        while (true)
        {
            for (int shot = 0; shot < shotsPerSet; shot++)
            {
                FireCircle(); // 全方位1回分
                if (shot < shotsPerSet - 1)
                    yield return new WaitForSeconds(intervalBetweenShots);
            }

            yield return new WaitForSeconds(intervalBetweenSets);
        }
    }

    /// <summary>
    /// 1回分の全方位弾を発射
    /// </summary>
    void FireCircle()
    {
        float angleStep = 360f / bulletsPerCircle;

        for (int i = 0; i < bulletsPerCircle; i++)
        {
            float angle = angleStep * i;
            Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.right;

            BulletPoolManager.Instance.FireBullet(bulletbase, transform.position, dir);
        }
    }
}