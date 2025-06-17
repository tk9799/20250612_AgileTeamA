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
    [SerializeField] public float lifeTime = 10f;

    [Header("画面外削除設定")]
    [SerializeField] public Vector2 boundsCenter = Vector2.zero;
    //幅×高さを自由に指定
    [SerializeField] public Vector2 boundsSize = new Vector2(8f, 12f);

    //寿命カウント用タイマー
    private float timer = 0f;

    //弾を発射したオブジェクトへの参照
    public MonoBehaviour owner;

    private bool isStopped = false;

    private void OnEnable()
    {
        //弾が再利用された時のタイマーリセット
        timer = 0f;
    }

    private void Update()
    {
        if (isStopped)
        {
            return;
        }

        //指定方向へ弾を移動
        transform.Translate(direction.normalized * BulletSpeed * Time.deltaTime, Space.World);

        //寿命タイマー更新。時間を過ぎたらオブジェクトプールに返す
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            //Poolに戻る
            BulletPool.Instance.ReturnBullet(this.gameObject);
            return;
        }

        //
        Vector2 pos = transform.position;
        Vector2 min = boundsCenter - boundsSize / 2f;
        Vector2 max = boundsCenter + boundsSize / 2f;

        if (pos.x < min.x || pos.x > max.x || pos.y < min.y || pos.y > max.y)
        {
            BulletPool.Instance.ReturnBullet(this.gameObject);
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //プレイヤーに衝突したときの処理
        if (other.CompareTag("Player"))
        {
            var playerHP = other.GetComponent<PlayerHP>();
            if (playerHP != null)
            {
                //プレイヤー即死処理
                playerHP.Die();
            }

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

    public void Stop()
    {
        isStopped = true;
    }
}