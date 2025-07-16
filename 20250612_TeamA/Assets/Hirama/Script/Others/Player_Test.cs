using UnityEngine;

public class Player_Test : MonoBehaviour
{
    [Header("移動速度")]
    public float moveSpeed = 5f;

    [Header("画面端の移動制限")]
    public float minX = -12f;
    public float maxX = 12f;
    public float minY = -6f;
    public float maxY = 6f;

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector2 input = new Vector2(h, v).normalized;
        Vector2 newPosition = (Vector2)transform.position +
            input * moveSpeed * Time.deltaTime;

        // 画面端の制限（任意）
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        transform.position = newPosition;
    }
}