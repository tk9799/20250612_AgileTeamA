using UnityEngine;

/// <summary>
/// 雑魚敵の共通処理
/// </summary>
public class EnemyBase : MonoBehaviour
{
    [Header("設定データ")]
    public EnemyData enemyData;

    protected int currentHP;
    protected bool isDead = false;
    private bool Appeared = false;

    //ールやWaveManagerに通知する
    public event System.Action<EnemyBase> OnEnemyDestroyed;

    protected virtual void OnEnable()
    {
        if (enemyData == null)
        {
            Debug.LogWarning("[EnemyBase] EnemyDataが設定されていません！");
            return;
        }

        currentHP = enemyData.maxHP;
        isDead = false;
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    public virtual void TakeDamage(int amount)
    {
        if (isDead)
        {
            return;
        }

        currentHP -= amount;
        Debug.Log($"[EnemyBase] ダメージ: -{amount} → 残HP: {currentHP}");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    protected virtual void Die()
    {
        isDead = true;

        //スコア加算
        if (enemyData != null && enemyData.score > 0)
        {
            //ここにスコアの処理
        }

        //ここに死亡エフェクト処理(あれば)

        OnEnemyDestroyed?.Invoke(this);
        gameObject.SetActive(false);
    }

    //画面に初めて表示されたとき
    private void OnBecameVisible()
    {
        Appeared = true;
    }

    //画面外に行ったとき
    private void OnBecameInvisible()
    {
        if (!isDead && Appeared)
        {
            Debug.Log("[EnemyBase] 画面外に出たためプールへ戻す");
            OnEnemyDestroyed?.Invoke(this);
            gameObject.SetActive(false);
            Appeared = false;
        }
    }
}