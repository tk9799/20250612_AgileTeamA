using UnityEngine;

/// <summary>
/// プレイヤーの移動を担当するコンポーネント
/// </summary>
public class PlayerMoveController : MonoBehaviour
{
    [Header("移動速度")]
    public float moveSpeed = 5f;

    [Header("画面端の制限（ワールド座標）")]
    public Vector2 minBounds = new Vector2(-8f, -4.5f);
    public Vector2 maxBounds = new Vector2(8f, 4.5f);

    private void Update()
    {
        //入力取得（WASDや矢印キー）
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        //移動ベクトル計算
        Vector3 move = new Vector3(x, y, 0).normalized * moveSpeed * Time.deltaTime;
        Vector3 newPos = transform.position + move;

        //画面外に出ないよう制限
        newPos.x = Mathf.Clamp(newPos.x, minBounds.x, maxBounds.x);
        newPos.y = Mathf.Clamp(newPos.y, minBounds.y, maxBounds.y);

        transform.position = newPos;
    }
}