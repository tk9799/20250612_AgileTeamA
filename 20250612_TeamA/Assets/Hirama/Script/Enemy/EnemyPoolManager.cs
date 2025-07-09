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

    //Œ»İƒAƒNƒeƒBƒu‚È“GƒŠƒXƒg
    private List<EnemyHP> activeEnemies = new List<EnemyHP>();

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
            //return poolDictionary[type].GetEnemy(position);
            GameObject enemyObj = poolDictionary[type].GetEnemy(position);

            //Enemy‚ğ“o˜^
            EnemyHP enemy = enemyObj.GetComponent<EnemyHP>();
            if (enemy != null)
            {
                RegisterEnemy(enemy);
            }

            return enemyObj;
        }
        else
        {
            Debug.LogError($"Enemy type '{type}' ‚ª“o˜^‚³‚ê‚Ä‚¢‚Ü‚¹‚ñI");
            return null;
        }
    }

    //“G‚Ì“o˜^E‰ğœ
    public void RegisterEnemy(EnemyHP enemy)
    {
        if (!activeEnemies.Contains(enemy))
        {
            activeEnemies.Add(enemy);
            enemy.OnEnemyDestroyed += HandleEnemyDestroyed;
        }
    }

    private void HandleEnemyDestroyed(EnemyHP enemy)
    {
        activeEnemies.Remove(enemy);
        enemy.OnEnemyDestroyed -= HandleEnemyDestroyed;
    }

    public bool AreAllEnemiesGone()
    {
        return activeEnemies.Count == 0;
    }
}