using System;
using System.Collections;
using TMPro;
//using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController:MonoBehaviour
{
    [Header("復活までの待ち時間")]
    [SerializeField] public float respawnDelay = 2f;

    [Header("復活時の無敵時間")]
    [SerializeField] public float invincibleTime = 2f;

    [Header("リスポーン位置")]
    [SerializeField] public Vector2 respawnPosition = new Vector2(0, -4.45f);
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform judgementTransform;
    [Header("弾幕のプレハブ")]
    [SerializeField] GameObject barrage;//弾幕のプレハブ
    [SerializeField] BossHP bossHP;
    [SerializeField] BalletPoolPlayer poolPlayer;//オブジェクトプール
    private Gamepad gamepad;
    [SerializeField] private Animator animator;
    //[SerializeField] BoxCollider2D boxCollider;
    //private Vector2 movement;
    [SerializeField] public float moveSpeed = 5f;
    public bool generate = false;
    public float coolTime = 0f;
    private bool isRight = false;//アニメーション切替のbool
    private bool isLeft = false;
    private bool addProcess = false;//スコアの加算処理判定
    private int hitCount = 0;
    private int score = 0;//スコア
    private int totalScore = 0;//合計スコア
    private Text lifeText;

    [SerializeField] PlayerJudgement playerJudgement;
    public bool isDead = false;
    public bool isDeath = false;

    public event Action<int> OnLifeChanged;
    public event Action OnPlayerDied;

    private SpriteRenderer spriteRenderer;
    private Collider2D hitCollider;
    public bool blinking = false;

    bool isDeadJudge = false;

    [SerializeField] GameObject judgementObject;
    [SerializeField] SpriteRenderer judgementObjectSpriteRenderer;

    //private PlayerControls controls;

    //private void Awake()
    //{
    //    controls = new PlayerControls();

    //    // RTの入力を取得（performed: 押されたとき）
    //    controls.Player.Shoot.performed += ctx =>
    //    {
    //        rtValue = ctx.ReadValue<float>();  // アナログ値（0〜1）
    //        Debug.Log("RT Trigger pressed: " + rtValue);
    //    };

    //    // 離したとき（canceled）も検出したい場合
    //    controls.Player.Shoot.canceled += ctx =>
    //    {
    //        rtValue = 0;
    //        Debug.Log("RT Trigger released");
    //    };
    //}

    //移動
    public void DomoVe(InputAction.CallbackContext context)
    {
        //performed、canceledコールバックを受け取る
        if (context.started) return;

        //Moveアクションの入力取得
        var inputMove = context.ReadValue<Vector2>();

        //移動処理など

    }

    private void BossDestroy()
    {
        this.score += 5000;
    }

    private void Start()
    {
        generate = true;
        this.animator = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        hitCollider = GetComponent<Collider2D>();
        judgementObjectSpriteRenderer.enabled = false;
    }

    void Update()
    {
        //playerTransform.position = judgementTransform.position;
        
        if (Input.GetKey(KeyCode.JoystickButton0))
        {
            if (generate && !isDeadJudge)
            {
                //Debug.Log("発射ぁ");
                //barrage.SetActive(true);
                //barrage.transform.position = playerTransform.position;
                //Instantiate(barrage, playerTransform.position, playerTransform.rotation);
                //barrage = BulletPool.Instance.GetBullet();
                //barrage.SetActive(true);
                poolPlayer.Launch(transform.position);
                Invoke("Generate", 0.1f);//連射速度の調整
                generate = false;//生成できない
            }
        }

        // 現在接続されているGamepadを取得
        gamepad = Gamepad.current;
        if (gamepad == null) return;

        // 入力を取得
        float moveX = Input.GetAxis("Horizontal"); // 左右
        float moveY = Input.GetAxis("Vertical");   // 上下

        if (Input.GetKey(KeyCode.Joystick1Button2))
        {
            moveSpeed = 2f;
            judgementObjectSpriteRenderer.enabled = true;
        }
        if (Input.GetKeyUp(KeyCode.Joystick1Button2))
        {
            moveSpeed = 5f;
            judgementObjectSpriteRenderer.enabled = false;
        }

        Vector3 move = new Vector3(moveX, moveY,0f) * moveSpeed * Time.deltaTime;

        
        
        animator.SetBool("isRight", isRight);
        animator.SetBool("isLeft", isLeft);
        //this.animator.SetTrigger("Move");

        if (moveX >= 0.1)
        {
            isRight = true;
        }
        else if (moveX <= -0.1)
        {
            isLeft = true;
        }
        else if (moveX == 0)
        {
            isRight = false;
            isLeft = false;
        }

        playerTransform.position += move;

        playerTransform.position = new Vector2(
            //移動範囲を制限する
            Mathf.Clamp(transform.position.x , -2.6f, 2.6f),
            Mathf.Clamp(transform.position.y , -4.8f, 5.0f)
            );

        //if(bossHP != null && bossHP.destroy)
        //{
        //    this.scoreText.text = this.score.ToString();
        //}
        if (playerJudgement != null && playerJudgement.isDead && !isDeadJudge)
        {
            //Debug.LogError("呼び出されている");
            StartCoroutine(RespawnAfterDelay());
            isDeadJudge = true;
            generate = false;
            //if (Input.GetKey(KeyCode.JoystickButton1))
            //{
            //    generate = false;
            //}
        }
    }

    IEnumerator RespawnAfterDelay()
    {
        //一時的に無敵＆非表示
        spriteRenderer.enabled = false;
        hitCollider.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        //transform.position = respawnPosition;
        playerTransform.position = respawnPosition;
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

        while (timer < duration)//無敵時間と同じ時間になるまで
        {
            //blinking = true;//点滅している
            //点滅演出（0.1秒ごとに切り替え）
            spriteRenderer.enabled = !spriteRenderer.enabled;//非表示
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
            //Debug.Log(timer);
        }

        //無敵終了
        spriteRenderer.enabled = true;
        hitCollider.enabled = true;
        blinking = false;//点滅終了
        isDeadJudge = false;
        generate = true;
        //isInvincible = false;
    }

    /// <summary>
    /// プレイヤーのHPシステムに弾を登録（生存弾の管理）
    /// </summary>
    public void AddBullet(Bullet bullet)
    {
        if (bossHP == null)
        {
            return;
        }
        //bossHP.RegisterBullet(bullet.gameObject);
    }

    private void Generate()
    {
        generate = true;//生成可能
    }
}


