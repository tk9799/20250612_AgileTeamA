using UnityEngine;

/// <summary>
/// プレイヤーに向けて扇形弾幕を発射するクラス
/// </summary>
public class BossFanBarrage : MonoBehaviour
{
    [Header("弾の基本設定")]
    public BulletBase bulletBase;

    [Header("弾を撃つ間隔（秒）")]
    public float fireInterval = 1.0f;

    [Header("1回の発射で撃つ弾の数")]
    public int bulletCount = 3;

    [Header("扇の角度")]
    public float fanAngle = 30f;

    [Header("ターゲットはプレイヤー")]
    public Transform target;

    [Header("PatternManager参照")]
    public PatternManager patternManager;

    [Header("BulletPoolManager参照")]
    public BulletPoolManager poolManager;


    private float timer;

    private bool hasFired = false;

    void Awake()
    {
        //最初は無効化（BossEntranceなどから制御）
        enabled = false;
    }

    void Start()
    {
        timer = 0f;
    }

    void OnEnable()
    {
        timer = 0f;
        hasFired = false;
    }

    void Update()
    {
        if (target == null || hasFired)
        {
            return;
        }

        timer += Time.deltaTime;
        if (timer >= fireInterval)
        {
            Fire();
            timer = 0f;

            patternManager?.ReportFanBarrageFired();
        }
    }

    private void Fire()
    {
        var pool = poolManager.GetPool(bulletBase);
        if (pool == null) return;

        Vector2 position = transform.position;
        Vector2 toPlayer = (target.position - transform.position).normalized;
        float baseAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;

        float angleStart = baseAngle - fanAngle / 2f;
        float angleStep = bulletCount > 1 ? fanAngle / (bulletCount - 1) : 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = angleStart + angleStep * i;
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad));

            Bullet bullet = pool.GetBullet();
            bullet.Init(bulletBase, position, dir.normalized * bulletBase.speed);
        }
    }
}