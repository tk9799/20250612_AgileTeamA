using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    [Header("フェード設定")]
    [Header("黒いフェード用Image")]
    public Image fadeImage;
    [Header("フェード時間")]
    public float fadeDuration = 1.5f;

    [Header("ローディング表示")]
    [Header("ローディング文字")]
    public TextMeshProUGUI loadingText;
    [Header("回転オブジェクト")]
    public GameObject loadingSpinner;
    [Header("ローディング時間")]
    public float loadingTime = 2f;

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

    [Header("サウンド設定")]
    [Header("BGM用")]
    public AudioSource bgmAudioSource;
    [Header("タイトルBGM")]
    public AudioClip bgmClip;

    [Header("効果音用")]
    public AudioSource seAudioSource;
    [Header("決定音")]
    public AudioClip confirmClip;
    [Header("キャンセル音")]
    public AudioClip cancelClip;

    private int selectedIndex = 0;
    private Button[] currentMenu;
    private bool inDifficultyMenu = false;
    private bool canControl = false;

    //入力間隔
    private float inputCooldown = 0.2f;
    private float lastInputTime = 0f;

    //スピナー回転コルーチン
    private Coroutine spinnerCoroutine;

    void Start()
    {
        //スコアをリセット
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
        }

        currentMenu = mainMenuButtons;
        difficultyPanel.SetActive(false);

        //最初は黒画面を完全不透明に
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 1f;
            fadeImage.color = c;
        }

        //ローディングUIを有効化
        if (loadingText != null)
        {
            loadingText.gameObject.SetActive(true);
        }
        if (loadingSpinner != null)
        {
            loadingSpinner.SetActive(true);
            spinnerCoroutine = StartCoroutine(SpinWhileActive(loadingSpinner));
        }

        //ローディング→フェードイン
        StartCoroutine(LoadingThenFadeIn());
    }

    //スピナーがアクティブな間だけ回転させる
    IEnumerator SpinWhileActive(GameObject spinner)
    {
        while (spinner != null && spinner.activeInHierarchy)
        {
            spinner.transform.Rotate(Vector3.forward * -180f * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator LoadingThenFadeIn()
    {
        //ローディング時間待機
        yield return new WaitForSeconds(loadingTime);

        //ローディングUI非表示
        if (loadingText != null)
        {
            loadingText.gameObject.SetActive(false);
        }

        if (loadingSpinner != null)
        {
            if (spinnerCoroutine != null)
            {
                StopCoroutine(spinnerCoroutine);
                spinnerCoroutine = null;
            }
            loadingSpinner.SetActive(false);
        }

        //フェードイン開始と同時にBGMもフェードイン
        if (bgmAudioSource != null && bgmClip != null)
        {
            bgmAudioSource.clip = bgmClip;
            bgmAudioSource.volume = 0f;
            bgmAudioSource.loop = true;
            bgmAudioSource.Play();
        }

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);

            //フェード処理
            if (fadeImage != null)
            {
                Color c = fadeImage.color;
                c.a = alpha;
                fadeImage.color = c;
            }

            //BGMの音量フェード
            if (bgmAudioSource != null)
            {
                bgmAudioSource.volume = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            }

            yield return null;
        }

        //完全に透明に
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }

        canControl = true;
        UpdateSelection();
    }


    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (!canControl) return;

        Gamepad gamepad = Gamepad.current;

        //キーボード操作
        if (Keyboard.current != null)
        {
            if (Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame)
            {
                MoveSelection(-1);
            }
            else if (Keyboard.current.downArrowKey.wasPressedThisFrame || Keyboard.current.sKey.wasPressedThisFrame)
            {
                MoveSelection(1);
            }

            if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.zKey.wasPressedThisFrame)
            {
                ExecuteSelection();
            }

            if (inDifficultyMenu && Keyboard.current.xKey.wasPressedThisFrame)
            {
                ReturnToMainMenu();
            }
        }

        //ゲームパッド操作
        if (gamepad != null)
        {
            Vector2 dpad = gamepad.dpad.ReadValue();
            float now = Time.time;

            if (now - lastInputTime >= inputCooldown)
            {
                if (dpad.y > 0.5f)
                {
                    MoveSelection(-1);
                    lastInputTime = now;
                }
                else if (dpad.y < -0.5f)
                {
                    MoveSelection(1);
                    lastInputTime = now;
                }
            }

            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                ExecuteSelection();
            }

            if (inDifficultyMenu && gamepad.buttonEast.wasPressedThisFrame)
            {
                ReturnToMainMenu();
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
        foreach (var btn in currentMenu)
        {
            TextMeshProUGUI tmpText = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (tmpText != null)
            {
                Color c = tmpText.color;
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

        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
        blinkCoroutine = StartCoroutine(BlinkSelected(currentMenu[selectedIndex]));
    }

    IEnumerator BlinkSelected(Button btn)
    {
        TextMeshProUGUI tmpText = btn.GetComponentInChildren<TextMeshProUGUI>();
        if (tmpText != null)
        {
            Color baseColor = tmpText.color;
            float t = 0f;

            while (true)
            {
                t += Time.deltaTime * (1f / blinkInterval) * Mathf.PI;
                float alpha = Mathf.Lerp(0.5f, 1f, (Mathf.Sin(t) + 1f) / 2f);
                tmpText.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
                yield return null;
            }
        }

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
        PlaySE(confirmClip);

        if (!inDifficultyMenu)
        {
            switch (selectedIndex)
            {
                case 0:
                    ShowDifficultyMenu();
                    break;
                case 1:
                    Application.Quit();
                    UnityEditor.EditorApplication.isPlaying = false;
                    break;
            }
        }
        else
        {
            if (bgmAudioSource != null && bgmAudioSource.isPlaying)
            {
                bgmAudioSource.Stop();
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.ResetGameState();
            }

            switch (selectedIndex)
            {
                case 0:
                    if (!string.IsNullOrEmpty(tutorialSceneName))
                    {
                        SceneManager.LoadScene(tutorialSceneName);
                    }
                    break;
                case 1:
                    if (!string.IsNullOrEmpty(normalSceneName))
                    {
                        SceneManager.LoadScene(normalSceneName);

                    }
                    break;
            }
        }
    }

    void ReturnToMainMenu()
    {
        PlaySE(cancelClip);

        inDifficultyMenu = false;
        difficultyPanel.SetActive(false);
        currentMenu = mainMenuButtons;
        selectedIndex = 0;

        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);

        foreach (var obj in hideWhenDifficulty)
        {
            if (obj != null) obj.SetActive(true);
        }

        UpdateSelection();
    }

    void ShowDifficultyMenu()
    {
        inDifficultyMenu = true;
        difficultyPanel.SetActive(true);
        currentMenu = difficultyButtons;
        selectedIndex = 0;

        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);

        foreach (var obj in hideWhenDifficulty)
        {
            if (obj != null) obj.SetActive(false);
        }

        Image panelImage = difficultyPanel.GetComponent<Image>();
        if (panelImage != null)
        {
            Color c = panelImage.color;
            c.a = 0.5f;
            panelImage.color = c;
        }

        UpdateSelection();
    }

    void PlaySE(AudioClip clip)
    {
        if (seAudioSource != null && clip != null)
        {
            seAudioSource.PlayOneShot(clip);
        }
    }
}