using UnityEngine;

/// <summary>
/// 雑魚敵の移動処理。指定した方向にスピードをつけて移動
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    [Header("移動方向")]
    public Vector2 moveDirection = Vector2.down;

    [Header("移動速度")]
    public float moveSpeed = 2f;

    //一度でも画面内に入ったかどうかのフラグ
    private bool hasAppearedOnScreen = false;

    private void Update()
    {
        //毎フレーム、方向×スピード×時間で移動
        Vector2 movement = moveDirection.normalized * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }

    //カメラに映ったときに呼ばれる処理
    private void OnBecameVisible()
    {
        hasAppearedOnScreen = true;
    }

    //カメラから見えなくなった時に呼ばれる処理
    private void OnBecameInvisible()
    {
        if (hasAppearedOnScreen)
        {
            GetComponent<Enemy>().ReturnToPoolFromOffScreen();
        }
    }
}