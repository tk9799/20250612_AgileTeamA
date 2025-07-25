using UnityEngine;

/// <summary>
/// 敵が横方向に直線移動するパターン
/// </summary>
[CreateAssetMenu(fileName = "LateralMovementPattern", menuName = "Barrage/Enemy/MovementPattern/LateralMovement")]
public class LateralMovementPattern : MovementPattern
{
    [Header("進行方向(trueなら右→左、falseなら左→右)")]
    public bool moveLeft = true;

    private float speed;

    public override void Initialize(EnemyController controller)
    {
        speed = controller.enemyData.moveSpeed;
    }

    public override void UpdateMovement(EnemyController controller)
    {
        Vector3 direction = moveLeft ? Vector3.left : Vector3.right;
        controller.transform.position += direction * speed * Time.deltaTime;
    }
}