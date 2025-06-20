using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class BalletPoolPlayer : MonoBehaviour
{
    //シングルトンインスタンス
    //public static BalletPoolPlayer Instance;

    [Header("弾の設定")]
    [Header("使用する弾のプレハブ")]
    [SerializeField] public BarrageController playerBulletPrefab;

    [Header("最初に生成しておく弾の数")]
    public int initialPoolSize = 20;

    Vector3 setPosition= new Vector3(0,0,0);

    //非アクティブ弾を保持するプール
    Queue<BarrageController> balletQueue;

    private void Awake()
    {
        //pool = new Queue<GameObject>();
        ////シングルトンインスタンスの初期化（複数存在する場合は破棄）
        //if (Instance == null)
        //{
        //    Instance = this;
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}

        //初期プール生成：弾を非アクティブで生成しキューに追加
        for (int i = 0; i < initialPoolSize; i++)
        {
            Debug.Log(initialPoolSize);
            //生成
            BarrageController tmpBullet = Instantiate(playerBulletPrefab, setPosition,Quaternion.identity,transform);
            //tmpBullet.SetActive(false);
            Debug.Log("非表示");
            //Queueに追加
            balletQueue.Enqueue(tmpBullet);
        }
    }

    //public GameObject GetObject()
    //{
    //    foreach (GameObject gameObject in balletQueue)
    //    {
    //        if (!gameObject.activeInHierarchy) // 非アクティブなものだけ返す
    //            return gameObject;
    //    }
    //    // 足りなくなったら増やす（Optional）
    //    GameObject tmpBullet = Instantiate(playerBulletPrefab);
    //    tmpBullet.SetActive(false);
    //    pool.Add(tmpBullet);
    //    return tmpBullet;
    //}

    /// <summary>
    /// 使用可能な弾をプールから取り出す。なければ新規生成
    /// </summary>
    public BarrageController Launch(Vector2 pos)
    {
        //Queueが空ならnull
        if (balletQueue.Count <= 0)
        {
            Debug.LogError("何も登録されていませんでした");
            return null;
        }

        //Queueから弾を一つ取り出す
        BarrageController tmpBullet = balletQueue.Dequeue();
        //弾を表示する
        tmpBullet.gameObject.SetActive(true);
        //渡された座標に弾を移動する
        tmpBullet.ShowInStage(pos);
        //呼び出し元に渡す
        return tmpBullet;
    }

    /// <summary>
    /// 使用済みの弾をプールに戻す
    /// </summary>
    public void ReturnBullet(BarrageController bullet)
    {
        //弾のゲームオブジェクトを非表示
        bullet.gameObject.SetActive(false);
        //Queueに格納
        balletQueue.Enqueue(bullet);
        Debug.Log("回収");
        ////同じ弾が重複して戻されないようチェック
        //if (!playerBulletQueue.Contains(bullet))
        //{
        //    //再利用準備のため非アクティブ化
        //    bullet.SetActive(false);
        //    //プールに戻す
        //    playerBulletQueue.Enqueue(bullet);
        //}
    }
}
