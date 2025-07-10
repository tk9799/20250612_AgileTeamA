//using UnityEngine;

///// <summary>
///// 敵の攻撃制御（通常ループ / 即時発射 / 発射なし）
///// </summary>
//public class EnemyAttack : MonoBehaviour
//{
//    public Transform firePoint;
//    public BulletPoolManager bulletPoolManager;
//    public float fireInterval = 2f;
//    public float bulletSpeed = 5f;

//    private float timer;
//    private Transform player;
//    private EnemyBase owner;
//    private bool hasBurstFired = false;

//    private void Awake()
//    {
//        player = GameObject.FindWithTag("Player")?.transform;
//        owner = GetComponent<EnemyBase>();
//    }

//    private void OnEnable()
//    {
//        timer = 0f;
//        hasBurstFired = false;
//    }

//    void Update()
//    {
//        if (owner == null || owner.enemyData == null || bulletPoolManager == null || player == null) return;

//        switch (owner.enemyData.attackPattern)
//        {
//            case AttackPatternType.Loop:
//                timer += Time.deltaTime;
//                if (timer >= fireInterval)
//                {

//                    //FireBullet();
//                    timer = 0f;
//                }
//                break;

//            case AttackPatternType.Burst:
//                if (!hasBurstFired)
//                {
//                    //FireBullet();
//                    hasBurstFired = true;
//                }
//                break;

//            case AttackPatternType.None:
//                break;
//        }
//    }

//    /// <summary>
//    /// プレイヤー方向に弾を1発発射
//    /// </summary>
//    //private void FireBullet()
//    //{
//    //    if (firePoint == null)
//    //    {
//    //        return;
//    //    }

//    //    Vector3 dir = (player.position - firePoint.position).normalized;

//    //    bulletPoolManager.ShootBullet(
//    //        BulletType.Enemy,
//    //        firePoint.position,
//    //        dir,
//    //        bulletSpeed
//    //    );

//    //    Debug.Log("[EnemyAttack] 弾を発射しました");
//    //}
//}