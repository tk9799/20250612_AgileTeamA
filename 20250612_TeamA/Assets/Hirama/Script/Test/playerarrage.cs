using UnityEngine;

//後でこのスクリプトを絶対消す

public class playerarrage : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f;
    public int damage = 10;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss"))
        {
            BossHP boss = other.GetComponent<BossHP>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }

            Destroy(gameObject); // 弾を消す
        }
    }
}