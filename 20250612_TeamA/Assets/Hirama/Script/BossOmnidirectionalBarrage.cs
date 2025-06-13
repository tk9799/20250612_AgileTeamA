using System.Collections.Generic;
using UnityEngine;

public class BossOmnidirectionalBarrage : MonoBehaviour
{
    [Header("弾のプール(BarragePool)")]
    [SerializeField] private BarragePool barragePool;
    [Header("弾の速度")]
    [SerializeField] float bulletSpeed = 2f;
    [Header("発射間隔")]
    [SerializeField] float interval = 1f;
    [Header("弾の生存時間(秒)")]
    [SerializeField] float bulletLifeTime = 6f;
    [Header("弾を撃つまでの時間")]
    [SerializeField] float remainingTime = 2f;
    [Header("弾数(全方位)")]
    [SerializeField] private int bulletCount = 32;

    private List<Barrage> activeBullets = new List<Barrage>();

    void FixedUpdate()
    {
        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0.0f)
        {
            FireBarrage();
            remainingTime = interval;
        }
    }

    void FireBarrage()
    {
        float angleStep = 360f / bulletCount;
        float angle = 0.0f;

        for (int i = 0; i < bulletCount; i++)
        {
            float rad = angle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;

            GameObject bulletObj = barragePool.GetBullet(transform.position, Quaternion.identity);
            Barrage bullet = bulletObj.GetComponent<Barrage>();
            //bullet.Initialize(direction, bulletSpeed, bulletLifeTime, barragePool, this);

            angle += angleStep;
        }
    }

    public void AddBullet(Barrage bullet)
    {
        //if (!activeBullets.Contains(bullet))
        //{
        //    activeBullets.Add(bullet);
        //}

        
    }

    public void RemoveBullet(Barrage bullet)
    {
        //if (activeBullets.Contains(bullet))
        //{
        //    activeBullets.Remove(bullet);
        //}

        
    }

    public void OnDeath()
    {

        foreach (var bullet in activeBullets)
        {
            if (bullet != null && bullet.gameObject.activeInHierarchy)
            {
                bullet.gameObject.SetActive(false);
            }
        }
        activeBullets.Clear();
    }
}