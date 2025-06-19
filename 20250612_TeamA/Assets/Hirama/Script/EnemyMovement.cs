using System.Threading;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("移動先の座標")]
    [SerializeField] private Vector3 targetPosition = new Vector3(0f, 2f, 0f);

    [Header("移動速度")]
    [SerializeField] private float moveDuration = 3f;

    [Header("到達判定距離")]
    [SerializeField] private float stopThreshold = 0.1f;

    [SerializeField] private EntryDirection entryDirection = EntryDirection.Top;

    [SerializeField] private Vector3 targetPos;

    public enum EntryDirection { Top, Bottom, Left, Right }

    private Vector3 startPos;

    private float timer = 0f;

    private bool hasArrived = false;

    private void Start()
    {
        startPos = targetPosition;

        switch (entryDirection)
        {
            case EntryDirection.Top:
                startPos.y += 6f;
                break;
            case EntryDirection.Bottom:
                startPos.y -= 6f;
                break;
            case EntryDirection.Left:
                startPos.x -= 6f;
                break;
            case EntryDirection.Right:
                startPos.x += 6f;
                break;
        }

        transform.position = startPos;
    }

    void Update()
    {
        if (hasArrived)
        {
            return;
        }

        timer += Time.deltaTime;

        float t = Mathf.Clamp01(timer / moveDuration);
        float smoothT = Mathf.SmoothStep(0f, 1f, t);

        transform.position = Vector3.Lerp(startPos, targetPosition, smoothT);

        if (t >= 1f || Vector3.Distance(transform.position, targetPosition) <= stopThreshold)
        {
            hasArrived = true;
            OnArrived();
        }
    }

    private void OnArrived()
    {
        Debug.Log("敵が目的地に到達しました！");
    }
}