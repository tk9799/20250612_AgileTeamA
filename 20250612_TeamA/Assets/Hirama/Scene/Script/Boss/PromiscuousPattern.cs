using UnityEngine;

/// <summary>
/// ボス専用の全方位無差別攻撃パターン
/// </summary>
[CreateAssetMenu(menuName = "Barrage/Boss/AttackPattern/Promiscuous")]
public class PromiscuousPattern : BossAttackPattern
{
    [Header("使用する弾")]
    public BulletBase bulletBase;

    [Header("1セット内の弾数")]
    public int bulletsPerSet = 12;
    [Header("セット間の待機時間")]
    public float intervalBetweenSets = 2f;

    [Header("弾の初速")]
    public float initialSpeed = 1f;
    [Header("弾の加速度(正なら加速、負なら減速)")]
    public float acceleration = -0.5f;

    private float timer = 0f;
    private bool isFiringSet = false;

    public override void Initialize(BossController bossController)
    {
        timer = 0f;
        isFiringSet = false;
    }

    public override void UpdateAttack(BossController bossController)
    {
        if (bulletBase == null || bossController == null)
            return;

        timer += Time.deltaTime;

        if (!isFiringSet)
        {
            //セット開始待機
            if (timer >= intervalBetweenSets)
            {
                timer = 0f;
                isFiringSet = true;

                FireSet(bossController);
                isFiringSet = false;
            }
        }
    }

    /// <summary>
    /// 1セット分の全方位弾を発射
    /// </summary>
    private void FireSet(BossController bossController)
    {
        float angleStep = 360f / bulletsPerSet;

        for (int i = 0; i < bulletsPerSet; i++)
        {
            float angle = angleStep * i;
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            //弾を取得して初期化
            Bullet bullet = BulletPoolManager.Instance.FireBullet(bulletBase, bossController.transform.position, dir);

            if (bullet != null)
            {
                //初期速度と加速度を設定
                bullet.SetInitialSpeed(initialSpeed);
                bullet.acceleration = acceleration;
            }

            if (bulletBase.shotSE != null)
                AudioSource.PlayClipAtPoint(bulletBase.shotSE, bossController.transform.position);
        }
    }

    public override void OnExit(BossController bossController)
    {
    }

    public override bool IsFinished() => false;
}