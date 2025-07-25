using UnityEngine;

/// <summary>
/// G‹›“G‚Ì‹¤’Êˆ—
/// </summary>
public class EnemyBase : MonoBehaviour, IDanmakuEnemy
{
    [Header("G‹›“Gİ’èƒf[ƒ^")]
    public EnemyData enemyData;

    public int CurrentHP { get; private set; }
    public bool IsDead { get; private set; } = false;

    private EnemyPool enemyPool;
    private bool isReturned = false;
    private bool hasEnteredScreen = false;
    private bool scoreAdded = false;

    void OnEnable()
    {
        isReturned = false;
        hasEnteredScreen = false;

        if (enemyData != null)
        {
            CurrentHP = enemyData.maxHP;
        }
    }

    /// <summary>
    /// “G‚Ì‰Šú‰»ˆ—
    /// </summary>
    public void Init(EnemyData data, Vector3 position)
    {
        enemyData = data;
        transform.position = position;
        CurrentHP = data.maxHP;
        isReturned = false;
        IsDead = false;

        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            if (enemyData.enemySprite != null)
            {
                spriteRenderer.sprite = enemyData.enemySprite;
            }

            spriteRenderer.color = enemyData.enemyColor;
        }

        transform.localScale = enemyData.scale;

        var circleCollider = GetComponent<CircleCollider2D>();
        if (circleCollider != null)
        {
            float baseRadius = 1f;
            float scaleFactor = Mathf.Max(enemyData.scale.x, enemyData.scale.y);
            circleCollider.radius = baseRadius * scaleFactor;
        }
    }

    /// <summary>
    /// Å‘åHP‚ğ•Ô‚·
    /// </summary>
    public int GetMaxHP()
    {
        return enemyData != null ? enemyData.maxHP : 0;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    /// <summary>
    /// ƒ_ƒ[ƒWˆ—
    /// </summary>
    public void TakeDamage(int damage)
    {
        if (isReturned)
        {
            return;
        }

        CurrentHP -= damage;
        if (CurrentHP <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// €–Sˆ—
    /// </summary>
    private void Die()
    {
        if (scoreAdded)
        {
            return;
        }

        scoreAdded = true;
        IsDead = true;

        Debug.Log($"G‹›“Gu{enemyData.EnemydisplayName}v‚ğŒ‚”j");

        //ƒXƒRƒA‰ÁZ
        ScoreManager.Instance?.AddScore(enemyData.score);

        ReturnToPool();
    }

    public void SetOwnerPool(EnemyPool pool)
    {
        enemyPool = pool;
    }

    /// <summary>
    /// ƒv[ƒ‹‚É•Ô‹p
    /// </summary>
    public void ReturnToPool()
    {
        if (isReturned)
        {
            return;
        }
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

    /// <summary>
    /// Å‰‚Ì1‰ñƒJƒƒ‰‚É‰f‚Á‚½‚Æ‚«
    /// </summary>
    void OnBecameVisible()
    {
        hasEnteredScreen = true;
    }

    /// <summary>
    /// ƒJƒƒ‰‚©‚çŠ®‘S‚ÉŒ©‚¦‚È‚­‚È‚Á‚½‚Æ‚«
    /// </summary>
    void OnBecameInvisible()
    {
        if (hasEnteredScreen)
        {
            ReturnToPool();
        }
    }
}