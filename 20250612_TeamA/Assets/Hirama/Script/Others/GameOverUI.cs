using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameOverUI : MonoBehaviour
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

    void Awake()
    {
        // 最初は透明＋非表示
        Color c = fadePanel.color;
        c.a = 0;
        fadePanel.color = c;
        gameOverText.gameObject.SetActive(false);
    }

    public void ShowGameOver()
    {
        fadePanel.gameObject.SetActive(true);
        StartCoroutine(ShowSequence());
    }

    private System.Collections.IEnumerator ShowSequence()
    {
        // フェードイン
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

        // 1秒待って「GAME OVER」表示
        yield return new WaitForSeconds(1f);
        gameOverText.gameObject.SetActive(true);

        // さらに1秒後に戻れるようにする
        yield return new WaitForSeconds(1f);
        canReturn = true;
    }

    void Update()
    {
        if (canReturn && Input.GetKeyDown(KeyCode.A))
        {
            SceneManager.LoadScene(returnSceneName);
        }
    }
}