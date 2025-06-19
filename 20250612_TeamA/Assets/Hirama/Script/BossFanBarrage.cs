using UnityEngine;

/// <summary>
/// ボスがプレイヤー方向に向けて扇形弾幕を発射するクラス
/// </summary>
public class BossFanBarrage : MonoBehaviour
{
    [Header("弾幕設定")]
    [Header("弾を生成・管理するコンポーネント")]
    [SerializeField] private BulletSpawner bulletSpawner;

    [Header("プレイヤーのTransform")]
    [SerializeField] private Transform playerTransform;

    [Header("弾の速度")]
    [SerializeField] private float bulletSpeed = 4f;

    [Header("弾の寿命（秒）")]
    [SerializeField] private float bulletLifeTime = 6f;

    [Header("弾幕発射間隔（秒）")]
    [SerializeField] private float interval = 1f;

    [Header("1回の発射あたりの弾数")]
    [SerializeField] private int bulletPerShot = 3;

    [Header("扇形の角度")]
    [SerializeField] private float fanAngle = 30f;

    [SerializeField] private FanBulletPool fanBulletPool;

    [Header("追加発射の間隔（秒）")]
    [SerializeField] private float fireInterval = 3f;

    //発射タイマー
    private float timer = 0f;

    //
    private float fireTimer;

    //弾幕発射中フラグ
    private bool isFiring = true;

    void Update()
    {
        //ボスからプレイヤーへのベクトル
        if (!isFiring || playerTransform == null)
        {
            return;
        }

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            //扇形弾幕を発射
            FireFan();
            //タイマーリセット
            timer = interval;
        }

        fireTimer += Time.deltaTime;
        if (fireTimer >= fireInterval)
        {
            fireTimer = 0f;
            FireFan();
        }
    }

    /// <summary>
    /// プレイヤー方向に向けた扇形弾幕を発射する
    /// </summary>
    private void FireFan()
    {
        //ボスからプレイヤーへのベクトル
        Vector3 toPlayer = playerTransform.position - transform.position;

        //ベクトルから角度（度数）を取得
        float baseAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;

        // bulletSpawnerのFanFireを使って弾を発射（角度中心をプレイヤー方向に）
        bulletSpawner.FireFan(
            transform.position,
            bulletPerShot,
            baseAngle - fanAngle / 2f,
            fanAngle,
            bulletSpeed,
            bulletLifeTime,
            fanBulletPool,
            this
        );
    }

    /// <summary>
    /// 弾幕を有効にする
    /// </summary>
    public void StartBarrage() => isFiring = true;

    /// <summary>
    /// 弾幕を停止する
    /// </summary>
    public void StopBarrage() => isFiring = false;
}