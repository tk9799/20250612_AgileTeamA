using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameClearUI : MonoBehaviour
{
    [Header("黒いフェード")]
    public Image fadePanel;

    [Header("最終スコア表示")]
    public TextMeshProUGUI scoreText;

    [Header("文字を表示")]
    public TextMeshProUGUI returnText;

    [Header("遷移先シーン名")]
    public string returnSceneName = "Title";

    private bool canReturn = false;

    //コントローラー
    private Gamepad gamepad;
    //入力間隔をあけるための時間
    private float inputCooldown = 0.2f;
    private float lastInputTime = 0f;
    private Vector2 prevDpad = Vector2.zero;

    void Awake()
    {
        Color c = fadePanel.color;
        c.a = 0;
        fadePanel.color = c;

        scoreText.gameObject.SetActive(false);
        returnText.gameObject.SetActive(false);
    }

    public void ShowGameClear(int score)
    {
        fadePanel.gameObject.SetActive(true);
        scoreText.text = $"Score: {score:N0}";
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

        //スコア表示
        scoreText.gameObject.SetActive(true);

        // 2秒後に表示
        yield return new WaitForSeconds(3f);
        returnText.gameObject.SetActive(true);
        canReturn = true;
    }

    void Update()
    {
        if (canReturn && Input.GetKeyDown(KeyCode.A))
        {
            //シーンに戻る
            SceneManager.LoadScene(returnSceneName);
        }

        //ゲームパッド操作
        if (gamepad != null)
        {
            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                //シーンに戻る
                SceneManager.LoadScene(returnSceneName);
            }
        }
    }
}