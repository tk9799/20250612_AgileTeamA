using UnityEngine;

[CreateAssetMenu(menuName = "EnemyMovement/Sway")]
public class SwayMovementSettings : EnemyMovementSettings
{
    [Header("出現時の位置")]
    public Vector2 spawnPosition = new Vector2(8f, 0f);

    [Header("進行方向（例：Vector2.right, Vector2.downなど）")]
    public Vector2 moveDirection = Vector2.left;

    [Header("カーブの振れ幅（上下の幅）")]
    public float amplitude = 1f;

    [Header("カーブの周波数（揺れる速さ）")]
    public float frequency = 2f;
}