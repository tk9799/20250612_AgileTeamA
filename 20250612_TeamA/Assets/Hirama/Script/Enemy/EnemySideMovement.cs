using UnityEngine;

/// <summary>
/// 横方向にまっすぐ移動して、反対側に抜けたら自動でプールに戻る
/// </summary>
public class EnemySideMovement : MonoBehaviour
{
    [Header("移動速度（正の値）")]
    public float moveSpeed = 3f;

    [Header("出現時の位置")]
    public Vector2 fixedPosition = new Vector2(12f, 5f);

    [Header("進行方向(trueなら右→左、falseなら左→右)")]
    public bool fromRightToLeft = true;

    private EnemyHP enemy;

    private bool isMoving = false;

    /// <summary>
    /// 初期化（外部から位置や速度を渡す）
    /// </summary>
    public void Initialize(SideMovementSettings settings)
    {
        fixedPosition = settings.fixedPosition;
        fromRightToLeft = settings.fromRightToLeft;
        moveSpeed = settings.moveSpeed;

        // 初期位置セット
        Vector3 pos = transform.position;
        pos.x = fixedPosition.x;
        pos.y = fixedPosition.y;
        transform.position = pos;

        isMoving = true;
    }

    private void OnEnable()
    {
        enemy = GetComponent<EnemyHP>();
        isMoving = false;
    }

    private void Update()
    {
        if (!isMoving)
        {
            return;
        }
        float direction = fromRightToLeft ? -1f : 1f;
        transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);
    }

    private void OnBecameInvisible()
    {
        if (enemy != null)
        {
            enemy.ReturnToPoolFromOffScreen();
            isMoving = false;
        }
    }
}