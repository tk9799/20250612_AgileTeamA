using UnityEngine;

/// <summary>
/// 雑魚敵の共通処理
/// </summary>
public class EnemyBase : MonoBehaviour
{
    [Header("雑魚敵設定データ")]
    public EnemyData enemyData;

    //現在HP
    private int currentHP;

    //所属プール
    private EnemyPool enemyPool;

    //二重返却防止
    private bool isReturned = false;

    //一度でもカメラに映ったか
    private bool hasEnteredScreen = false;

    public void SetOwnerPool(EnemyPool pool)
    {
        enemyPool = pool;
    }

    void OnEnable()
    {
        //フラグ初期化
        isReturned = false;
        hasEnteredScreen = false;

        // 初期化処理
        if (enemyData != null)
        {
            currentHP = enemyData.maxHP;
        }
    }

    /// <summary>
    /// 敵の初期化処理
    /// </summary>
    public void Init(EnemyData data, Vector3 position)
    {
        enemyData = data;
        transform.position = position;
        currentHP = data.maxHP;
        isReturned = false;

        //見た目の設定
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            //スプライト設定
            if (enemyData.enemySprite != null)
            {
                spriteRenderer.sprite = enemyData.enemySprite;
            }

            //色の反映
            spriteRenderer.color = enemyData.enemyColor;
        }

        //スケールの反映
        transform.localScale = enemyData.scale;

        //当たり判定の半径をスケールに応じて調整（CircleCollider2D前提）
        var circleCollider = GetComponent<CircleCollider2D>();
        if (circleCollider != null)
        {
            // スケールの大きい方を基準にradiusを調整
            float baseRadius = 1f;
            float scaleFactor = Mathf.Max(enemyData.scale.x, enemyData.scale.y);
            circleCollider.radius = baseRadius * scaleFactor;
        }
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    public void TakeDamage(int damage)
    {
        if (isReturned)
        {
            return;
        }

        currentHP -= damage;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    private void Die()
    {
        //ここで死亡演出やエフェクトがあれば再生
        ReturnToPool();
    }

    /// <summary>
    /// 最初の1回カメラに映ったとき
    /// </summary>
    void OnBecameVisible()
    {
        hasEnteredScreen = true;
    }

    /// <summary>
    /// カメラから完全に見えなくなったとき
    /// </summary>
    void OnBecameInvisible()
    {
        if (hasEnteredScreen)
        {
            ReturnToPool();
        }
    }

    /// <summary>
    /// プールに返却
    /// </summary>
    private void ReturnToPool()
    {
        if (isReturned) return;
        isReturned = true;

        if (enemyPool != null)
        {
            enemyPool.ReturnEnemy(this);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}