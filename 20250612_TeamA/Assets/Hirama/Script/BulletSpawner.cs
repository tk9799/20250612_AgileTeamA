using UnityEngine;

/// <summary>
/// 弾を生成・発射するクラス
/// </summary>
public class BulletSpawner : MonoBehaviour
{
    /// <summary>
    /// 指定した中心点から円形状に弾を発射する。
    /// </summary>
    /// <param name="center">弾の発射位置（中心点）</param>
    /// <param name="bulletCount">発射する弾の数</param>
    /// <param name="speed">弾の速度</param>
    /// <param name="lifetime">弾の寿命（秒）</param>
    /// <param name="owner">（オプション）弾の所有者。必要に応じて利用可能。</param>
    public void FireCircle(Vector3 center, int bulletCount, float speed, float lifetime, MonoBehaviour owner = null)
    {
        //360度を弾数で割り、各弾の間隔角度を算出
        float angleStep = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            //ごとの発射角度（度単位）を計算
            float angle = angleStep * i;

            //Z軸回転を使って角度から2D方向ベクトルを取得
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.right;

            //プールから弾を取得し、位置と回転を設定してアクティブ化
            GameObject bulletObj = BulletPool.Instance.GetBullet();
            bulletObj.transform.position = center;
            bulletObj.transform.rotation = Quaternion.identity;
            bulletObj.SetActive(true);

            //弾に移動方向・速度・寿命・所有者情報を設定
            Bullet bullet = bulletObj.GetComponent<Bullet>();
            bullet.direction = direction;
            bullet.BulletSpeed = speed;
            bullet.lifeTime = lifetime;
            bullet.owner = owner;

            //所有者がボスの場合、弾をボスの管理対象に追加
            if (owner is BossOmnidirectionalBarrage bossBarrage)
            {
                bossBarrage.AddBullet(bullet);
            }
        }
    }
}