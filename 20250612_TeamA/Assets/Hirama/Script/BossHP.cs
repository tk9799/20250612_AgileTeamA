using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class BossHP : MonoBehaviour, IDamageable
{
    [Header("最大HP")]
    [SerializeField] public int maxHP = 100;

    private int currentHP;

    private bool isInvincible = false;

    void Start()
    {
        currentHP = maxHP;
    }

    public void SetInvincible(bool value)
    {
        //isInvincible = invincible;
        isInvincible = value;
        Debug.Log($"[BossHP] 無敵状態: {isInvincible}");
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible)
        {
            return;
        }

        currentHP -= amount;
        Debug.Log($"[BossHP] ダメージを受けた: -{amount} → 残りHP: {currentHP}");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("[BossHP] ボス撃破！");

        //全敵弾を消す
        Bullet[] bullets = FindObjectsByType<Bullet>(FindObjectsSortMode.None);

        foreach (var bullet in bullets)
        {
            if (bullet.CompareTag("EnemyBullet"))
            {
                bullet.gameObject.SetActive(false);
            }
        }

        Destroy(gameObject);
    }
}