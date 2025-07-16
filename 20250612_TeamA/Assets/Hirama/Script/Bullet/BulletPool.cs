using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾のオブジェクトプール。弾の生成コストを削減。
/// </summary>
public class BulletPool : MonoBehaviour
{
    [Header("プール用に使用する弾のプレハブ")]
    public GameObject bulletPrefab;

    [Header("初期生成数")]
    public int initialPoolSize = 300;

    //実際のオブジェクトプール
    private Queue<Bullet> pool = new Queue<Bullet>();

    private List<Bullet> activeBullets = new List<Bullet>();
    [SerializeField] public BulletPoolManager bulletPoolManager;

    void Awake()
    {
        //初期数だけ弾を生成して非アクティブ化し、プールに入れる
        for (int i = 0; i < initialPoolSize; i++)
        {
            AddBulletToPool();
        }
    }

    // プールに弾を追加
    private void AddBulletToPool()
    {
        //プレハブから弾を生成し、非アクティブ状態でプールへ
        var obj = Instantiate(bulletPrefab);
        var bullet = obj.GetComponent<Bullet>();
        bullet.SetOwnerPool(this);
        obj.SetActive(false);
        pool.Enqueue(bullet);
    }

    // 使用可能な弾を取得
    public Bullet GetBullet()
    {
        //プールから弾を取り出し、必要なら追加生成
        if (pool.Count == 0)
        {
            AddBulletToPool();
        }
        var bullet = pool.Dequeue();
        bullet.SetOwnerPool(this);
        bullet.gameObject.SetActive(true);
        activeBullets.Add(bullet);
        bulletPoolManager.activeBulletList.Add(bullet.gameObject);
        //Debug.Log($"[BulletPool] 発射: {bullet.name} を使用 (残り: {pool.Count})");
        return bullet;
    }

    // 弾をプールに戻す
    public void ReturnBullet(Bullet bullet)
    {
        //弾を非アクティブ化してプールへ戻す
        bullet.gameObject.SetActive(false);
        pool.Enqueue(bullet);
        activeBullets.Remove(bullet);

        bulletPoolManager.activeBulletList.Remove(bullet.gameObject);
        //Debug.Log($"[BulletPool] 回収: {bullet.name} をプールに戻した (合計: {pool.Count})");
    }

    public List<Bullet> GetAllActiveBullets()
    {
        return new List<Bullet>(activeBullets);
    }
}