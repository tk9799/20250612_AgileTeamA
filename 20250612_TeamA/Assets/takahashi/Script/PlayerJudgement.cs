using NUnit.Framework;
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

    [Header("ジャストガード時の無敵時間")]
    [SerializeField] public float justGuardInvincibleTime = 0f;

    [Header("リスポーン位置")]
    [SerializeField] public Vector2 respawnPosition = new Vector2(0f, 0f);

    [SerializeField] private Transform myTransform;
    [SerializeField] private Transform playerTransfom;
    [SerializeField] public PlayerController playerController;
    [SerializeField] public GuardController2 guardController;
    [SerializeField] private GameObject guradObject;
    [SerializeField] public float invincible = 1.0f;//ジャストガード時の無敵時間
    [SerializeField] private GuardStamina guardStamina;


    public event Action<int> OnLifeChanged;
    public event Action OnPlayerDied;

    [SerializeField]private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D hitCollider;

    private Vector2 myPosition;
    public bool isDead = false;
    public bool isInput = false;
    public bool isInvincible = false;//無敵判定
    public bool isGuardJudgement = false;//ジャストガード判定
    private Text lifeText;
    //private float chargeTime = 0f;

    private bool oneProcess = false;
    private bool isGenerateGuard = false;//シールドを表示非表示判定

    private void Start()
    {
        lifeText = GameObject.Find("LifeText").GetComponent<Text>();
        lifeText.text = "" + lives;

        //spriteRenderer = GetComponent<SpriteRenderer>();
        //hitCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button5)&& !isInvincible&&!isInput)
        {
            isGuardJudgement = true;
            if (guardStamina != null && guardStamina.canStamina == true)
            {
                isInput = true;
                
                StartCoroutine(GeneratorGuard());
            }
        }
        else if(guardStamina!=null&&guardStamina.canStamina == false)
        {
            isGuardJudgement = false;
        }
        //if (guardStamina != null && guardStamina.currentStamina <= guardStamina.maxStaminaGauge)
        //{
        //    Debug.Log("チャージ開始");
        //    chargeTime += Time.deltaTime;
        //    if (chargeTime == 3.0f)
        //    {
        //        guardStamina.currentStamina += 1;
        //        Debug.Log(guardStamina.currentStamina);
        //    }
        //}
        //if (isGenerateGuard=false && hitCollider.enabled == true)
        //{
        //    guardController.spriteRenderer.enabled = false;
        //    guardController.circeCollider.enabled = false;
        //    //guradObject.SetActive(false);
        //    if (!isInvincible)
        //    {
        //        guradObject.SetActive(false);
        //    }
        //}
        if (hitCollider.enabled == true&&spriteRenderer.color==Color.red)
        {
            spriteRenderer.color=Color.white;
        }

        //if (guradObject.activeSelf==false)
        //{
        //    guardController.enabled = false;
        //}
        //else if(guradObject.activeSelf == true)
        //{
        //    guardController.enabled = true;
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("hit");
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss"))
        {
            //敵の弾幕、本体に当たった時ライフを減らす
            Debug.Log("hit");

            if (lives <= 0)
            {
                Debug.Log("シーン移行");
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
        Debug.Log(lives);
        isDead = true;
        //lifeCount--;
        OnLifeChanged?.Invoke(lives);

        if (lives >= 0)
        {
            isInput = true;//シールド表示に使っているbool復活したときシールド表示できない
            Debug.Log("プレイヤー被弾、残機あり → 復活予定");
            StartCoroutine(RespawnAfterDelay());
        }
        else
        {
            Debug.Log("プレイヤーがやられた！残機なし → ゲームオーバー");
            OnPlayerDied?.Invoke();
            GameOverManager.Instance?.StopAllEnemies();
            gameObject.SetActive(false);
            //SceneManager.LoadScene("GameOverScene");
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
        //hitCollider.enabled = true;
        isDead = true;
        if (guradObject != null && guradObject.activeSelf)
        {
            guardController.spriteRenderer.enabled = true;
            guardController.circeCollider.enabled = true;
            //guradObject.SetActive(false);
        }

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
        isDead = false;
        //if (!isInvincible && guardController != null && guardController.judgementEnd == false)
        //{
        //    guardController.spriteRenderer.enabled = true;
        //    guardController.circeCollider.enabled = true;
        //    guradObject.SetActive(false);
        //}
        isInput = false;//シールド表示できるようになる
        //if(guradObject != null&& guradObject.activeSelf)
        //{
        //    guardController.spriteRenderer.enabled = true;
        //    guardController.circeCollider.enabled = true;
        //    guradObject.SetActive(false );
        //} 
        //isInvincible = false;
    }

    private IEnumerator GeneratorGuard()
    {
        Debug.Log("ガード");
        isGenerateGuard = true;
        //一定時間シールドを出現する
        //guradObject.SetActive(true);
        guardController.spriteRenderer.enabled = true;
        guardController.circeCollider.enabled = true;
        yield return new WaitForSeconds(0.05f);
        Debug.Log("ガード非表示");

        //シールドを消す
        isInput = false;
        //guardController.spriteRenderer.enabled = false;
        //guardController.circeCollider.enabled = false;
        //if (!isInvincible && guardController != null && guardController.judgementEnd == false)
        //{
        //    guardController.spriteRenderer.enabled = true;
        //    guardController.circeCollider.enabled = true;
        //    guradObject.SetActive(false);
        //}
        isGenerateGuard = false;
        //yield break;

    }

    public IEnumerator InvincibleTime()
    {
        if (oneProcess)
        {
            yield break;
        }

        oneProcess = true;
        Debug.Log("無敵時間開始");
        hitCollider.enabled = false;
        isInvincible = true;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(justGuardInvincibleTime);

        Debug.Log("無敵時間終了");
        spriteRenderer.color = Color.white;
        hitCollider.enabled = true;
        oneProcess = false;
        isInvincible = false;

        //if (!isInvincible && guardController != null && guardController.judgementEnd)
        //{
        //    guardController.spriteRenderer.enabled = true;
        //    guardController.circeCollider.enabled = true;
        //    guradObject.SetActive(false);
        //}
        //else
        //{
        //    //guardController.spriteRenderer.enabled = true;
        //    //guardController.circeCollider.enabled = true;
        //    guradObject.SetActive(false);
        //}

        //guradObject.SetActive(false);
        //yield break;
    }
}
