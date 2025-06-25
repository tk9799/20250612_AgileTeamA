using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
///　敵のオブジェクトプール
/// </summary>
public class EnemyPool : MonoBehaviour
{
    [Header("プールする敵のプレハブ")]
    public GameObject enemyPrefab;

    [Header("初期生成数")]
    public int initialSize = 10;

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Start()
    {
        //初期生成
        for (int i = 0; i < initialSize; i++)
        {
            CreateNewEnemy();
        }
    }

    /// <summary>
    ///  敵をプールから取り出す
    /// </summary>
    public GameObject GetEnemy(Vector3 positio)
    {
        GameObject enemy;

        if (pool.Count > 0)
        {
            enemy = pool.Dequeue();
        }
        else
        {
            Debug.Log("再生成");
            enemy = CreateNewEnemy();
        }

        enemy.transform.position = positio;
        enemy.SetActive(true);
        return enemy;
    }

    /// <summary>
    /// 敵をプールに戻す
    /// </summary>
    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        pool.Enqueue(enemy);
    }

    /// <summary>
    /// 新しい敵を生成してプールに入れる
    /// </summary>
    private GameObject CreateNewEnemy()
    {
        GameObject newEnemy = Instantiate(enemyPrefab);
        newEnemy.SetActive(false);

        //Enemyにこのプールを渡す
        newEnemy.GetComponent<Enemy>().SetPool(this);

        pool.Enqueue(newEnemy);
        return newEnemy;
    }
}