using UnityEngine;

/// <summary>
/// 雑魚敵の基本クラス
/// </summary>
public class Enemy : MonoBehaviour,IDamageable
{
    [Header("最大HP")]
    public int maxHP = 10;

    //現在のHP（内部的に管理）
    private int currentHP;

    private EnemyPool pool;

    //一度でも画面内に入ったかどうかのフラグ
    private bool hasAppearedOnScreen = false;

    //死亡通知イベント
    public event System.Action<Enemy> OnEnemyDestroyed;

    public void SetPool(EnemyPool pool)
    {
        this.pool = pool;
    }

    void OnEnable()
    {
        // 再利用時もHPを初期化できる
        currentHP = maxHP;
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        Debug.Log($"[Enemy] ダメージ: -{amount} → 残HP: {currentHP}");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    //カメラに映ったときに呼ばれる処理
    private void OnBecameVisible()
    {
        hasAppearedOnScreen = true;
    }

    //カメラから見えなくなった時に呼ばれる処理
    private void OnBecameInvisible()
    {
        if (hasAppearedOnScreen)
        {
            ReturnToPoolFromOffScreen();
            hasAppearedOnScreen = false;
        }
    }

    /// <summary>
    /// 雑魚が倒されたときの処理
    /// </summary>
    void Die()
    {
        Debug.Log("[Enemy] 撃破 → プールに戻す");
        //通知を飛ばす
        OnEnemyDestroyed?.Invoke(this);
        pool.ReturnEnemy(gameObject);
    }

    public void ReturnToPoolFromOffScreen()
    {
        //消滅でも通知
        OnEnemyDestroyed?.Invoke(this);
        pool.ReturnEnemy(gameObject);
    }
}