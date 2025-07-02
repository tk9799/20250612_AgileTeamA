using UnityEngine;

[CreateAssetMenu(menuName = "EnemyMovement/Complex")]
public class ComplexMovementSettings : EnemyMovementSettings
{
    [Header("oŒ»ˆÊ’u")]
    public Vector2 spawnPosition = new Vector2(-6f, 4f);

    [Header("’†‰›•t‹ß‚Ì–Ú•WˆÊ’u")]
    public Vector2 centerPosition = Vector2.zero;

    [Header("’â~Œã‚ÌˆÚ“®•ûŒü")]
    public Vector2 finalMoveDirection = Vector2.down;

    [Header("’â~ŠÔi•bj")]
    public float stopDuration = 1.0f;

    [Header("’â~’†‚Ì—h‚ê•")]
    public float stopSwayAmplitude = 0.1f;

    [Header("’â~’†‚Ì—h‚ê‘¬“x")]
    public float stopSwayFrequency = 3f;
}