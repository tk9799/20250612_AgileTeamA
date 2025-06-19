using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [Header("")]
    [SerializeField] public float fireInterval = 1f;
    [SerializeField] public float bulletSpeed = 3f;
    [SerializeField] public float bulletLifetime = 10f;

    [Header("")]
    [SerializeField] public Transform firePoint;
    [SerializeField] public Transform player;
    [SerializeField] BulletSpawner bulletSpawner;
    [SerializeField] BulletPool bulletPool;

    private float timer = 0f;

    private void Update()
    {
        //
        if (player == null || bulletSpawner == null || bulletPool == null)
        {
            return;
        }

        timer += Time.deltaTime;
        if (timer >= fireInterval)
        {
            //
            FireAtPlayer();
            timer = 0f;
        }
    }

    void FireAtPlayer()
    {
        Vector2 dir = (player.position - firePoint.position).normalized;
        bulletSpawner.SpawnBullet(firePoint.position, dir, bulletSpeed, bulletLifetime, bulletPool, this);
    }
}