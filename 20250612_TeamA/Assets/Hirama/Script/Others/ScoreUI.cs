using UnityEngine;
using TMPro;

/// <summary>
/// ゲーム画面にスコアを表示する
/// </summary>
public class ScoreUI : MonoBehaviour
{
    [Header("スコア表示用Text")]
    public TextMeshProUGUI scoreText;

    //UIに表示中のスコア
    private int displayedScore = 0;
    //実際のスコア
    private int targetScore = 0;

    void Update()
    {
        //現在のスコアを取得
        int currentScore = ScoreManager.Instance?.GetScore() ?? 0;

        //ここで目標スコアを更新する
        targetScore = currentScore;

        //表示中スコアを1ずつ増やす
        if (displayedScore < targetScore)
        {
            int difference = targetScore - displayedScore;

            //差が大きいほど早く加算
            int increment = Mathf.Clamp(difference / 30, 1, 500000);

            displayedScore += increment;
            if (displayedScore > targetScore)
            {
                displayedScore = targetScore;
            }
        }

        //表示は毎フレーム更新
        scoreText.text = $"{displayedScore:N0}";
    }
}