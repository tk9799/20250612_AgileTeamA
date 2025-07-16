using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BossHP : MonoBehaviour, IDamageable
{
    [Header("最大HP")]
    public int maxHP = 100;

    [Header("クリアシーンへ移動するまでの待機時間")]
    [SerializeField] private float SceneDelay = 1.0f;

    [Header("遷移先シーン名")]
    [SerializeField] private string ClearSceneName = "GameClearScene";

    //[SerializeField] public BulletPoolManager bulletPoolManager;

    private int currentHP;

    private bool isInvincible = false;

    void Start()
    {
        currentHP = maxHP;
    }

    public void SetInvincible(bool value)
    {
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
            //Die();
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        Debug.Log("[BossHP] ボス撃破！");

        //全敵弾を消す
        Bullet[] bullets = FindObjectsByType<Bullet>(FindObjectsSortMode.None);

        foreach (var bullet in bullets)
        {
            if (bullet.CompareTag("EnemyBullet"))
            {
                bullet.gameObject.SetActive(false);
                //bulletPoolManager.activeBulletList.Remove(bullet.gameObject);
            }
        }

        Debug.Log("シーン移動");
        //ーン遷移をヘルパーに任せる
        SceneTransitionHelper.DelaySceneLoad(ClearSceneName, 2f);

        yield return null;

        //ボス削除
        Destroy(gameObject);
    }
}