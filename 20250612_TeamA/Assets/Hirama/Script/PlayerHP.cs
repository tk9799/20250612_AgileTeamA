using UnityEngine;
using System;
using System.Collections;

public class PlayerHP : MonoBehaviour
{
    [Header("残機数（初期値）")]
    [SerializeField] public int lifeCount = 3;

    [Header("復活までの待ち時間")]
    [SerializeField] public float respawnDelay = 2f;

    [Header("復活時の無敵時間")]
    [SerializeField] public float invincibleTime = 2f;

    [Header("リスポーン位置")]
    [SerializeField] public Vector2 respawnPosition = new Vector2(0, -3f);

    public event Action<int> OnLifeChanged;
    public event Action OnPlayerDied;

    private SpriteRenderer spriteRenderer;
    private Collider2D hitCollider;

    private bool isDead = false;
    //private bool isInvincible = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitCollider = GetComponent<Collider2D>();

        OnLifeChanged?.Invoke(lifeCount);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //EnemyBulleのタグを付ける
        if (other.CompareTag("EnemyBullet") && !isDead)
        {
            other.gameObject.SetActive(false);
            HandleDeath();
        }
    }

    void HandleDeath()
    {
        isDead = true;
        lifeCount--;
        OnLifeChanged?.Invoke(lifeCount);

        if (lifeCount > 0)
        {
            Debug.Log("プレイヤー被弾、残機あり → 復活予定");
            StartCoroutine(RespawnAfterDelay());
        }
        else
        {
            Debug.Log("プレイヤーがやられた！残機なし → ゲームオーバー");
            OnPlayerDied?.Invoke();
            GameOverManager.Instance?.StopAllEnemies();
            gameObject.SetActive(false);
        }
    }

    IEnumerator RespawnAfterDelay()
    {
        //一時的に無敵＆非表示
        spriteRenderer.enabled = false;
        hitCollider.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        transform.position = respawnPosition;
        spriteRenderer.enabled = true;
        hitCollider.enabled = true;
        isDead = false;

        //無敵時間スタート
        StartCoroutine(InvincibilityCoroutine(invincibleTime));
    }

    IEnumerator InvincibilityCoroutine(float duration)
    {
        //isInvincible = true;
        float timer = 0f;
        hitCollider.enabled = false;

        while (timer < duration)
        {
            //点滅演出（0.1秒ごとに切り替え）
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }

        //無敵終了
        spriteRenderer.enabled = true;
        hitCollider.enabled = true;
        //isInvincible = false;
    }
}