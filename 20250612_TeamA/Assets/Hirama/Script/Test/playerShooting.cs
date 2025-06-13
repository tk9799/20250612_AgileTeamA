using UnityEngine;

//後でこのスクリプトを絶対消す

public class playerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.2f;

    private float fireTimer = 0f;

    void Update()
    {
        fireTimer += Time.deltaTime;

        // スペースキーで撃つ（押しっぱなし連射）
        if (Input.GetKey(KeyCode.Space) && fireTimer >= fireRate)
        {
            Shoot();
            fireTimer = 0f;
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}