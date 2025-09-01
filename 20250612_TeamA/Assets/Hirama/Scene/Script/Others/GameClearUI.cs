using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameClearUI : MonoBehaviour
{
    [Header("黒いフェード")]
    public Image fadePanel;

    [Header("「ゲームクリア」を表示")]
    public TextMeshProUGUI messageText;

    [Header("最終スコア表示")]
    public TextMeshProUGUI scoreText;

    [Header("「タイトルへ」を表示")]
    public TextMeshProUGUI returnText;

    [Header("遷移先シーン名")]
    public string returnSceneName = "Title";

    [Header("BGMとSE")]
    [Header("BGM用")]
    public AudioSource bgmAudioSource;
    [Header("クリア時のBGM")]
    public AudioClip bgmClip;

    [Header("SE用")]
    public AudioSource seAudioSource;
    [Header("決定SE")]
    public AudioClip selectSE;

    private bool canReturn = false;
    private bool isReturning = false;

    //コントローラー
    private Gamepad gamepad;
    //入力間隔をあけるための時間
    //private float inputCooldown = 0.2f;
    //private float lastInputTime = 0f;
    private Vector2 prevDpad = Vector2.zero;

    void Awake()
    {
        Color c = fadePanel.color;
        c.a = 0;
        fadePanel.color = c;

        messageText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        returnText.gameObject.SetActive(false);

        gamepad = Gamepad.current;
    }

    public void ShowGameClear(int score)
    {
        fadePanel.gameObject.SetActive(true);
        scoreText.text = $"Score: {score:N0}";

        //BGM再生
        if (bgmAudioSource != null && bgmClip != null)
        {
            if (!bgmAudioSource.isPlaying)
            {
                bgmAudioSource.clip = bgmClip;
                bgmAudioSource.loop = false;
                bgmAudioSource.Play();
            }
        }

        StartCoroutine(ShowSequence());
    }

    private System.Collections.IEnumerator ShowSequence()
    {
        //フェードイン
        float t = 0f;
        float fadeDuration = 1.5f;
        float targetAlpha = 0.7f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            Color c = fadePanel.color;
            c.a = Mathf.Lerp(0f, targetAlpha, t / fadeDuration);
            fadePanel.color = c;
            yield return null;
        }

        //ゲームクリアを表示
        messageText.gameObject.SetActive(true);

        //何秒後に表示
        yield return new WaitForSeconds(2f);

        //スコア表示
        scoreText.gameObject.SetActive(true);

        //何秒後に表示
        yield return new WaitForSeconds(2f);

        //「タイトルへ」を表示
        returnText.gameObject.SetActive(true);

        canReturn = true;
    }

    void Update()
    {
        if (!canReturn) return;

        //キーボード操作
        if (canReturn && Input.GetKeyDown(KeyCode.A))
        {
            //決定SE
            if (seAudioSource != null && selectSE != null)
            {
                seAudioSource.PlayOneShot(selectSE);
            }

            ReturnToTitle();
        }

        //ゲームパッド操作
        var pad = Gamepad.current;
        if (canReturn && pad != null &&
            pad.buttonSouth.wasPressedThisFrame)
        {
            //決定SE
            if (seAudioSource != null && selectSE != null)
            {
                seAudioSource.PlayOneShot(selectSE);
            }

            ReturnToTitle();
        }
    }

    private void ReturnToTitle()
    {
        if (isReturning) return;
        isReturning = true;

        //SEを再生してからシーン移動するコルーチンを開始
        StartCoroutine(ReturnAfterSE());
    }

    private System.Collections.IEnumerator ReturnAfterSE()
    {
        //決定SE
        float waitTime = 0f;
        if (seAudioSource != null && selectSE != null)
        {
            seAudioSource.PlayOneShot(selectSE);
            waitTime = selectSE.length;
        }

        //BGM停止
        if (bgmAudioSource != null && bgmAudioSource.isPlaying)
        {
            bgmAudioSource.Stop();
        }

        //SEが終わるまで待機
        yield return new WaitForSeconds(waitTime);

        //シーン遷移
        SceneManager.LoadScene(returnSceneName);
    }
}