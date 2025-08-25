using System.Linq;
using UnityEngine;

/// <summary>
/// ゲーム全体の状態を管理(シングルトン)
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    //ゲームクリア済みフラグ
    public bool IsGameClear { get; private set; } = false;
    //ゲームオーバー済みフラグ
    public bool IsGameOver { get; private set; } = false;

    private void Awake()
    {
        //シングルトン初期化
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// ゲームクリア処理
    /// </summary>
    public void GameClear()
    {
        //二重実行防止
        if (IsGameClear)
        {
            return;
        }

        IsGameClear = true;

        Debug.Log("ゲームクリア！");
        int finalScore = ScoreManager.Instance?.GetScore() ?? 0;

        //合計スコア表示
        Debug.Log($"最終スコア: {ScoreManager.Instance?.GetScore()}");

        //プレイヤー操作を止める
        var player = FindObjectOfType<PlayerMoveController>();
        if (player != null)
        {
            player.enabled = false;
        }

        //UI演出を開始
        var ui = FindObjectOfType<GameClearUI>();
        if (ui != null)
        {
            ui.ShowGameClear(finalScore);
        }
    }

    /// <summary>
    /// ゲームオーバー処理
    /// </summary>
    public void GameOver()
    {
        IsGameOver = true;
        Debug.Log("ゲームオーバー...");

        //スコア取得
        int finalScore = ScoreManager.Instance?.GetScore() ?? 0;

        //プレイヤー操作止める
        var player = FindObjectOfType<PlayerMoveController>();
        if (player != null)
        {
            player.enabled = false;
        }

        //UI演出開始
        var ui = FindObjectOfType<GameOverUI>();
        if (ui != null)
        {
            ui.ShowGameOver(finalScore);
        }
    }
}