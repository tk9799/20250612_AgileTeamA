using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [Header("初期残り残機")]
    [SerializeField] private int lives = 3;

    [Header("復活後の無敵時間")]
    [SerializeField] private float invincibilityTime = 2f;

    private bool isDead = false;

    private bool isInvincible = false;

    public void Die()
    {
        if (isDead || isInvincible)
        {
            return;
        }

        isDead = true;
        lives--;

        Debug.Log($"プレイヤーが死んだ。残り残機: {lives}");

        //プレイヤー非表示
        gameObject.SetActive(false);

        if (lives > 0)
        {
            //
            Invoke(nameof(Respawn), 1f);
        }
        else
        {
            var boss = FindObjectOfType<BossOmnidirectionalBarrage>();
            if (boss != null)
            {
                boss.StopBarrage();
            }

            var bossMovement = FindObjectOfType<BossMovement>();
            if (bossMovement != null)
            {
                bossMovement.StopMovement();
            }

            Debug.Log("ゲームオーバー");
        }
    }

    private void Respawn()
    {
        isDead = false;

        //
        transform.position = new Vector3(0, -3, 0);
        gameObject.SetActive(true);

        //
        isInvincible = true;
        Invoke(nameof(EndInvincibility), invincibilityTime);

        Debug.Log("プレイヤーが復活した（無敵状態）");
    }

    private void EndInvincibility()
    {
        isInvincible = false;
        Debug.Log("無敵終了");
    }
}