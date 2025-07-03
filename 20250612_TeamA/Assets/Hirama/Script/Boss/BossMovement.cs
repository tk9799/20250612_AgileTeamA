using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [Header("移動ポイント")]
    public List<Vector2> movePositions;

    [Header("移動速度")]
    public float moveSpeed = 2f;

    [Header("各ポイントでの待機時間")]
    public float waitTime = 1f;

    private int currentIndex = 0;
    private float waitTimer = 0f;
    private bool isWaiting = false;

    void Update()
    {
        if (movePositions == null || movePositions.Count == 0)
        {
            return;
        }

        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                isWaiting = false;
                currentIndex = (currentIndex + 1) % movePositions.Count;
            }
        }
        else
        {
            Vector2 target = movePositions[currentIndex];
            transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, target) < 0.01f)
            {
                isWaiting = true;
                waitTimer = 0f;
            }
        }
    }
}