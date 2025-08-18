using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// プレイヤーのHP管理・被弾・復活・無敵処理
/// </summary>
public class PlayerHP : MonoBehaviour
{
    [Header("残機")]
    public int lifeCount = 3;

    [Header("復活位置")]
    public Vector3 respawnPosition = new Vector3(0, -3, 0);

    [Header("死亡後の待機時間（秒）")]
    public float respawnDelay = 0.5f;

    [Header("復活後の無敵時間（秒）")]
    public float invincibleTime = 2f;

    //残機が変わったとき通知
    public event Action<int> OnLifeChanged;

    //残機0になったとき通知
    public event Action OnPlayerDied;

    // 無敵状態中フラグ
    private bool isInvincible = false;

    private SpriteRenderer sr;
    private Collider2D hitCollider;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        hitCollider = GetComponent<Collider2D>();
        transform.position = respawnPosition;

        OnLifeChanged?.Invoke(lifeCount);
    }

    /// <summary>
    /// 被弾時に外部から呼び出される（弾や敵から）
    /// </summary>
    public void TakeDamage()
    {
        //無敵中は無視
        if (isInvincible)
        {
            return;
        }

        Die();
    }

    /// <summary>
    /// 残機を1減らし、ゲームオーバーか復活処理へ
    /// </summary>
    private void Die()
    {
        //弾幕停止
        var shooter = GetComponent<PlayerShooter>();
        if (shooter != null)
        {
            shooter.StopShooting();
        }

        lifeCount--;
        OnLifeChanged?.Invoke(lifeCount);

        if (lifeCount > 0)
        {
            Debug.Log("プレイヤー被弾、残機あり → 復活予定");
            StartCoroutine(Respawn());
        }
        else
        {
            Debug.Log("プレイヤーがやられた！残機なし → ゲームオーバー");
            OnPlayerDied?.Invoke();
            GameManager.Instance?.GameOver();
            //必要なら敵停止などもここで呼び出す
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 一定時間後にリスポーン＋無敵状態へ
    /// </summary>
    private IEnumerator Respawn()
    {
        //一時的に非表示・当たり判定無効
        sr.enabled = false;
        hitCollider.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        //復活位置へ移動・見えるように戻す
        transform.position = respawnPosition;
        sr.enabled = true;
        hitCollider.enabled = true;

        //無敵開始
        StartCoroutine(Invincibility(invincibleTime));
    }

    /// <summary>
    /// 無敵状態中は当たり判定オフ＋点滅演出
    /// </summary>
    private IEnumerator Invincibility(float duration)
    {
        isInvincible = true;
        hitCollider.enabled = false;

        float timer = 0f;
        while (timer < duration)
        {
            //点滅
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }

        //無敵終了
        sr.enabled = true;
        hitCollider.enabled = true;
        isInvincible = false;
    }
}