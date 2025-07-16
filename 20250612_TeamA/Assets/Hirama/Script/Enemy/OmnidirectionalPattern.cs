using UnityEngine;

/// <summary>
/// 敵が全方位弾を発射する攻撃パターン
/// </summary>
[CreateAssetMenu(fileName = "OmnidirectionalPattern", menuName = "Barrage/EnemyAttackPattern/Omnidirectional")]
public class OmnidirectionalPattern : EnemyAttackPattern
{
    public enum ShotMode
    {
        //通常ループ発射
        Loop,
        //1回限りの1セット発射
        OneShotSet,
        //発射しない
        None,
    }

    [Header("発射モード")]
    public ShotMode shotMode = ShotMode.Loop;

    [Header("弾性能データ")]
    public BulletBase bulletBase;

    [Header("1セットあたりの全方位弾の数")]
    public int bulletsPerCircle = 8;

    [Header("1セット内で何回連続発射するか")]
    public int burstCount = 1;

    [Header("1発ごとの間隔(秒)")]
    public float intervalBetweenBursts = 0.3f;

    [Header("通常ループ発射でのセット間待機時間(秒)")]
    public float waitTimeBetweenSets = 2.0f;

    [Header("1回限りの1セット発射での開始前待機時間(秒)")]
    public float waitTimeBeforeOneShotSet = 2.0f;

    private EnemyController controller;
    private float shotTimer = 0f;
    private int burstsFired = 0;
    private bool hasFiredOneShotSet = false;
    private float waitTimer = 0f;
    private bool isWaitingBeforeOneShotSet = false;

    public override void Initialize(EnemyController controller)
    {
        this.controller = controller;
        shotTimer = 0f;
        burstsFired = 0;
        waitTimer = 0f;
        hasFiredOneShotSet = false;

        if (bulletBase == null && controller.enemyData.bulletBase != null)
        {
            bulletBase = controller.enemyData.bulletBase;
        }

        if (shotMode == ShotMode.OneShotSet)
        {
            isWaitingBeforeOneShotSet = true;
        }
    }

    public override void UpdateAttack(EnemyController controller)
    {
        if (shotMode == ShotMode.None || bulletBase == null)
        {
            return;
        }

        //OneShotSetの待機処理
        if (shotMode == ShotMode.OneShotSet)
        {
            if (hasFiredOneShotSet)
            {
                return;
            }

            if (isWaitingBeforeOneShotSet)
            {
                waitTimer += Time.deltaTime;
                if (waitTimer < waitTimeBeforeOneShotSet)
                {
                    return;
                }

                isWaitingBeforeOneShotSet = false;
                waitTimer = 0f;
            }
        }

        //発射タイマー更新
        shotTimer += Time.deltaTime;

        if (burstsFired < burstCount)
        {
            if (shotTimer >= intervalBetweenBursts)
            {
                shotTimer = 0f;
                FireRadialShot();
                burstsFired++;
            }
        }
        else
        {
            //発射が終わった後の処理
            if (shotMode == ShotMode.Loop)
            {
                waitTimer += Time.deltaTime;
                if (waitTimer >= waitTimeBetweenSets)
                {
                    waitTimer = 0f;
                    burstsFired = 0;
                }
            }
            else if (shotMode == ShotMode.OneShotSet)
            {
                hasFiredOneShotSet = true;
            }
        }
    }

    /// <summary>
    /// 弾を全方位に向けて発射
    /// </summary>
    private void FireRadialShot()
    {
        Vector2 center = controller.transform.position;

        for (int i = 0; i < bulletsPerCircle; i++)
        {
            float angleDeg = 360f * i / bulletsPerCircle;
            float rad = angleDeg * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            Bullet bullet = BulletPoolManager.Instance.FireBullet(bulletBase, center, dir);
            if (bullet == null)
            {
                Debug.LogWarning("Bullet生成失敗：BulletPoolManagerに未設定のBulletBaseがあります");
            }
        }
    }
}