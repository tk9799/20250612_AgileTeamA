using System.Collections;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Pool;

public class PlayerController:MonoBehaviour
{
    [Header("復活後の無敵時間")]
    [SerializeField] private float invincibilityTime = 4f;
    [Header("デフォルト無敵時間")]
    [SerializeField] private float DefoltInvincibilityTime = 4f;
    [SerializeField] Transform playerTransform;
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
        
        
    }

    void Update()
    {
        
        if (Input.GetKey(KeyCode.JoystickButton1))
        {
            if (generate)
            {
                Debug.Log("発射ぁ");
                //barrage.SetActive(true);
                //barrage.transform.position = playerTransform.position;
                //Instantiate(barrage, playerTransform.position, playerTransform.rotation);
                //barrage = BulletPool.Instance.GetBullet();
                //barrage.SetActive(true);
                poolPlayer.Launch(transform.position);
                Invoke("Generate", 0.1f);//連射速度の調整
                generate = false;//生成できない

                //if (coolTime >= 0.5f)
                //{
                //    generate = true;//生成可能
                //}
                //StartCoroutine(Shot());
            }

        }

        // 現在接続されているGamepadを取得
        gamepad = Gamepad.current;
        if (gamepad == null) return;

        // 入力を取得
        float moveX = Input.GetAxis("Horizontal"); // 左右
        float moveY = Input.GetAxis("Vertical");   // 上下

        if (Input.GetKey(KeyCode.Joystick1Button3))
        {
            moveSpeed = 2f;
        }
        if (Input.GetKeyUp(KeyCode.Joystick1Button3))
        {
            moveSpeed = 5f;
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


