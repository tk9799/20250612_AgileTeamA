using UnityEngine;

/// <summary>
/// プレイヤーがスペースキーで弾を発射する処理
/// </summary>
public class PlayerShooter : MonoBehaviour
{
    [Header("弾のプレハブ")]
    [SerializeField] private GameObject bulletPrefab;

    [Header("弾を発射する位置")]
    [SerializeField] private Transform firePoint;

    [Header("弾の連射速度")]
    [SerializeField] private float fireRate = 0.2f;

    //発射タイミングを制御するタイマー
    private float fireTimer;

    private void Update()
    {
        //経過時間を加算
        fireTimer += Time.deltaTime;

        //スペースキーを押していて、発射可能なタイミングなら発射
        if (Input.GetKey(KeyCode.Space) && fireTimer >= fireRate)
        {
            Shoot();
            fireTimer = 0f;
        }
    }

    /// <summary>
    /// 弾を生成して発射する
    /// </summary>
    private void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}