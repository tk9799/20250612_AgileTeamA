using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ClearSceneController : MonoBehaviour
{
    [Header("タイトルシーン名")]
    [SerializeField] private string titleSceneName = "GameTitleScene";

    //コントローラー
    private Gamepad gamepad;

    void Update()
    {
        //現在の接続中ゲームパッドを取得
        Gamepad gamepad = Gamepad.current;

        //ゲームパッド操作
        if (gamepad != null)
        {
            //Aボタンを押したら決定
            //buttonSouth は「Aボタン」に対応
            //.wasPressedThisFrame を使って「このフレームで押された瞬間だけ反応」します。
            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                SceneManager.LoadScene(titleSceneName);
            }
        }

        //キーボード操作
        if (Keyboard.current != null)
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                SceneManager.LoadScene(titleSceneName);
            }
        }
    }
}