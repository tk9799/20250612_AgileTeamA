using System.Linq;
using UnityEngine;

/// <summary>
/// ゲーム全体の状態を管理(シングルトン)
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("シーンにあるゲームオーバーのAudioSource")]
    public AudioSource bgmSource;

    public static GameManager Instance { get; private set; }

    //ゲームクリア済みフラグ
    public bool IsGameClear { get; private set; } = false;
    //ゲームオーバー済みフラグ
    public bool IsGameOver { get; private set; } = false;

    public bool isClear = false;

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

    public void StopBGM()
    {
        if (bgmSource != null && bgmSource.isPlaying)
            bgmSource.Stop();
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
        isClear = true;

        Debug.Log("ゲームクリア！");
        int finalScore = ScoreManager.Instance?.GetScore() ?? 0;

        //合計スコア表示
        Debug.Log($"最終スコア: {ScoreManager.Instance?.GetScore()}");

        //プレイヤー操作を止める
        var player = FindFirstObjectByType<PlayerMoveController>();
        if (player != null)
        {
            player.enabled = false;
        }

        var player2 = FindFirstObjectByType<PlayerController2>();
        if (player2 != null)
        {
            player2.enabled = false;
        }

        //プレイヤー弾幕を止める
        var playerbullet = FindFirstObjectByType<PlayerShooter>();
        if (playerbullet != null)
        {
            playerbullet.canShoot = false;
            playerbullet.StopShooting();
        }

        var playerbullet2 = FindFirstObjectByType<PlayerControllerShoot2>();
        if (playerbullet2 != null)
        {
            playerbullet2.enabled = false;
        }

        //UI演出を開始
        var ui = FindFirstObjectByType<GameClearUI>();
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

        //BGM停止
        if (bgmSource != null && bgmSource.isPlaying)
        {
            bgmSource.Stop();
            Debug.Log("[GameManager] BGM 停止（ゲームオーバー）");
        }

        //スコア取得
        int finalScore = ScoreManager.Instance?.GetScore() ?? 0;

        //プレイヤー操作止める
        var player = FindFirstObjectByType<PlayerMoveController>();
        if (player != null)
        {
            player.enabled = false;
        }

        //プレイヤー弾幕を止める
        var playerbullet = FindFirstObjectByType<PlayerShooter>();
        if (playerbullet != null)
        {
            playerbullet.canShoot = false;
            playerbullet.StopShooting();
        }

        var player2 = FindFirstObjectByType<PlayerController2>();
        if (player2 != null)
        {
            player2.enabled = false;
        }

        //弾幕停止
        var bullet = FindFirstObjectByType<Bullet>();
        if (bullet != null)
        {
            bullet.enabled = false;
        }

        var playerbullet2 = FindFirstObjectByType<PlayerControllerShoot2>();
        if (playerbullet2 != null)
        {
            playerbullet2.enabled = false;
        }

        //UI演出開始
        var ui = FindFirstObjectByType<GameOverUI>();
        if (ui != null)
        {
            ui.ShowGameOver(finalScore);
        }
    }
}