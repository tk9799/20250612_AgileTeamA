using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("発射する弾の設定")]
    public BulletBase bulletBase;

    [Header("弾のPrefab（Bulletを含む）")]
    public GameObject bulletPrefab;

    [Header("弾の発射間隔（秒）")]
    public float shotInterval = 0.2f;

    [Header("弾の初速方向（単位ベクトル）")]
    public Vector2 shotDirection = Vector2.up;

    [Header("発射位置のオフセット")]
    public Vector2 shotOffset = new Vector2(0, 0.5f);

    private bool isShooting = false;

    private Coroutine shootingCoroutine;

    void Update()
    {
        // スペースキー押したら発射開始、離したら停止（例）
        if (Input.GetKeyDown(KeyCode.Space) && !isShooting)
        {
            shootingCoroutine = StartCoroutine(ShootContinuously());
        }
        else if (Input.GetKeyUp(KeyCode.Space) && isShooting)
        {
            StopCoroutine(shootingCoroutine);
            isShooting = false;
        }
    }

    private IEnumerator ShootContinuously()
    {
        isShooting = true;
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(shotInterval);
        }
    }

    private void Shoot()
    {
        Vector2 spawnPos = (Vector2)transform.position + shotOffset;

        if (bulletPrefab == null)
        {
            Debug.LogWarning("BulletPrefabが設定されていません！");
            return;
        }

        GameObject bulletObj = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            Vector2 velocity = shotDirection.normalized * bulletBase.speed;
            bullet.Init(bulletBase, spawnPos, velocity);
        }

        if (bulletBase.shotSE != null)
        {
            AudioSource.PlayClipAtPoint(bulletBase.shotSE, transform.position);
        }
    }
}