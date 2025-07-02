using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [Header("発射する弾のプレハブ")]
    public GameObject bulletPrefab;

    [Header("発射間隔（秒）")]
    public float fireRate = 0.2f;

    [Header("弾のスピード")]
    public float bulletSpeed = 10f;

    [Header("弾の発射位置")]
    public Transform firePoint;

    private float fireTimer = 0f;

    void Update()
    {
        fireTimer += Time.deltaTime;

        // スペースキーが押されていて、発射間隔を超えていれば弾発射
        if (Input.GetKey(KeyCode.Space) && fireTimer >= fireRate)
        {
            Fire();
            fireTimer = 0f;
        }
    }

    void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.up * bulletSpeed;
        }
    }
}