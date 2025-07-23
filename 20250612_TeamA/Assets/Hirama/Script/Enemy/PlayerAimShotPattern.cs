using UnityEngine;
/// <summary>
/// プレイヤー狙いの弾を発射する攻撃パターン
/// </summary>
[CreateAssetMenu(fileName = "PlayerAimShotPattern", menuName = "Barrage/AttackPattern/PlayerAimShot")]
public class PlayerAimShotPattern : AttackPattern
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

    [Header("1セット内の弾数")]
    public int bulletsPerSet = 1;

    [Header("1発ごとの間隔(秒)")]
    public float intervalBetweenShots = 0.3f;

    [Header("通常ループ発射でのセット間待機時間(秒)")]
    public float waitTimeBetweenSets = 2.0f;

    [Header("1回限りの1セット発射での開始前待機時間(秒)")]
    public float waitTimeBeforeOneShotSet = 2.0f;

    private EnemyController controller;
    private Transform playerTransform;

    private float shotTimer;
    private int bulletsFiredInSet;
    private bool hasFiredOneShotSet;
    private float waitTimer;

    private bool isWaitingBeforeOneShotSet = false;

    public override void Initialize(EnemyController controller)
    {
        this.controller = controller;
        shotTimer = 0f;
        bulletsFiredInSet = 0;
        hasFiredOneShotSet = false;
        waitTimer = 0f;

        if (bulletBase == null && controller.enemyData.bulletBase != null)
        {
            bulletBase = controller.enemyData.bulletBase;
        }

        if (shotMode == ShotMode.OneShotSet)
        {
            isWaitingBeforeOneShotSet = true;
            waitTimer = 0f;
        }
        else
        {
            isWaitingBeforeOneShotSet = false;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("Playerが見つかりません。Playerはタグ 'Player' を付けてください。");
        }
    }

    public override void UpdateAttack(EnemyController controller)
    {
        //Debug.Log("EnemyPlayerAimShotPattern UpdateAttack");
        if (shotMode == ShotMode.None)
        {
            return;
        }

        if (shotMode == ShotMode.OneShotSet)
        {
            if (hasFiredOneShotSet)
            {
                //発射済みなら何もしない
                return;
            }

            if (isWaitingBeforeOneShotSet)
            {
                waitTimer += Time.deltaTime;
                if (waitTimer >= waitTimeBeforeOneShotSet)
                {
                    isWaitingBeforeOneShotSet = false;
                    waitTimer = 0f;
                }
                else
                {
                    return;//待機中は発射しない
                }
            }
        }

        shotTimer += Time.deltaTime;

        if (bulletsFiredInSet < bulletsPerSet)
        {
            if (shotTimer >= intervalBetweenShots)
            {
                shotTimer = 0f;
                FireBullet();
                bulletsFiredInSet++;
            }
        }
        else
        {
            //1セット分撃ち終わったら待機時間に入る
            waitTimer += Time.deltaTime;
            if (shotMode == ShotMode.Loop)
            {
                if (waitTimer >= waitTimeBetweenSets)
                {
                    waitTimer = 0f;
                    bulletsFiredInSet = 0;
                }
            }
            else if (shotMode == ShotMode.OneShotSet)
            {
                hasFiredOneShotSet = true;
                waitTimer = 0f;
            }
        }
    }

    private void FireBullet()
    {
        if (bulletBase == null)
        {
            Debug.LogWarning("bulletBaseが設定されていません");
            return;
        }

        Vector3 firePosition = controller.transform.position;

        //プレイヤーがいなければ下方向に撃つ
        Vector3 dirToPlayer = Vector3.down;
        if (playerTransform != null)
        {
            dirToPlayer = (playerTransform.position - firePosition).normalized;
        }

        float angle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0, 0, angle - 90f);

        Bullet bullet = BulletPoolManager.Instance.FireBullet(bulletBase, firePosition, dirToPlayer);
        if (bullet == null)
        {
            Debug.LogWarning("Bulletの生成に失敗しました。BulletPoolManagerに設定がありますか？");
        }
    }
}