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

    //一度でもカメラに映ったか
    private bool hasEnteredScreen = false;

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
        hasEnteredScreen = false;
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

    //最初にカメラに映ったとき
    private void OnBecameVisible()
    {
        hasEnteredScreen = true;
    }

    //カメラから完全に見えなくなったとき
    private void OnBecameInvisible()
    {
        if (hasEnteredScreen)
        {
            ReturnToPool();
        }
    }

    /// <summary>
    /// 弾が何かに当たったとき
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        //弾の所属に応じた当たり判定処理
        if (bulletBase.type == BulletType.Player && (other.CompareTag("Enemy") || other.CompareTag("Boss")))
        {
            //敵に当たった処理
            OnHit(other.gameObject);
        }
        else if ((bulletBase.type == BulletType.Enemy || bulletBase.type == BulletType.Boss) && other.CompareTag("Player"))
        {
            //プレイヤーに当たった処理
            OnHit(other.gameObject);
        }
    }

    private void OnHit(GameObject target)
    {
        //プレイヤーにダメージを与える
        var player = target.GetComponent<PlayerHP>();
        if (player != null)
        {
            player.TakeDamage();
        }

        //敵にダメージを与える
        var enemy = target.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            enemy.TakeDamage(bulletBase.damage);
        }

        //ボスにダメージを与える
        var boss = target.GetComponent<BossBase>();
        if (boss != null)
        {
            boss.TakeDamage(bulletBase.damage);
        }

        //ヒットエフェクト
        if (bulletBase.hitEffect != null)
        {
            Instantiate(bulletBase.hitEffect, transform.position, Quaternion.identity);
        }

        //ヒット効果音
        if (bulletBase.hitSE != null)
        {
            AudioSource.PlayClipAtPoint(bulletBase.hitSE, transform.position);
        }

        //貫通弾でなければ消滅
        if (!bulletBase.piercing)
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

        // 見た目の設定
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            //スプライトが設定されていれば差し替え
            if (baseData.bulletsprite != null)
            {
                sr.sprite = baseData.bulletsprite;
            }

            //色の適用
            sr.color = baseData.color;

            //スケールの適用
            transform.localScale = baseData.scale;
        }

        //当たり判定（CircleCollider2D）がある場合、hitboxRadiusに応じてサイズ調整
        var circleCollider = GetComponent<CircleCollider2D>();
        if (circleCollider != null)
        {
            circleCollider.radius = baseData.hitboxRadius * Mathf.Max(baseData.scale.x, baseData.scale.y);
        }
    }
}