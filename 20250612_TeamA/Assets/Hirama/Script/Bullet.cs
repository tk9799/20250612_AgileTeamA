using UnityEngine;

/// <summary>
/// 敵の弾の挙動を管理するスクリプト
/// </summary>
public class Bullet : MonoBehaviour
{
    [Header("移動設定")]
    [Header("弾の移動速度")]
    [SerializeField] public float BulletSpeed = 5f;
    //弾の進行方向
    public Vector2 direction;

    [Header("寿命設定")]
    [Header("弾が自動で消えるまでの時間")]
    [SerializeField] public float lifeTime = 5f;
    //寿命カウント用タイマー
    private float timer = 0f;

    //弾を発射したオブジェクトへの参照
    public MonoBehaviour owner;

    private void OnEnable()
    {
        //弾が再利用された時のタイマーリセット
        timer = 0f;
    }

    private void Update()
    {
        //指定方向へ弾を移動
        transform.Translate(direction.normalized * BulletSpeed * Time.deltaTime, Space.World);

        //寿命タイマー更新。時間を過ぎたらオブジェクトプールに返す
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            //Poolに戻る
            BulletPool.Instance.ReturnBullet(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //プレイヤーに衝突したときの処理
        if (other.CompareTag("Player"))
        {
            //所有者が特定の型（例：ボス）なら、弾管理からも削除
            if (owner is BossOmnidirectionalBarrage boss)
            {
                boss.RemoveBullet(this);
            }
            //弾を非アクティブ化
            BulletPool.Instance.ReturnBullet(this.gameObject);
            return;
        }
    }
}