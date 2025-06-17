using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ボスのHPを管理し、死亡時の弾の処理も行うクラス
/// </summary>
public class BossHP : MonoBehaviour
{
    [Header("初期のボスのHP")]
    [SerializeField] public int maxHP = 100;

    [Header("現在のボスのHP")]
    [SerializeField] private int currentHP;

    //無敵状態かどうか
    private bool isInvincible = false;

    //ボスが発射した、現在アクティブな弾のリスト
    private List<GameObject> activeBullets = new List<GameObject>();

    private void Start()
    {
        //ゲーム開始時にHPを初期化
        currentHP = maxHP;
    }

    /// <summary>
    /// 無敵状態の設定
    /// </summary>
    public void SetInvincible(bool value)
    {
        isInvincible = value;
    }

    /// <summary>
    /// ダメージを受けてHPを減少させ、0以下なら死亡処理
    /// </summary>
    public void TakeDamage(int damage)
    {
        if (isInvincible)
        {
            //無敵中はダメージ無視
            return;
        }

        currentHP -= damage;

        //HPが0以下になったら死亡処理
        if (currentHP <= 0)
        {
            currentHP = 0;
            OnDeath();
        }
    }

    /// <summary>
    /// 弾をアクティブなリストに登録
    /// </summary>
    public void RegisterBullet(GameObject bullet)
    {
        if (bullet != null)
        {
            activeBullets.Add(bullet);
        }
    }

    /// <summary>
    /// 弾をアクティブなリストから除外
    /// </summary>
    public void UnregisterBullet(GameObject bullet)
    {
        if (bullet != null)
        {
            activeBullets.Remove(bullet);
        }
    }

    /// <summary>
    /// ボスが死亡したときの処理
    /// </summary>
    private void OnDeath()
    {
        //ボス本体を非アクティブに
        gameObject.SetActive(false);

        //アクティブな弾をすべて無効化してプールに戻す
        foreach (var bullet in activeBullets)
        {
            if (bullet != null)
            {
                bullet.SetActive(false);
                BulletPool.Instance.ReturnBullet(bullet);
            }
        }
    }
}