using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// •¡”í—Ş‚Ì“Gƒv[ƒ‹‚ğ‚Ü‚Æ‚ß‚ÄŠÇ—‚·‚éƒ}ƒl[ƒWƒƒ[
/// </summary>
public class EnemyPoolManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyPoolEntry
    {
        [Header("“G‚Ì¯•Ê–¼")]
        public string enemyType;
        [Header("‘Î‰‚·‚éEnemyPool")]
        public EnemyPool pool;
    }

    [Header("“o˜^‚·‚é“G‚Ìí—Ş‚Æƒv[ƒ‹")]
    public List<EnemyPoolEntry> enemyPools = new List<EnemyPoolEntry>();

    private Dictionary<string, EnemyPool> poolDictionary = new Dictionary<string, EnemyPool>();

    void Awake()
    {
        // Dictionary‚É•ÏŠ·
        foreach (var entry in enemyPools)
        {
            if (!poolDictionary.ContainsKey(entry.enemyType))
            {
                poolDictionary.Add(entry.enemyType, entry.pool);
            }
        }
    }

    /// <summary>
    /// í—Ş‚ğw’è‚µ‚Ä“G‚ğæ“¾‚·‚é
    /// </summary>
    public GameObject GetEnemy(string type, Vector3 position)
    {
        if (poolDictionary.ContainsKey(type))
        {
            return poolDictionary[type].GetEnemy(position);
        }
        else
        {
            Debug.LogError($"Enemy type '{type}' ‚ª“o˜^‚³‚ê‚Ä‚¢‚Ü‚¹‚ñI");
            return null;
        }
    }
}