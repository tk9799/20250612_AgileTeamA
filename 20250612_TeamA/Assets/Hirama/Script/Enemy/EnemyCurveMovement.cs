using UnityEngine;

/// <summary>
/// カーブするように移動させる（始点→中間→終点）
/// </summary>
public class EnemyCurveMovement : MonoBehaviour
{
    [Header("経由する3点（Start → Mid → End）")]
    public Vector3 startPoint;
    public Vector3 midPoint;
    public Vector3 endPoint;

    [Header("移動時間（秒）")]
    public float duration = 3f;

    private float elapsedTime = 0f;
    private bool moving = false;

    /// <summary>
    /// 初期化（外部から経路と時間を渡す）
    /// </summary>
    public void Initialize(CurveMovementSettings settings)
    {
        startPoint = settings.startPoint;
        midPoint = settings.midPoint;
        endPoint = settings.endPoint;
        duration = Mathf.Max(settings.duration, 0.01f);

        elapsedTime = 0f;
        transform.position = startPoint;
        moving = true;
    }

    void Update()
    {
        if (!moving)
        {
            return;
        }

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / duration);

        Vector3 a = Vector3.Lerp(startPoint, midPoint, t);
        Vector3 b = Vector3.Lerp(midPoint, endPoint, t);
        transform.position = Vector3.Lerp(a, b, t);

        if (t >= 1f)
        {
            moving = false;
        }
    }
}