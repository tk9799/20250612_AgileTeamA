using UnityEngine;
using System.Collections;

/// <summary>
/// ボス専用の全方位無差別攻撃パターン
/// </summary>
[CreateAssetMenu(menuName = "Barrage/Boss/AttackPattern/Promiscuous")]
public class PromiscuousPattern : BossAttackPattern
{
    [Header("使用する弾")]
    public BulletBase bulletBase;

    [Header("初回発射までの待機時間")]
    public float initialDelay = 1f;

    [Header("1セット内の弾数")]
    public int bulletsPerSet = 8;
    [Header("セット内の弾の連射間隔")]
    public float intervalBetweenBullets = 0.1f;
    [Header("セット間の待機時間")]
    public float intervalBetweenSets = 2f;

    [Header("弾の散らす角度の幅(±)")]
    public float spreadAngle = 15f;

    private bool isFiring = false;
    private float timer = 0f;

    public override void Initialize(BossController bossController)
    {
        timer = -initialDelay;
        isFiring = false;
    }

    public override void UpdateAttack(BossController bossController)
    {
        if (bulletBase == null || bossController == null)
            return;

        timer += Time.deltaTime;

        if (!isFiring && timer >= 0f)
        {
            isFiring = true;
            timer = 0f;
            bossController.StartCoroutine(FireSet(bossController));
        }
    }

    private IEnumerator FireSet(BossController bossController)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            isFiring = false;
            yield break; //プレイヤーが存在しなければ発射しない
        }

        for (int i = 0; i < bulletsPerSet; i++)
        {
            if (bulletBase == null)
                yield break;

            Vector2 direction = ((Vector2)player.transform.position - (Vector2)bossController.transform.position).normalized;

            //角度をランダムに散らす
            float angleOffset = Random.Range(-spreadAngle, spreadAngle);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angleOffset;
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            Bullet bullet = BulletPoolManager.Instance.FireBullet(bulletBase, bossController.transform.position, dir);

            if (bulletBase.shotSE != null)
                AudioSource.PlayClipAtPoint(bulletBase.shotSE, bossController.transform.position);

            yield return new WaitForSeconds(intervalBetweenBullets);
        }

        //セット間待機
        yield return new WaitForSeconds(intervalBetweenSets);
        isFiring = false;
    }

    public override void OnExit(BossController bossController)
    {
    }

    public override bool IsFinished() => false;
}