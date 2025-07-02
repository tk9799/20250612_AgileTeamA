using UnityEngine;

[CreateAssetMenu(menuName = "EnemyMovement/SideMovement")]
public class SideMovementSettings : EnemyMovementSettings
{
    [Header("出現時の位置")]
    public Vector2 fixedPosition = new Vector2(12f, 5f);

    [Header("進行方向(trueなら右→左、falseなら左→右)")]
    public bool fromRightToLeft = true;
}