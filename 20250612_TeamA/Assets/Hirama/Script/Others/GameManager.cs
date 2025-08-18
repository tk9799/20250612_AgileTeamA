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
        //合計スコア表示
        Debug.Log($"最終スコア: {ScoreManager.Instance?.GetScore()}");
    }

    /// <summary>
    /// ゲームオーバー処理
    /// </summary>
    public void GameOver()
    {
        //二重実行防止
        if (IsGameClear || IsGameOver)
        {
            return;
        }

        IsGameOver = true;

        Debug.Log("ゲームオーバー...");
    }
}