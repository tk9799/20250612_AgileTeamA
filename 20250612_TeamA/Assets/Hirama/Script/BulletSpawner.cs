using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾を生成・発射するクラス
/// </summary>
public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private BulletPool defaultPool;

    private List<Bullet> spawnedBullets = new List<Bullet>();

    /// <summary>
    /// 指定した中心点から円形状に弾を発射する。
    /// </summary>
    /// <param name="center">弾の発射位置（中心点）</param>
    /// <param name="bulletCount">発射する弾の数</param>
    /// <param name="speed">弾の速度</param>
    /// <param name="lifetime">弾の寿命（秒）</param>
    /// <param name="owner">（オプション）弾の所有者。必要に応じて利用可能。</param>
    public void FireCircle(Vector3 center,
        int bulletCount,
        float speed,
        float lifetime,
        MonoBehaviour owner = null)
    {
        //360度を弾数で割り、各弾の間隔角度を算出
        float angleStep = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            //ごとの発射角度（度単位）を計算
            float angle = angleStep * i;

            //Z軸回転を使って角度から2D方向ベクトルを取得
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.right;

            //プールから弾を取得し、位置と回転を設定してアクティブ化
            GameObject bulletObj = BulletPool.Instance.GetBullet();
            bulletObj.transform.position = center;
            bulletObj.transform.rotation = Quaternion.identity;
            bulletObj.SetActive(true);

            //弾に移動方向・速度・寿命・所有者情報を設定
            Bullet bullet = bulletObj.GetComponent<Bullet>();
            bullet.direction = direction;
            bullet.BulletSpeed = speed;
            bullet.lifeTime = lifetime;
            bullet.owner = owner;

            spawnedBullets.Add(bullet);

            //所有者がボスの場合、弾をボスの管理対象に追加
            if (owner is BossOmnidirectionalBarrage bossBarrage)
            {
                bossBarrage.AddBullet(bullet);
            }
        }
    }

    /// <summary>
    /// 扇形（一定角度内）に弾を発射する
    /// </summary>
    /// <param name="center">弾の発射位置（中心点）</param>
    /// <param name="bulletCount">発射する弾の数</param>
    /// <param name="startAngle">扇形の開始角度（度）</param>
    /// <param name="spreadAngle">扇形の開き（度）</param>
    /// <param name="speed">弾の速度</param>
    /// <param name="lifetime">弾の寿命（秒）</param>
    /// <param name="owner">弾の所有者</param>
    public void FireFan(Vector3 center,
        int bulletCount,
        float startAngle,
        float spreadAngle,
        float speed,
        float lifetime,
        BulletPool pool,
        MonoBehaviour owner = null)
    {
        if (bulletCount <= 1)
        {
            // 弾が1つだけなら中央に発射
            float angle = startAngle + spreadAngle / 2f;
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.right;
            SpawnBullet(center, direction, speed, lifetime, pool, owner);
            return;
        }

        float angleStep = spreadAngle / (bulletCount - 1);

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + angleStep * i;
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.right;
            SpawnBullet(center, direction, speed, lifetime, pool, owner);
        }
    }

    public void FireFanWithPool(Vector3 center,
        int bulletCount,
        float startAngle,
        float spreadAngle,
        float speed,
        float lifetime,
        BulletPool pool,
        MonoBehaviour owner = null)
    {
        if (bulletCount <= 1)
        {
            float angle = startAngle + spreadAngle / 2f;
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.right;
            SpawnBullet(center, direction, speed, lifetime, pool, owner);
            return;
        }

        float angleStep = spreadAngle / (bulletCount - 1);
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + angleStep * i;
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.right;
            SpawnBullet(center, direction, speed, lifetime, pool, owner);
        }
    }

    /// <summary>
    /// 弾を1発生成し、設定する共通処理
    /// </summary>
    public void SpawnBullet(Vector3 position,
        Vector2 direction,
        float speed,
        float lifetime,
        BulletPool pool,
        MonoBehaviour owner)
    {
        GameObject bulletObj = pool.GetBullet();
        bulletObj.transform.position = position;
        bulletObj.transform.rotation = Quaternion.identity;
        bulletObj.SetActive(true);

        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.direction = direction;
        bullet.BulletSpeed = speed;
        bullet.lifeTime = lifetime;
        bullet.owner = owner;

        spawnedBullets.Add(bullet);

        if (owner is BossOmnidirectionalBarrage omni)
        {
            omni.AddBullet(bullet);
        }

        if (owner is BossFanBarrage fan)
        {

        }

        if (owner is BossOmnidirectionalBarrage bossBarrage)
        {
            bossBarrage.AddBullet(bullet);
        }
    }


    public void StopAllBullets()
    {
        foreach (var bullet in spawnedBullets)
        {
            if (bullet != null && bullet.gameObject.activeInHierarchy)
            {
                bullet.Stop();
            }
        }
        spawnedBullets.Clear();
    }
}