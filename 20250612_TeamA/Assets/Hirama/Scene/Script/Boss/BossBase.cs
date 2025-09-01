using UnityEngine;

/// <summary>
/// ボスの共通基底クラス
/// </summary>
public class BossBase : MonoBehaviour, IDanmakuEnemy
{
    [Header("ボス設定データ")]
    public BossData bossData;

    public int CurrentHP { get; private set; }
    private int currentMaxHP;
    public bool isDead = false;
    public bool IsDead => isDead;

    public bool IsInvincible { get; set; } = false;

    public event System.Action OnBossDied;

    protected virtual void OnEnable()
    {
        if (bossData != null)
        {
            Init(bossData);
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public virtual void Init(BossData data)
    {
        bossData = data;
        transform.position = data.startPosition;
        isDead = false;

        //フェーズ未設定のときの仮HP初期化
        SetPhaseHP(data.maxHP);

        var sr = GetComponent<SpriteRenderer>();
        if (sr && data.bossSprite)
        {
            sr.sprite = data.bossSprite;
        }
    }

    /// <summary>
    /// 最大HPを返す
    /// </summary>
    public int GetMaxHP()
    {
        return currentMaxHP;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetPhaseHP(int maxHP)
    {
        currentMaxHP = maxHP;
        CurrentHP = maxHP;
    }

    /// <summary>
    /// ダメージを受ける処理
    /// </summary>
    public virtual void TakeDamage(int damage)
    {
        if (isDead || bossData == null || IsInvincible)
        {
            return;
        }

        CurrentHP -= damage;
        CurrentHP = Mathf.Max(CurrentHP, 0);

        if (CurrentHP <= 0)
        {
            var controller = GetComponent<BossController>();
            if (controller != null)
            {
                controller.OnBossHPDepleted();
            }
            else
            {
                Die();
            }
        }
    }

    public void ForceDie()
    {
        if (!isDead)
        {
            Die();
        }
    }

    /// <summary>
    /// 撃破処理
    /// </summary>
    protected virtual void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        Debug.Log($"ボス「{bossData.BossDisplayName}」を撃破");

        //スコア処理
        AddScore();

        OnBossDied?.Invoke();

        //最終ボスだった場合のみ、ゲームクリアを呼ぶ
        if (bossData.bossType == BossType.Boss)
        {
            GameManager.Instance?.GameClear();
        }

        Destroy(gameObject);
    }

    private void AddScore()
    {
        if (bossData.score > 0)
        {
            //スコア加算
            ScoreManager.Instance?.AddScore(bossData.score);
        }
    }
}