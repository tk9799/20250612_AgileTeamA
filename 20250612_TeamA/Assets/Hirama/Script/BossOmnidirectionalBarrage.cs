using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボスが全方位弾幕を定期的に発射するクラス
/// </summary>
public class BossOmnidirectionalBarrage : MonoBehaviour
{
    [Header("弾発射管理")]
    [SerializeField] private BulletSpawner bulletSpawner;

    [Header("弾の速度")]
    [SerializeField] private float bulletSpeed = 2f;

    [Header("発射間隔")]
    [SerializeField] private float interval = 1f;

    [Header("弾の寿命")]
    [SerializeField] private float bulletLifeTime = 6f;

    [Header("発射までの待機時間")]
    [SerializeField] private float remainingTime = 2f;

    [Header("弾数(全方位)")]
    [SerializeField] private int bulletCount = 32;

    [SerializeField] private BossHP bossHP;

    [SerializeField] private BulletPool omniBulletPool;

    [Header("追加発射の間隔（秒）")]
    [SerializeField] private float fireInterval = 3f;

    private float fireTimer;
    private List<Bullet> activeBullets = new List<Bullet>();

    private bool isFiring = true;

    private void Update()
    {
        if (!isFiring)
        {
            return;
        }

        //発射タイマーを減算し、一定時間ごとに弾幕を発射
        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0.0f)
        {
            FireBarrage();
            remainingTime = interval;
        }

        fireTimer += Time.deltaTime;
        if (fireTimer >= fireInterval)
        {
            fireTimer = 0f;
            ClearBullets();
            FireOmnidirectional();
        }
    }

    private void FireOmnidirectional()
    {
        bulletSpawner.FireFanWithPool(
            transform.position,
            bulletCount,
            0f,
            360f,
            bulletSpeed,
            bulletLifeTime,
            omniBulletPool,
            this
        );
    }

    /// <summary>
    /// 弾幕の中心を自分の位置にして全方位弾を発射
    /// </summary>
    private void FireBarrage()
    {
        bulletSpawner.FireCircle(transform.position, bulletCount, bulletSpeed, bulletLifeTime, this);
    }

    /// <summary>
    /// ボスのHPシステムに弾を登録（生存弾の管理）
    /// </summary>
    public void AddBullet(Bullet bullet)
    {
        if (bossHP == null)
        {
            return;
        }
        bossHP.RegisterBullet(bullet.gameObject);

        activeBullets.Add(bullet);
    }

    public void ClearBullets()
    {
        foreach (var bullet in activeBullets)
        {
            if (bullet != null)
            {
                bullet.gameObject.SetActive(false);
            }
        }
        activeBullets.Clear();
    }

    /// <summary>
    /// 弾が消滅したときにHP管理リストから削除
    /// </summary>
    public void RemoveBullet(Bullet bullet)
    {
        bossHP?.UnregisterBullet(bullet.gameObject);
    }

    /// <summary>
    /// 即時に弾幕を1回発射し、以降は通常の間隔で発射を継続
    /// </summary>
    public void StartBarrageImmediately()
    {
        //即発射
        FireBarrage();
        //次回の発射まで通常通り待機
        remainingTime = interval;
    }

    /// <summary>
    /// 
    /// </summary>
    public void StopBarrage()
    {
        isFiring = false;
        bulletSpawner.StopAllBullets();

        Debug.Log("ボスの弾幕を全て停止した");
    }
}