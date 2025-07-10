using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleMenu : MonoBehaviour
{
    //「▶」のTransform
    public RectTransform cursor;
    //選択肢
    public RectTransform[] menuItems;
    private int selectedIndex = 0;

    [Header("ゲームスタート時に読み込むシーン名")]
    public string gameStartSceneName = "GamePlay";

    public Color normalColor = Color.white;
    public Color selectedColor = Color.yellow;

    //コントローラー
    private Gamepad gamepad;
    //入力間隔をあけるための時間
    private float inputCooldown = 0.2f;
    private float lastInputTime = 0f;
    private Vector2 prevDpad = Vector2.zero;

    void Start()
    {
        UpdateMenuDisplay();
    }

    void MoveSelection(int direction)
    {
        selectedIndex = (selectedIndex + direction + menuItems.Length) % menuItems.Length;
        UpdateMenuDisplay();
    }

    void Update()
    {
        ////現在接続されているGamepadを取得
        //gamepad = Gamepad.current;
        //if (gamepad == null)
        //{
        //    return;
        //}

        ////上下キー or W/Sキーで選択
        //if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        //{
        //    selectedIndex = (selectedIndex - 1 + menuItems.Length) % menuItems.Length;
        //    UpdateMenuDisplay();
        //}
        //else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        //{
        //    selectedIndex = (selectedIndex + 1) % menuItems.Length;
        //    UpdateMenuDisplay();
        //}

        ////決定キー（ZキーやEnterなど）
        //if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Z)|| Input.GetKey(KeyCode.JoystickButton0))
        //{
        //    ExecuteSelection();
        //}

        //現在の接続中ゲームパッドを取得
        Gamepad gamepad = Gamepad.current;

        //キーボード操作
        if (Keyboard.current != null)
        {
            //上下キー（またはW/S）で選択
            //上矢印キーまたはWキーを押した瞬間 → MoveSelection(-1);
            if (Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame)
            {
                MoveSelection(-1);
            }
            //下矢印キーまたは S キーを押した瞬間 → MoveSelection(1);
            else if (Keyboard.current.downArrowKey.wasPressedThisFrame || Keyboard.current.sKey.wasPressedThisFrame)
            {
                MoveSelection(1);
            }

            //EnterまたはZキーで決定
            //どちらかのキーを押した瞬間 → ExecuteSelection();
            if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.zKey.wasPressedThisFrame)
            {
                ExecuteSelection();
            }
        }

        //ゲームパッド操作
        if (gamepad != null)
        {
            //D-Pad(十字キー)の上下を検出
            Vector2 dpad = gamepad.dpad.ReadValue();
            float now = Time.time;

            if (now - lastInputTime >= inputCooldown)
            {
                if (dpad.y > 0.5f)
                {
                    //上へ移動
                    MoveSelection(-1);
                    lastInputTime = now;
                }
                else if (dpad.y < -0.5f)
                {
                    ////下へ移動
                    MoveSelection(1);
                    lastInputTime = now;
                }
            }

            //Aボタンを押したら決定
            //buttonSouth は「Aボタン」に対応
            //.wasPressedThisFrame を使って「このフレームで押された瞬間だけ反応」します。
            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                ExecuteSelection();
            }
        }
    }

    void UpdateCursorPosition()
    {
        //選択中のメニュー項目の位置にカーソルを合わせる（X座標は少し左にずらす）
        Vector3 targetPos = menuItems[selectedIndex].position;
        //左にずらす
        targetPos.x -= 250f;
        cursor.position = targetPos;
    }

    void UpdateMenuDisplay()
    {
        for (int i = 0; i < menuItems.Length; i++)
        {
            Text text = menuItems[i].GetComponent<Text>();
            if (text != null)
            {
                text.color = (i == selectedIndex) ? selectedColor : normalColor;
            }
        }

        //カーソル位置の更新を忘れずに！
        UpdateCursorPosition();
    }

    void ExecuteSelection()
    {
        switch (selectedIndex)
        {
            case 0:
                //ゲームスタート
                if (!string.IsNullOrEmpty(gameStartSceneName))
                {
                    SceneManager.LoadScene(gameStartSceneName);
                }
                else
                {
                    Debug.Log("ゲームスタートシーン名が設定されてません");
                }
                break;
            case 1:
                //オプション
                Debug.Log("オプションは未実装");
                break;
            case 2:
                //終了
                Debug.Log("終了（エディタなので何もしない）");
                Application.Quit();
                break;
        }
    }
}