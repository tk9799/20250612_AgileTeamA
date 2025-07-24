using UnityEngine;

/// <summary>
/// 目標位置まで移動→停止→一定時間後に別方向へ移動するパターン
/// </summary>
[CreateAssetMenu(fileName = "StopAndGoPattern", menuName = "Barrage/MovementPattern/StopAndGo")]
public class StopAndGoMovementPattern : MovementPattern
{
    [Header("目標位置")]
    public Vector2 targetPosition = new Vector2(0, 3);

    [Header("相対移動(スポーン位置から加算)")]
    public Vector2 relativeOffset = Vector2.zero;

    [Header("相対移動を使用するか(true = スポーン位置基準)")]
    public bool useRelativeTarget = false;

    [Header("停止時間(秒)")]
    public float stopDuration = 5f;

    [Header("停止後の移動方向")]
    public Vector2 moveAfterStopDirection = Vector2.down;

    private float speed;
    private float stopTimer;
    private Phase currentPhase;
    private Vector3 actualTargetPosition;

    private enum Phase
    {
        //目標位置に移動中
        MovingToTarget,
        //停止中
        Stopping,
        //停止後の移動中
        MovingAfterStop
    }

    public override void Initialize(EnemyController controller)
    {
        speed = controller.enemyData.moveSpeed;
        currentPhase = Phase.MovingToTarget;
        stopTimer = 0f;

        Vector3 spawnPosition = controller.transform.position;

        if (useRelativeTarget)
        {
            actualTargetPosition = spawnPosition + (Vector3)relativeOffset;
        }
        else
        {
            actualTargetPosition = (Vector3)targetPosition;
        }
    }

    public override void UpdateMovement(EnemyController controller)
    {
        Vector3 currentPos = controller.transform.position;

        switch (currentPhase)
        {
            case Phase.MovingToTarget:
                Vector3 dir = (actualTargetPosition - currentPos).normalized;
                controller.transform.position += dir * speed * Time.deltaTime;

                if (Vector3.Distance(currentPos, actualTargetPosition) < 0.05f)
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