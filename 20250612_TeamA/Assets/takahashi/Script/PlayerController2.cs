using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2 : MonoBehaviour
{
    [Header("移動速度")]
    public float moveSpeed = 5f;

    [Header("画面端の制限（ワールド座標）")]
    public Vector2 minBounds = new Vector2(-8f, -4.5f);
    public Vector2 maxBounds = new Vector2(8f, 4.5f);
    private Gamepad gamepad;
    [SerializeField] private Animator animator;
    private bool isRight = false;//アニメーション切替のbool
    private bool isLeft = false;

    public void DomoVe(InputAction.CallbackContext context)
    {
        //performed、canceledコールバックを受け取る
        if (context.started) return;

        //Moveアクションの入力取得
        var inputMove = context.ReadValue<Vector2>();

    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 現在接続されているGamepadを取得
        gamepad = Gamepad.current;
        if (gamepad == null) return;

        //入力取得（WASDや矢印キー）
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            Debug.Log("スロー移動");
            moveSpeed = 2f;
        }
        if (Input.GetKeyUp(KeyCode.JoystickButton2))
        {
            moveSpeed = 5f;
        }
        Vector3 move = new Vector3(moveX, moveY, 0f).normalized * moveSpeed * Time.deltaTime;

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

        //移動ベクトル計算
        Vector3 newPos = transform.position + move;

        //画面外に出ないよう制限
        newPos.x = Mathf.Clamp(newPos.x, minBounds.x, maxBounds.x);
        newPos.y = Mathf.Clamp(newPos.y, minBounds.y, maxBounds.y);

        transform.position = newPos;
    }
}
