using UnityEngine;

/// <summary>
/// ボスが指定された複数のポイントを順に移動する挙動を制御する
/// </summary>
public class BossMovement : MonoBehaviour
{
    [Header("ボスが移動するポイントの配列")]
    [SerializeField] private Transform[] movePoints;

    [Header("ボスの移動速度")]
    [SerializeField] private float moveSpeed = 2f;

    [Header("各ポイントで停止する時間")]
    [SerializeField] private float moveDelay = 1f;

    //現在の移動先ポイントのインデックス
    private int currentPointIndex = 0;
    //停止中の待機タイマー
    private float delayTimer = 0f;

    //現在移動中かどうか
    private bool isMoving = false;
    //外部から有効化されるまで移動しない
    private bool isActive = false;

    /// <summary>
    /// ボスの移動を開始する。外部（演出など）から呼び出すことを想定。
    /// </summary>
    public void BeginMovement()
    {
        //移動を有効化
        isActive = true;
        //最初の移動はすぐに開始
        isMoving = true;
    }

    private void Update()
    {
        //移動が無効、または移動ポイント未設定なら何もしない
        if (!isActive || movePoints.Length == 0)
        {
            return;
        }

        if (isMoving)
        {
            //現在の移動ターゲットポイント
            Transform target = movePoints[currentPointIndex];

            //ターゲットに向かって移動
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            //目的地に到着したら一時停止し、次の移動ポイントへ
            if (Vector3.Distance(transform.position, target.position) < 0.01f)
            {
                //停止状態にする
                isMoving = false;
                //次の移動までの待機時間をセット
                delayTimer = moveDelay;
                //次のポイントへ
                currentPointIndex = (currentPointIndex + 1) % movePoints.Length;
            }
        }
        else
        {
            //停止中：待機タイマーを減らす
            delayTimer -= Time.deltaTime;

            //待機終了後、次のポイントへ移動再開
            if (delayTimer <= 0f)
            {
                isMoving = true;
            }
        }
    }
}