using UnityEngine;

// クラスにはコメントを！！：八重樫
public class PlayerBullet : MonoBehaviour
{
    [Header("この弾のダメージ量")]
    public int damage = 1;　// bulletDamageのようにもう少し具体的な名前を：八重樫

    [Header("生存時間")]
    public float lifeTime = 5f;  // .0fを付けること！！：八重樫

    // メソッドにはコメントを！！：八重樫
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // メソッドにはコメントを！！：八重樫
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