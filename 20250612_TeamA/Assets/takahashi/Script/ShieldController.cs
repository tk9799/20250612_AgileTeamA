using UnityEngine;

public class ShieldController : MonoBehaviour
{
    [SerializeField] CircleCollider2D circleCollider2D;
    [SerializeField] GuardController2 guardController2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("generate");
        if (collision.gameObject.CompareTag("Bullet"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}
