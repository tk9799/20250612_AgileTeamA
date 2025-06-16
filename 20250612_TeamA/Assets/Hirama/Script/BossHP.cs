using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ボスのHPを管理し、死亡時の弾の処理も行うクラス
/// </summary>
public class BossHP : MonoBehaviour
{
    [Header("ボスのHP")]
    [SerializeField] public int maxHP = 100;

    //現在のHP
    private int currentHP;

    //ボスが発射した、現在アクティブな弾のリスト
    private List<GameObject> activeBullets = new List<GameObject>();

    private void Start()
    {
        //ゲーム開始時にHPを初期化
        currentHP = maxHP;
    }

    /// <summary>
    /// ダメージを受けてHPを減少させ、0以下なら死亡処理
    /// </summary>
    public void TakeDamage(int damage)
    {
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