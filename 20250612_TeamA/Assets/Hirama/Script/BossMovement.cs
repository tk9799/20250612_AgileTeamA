using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [Header("移動ポイント")]
    [SerializeField] public List<Transform> movePoints;

    [Header("移動速度")]
    [SerializeField] public float moveSpeed = 2f;

    [Header("各ポイントでの待機時間")]
    [SerializeField] public float waitTime = 1f;

    private int currentIndex = 0;
    private float waitTimer = 0f;
    private bool isWaiting = false;

    void Update()
    {
        if (movePoints == null || movePoints.Count == 0) return;

        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                isWaiting = false;
                currentIndex = (currentIndex + 1) % movePoints.Count;
            }
        }
        else
        {
            Vector2 target = movePoints[currentIndex].position;
            transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, target) < 0.01f)
            {
                isWaiting = true;
                waitTimer = 0f;
            }
        }
    }
}