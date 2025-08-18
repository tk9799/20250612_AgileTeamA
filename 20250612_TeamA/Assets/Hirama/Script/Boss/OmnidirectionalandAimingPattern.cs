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

    [Header("自機狙い弾")]
    [Header("自機狙い弾のデータ")]
    public BulletBase aimBulletBase;
    [Header("1セットあたりの自機狙い弾の数")]
    public int aimBulletsPerSet = 5;
    [Header("1セットで何回自機狙い弾を撃つか")]
    public int aimShotsPerSet = 4;
    [Header("自機狙い弾の連射間隔")]
    public float aimShotInterval = 0.3f;

    [Header("セット切り替え")]
    [Header("全方位弾のセットを何回撃ったら自機狙い弾に切り替えるか")]
    public int radialSetsBeforeAim = 2;
    [Header("自機狙い弾のセットを何回撃ったら全方位弾に戻るか")]
    public int aimSetsBeforeRadial = 3;

    private BossController bossController;

    private enum AttackPhase
    {
        Radial,
        Aim
    }

    private AttackPhase currentPhase = AttackPhase.Radial;

    private int setsFiredInPhase = 0;
    private int shotsFiredInSet = 0;
    private float shotTimer = 0f;
    private bool clockwise = true;

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
                    //1セット終了
                    shotsFiredInSet = 0;
                    setsFiredInPhase++;

                    //回転方向切り替え
                    clockwise = !clockwise;

                    if (setsFiredInPhase >= radialSetsBeforeAim)
                    {
                        //自機狙いフェーズに切り替え
                        currentPhase = AttackPhase.Aim;
                        setsFiredInPhase = 0;
                        shotsFiredInSet = 0;
                        shotTimer = 0f;
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
                    //1セット終了
                    shotsFiredInSet = 0;
                    setsFiredInPhase++;

                    if (setsFiredInPhase >= aimSetsBeforeRadial)
                    {
                        //全方位フェーズに戻る
                        currentPhase = AttackPhase.Radial;
                        setsFiredInPhase = 0;
                        shotsFiredInSet = 0;
                        shotTimer = 0f;

                        //次回の全方位は今の回転方向を維持しない
                        clockwise = !clockwise;
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
            float angle = angleStep * i;

            if (!clockwise)
            {
                angle = 360f - angle;
            }

            Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.up;

            // Instantiateの代わりにBulletPoolManagerを使う
            BulletPoolManager.Instance.FireBullet(radialBulletBase, origin, dir);
        }
    }

    private void FireAim()
    {
        Vector3 origin = bossController.transform.position;
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Vector3 playerPos = player.transform.position;
        Vector2 dir = (playerPos - origin).normalized;

        for (int i = 0; i < aimBulletsPerSet; i++)
        {
            BulletPoolManager.Instance.FireBullet(aimBulletBase, origin, dir);
        }
    }

    public override bool IsFinished()
    {
        return false;
    }

    public override void OnExit(BossController controller)
    {
        // 攻撃終了時の後処理があればここ
    }
}