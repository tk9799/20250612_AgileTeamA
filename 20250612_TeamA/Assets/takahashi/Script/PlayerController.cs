using System.Collections;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController:MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] GameObject barrage;//弾幕のプレハブ
    private Gamepad gamepad;
    [SerializeField] private Animator animator;
    //private Vector2 movement;
    public float moveSpeed = 5f;
    public bool generate = false;
    public float coolTime = 0f;
    private bool isRight = false;
    private bool isLeft = false;
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

    private void Start()
    {
        generate = true;
        this.animator = GetComponent<Animator>();
    }

    void Update()
    {
        
        if (Input.GetKey(KeyCode.Joystick1Button1))
        {
            if (generate)
            {
                //Debug.Log("発射ぁ");
                Instantiate(barrage, playerTransform.position, playerTransform.rotation);
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

        Vector3 move = new Vector3(moveX, moveY,0f) * moveSpeed * Time.deltaTime;
        
        animator.SetBool("isRight", isRight);
        animator.SetBool("isLeft", isLeft);
        //this.animator.SetTrigger("Move");

        if (moveX >= 0.1)
        {
            Debug.Log("right");
            isRight = true;
        }
        else if (moveX <= -0.1)
        {
            Debug.Log("left");
            isLeft = true;
        }
        else if (moveX == 0)
        {
            isRight = false;
            isLeft = false;
        }

        playerTransform.position += move;
    }
    
    private void Generate()
    {
        generate = true;//生成可能
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBarrage"))
        {
            Debug.Log("hit");
            SceneManager.LoadScene("GameOverScene");
        }
    }
}


