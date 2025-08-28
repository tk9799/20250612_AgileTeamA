using UnityEngine;

/// <summary>
/// プレイヤーを中心に扇形弾を発射するパターン
/// </summary>
[CreateAssetMenu(menuName = "Barrage/Enemy/AttackPattern/FanSpreadAtPlayer")]
public class FanSpreadAtPlayerPattern : AttackPattern
{
    [Header("使用する弾データ")]
    public BulletBase bulletBase;

    [Header("発射モード")]
    public ShotMode shotMode = ShotMode.Loop;

    [Header("扇形の弾数")]
    public int bulletCount = 5;
    [Header(" 扇形の角度幅")]
    public float spreadAngle = 60f;

    [Header("1セットを何回繰り返すか")]
    public int setRepeatCount = 3;
    [Header("1セット内で何回撃つか")]
    public int shotsPerSet = 5;
    [Header("セット間の間隔")]
    public float intervalBetweenSets = 2f;
    [Header("セット内の発射間隔")]
    public float intervalWithinSet = 0.2f;

    private float timer = 0f;
    private int currentSet = 0;
    private int shotsInCurrentSet = 0;
    private bool isFiringSet = false;
    private bool finished = false;

    public override void Initialize(EnemyController controller)
    {
        timer = 0f;
        currentSet = 0;
        shotsInCurrentSet = 0;
        isFiringSet = false;
        finished = false;
    }

    public override void UpdateAttack(EnemyController controller)
    {
        if (bulletBase == null || finished || GameManager.Instance.IsGameClear || GameManager.Instance.IsGameOver)
            return;

        timer += Time.deltaTime;

        switch (shotMode)
        {
            case ShotMode.Loop:
                HandleLoop(controller);
                break;

            case ShotMode.OneShotSet:
                HandleOneShot(controller);
                break;

            case ShotMode.None:
                // 何もしない
                break;
        }
    }

    private void HandleLoop(EnemyController controller)
    {
        if (!isFiringSet)
        {
            if (timer >= intervalBetweenSets)
            {
                timer = 0f;
                isFiringSet = true;
                shotsInCurrentSet = 0;
            }
        }
        else
        {
            if (timer >= intervalWithinSet)
            {
                timer = 0f;
                Fire(controller);
                shotsInCurrentSet++;

                if (shotsInCurrentSet >= shotsPerSet)
                {
                    isFiringSet = false;
                }
            }
        }
    }

    private void HandleOneShot(EnemyController controller)
    {
        if (!isFiringSet && currentSet < setRepeatCount)
        {
            if (timer >= intervalBetweenSets)
            {
                timer = 0f;
                isFiringSet = true;
                shotsInCurrentSet = 0;
            }
        }
        else if (isFiringSet)
        {
            if (timer >= intervalWithinSet)
            {
                timer = 0f;
                Fire(controller);
                shotsInCurrentSet++;

                if (shotsInCurrentSet >= shotsPerSet)
                {
                    currentSet++;
                    isFiringSet = false;

                    if (currentSet >= setRepeatCount)
                    {
                        finished = true; // OneShot は終了
                    }
                }
            }
        }
    }

    private void Fire(EnemyController controller)
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Vector2 dirToPlayer = (player.transform.position - controller.transform.position).normalized;
        float centerAngle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;

        float startAngle = centerAngle - spreadAngle / 2f;
        float angleStep = (bulletCount > 1) ? spreadAngle / (bulletCount - 1) : 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + angleStep * i;
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            // 修正: FireBullet を使う
            Bullet bullet = BulletPoolManager.Instance.FireBullet(bulletBase, controller.transform.position, dir);

            if (bulletBase.shotSE != null)
                AudioSource.PlayClipAtPoint(bulletBase.shotSE, controller.transform.position);
        }
    }
}