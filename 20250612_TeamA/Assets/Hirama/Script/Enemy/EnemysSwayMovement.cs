using UnityEngine;

/// <summary>
/// 上下に揺れるような動きで移動する敵用スクリプト
/// </summary>
public class EnemysSwayMovement : MonoBehaviour
{
    [Header("移動速度（進行方向の速さ）")]
    public float moveSpeed = 2f;

    [Header("出現時の位置")]
    public Vector2 spawnPosition = new Vector2(8f, 0f);

    [Header("進行方向（例：Vector2.right, Vector2.downなど）")]
    public Vector2 moveDirection = Vector2.left;

    [Header("カーブの振れ幅（上下の幅）")]
    public float amplitude = 1f;

    [Header("カーブの周波数（揺れる速さ）")]
    public float frequency = 2f;

    //出現位置を記録（揺れの基準位置）
    private Vector3 startPosition;

    //カーブ用タイマー
    private float curveTimer = 0f;

    /// <summary>
    /// 初期化（外部から設定SOを渡してセットアップ）
    /// </summary>
    public void Initialize(SwayMovementSettings settings)
    {
        moveSpeed = settings.moveSpeed;
        spawnPosition = settings.spawnPosition;
        moveDirection = settings.moveDirection;
        amplitude = settings.amplitude;
        frequency = settings.frequency;

        transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0f);
        startPosition = transform.position;
        curveTimer = 0f;
    }

    void Update()
    {
        curveTimer += Time.deltaTime;

        Vector3 linearMove = moveDirection.normalized * moveSpeed * Time.deltaTime;
        Vector2 perpendicular = new Vector2(-moveDirection.y, moveDirection.x).normalized;
        Vector3 curveOffset = perpendicular * Mathf.Sin(curveTimer * frequency) * amplitude;

        transform.position += linearMove + curveOffset * Time.deltaTime;
    }
}