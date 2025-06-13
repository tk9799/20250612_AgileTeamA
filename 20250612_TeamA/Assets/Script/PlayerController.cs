using System.Collections;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController:MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] GameObject barrage;//弾幕のプレハブ
    private Gamepad gamepad;
    //private Vector2 movement;
    public float moveSpeed = 5f;
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

    void Update()
    {
        if (Input.GetKey(KeyCode.Joystick1Button1))
        {
            //Debug.Log("発射ぁ");
            StartCoroutine(Shot());
        }

        // 現在接続されているGamepadを取得
        gamepad = Gamepad.current;
        if (gamepad == null) return;

        // 入力を取得
        float moveX = Input.GetAxis("Horizontal"); // 左右
        float moveY = Input.GetAxis("Vertical");   // 上下

        Vector3 move = new Vector3(moveX, moveY,0f) * moveSpeed * Time.deltaTime;

        playerTransform.position += move;
    }

    private IEnumerator Shot()
    {
        
        
            
         Instantiate(barrage, playerTransform.position, playerTransform.rotation);

         yield return new WaitForSeconds(0.5f);//次の弾幕が発射されるまでの時間
        
        
    }
}
