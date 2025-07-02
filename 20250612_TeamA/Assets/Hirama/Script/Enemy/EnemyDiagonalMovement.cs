using UnityEngine;

/// <summary>
/// 斜め移動のような動きで移動する敵用スクリプト
/// </summary>
public class EnemyDiagonalMovement : MonoBehaviour
{
    [Header("移動速度")]
    public float moveSpeed = 3f;

    [Header("出現位置(X,Y)")]
    public Vector2 spawnPosition = new Vector2(0f, 5f);

    [Header("移動方向（右下は１，-1　左下は-1,-1）")]
    public Vector2 moveDirection = new Vector2(1f, -1f);

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize(DiagonalMovementSettings settings)
    {
        moveSpeed = settings.moveSpeed;
        spawnPosition = settings.spawnPosition;
        moveDirection = settings.moveDirection;

        transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0f);
    }

    void Update()
    {
        Vector3 delta = (Vector3)moveDirection.normalized * moveSpeed * Time.deltaTime;
        transform.position += delta;
    }
}