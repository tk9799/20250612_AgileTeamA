using UnityEngine;

public class EnemyComplexMovement : MonoBehaviour
{
    [Header("oŒ»ˆÊ’u")]
    public Vector2 spawnPosition = new Vector2(-10f, 4f);

    [Header("’†‰›•t‹ß‚Ì–Ú•WˆÊ’u")]
    public Vector2 centerPosition = new Vector2(0f, 0f);

    [Header("ˆÚ“®‘¬“x")]
    public float moveSpeed = 3f;

    [Header("’âŽ~Œã‚ÌˆÚ“®•ûŒü")]
    public Vector2 finalMoveDirection = Vector2.down;

    [Header("’âŽ~ŽžŠÔi•bj")]
    public float stopDuration = 1.0f;

    [Header("’âŽ~’†‚Ì—h‚ê•")]
    public float stopSwayAmplitude = 0.1f;

    [Header("’âŽ~’†‚Ì—h‚ê‘¬“x")]
    public float stopSwayFrequency = 3f;

    private enum State
    {
        MoveToCenter,
        Stop,
        MoveFinalDirection
    }

    private State currentState = State.MoveToCenter;
    private float stopTimer = 0f;

    private Vector3 baseStopPosition;
    private float swayTimer = 0f;

    public void Initialize(ComplexMovementSettings settings)
    {
        spawnPosition = settings.spawnPosition;
        centerPosition = settings.centerPosition;
        finalMoveDirection = settings.finalMoveDirection;
        stopDuration = settings.stopDuration;
        stopSwayAmplitude = settings.stopSwayAmplitude;
        stopSwayFrequency = settings.stopSwayFrequency;
        moveSpeed = settings.moveSpeed;

        transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0f);
        currentState = State.MoveToCenter;
        stopTimer = 0f;
        swayTimer = 0f;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.MoveToCenter:
                MoveToCenter();
                break;

            case State.Stop:
                StopAtCenter();
                break;

            case State.MoveFinalDirection:
                MoveInFinalDirection();
                break;
        }
    }

    private void MoveToCenter()
    {
        Vector3 targetPos = new Vector3(centerPosition.x, centerPosition.y, 0f);
        Vector3 toTarget = targetPos - transform.position;
        float distance = toTarget.magnitude;

        if (distance > 0.01f)
        {
            //‹ß‚Ã‚­‚Ù‚ÇŒ¸‘¬(‘¬“x‚ð‹——£‚É”ä—á‚³‚¹‚é)
            float speed = Mathf.Lerp(0f, moveSpeed, Mathf.Clamp01(distance / 1.0f));
            Vector3 move = toTarget.normalized * speed * Time.deltaTime;
            if (move.magnitude > distance) move = toTarget;

            transform.position += move;
        }
        else
        {
            currentState = State.Stop;
            //’âŽ~ˆÊ’u‚ð‹L˜^
            baseStopPosition = transform.position;
            swayTimer = 0f;
        }
    }

    private void StopAtCenter()
    {
        stopTimer += Time.deltaTime;
        swayTimer += Time.deltaTime;

        //ƒTƒCƒ“”g‚Åã‰º‚É‚ä‚ç‚ä‚ç
        float swayOffset = Mathf.Sin(swayTimer * stopSwayFrequency) * stopSwayAmplitude;

        transform.position = baseStopPosition + new Vector3(0f, swayOffset, 0f);

        if (stopTimer >= stopDuration)
        {
            currentState = State.MoveFinalDirection;
        }
    }

    private void MoveInFinalDirection()
    {
        Vector3 delta = (Vector3)finalMoveDirection.normalized * moveSpeed * Time.deltaTime;
        transform.position += delta;
    }
}