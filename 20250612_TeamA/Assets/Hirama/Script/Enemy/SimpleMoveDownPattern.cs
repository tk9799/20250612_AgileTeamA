using UnityEngine;

/// <summary>
/// 敵が上から下に移動する移動パターン
/// </summary>
[CreateAssetMenu(fileName = "SimpleMoveDownPattern", menuName = "Barrage/Enemy/MovementPattern/SimpleMoveDown")]
public class SimpleMoveDownPattern : MovementPattern
{
    private float speed;

    public override void Initialize(EnemyController controller)
    {
        //EnemyDataから速度を取得
        speed = controller.enemyData.moveSpeed;
    }

    public override void UpdateMovement(EnemyController controller)
    {
        //現在位置から下方向へ移動
        controller.transform.position += Vector3.down * speed * Time.deltaTime;
    }
}