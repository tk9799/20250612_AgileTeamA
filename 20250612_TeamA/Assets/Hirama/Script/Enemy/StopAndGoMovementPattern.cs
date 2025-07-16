using UnityEngine;

/// <summary>
/// 目標位置まで移動→停止→一定時間後に別方向へ移動するパターン
/// </summary>
[CreateAssetMenu(fileName = "StopAndGoPattern", menuName = "Barrage/EnemyMovementPattern/StopAndGo")]
public class StopAndGoMovementPattern : EnemyMovementPattern
{
    [Header("最初に向かう目標位置")]
    public Vector2 targetPosition;

    [Header("停止時間(秒)")]
    public float stopDuration = 2f;

    [Header("停止後の移動方向(ベクトル)")]
    public Vector2 moveAfterStopDirection = Vector2.down;

    private EnemyController controller;
    private EnemyBase enemyBase;

    private enum Phase
    {
        //目標位置に移動中
        MovingToTarget,
        //停止中
        Stopping,
        //停止後の移動中
        MovingAfterStop
    }

    private Phase currentPhase = Phase.MovingToTarget;
    private float stopTimer = 0f;

    public override void Initialize(EnemyController controller)
    {
        this.controller = controller;
        this.enemyBase = controller.GetComponent<EnemyBase>();

        currentPhase = Phase.MovingToTarget;
        stopTimer = 0f;
    }

    public override void UpdateMovement(EnemyController controller)
    {
        if (enemyBase == null || enemyBase.enemyData == null) return;

        float speed = enemyBase.enemyData.moveSpeed;
        Vector3 currentPos = enemyBase.transform.position;

        switch (currentPhase)
        {
            case Phase.MovingToTarget:
                {
                    // 目標位置へ移動
                    Vector3 targetPos3D = new Vector3(targetPosition.x, targetPosition.y, currentPos.z);
                    Vector3 dir = (targetPos3D - currentPos).normalized;
                    enemyBase.transform.position += dir * speed * Time.deltaTime;

                    // 近づいたら停止フェーズへ
                    if (Vector3.Distance(currentPos, targetPos3D) < 0.05f)
                    {
                        currentPhase = Phase.Stopping;
                        stopTimer = 0f;
                    }
                    break;
                }

            case Phase.Stopping:
                {
                    // 時間経過をカウント
                    stopTimer += Time.deltaTime;
                    if (stopTimer >= stopDuration)
                    {
                        currentPhase = Phase.MovingAfterStop;
                    }
                    break;
                }

            case Phase.MovingAfterStop:
                {
                    // 停止後の方向に直線移動
                    Vector3 moveDir = moveAfterStopDirection.normalized;
                    enemyBase.transform.position += moveDir * speed * Time.deltaTime;
                    break;
                }
        }
    }
}