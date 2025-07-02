using UnityEngine;

[CreateAssetMenu(menuName = "EnemyMovement/Diagonal")]
public class DiagonalMovementSettings : EnemyMovementSettings
{
    [Header("出現位置(X,Y)")]
    public Vector2 spawnPosition = new Vector2(0f, 5f);

    [Header("移動方向（右下は1,-1、左下は-1,-1など）")]
    public Vector2 moveDirection = new Vector2(1f, -1f);
}