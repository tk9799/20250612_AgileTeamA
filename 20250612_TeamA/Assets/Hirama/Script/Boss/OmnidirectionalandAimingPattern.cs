using UnityEngine;

[CreateAssetMenu(menuName = "Barrage/Boss/AttackPattern/OmnidirectionalandAiming")]
public class OmnidirectionalandAimingPattern : BossAttackPattern
{
    [Header("全方位弾")]
    [Header("全方位弾のデータ")]
    public BulletBase radialBulletBase;
    [Header("1セットあたりの全方位弾の数")]
    public int radialBulletsPerSet = 12;
    [Header("1セットで何回全方位弾を撃つか")]
    public int radialShotsPerSet = 3;
    [Header("全方位弾の連射間隔")]
    public float radialShotInterval = 0.2f;
    [Header("螺旋角度ずれ")]
    public float rotationStepPerShot = 10f;
    [Header("全方位弾のセット間隔")]
    public float radialSetInterval = 1.0f;

    [Header("自機狙い弾")]
    [Header("自機狙い弾のデータ")]
    public BulletBase aimBulletBase;
    [Header("1セットあたりの自機狙い弾の数")]
    public int aimBulletsPerSet = 5;
    [Header("1セットで何回自機狙い弾を撃つか")]
    public int aimShotsPerSet = 4;
    [Header("自機狙い弾の連射間隔")]
    public float aimShotInterval = 0.3f;
    [Header("自機狙い弾のセット間隔")]
    public float aimSetInterval = 1.0f;

    [Header("セット切り替え")]
    [Header("全方位弾のセットを何回撃ったら自機狙い弾に切り替えるか")]
    public int radialSetsBeforeAim = 3;
    [Header("自機狙い弾のセットを何回撃ったら全方位弾に戻るか")]
    public int aimSetsBeforeRadial = 3;

    private BossController bossController;

    private AttackPhase currentPhase = AttackPhase.Radial;

    private int setsFiredInPhase = 0;
    private int shotsFiredInSet = 0;
    private float shotTimer = 0f;
    private bool clockwise = true;

    private float radialRotationOffset = 0f;

    private enum AttackPhase
    {
        Radial,
        Aim
    }

    public override void Initialize(BossController controller)
    {
        bossController = controller;
        currentPhase = AttackPhase.Radial;
        setsFiredInPhase = 0;
        shotsFiredInSet = 0;
        shotTimer = 0f;
        clockwise = true;
    }

    public override void UpdateAttack(BossController controller)
    {
        shotTimer += Time.deltaTime;

        switch (currentPhase)
        {
            case AttackPhase.Radial:
                if (shotsFiredInSet < radialShotsPerSet)
                {
                    if (shotTimer >= radialShotInterval)
                    {
                        FireRadial();
                        shotsFiredInSet++;
                        shotTimer = 0f;
                    }
                }
                else
                {
                    shotsFiredInSet = 0;
                    setsFiredInPhase++;
                    shotTimer = -radialSetInterval;

                    //セットごとに回転方向を反転
                    clockwise = !clockwise;

                    if (setsFiredInPhase >= radialSetsBeforeAim)
                    {
                        currentPhase = AttackPhase.Aim;
                        setsFiredInPhase = 0;
                        shotsFiredInSet = 0;
                        shotTimer = -aimSetInterval;
                        //shotTimer = 0f;
                    }
                }
                break;

            case AttackPhase.Aim:
                if (shotsFiredInSet < aimShotsPerSet)
                {
                    if (shotTimer >= aimShotInterval)
                    {
                        FireAim();
                        shotsFiredInSet++;
                        shotTimer = 0f;
                    }
                }
                else
                {
                    shotsFiredInSet = 0;
                    setsFiredInPhase++;

                    if (setsFiredInPhase >= aimSetsBeforeRadial)
                    {
                        currentPhase = AttackPhase.Radial;
                        setsFiredInPhase = 0;
                        shotsFiredInSet = 0;
                        shotTimer = 0f;
                        //Aimフェーズから戻る時は回転方向は維持
                    }
                }
                break;
        }
    }

    private void FireRadial()
    {
        Vector3 origin = bossController.transform.position;

        for (int i = 0; i < radialBulletsPerSet; i++)
        {
            float angleStep = 360f / radialBulletsPerSet;
            float angle = angleStep * i + radialRotationOffset;

            if (!clockwise)
            {
                angle = 360f - angle;
            }

            Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.up;
            BulletPoolManager.Instance.FireBullet(radialBulletBase, origin, dir);
        }

        //次のショットでは少し回転
        radialRotationOffset += clockwise ? rotationStepPerShot : -rotationStepPerShot;
    }

    private void FireAim()
    {
        Vector3 origin = bossController.transform.position;
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Vector2 dir = (player.transform.position - origin).normalized;

        for (int i = 0; i < aimBulletsPerSet; i++)
        {
            BulletPoolManager.Instance.FireBullet(aimBulletBase, origin, dir);
        }
    }

    public override bool IsFinished() => false;
    public override void OnExit(BossController controller) { }
}