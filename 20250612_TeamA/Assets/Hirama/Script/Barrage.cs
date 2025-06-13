using UnityEngine;

public class Barrage : MonoBehaviour
{
    private Rigidbody2D rb;
    private BarragePool pool;
    private float lifeTime;
    private BossOmnidirectionalBarrage owner;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 direction,
        float speed,
        float life,
        BarragePool poolRef,
        BossOmnidirectionalBarrage ownerRef)
    {
        pool = poolRef;
        lifeTime = life;
        owner = ownerRef;
        rb.linearVelocity = direction * speed;

        owner?.AddBullet(this);

        CancelInvoke();
        Invoke(nameof(ReturnToPool), lifeTime);
    }

    void ReturnToPool()
    {
        rb.linearVelocity = Vector2.zero;
        owner?.RemoveBullet(this);
        pool.ReturnBullet(gameObject);
    }
}