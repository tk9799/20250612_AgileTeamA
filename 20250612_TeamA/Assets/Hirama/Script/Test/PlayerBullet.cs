using UnityEngine;

/// <summary>
/// レイヤーの弾の挙動を制御する
/// </summary>
public class PlayerBullet : MonoBehaviour
{
    [Header("弾の移動速度")]
    [SerializeField] private float speed = 10f;

    [Header("弾の寿命（秒）")]
    [SerializeField] private float lifetime = 2f;

    [Header("与えるダメージ")]
    [SerializeField] private int damage = 1;

    private float timer;

    private void OnEnable()
    {
        //弾が生成または再利用されたときにタイマーをリセット
        timer = 0f;
    }

    private void Update()
    {
        //弾を上方向に直進させる
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        //寿命を超えたら削除
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //ボスに当たったらダメージを与えて削除
        if (other.CompareTag("Boss"))
        {
            var boss = other.GetComponent<BossHP>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}