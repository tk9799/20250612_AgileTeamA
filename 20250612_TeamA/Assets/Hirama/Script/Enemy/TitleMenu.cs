using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    void Start()
    {
        UpdateMenuDisplay();
    }

    void Update()
    {
        //上下キー or W/Sキーで選択
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            selectedIndex = (selectedIndex - 1 + menuItems.Length) % menuItems.Length;
            UpdateMenuDisplay();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            selectedIndex = (selectedIndex + 1) % menuItems.Length;
            UpdateMenuDisplay();
        }

        //決定キー（ZキーやEnterなど）
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Z))
        {
            ExecuteSelection();
        }
    }

    void UpdateCursorPosition()
    {
        //選択中のメニュー項目の位置にカーソルを合わせる（X座標は少し左にずらす）
        Vector3 targetPos = menuItems[selectedIndex].position;
        targetPos.x -= 250f; //左にずらす
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