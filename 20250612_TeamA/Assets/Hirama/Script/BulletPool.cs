using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 弾の再利用プール
/// </summary>
public class BulletPool : MonoBehaviour
{
    //シングルトンインスタンス
    public static BulletPool Instance;

    [Header("弾の設定")]
    [Header("使用する弾のプレハブ")]
    [SerializeField] public GameObject bulletPrefab;

    [Header("最初に生成しておく弾の数")]
    public int initialPoolSize = 200;

    //非アクティブ弾を保持するプール
    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        //シングルトンインスタンスの初期化（複数存在する場合は破棄）
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //初期プール生成：弾を非アクティブで生成しキューに追加
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            pool.Enqueue(bullet);
        }
    }

    /// <summary>
    /// 使用可能な弾をプールから取り出す。なければ新規生成
    /// </summary>
    public GameObject GetBullet()
    {
        if (pool.Count > 0)
        {
            //プールから弾を取り出し、現在の使用数と残数をログに出力
            //Debug.Log($"[POOL] 使用：{initialPoolSize - pool.Count}, 空き：{pool.Count}");
            return pool.Dequeue();
        }
        else
        {
            //プールが空の場合は新しい弾を生成
            Debug.Log("弾プールが不足しています。新たに生成します。");
            return Instantiate(bulletPrefab);
        }

        return pool.Count > 0 ? pool.Dequeue() : Instantiate(bulletPrefab);
    }

    /// <summary>
    /// 使用済みの弾をプールに戻す
    /// </summary>
    public void ReturnBullet(GameObject bullet)
    {
        //同じ弾が重複して戻されないようチェック
        if (!pool.Contains(bullet))
        {
            //再利用準備のため非アクティブ化
            bullet.SetActive(false);
            //プールに戻す
            pool.Enqueue(bullet);
        }
    }
}