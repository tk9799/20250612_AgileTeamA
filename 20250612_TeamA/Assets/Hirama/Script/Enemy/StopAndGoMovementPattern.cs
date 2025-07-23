using UnityEngine;

/// <summary>
/// 目標位置まで移動→停止→一定時間後に別方向へ移動するパターン
/// </summary>
[CreateAssetMenu(fileName = "StopAndGoPattern", menuName = "Barrage/MovementPattern/StopAndGo")]
public class StopAndGoMovementPattern : MovementPattern
{
    [Header("最初に向かう目標位置")]
    public Vector2 targetPosition;

    [Header("停止時間(秒)")]
    public float stopDuration = 2f;

    [Header("停止後の移動方向(ベクトル)")]
    public Vector2 moveAfterStopDirection = Vector2.down;

    //private EnemyController controller;
    //private EnemyBase enemyBase;

    private float speed;
    private float stopTimer;
    private Phase currentPhase;

    private enum Phase
    {
        //目標位置に移動中
        MovingToTarget,
        //停止中
        Stopping,
        //停止後の移動中
        MovingAfterStop
    }

    //private Phase currentPhase = Phase.MovingToTarget;
    //private float stopTimer = 0f;

    public override void Initialize(EnemyController controller)
    {
        //this.controller = controller;
        //this.enemyBase = controller.GetComponent<EnemyBase>();

        speed = controller.enemyData.moveSpeed;
        currentPhase = Phase.MovingToTarget;
        stopTimer = 0f;
    }

    public override void UpdateMovement(EnemyController controller)
    {
        Vector3 currentPos = controller.transform.position;

        switch (currentPhase)
        {
            case Phase.MovingToTarget:
                Vector3 target3D = new Vector3(targetPosition.x, targetPosition.y, currentPos.z);
                Vector3 dir = (target3D - currentPos).normalized;
                controller.transform.position += dir * speed * Time.deltaTime;

                if (Vector3.Distance(currentPos, target3D) < 0.05f)
                {
                    currentPhase = Phase.Stopping;
                    stopTimer = 0f;
                }
                break;

            case Phase.Stopping:
                stopTimer += Time.deltaTime;
                if (stopTimer >= stopDuration)
                {
                    currentPhase = Phase.MovingAfterStop;
                }
                break;

            case Phase.MovingAfterStop:
                controller.transform.position += (Vector3)(moveAfterStopDirection.normalized) * speed * Time.deltaTime;
                break;
        }
    }
}