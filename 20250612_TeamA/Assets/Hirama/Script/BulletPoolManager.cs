using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 複数の弾種ごとにBulletPoolを管理・提供するマネージャー
/// </summary>
public class BulletPoolManager : MonoBehaviour
{
    [System.Serializable]
    public class PoolEntry
    {
        [Header("弾の設定ScriptableObject")]
        [SerializeField] public BulletBase bulletBase;

        [Header("その弾用のプール")]
        [SerializeField] public BulletPool bulletPool;
    }

    [Header("弾種ごとのプールリスト")]
    public List<PoolEntry> poolEntries = new List<PoolEntry>();

    private Dictionary<BulletBase, BulletPool> lookup;

    void Awake()
    {
        lookup = new Dictionary<BulletBase, BulletPool>();
        foreach (var entry in poolEntries)
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
}