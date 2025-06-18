using System.Linq;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [Header("初期残り残機")]
    [SerializeField] private int lives = 3;

    [Header("復活後の無敵時間")]
    [SerializeField] private float invincibilityTime = 2f;

    //死亡中かどうか（死亡処理の多重実行を防ぐ）
    private bool isDead = false;

    //無敵状態かどうか（復活直後に即死しないようにする）
    private bool isInvincible = false;

    /// <summary>
    /// プレイヤーが死亡したときに呼ばれる
    /// </summary>
    public void Die()
    {
        //すでに死亡中、または無敵中なら何もしない
        if (isDead || isInvincible)
        {
            return;
        }

        //死亡状態に設定
        isDead = true;

        //残機を1つ減らす
        lives--;

        Debug.Log($"プレイヤーが死んだ。残り残機: {lives}");

        //レイヤーオブジェクトを一時的に非表示・無効化
        gameObject.SetActive(false);

        //残機が残っていれば復活処理を行う
        if (lives > 0)
        {
            //1秒後に Respawn() を呼び出す
            Invoke(nameof(Respawn), 1f);
        }
        else
        {
            //シーン内のボス弾幕コンポーネントを検索して弾幕停止
            var boss = FindFirstObjectByType<BossOmnidirectionalBarrage>();
            if (boss != null)
            {
                boss.StopBarrage();
            }

            //シーン内のボス移動コンポーネントを検索して移動停止
            var bossMovement = FindFirstObjectByType<BossMovement>();
            if (bossMovement != null)
            {
                bossMovement.StopMovement();
            }

            Debug.Log("ゲームオーバー");
        }
    }

    /// <summary>
    /// プレイヤーが復活するときに呼ばれる
    /// </summary>
    private void Respawn()
    {
        //死亡状態を解除
        isDead = false;

        //プレイヤーの位置を初期位置に戻す
        transform.position = new Vector3(0, -3, 0);

        //プレイヤーを再表示・再有効化
        gameObject.SetActive(true);

        //一定時間無敵状態にする
        isInvincible = true;
        Invoke(nameof(EndInvincibility), invincibilityTime);

        Debug.Log("プレイヤーが復活した（無敵状態）");
    }

    /// <summary>
    /// 無敵状態を終了する
    /// </summary>
    private void EndInvincibility()
    {
        //無敵状態を解除
        isInvincible = false;
        Debug.Log("無敵終了");
    }
}