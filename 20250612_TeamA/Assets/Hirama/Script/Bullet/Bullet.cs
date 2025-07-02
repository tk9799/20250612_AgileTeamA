using UnityEngine;

/// <summary>
/// 単体の弾を表すクラス
/// </summary>
public class Bullet : MonoBehaviour
{
    [Header("BulletBase")]
    public BulletBase bulletBase;

    [Header("移動速度")]
    public Vector2 velocity;

    [Header("生存時間")]
    public float lifeTime = 10f;

    //この弾が所属するオブジェクトプール
    private BulletPool bulletPool;

    //発射後の経過時間
    private float timer;

    //一時停止用フラグ
    public bool isStopped = false;

    //二重返却防止
    private bool isReturned = false;

    public void SetOwnerPool(BulletPool pool)
    {
        bulletPool = pool;
    }

    void OnEnable()
    {
        //弾が有効化されたら状態を初期化
        timer = 0f;
        isStopped = false;
        isReturned = false;
    }

    void Update()
    {
        //一時停止中でなければ毎フレーム処理を実行
        if (!isStopped)
        {
            //移動
            Move();
            //寿命チェック
            UpdateLifetime();
            //画面外チェック
            CheckOutOfBounds();
        }
    }

    //弾の移動処理
    private void Move()
    {
        //velocity に従って弾を移動
        transform.position += (Vector3)(velocity * Time.deltaTime);
    }

    //寿命による自動消去
    private void UpdateLifetime()
    {
        //生存時間が経過したらプールに返す
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            ReturnToPool();
        }
    }

    //画面外に出たら自動で消す処理
    private void CheckOutOfBounds()
    {
        //画面外に出たかを判定
        Vector3 screenPos = Camera.main.WorldToViewportPoint(transform.position);

        if (screenPos.x < -0.1f || screenPos.x > 1.1f ||
            screenPos.y < -0.1f || screenPos.y > 1.1f)
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        //二重返却防止
        if (isReturned)
        {
            return;
        }
        isReturned = true;

        //所属プールに返却 or 非アクティブ化
        if (bulletPool != null)
        {
            bulletPool.ReturnBullet(this);
        }
        else
        {
            //念のため、ownerPoolがなければ非アクティブ化だけ
            gameObject.SetActive(false);
        }
    }

    //弾の初期化
    public void Init(BulletBase baseData, Vector2 position, Vector2 velocity)
    {
        //弾の初期化（位置・速度・設定）
        this.bulletBase = baseData;
        transform.position = position;
        this.velocity = velocity;
        this.lifeTime = baseData.lifetime;
        isStopped = false;

        //見た目の設定
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            //弾の見た目となるスプライト画像
            //sr.sprite = baseData.sprite;
            sr.color = baseData.color;
            transform.localScale = baseData.scale;
        }
    }
}