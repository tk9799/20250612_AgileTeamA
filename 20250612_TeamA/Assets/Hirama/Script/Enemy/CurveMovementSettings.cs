using UnityEngine;

[CreateAssetMenu(menuName = "EnemyMovement/Curve")]
public class CurveMovementSettings : EnemyMovementSettings
{
    [Header("ベジェ曲線の制御点")]
    public Vector3 startPoint;
    public Vector3 midPoint;
    public Vector3 endPoint;

    [Header("移動にかかる時間（秒）")]
    public float duration = 3f;
}