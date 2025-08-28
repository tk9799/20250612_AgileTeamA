using UnityEngine;

/// <summary>
/// スコアを管理するシングルトン
/// 雑魚敵・ボスの撃破でスコアを加算する
/// </summary>
public class ScoreManager : MonoBehaviour
{
    /// <summary>
    /// スコア管理の唯一のインスタンス(シングルトン)
    /// </summary>
    public static ScoreManager Instance { get; private set; }

    /// <summary>
    /// 現在のスコア
    /// </summary>
    public int CurrentScore { get; private set; } = 0;

    /// <summary>
    /// ゲーム開始時に呼ばれる初期化
    /// </summary>
    private void Awake()
    {
        //すでにインスタンスが存在していたら削除(重複防止)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        //このインスタンスを唯一のものとして設定
        Instance = this;

        //シーンを跨いでもスコアを維持
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 指定されたスコアを加算する
    /// </summary>
    public void AddScore(int value)
    {
        CurrentScore += value;
        Debug.Log($"スコア加算: {value}（現在の合計スコア: {CurrentScore}）");
    }

    /// <summary>
    /// スコアを0にリセットする
    /// </summary>
    public void ResetScore()
    {
        CurrentScore = 0;
        Debug.Log("スコアをリセットしました");
    }

    /// <summary>
    /// 現在のスコアを取得
    /// </summary>
    public int GetScore()
    {
        return CurrentScore;
    }
}