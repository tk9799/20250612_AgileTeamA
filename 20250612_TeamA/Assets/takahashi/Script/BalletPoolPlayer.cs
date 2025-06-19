using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class BalletPoolPlayer : MonoBehaviour
{
    //シングルトンインスタンス
    public static BalletPoolPlayer Instance;

    [Header("弾の設定")]
    [Header("使用する弾のプレハブ")]
    [SerializeField] public BarrageController playerBulletPrefab;

    [Header("最初に生成しておく弾の数")]
    public int initialPoolSize = 200;

    Vector3 setPosition= new Vector3(0,0,0);

    //非アクティブ弾を保持するプール
    private Queue<BarrageController> bulletQueue;

    private void Awake()
    {
        bulletQueue = new Queue<BarrageController>();
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
            //生成
            BarrageController tmpBullet = Instantiate(playerBulletPrefab,setPosition, Quaternion.identity, transform);
            //bullet.SetActive(false);
            bulletQueue.Enqueue(tmpBullet);
        }
    }

    /// <summary>
    /// 使用可能な弾をプールから取り出す。なければ新規生成
    /// </summary>
    public BarrageController Launch(Vector2 pos)
    {
        //Queueが空ならnull
        if (bulletQueue.Count <= 0)
        {
            Debug.LogError("何も登録されていませんでした");
            return null;
        }

        //Queueから弾を一つ取り出す
        BarrageController tmpBullet = bulletQueue.Dequeue();
        //弾を表示する
        tmpBullet.gameObject.SetActive(true);
        //渡された座標に弾を移動する
        //tmpBullet.ShowInStage(pos);
        //呼び出し元に渡す
        return tmpBullet;
        //if (playerBulletQueue.Count > 0)
        //{
        //    //プールから弾を取り出し、現在の使用数と残数をログに出力
        //    //Debug.Log($"[POOL] 使用：{initialPoolSize - pool.Count}, 空き：{pool.Count}");
        //    return playerBulletQueue.Dequeue();
        //}
        //else
        //{
        //    //プールが空の場合は新しい弾を生成
        //    Debug.Log("弾プールが不足しています。新たに生成します。");
        //    return Instantiate(playerBulletPrefab);
        //}
    }

    /// <summary>
    /// 使用済みの弾をプールに戻す
    /// </summary>
    public void ReturnBullet(BarrageController bullet)
    {
        //Queueに格納
        bulletQueue.Enqueue(bullet);
        //弾のゲームオブジェクトを非表示
        bullet.gameObject.SetActive(false);
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
