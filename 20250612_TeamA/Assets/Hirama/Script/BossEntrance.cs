using UnityEngine;

/// <summary>
/// ボスの登場演出（降下 → 待機 → 弾幕開始）を制御するクラス
/// </summary>
public class BossEntrance : MonoBehaviour
{
    [Header("移動設定")]
    [Header("登場時の初期Y位置")]
    [SerializeField] private float startY = 10f;

    [Header("到達目標Y位置")]
    [SerializeField] private float targetY = 3f;

    [Header("降下スピード")]
    [SerializeField] private float fallSpeed = 2f;

    [Header("弾幕までの待機時間")]
    [Header("降下後に弾幕を始めるまでの待機時間")]
    [SerializeField] private float barrageDelay = 3f;

    [Header("弾幕制御スクリプト")]
    [SerializeField] private BossOmnidirectionalBarrage barrageScript;

    [Header("ボス移動スクリプト")]
    [SerializeField] private BossMovement bossMovement;

    // 弾幕開始までの残り時間
    private float delayTimer = 0f;
    //現在降下中かどうか
    private bool isFalling = false;
    //降下が完了して待機中かどうか
    private bool hasArrived = false;

    /// <summary>
    /// オブジェクトがアクティブになったときに呼ばれる処理
    /// </summary>
    private void OnEnable()
    {
        //ボスの位置を画面外（startY）に移動して初期化
        Vector3 pos = transform.position;
        pos.y = startY;
        transform.position = pos;

        //降下開始フラグを有効にする
        isFalling = true;
        hasArrived = false;

        //弾幕発射処理を無効化（登場演出が終わるまで弾を撃たせない）
        barrageScript = GetComponent<BossOmnidirectionalBarrage>();
        if (barrageScript != null)
        {
            barrageScript.enabled = false;
        }
    }

    /// <summary>
    /// 毎フレーム処理
    /// </summary>
    private void Update()
    {
        if (isFalling)
        {
            //降下処理：targetY に向かって毎フレーム移動
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(transform.position.x, targetY, transform.position.z),
                fallSpeed * Time.deltaTime);

            //到達したら降下を終了し、弾幕開始までの待機を開始
            if (Mathf.Abs(transform.position.y - targetY) < 0.01f)
            {
                isFalling = false;
                hasArrived = true;
                delayTimer = barrageDelay;
            }
        }
        else if (hasArrived)
        {
            //弾幕開始までの待機処理
            delayTimer -= Time.deltaTime;

            if (delayTimer <= 0f)
            {
                hasArrived = false;
                //弾幕開始
                StartBarrage();
            }
        }
    }

    /// <summary>
    /// 弾幕を有効化し、同時に移動も開始
    /// </summary>
    private void StartBarrage()
    {
        if (barrageScript != null)
        {
            barrageScript.enabled = true;
            barrageScript.StartBarrageImmediately();
            //弾幕と同時に移動開始
            bossMovement.BeginMovement();
        }
    }
}