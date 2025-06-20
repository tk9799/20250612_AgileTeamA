using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [Header("この弾のダメージ量")]
    [SerializeField] public int damage = 1;

    [Header("生存時間")]
    [SerializeField] public float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Enemyにヒットしたら
        if (other.CompareTag("Enemy"))
        {
            // 相手がIDamageableを持っていればダメージを与える
            IDamageable target = other.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}