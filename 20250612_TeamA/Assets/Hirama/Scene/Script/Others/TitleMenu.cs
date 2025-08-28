using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    [Header("メインメニュー")]
    [Header("メインメニュー全体をまとめたパネル")]
    public GameObject mainMenuPanel;
    [Header("メインメニュー選択ボタン")]
    public Button[] mainMenuButtons;

    [Header("難易度メニュー")]
    [Header("難易度メニュー全体をまとめたパネル")]
    public GameObject difficultyPanel;
    [Header("難易度選択ボタン")]
    public Button[] difficultyButtons;

    [Header("シーン設定")]
    [Header("チュートリアル")]
    public string tutorialSceneName = "TutorialScene";
    [Header("ノーマル")]
    public string normalSceneName = "NormalScene";
    //[Header("ハード")]
    //public string hardSceneName = "";

    [Header("非表示にしたい画像やUI")]
    public GameObject[] hideWhenDifficulty;

    [Header("点滅設定")]
    public float blinkInterval = 0.5f;
    private Coroutine blinkCoroutine;

    private int selectedIndex = 0;
    private Button[] currentMenu;
    private bool inDifficultyMenu = false;

    //コントローラー
    private Gamepad gamepad;
    //入力間隔をあけるための時間
    private float inputCooldown = 0.2f;
    private float lastInputTime = 0f;

    void Start()
    {
        currentMenu = mainMenuButtons;
        difficultyPanel.SetActive(false);
        UpdateSelection();

        //スコアをリセット
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
        }

    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
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

    void MoveSelection(int direction)
    {
        selectedIndex = (selectedIndex + direction + currentMenu.Length) % currentMenu.Length;
        UpdateSelection();
    }

    void UpdateSelection()
    {
        //全てのボタンを通常色に戻す
        foreach (var btn in currentMenu)
        {
            TextMeshProUGUI tmpText = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (tmpText != null)
            {
                Color c = tmpText.color;
                //完全表示に戻す
                c.a = 1f;
                tmpText.color = c;
            }

            Text legacyText = btn.GetComponentInChildren<Text>();
            if (legacyText != null)
            {
                Color c = legacyText.color;
                c.a = 1f;
                legacyText.color = c;
            }
        }

        //選択中のボタンを点滅開始
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
        blinkCoroutine = StartCoroutine(BlinkSelected(currentMenu[selectedIndex]));
    }

    IEnumerator BlinkSelected(Button btn)
    {
        //まずTextMeshProUGUIを探す
        TextMeshProUGUI tmpText = btn.GetComponentInChildren<TextMeshProUGUI>();
        if (tmpText != null)
        {
            //元の色を保存
            Color baseColor = tmpText.color;
            float t = 0f;

            while (true)
            {
                //sin波で透明度を変化させる
                t += Time.deltaTime * (1f / blinkInterval) * Mathf.PI;
                //0.5～1.0を往復
                float alpha = Mathf.Lerp(0.5f, 1f, (Mathf.Sin(t) + 1f) / 2f);
                tmpText.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);

                //毎フレーム更新
                yield return null;
            }
        }

        //旧Textコンポーネント対応
        Text legacyText = btn.GetComponentInChildren<Text>();
        if (legacyText != null)
        {
            Color baseColor = legacyText.color;
            float t = 0f;

            while (true)
            {
                t += Time.deltaTime * (1f / blinkInterval) * Mathf.PI;
                float alpha = Mathf.Lerp(0.5f, 1f, (Mathf.Sin(t) + 1f) / 2f);
                legacyText.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);

                yield return null;
            }
        }
    }

    void ExecuteSelection()
    {
        if (!inDifficultyMenu)
        {
            switch (selectedIndex)
            {
                case 0: //ゲームスタート
                    ShowDifficultyMenu();
                    break;
                case 1: //終了
                    Application.Quit();
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#endif
                    break;
            }
        }
        else
        {
            switch (selectedIndex)
            {
                case 0: //チュートリアル
                    if (!string.IsNullOrEmpty(tutorialSceneName))
                        SceneManager.LoadScene(tutorialSceneName);
                    break;
                case 1: //ノーマル
                    if (!string.IsNullOrEmpty(normalSceneName))
                        SceneManager.LoadScene(normalSceneName);
                    break;
                    //case 2: //ハード
                    //    if (!string.IsNullOrEmpty(hardSceneName))
                    //        SceneManager.LoadScene(hardSceneName);
                    //    break;
            }
        }
    }

    void ShowDifficultyMenu()
    {
        inDifficultyMenu = true;
        difficultyPanel.SetActive(true);
        currentMenu = difficultyButtons;
        selectedIndex = 0;

        //メインメニューを消す
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);

        //その他隠したい画像を消す
        foreach (var obj in hideWhenDifficulty)
        {
            if (obj != null) obj.SetActive(false);
        }

        //背景を半透明に
        Image panelImage = difficultyPanel.GetComponent<Image>();
        if (panelImage != null)
        {
            Color c = panelImage.color;
            //半透明に
            c.a = 0.5f;
            panelImage.color = c;
        }

        UpdateSelection();
    }
}