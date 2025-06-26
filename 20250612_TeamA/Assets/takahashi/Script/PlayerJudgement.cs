using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerJudgement : MonoBehaviour
{
    [Header("初期残り残機")]
    [SerializeField] public int lives = 3;
    [Header("復活までの待ち時間")]
    [SerializeField] public float respawnDelay = 2f;

    [Header("復活時の無敵時間")]
    [SerializeField] public float invincibleTime = 2f;

    [Header("リスポーン位置")]
    [SerializeField] public Vector2 respawnPosition = new Vector2(0f, 0f);

    [SerializeField] private Transform myTransform;
    [SerializeField] private Transform playerTransfom;
    [SerializeField] public PlayerController playerController;

    public event Action<int> OnLifeChanged;
    public event Action OnPlayerDied;

    private SpriteRenderer spriteRenderer;
    private Collider2D hitCollider;

    private Vector2 myPosition;
    public bool isDead = false;
    private Text lifeText;

    private void Start()
    {
        lifeText = GameObject.Find("LifeText").GetComponent<Text>();
        lifeText.text = "" + lives;

        spriteRenderer = GetComponent<SpriteRenderer>();
        hitCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("hit");
        if (collision.gameObject.CompareTag("EnemyBullet") || collision.gameObject.CompareTag("Boss"))
        {
            //敵の弾幕、本体に当たった時ライフを減らす
            Debug.Log("hit");

            if (lives == 0)
            {
                SceneManager.LoadScene("GameOverScene");
            }
            else
            {
                lives -= 1;
                lifeText.text = lives.ToString();
                HandleDeath();
                //playerController.HandleDeath();
            }
        }
    }

    void HandleDeath()
    {
        isDead = true;
        //lifeCount--;
        OnLifeChanged?.Invoke(lives);

        if (lives > 0)
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

        //myTransform.position = respawnPosition;
        //playerTransfom.position = respawnPosition;
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
        myTransform.position=playerTransfom.position;

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
