using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Barrage/Boss/MovementPattern/PointMovement")]
public class PointMovementPattern : BossMovementPattern
{
    [Header("初期目標位置")]
    public Vector2 startTarget = new Vector2(0f, 4f);

    [Header("初期目標位置での待機時間(秒)")]
    public float startWaitTime = 1.5f;

    [Header("移動ポイントのリスト")]
    public List<Vector2> waypoints = new List<Vector2>();

    [Header("各ポイントでの待機時間(秒)")]
    public float waitTimeAtPoints = 1f;

    [Header("移動速度")]
    public float moveSpeed = 2f;

    private int currentIndex = -1;
    private Vector2 currentTarget;
    private float waitTimer = 0f;
    private bool initialized = false;
    private BossController controller;
    private State currentState = State.ToStartTarget;

    private enum State
    {
        //初期目標位置へ移動
        ToStartTarget,
        //初期目標位置で待機
        WaitingAtStart,
        //ポイントの現在地へ移動
        MovingToWaypoint,
        //各ポイントで待機
        WaitingAtWaypoint
    }

    public override void Initialize(BossController controller)
    {
        this.controller = controller;
        currentIndex = -1;
        currentTarget = startTarget;
        waitTimer = 0f;
        initialized = true;
        currentState = State.ToStartTarget;

        // 初期移動中は無敵
        SetInvincible(true);
    }

    public override void UpdateMovement(BossController controller)
    {
        if (!initialized) return;

        switch (currentState)
        {
            case State.ToStartTarget:
                MoveTowards(currentTarget);
                if (IsArrived(currentTarget))
                {
                    currentState = State.WaitingAtStart;
                    waitTimer = 0f;

                    //初期移動完了 → 無敵解除
                    SetInvincible(false);
                }
                break;

            case State.WaitingAtStart:
                waitTimer += Time.deltaTime;
                if (waitTimer >= startWaitTime)
                {
                    //最初のwaypointへ移動開始
                    currentIndex = 0;
                    if (waypoints.Count > 0)
                    {
                        currentTarget = waypoints[currentIndex];
                        currentState = State.MovingToWaypoint;
                    }
                }
                break;

            case State.MovingToWaypoint:
                MoveTowards(currentTarget);
                if (IsArrived(currentTarget))
                {
                    currentState = State.WaitingAtWaypoint;
                    waitTimer = 0f;
                }
                break;

            case State.WaitingAtWaypoint:
                waitTimer += Time.deltaTime;
                if (waitTimer >= waitTimeAtPoints)
                {
                    //次のポイントへ。最後まで行ったら先頭に戻す
                    currentIndex = (currentIndex + 1) % waypoints.Count;
                    currentTarget = waypoints[currentIndex];
                    currentState = State.MovingToWaypoint;
                }
                break;
        }
    }

    public override bool IsFinished()
    {
        //ループするので常にfalse
        return false;
    }

    public override void OnExit(BossController bossController)
    {
        //念のため無敵解除
        SetInvincible(false);
    }

    private void MoveTowards(Vector2 target)
    {
        Vector3 current = controller.transform.position;
        Vector2 current2D = new Vector2(current.x, current.y);

        Vector2 next = Vector2.MoveTowards(current2D, target, moveSpeed * Time.deltaTime);
        controller.transform.position = new Vector3(next.x, next.y, 0f);
    }

    private bool IsArrived(Vector2 target)
    {
        Vector2 current2D = new Vector2(controller.transform.position.x, controller.transform.position.y);
        return Vector2.Distance(current2D, target) < 0.05f;
    }

    private void SetInvincible(bool value)
    {
        var boss = controller.GetComponent<BossBase>();
        if (boss != null)
        {
            boss.IsInvincible = value;
        }
    }
}