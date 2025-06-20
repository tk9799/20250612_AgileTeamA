using UnityEngine;

/// <summary>
/// ボスが全方位弾幕を定期的に発射するクラス
/// </summary>
public class BossOmnidirectionalBarrage : MonoBehaviour
{
    [Header("弾の基本設定")]
    [SerializeField] public BulletBase bulletBase;

    [Header("弾を撃つ間隔（秒）")]
    [SerializeField] public float fireInterval = 1.0f;

    [Header("全方位に撃つ弾数")]
    [SerializeField] public int bulletCount = 32;

    [Header("PatternManager参照")]
    [SerializeField] public PatternManager patternManager;

    [Header("BulletPoolManager参照")]
    [SerializeField] public BulletPoolManager poolManager;

    private float timer;

    void Awake()
    {
        enabled = false;
    }

    void OnEnable()
    {
        timer = fireInterval;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= fireInterval)
        {
            Fire();
            timer = 0f;

            //PatternManager に発射を報告
            patternManager?.ReportOmniBarrageFired();
        }
    }

    private void Fire()
    {
        var pool = poolManager.GetPool(bulletBase);

        if (pool == null)
        {
            return;
        }

        float angleStep = 360f / bulletCount;
        Vector2 pos = transform.position;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = angleStep * i;
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad));
            Bullet bullet = pool.GetBullet();
            bullet.Init(bulletBase, pos, dir.normalized * bulletBase.speed);
        }
    }
}