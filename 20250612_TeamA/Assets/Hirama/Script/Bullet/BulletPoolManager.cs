using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 複数の弾種ごとにBulletPoolを管理・提供するマネージャー
/// </summary>
public class BulletPoolManager : MonoBehaviour
{
    [System.Serializable]
    public class PoolEntry
    {
        [Header("BulletBase")]
        public BulletBase bulletBase;

        [Header("その弾用のプール")]
        public BulletPool bulletPool;
    }

    [Header("弾種ごとのプールリスト")]
    public List<PoolEntry> poolEntries = new List<PoolEntry>();

    //実行時の高速検索用
    private Dictionary<BulletBase, BulletPool> lookup = new Dictionary<BulletBase, BulletPool>();

    public static BulletPoolManager Instance { get; private set; }

    //敵弾幕が発射されたときに追加されるリスト
    //public List<Bullet> activeBulletList = new List<Bullet>();
    public List<GameObject> activeBulletList = new List<GameObject>();
    [SerializeField] public PlayerJudgement playerJudgement;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        //初期登録
        foreach (var entry in poolEntries)
        {
            if (entry.bulletBase != null && entry.bulletPool != null)
            {
                if (!lookup.ContainsKey(entry.bulletBase))
                {
                    lookup.Add(entry.bulletBase, entry.bulletPool);
                }
                else
                {
                    Debug.LogWarning($"BulletBase {entry.bulletBase.name} はすでに登録済みです");
                }
            }
        }

    }

    /// <summary>
    /// 引数のBulletBaseに対応するBulletPoolを返す
    /// </summary>
    public BulletPool GetPool(BulletBase baseData)
    {
        if (baseData == null)
        {
            Debug.LogError("BulletBaseがnullです");
            return null;
        }

        if (lookup.TryGetValue(baseData, out var pool))
        {
            return pool;
        }
        else
        {
            Debug.LogWarning($"BulletBase {baseData.name} に対応するプールが見つかりません");
            return null;
        }
    }

    /// <summary>
    /// 弾を発射する簡易関数。速度は baseData.speed に準拠。
    /// </summary>
    public Bullet FireBullet(BulletBase baseData, Vector2 position, Vector2 direction)
    {
        var pool = GetPool(baseData);
        if (pool == null)
        {
            return null;
        }

        Bullet bullet = pool.GetBullet();
        Vector2 velocity = direction.normalized * baseData.speed;
        bullet.Init(baseData, position, velocity);

        //activeBulletList.Add(gameObject);//リストに追加
        activeBulletList.Add(bullet.gameObject);//リストに追加
        //Debug.Log(activeBulletList.Count);
        foreach (GameObject activeBullet in activeBulletList)
        {
            //int num = activeBulletList.Count;
            Debug.Log(activeBullet.name);
        }

        return bullet;
    }

    /// <summary>
    /// 登録されているすべての弾を一時停止（isStopped = true）
    /// </summary>
    public void PauseAllBullets()
    {
        foreach (var pool in lookup.Values)
        {
            foreach (var bullet in pool.GetAllActiveBullets())
            {
                bullet.isStopped = true;
            }
        }
    }

    /// <summary>
    /// 登録されているすべての弾の一時停止を解除（isStopped = false）
    /// </summary>
    public void ResumeAllBullets()
    {
        foreach (var pool in lookup.Values)
        {
            foreach (var bullet in pool.GetAllActiveBullets())
            {
                bullet.isStopped = false;
            }
        }
    }

    /// <summary>
    /// 現在全プールで使用中の弾をすべて取得
    /// </summary>
    public List<Bullet> GetAllActiveBullets()
    {
        List<Bullet> result = new List<Bullet>();
        foreach (var pool in lookup.Values)
        {
            result.AddRange(pool.GetAllActiveBullets());
        }
        return result;
    }
}