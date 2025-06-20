using UnityEngine;

public class BossEntrance : MonoBehaviour
{
    [Header("開始位置（降下前）")]
    [SerializeField] public Vector2 startPosition = new Vector2(0, 10);

    [Header("登場位置（降下後）")]
    [SerializeField] public Vector2 enterPosition = new Vector2(0, 3);

    [Header("降下速度")]
    [SerializeField] public float descendSpeed = 2f;

    [Header("停止後の待機時間")]
    [SerializeField] public float waitTime = 2f;

    [Header("弾幕の管理コンポーネント")]
    [SerializeField] public PatternManager patternManager;

    [Header("移動コンポーネント（BossMover）")]
    [SerializeField] public BossMovement bossMovement;

    [Header("BossHPコンポーネント")]
    [SerializeField] public BossHP bossHP;

    //現在のステート（降下中・待機中・完了）
    private enum State { Descending, Waiting, Done }
    private State currentState = State.Descending;

    private float waitTimer = 0f;

    void Start()
    {
        //ボスを初期位置に配置
        transform.position = startPosition;

        //弾幕と移動を一時停止
        if (patternManager != null)
        {
            if (patternManager.omniBarrage != null)
                patternManager.omniBarrage.enabled = false;
            if (patternManager.fanBarrage != null)
                patternManager.fanBarrage.enabled = false;

            patternManager.enabled = false;
        }

        //移動とHPも一時停止・無敵化
        if (bossMovement != null)
            bossMovement.enabled = false;

        if (bossHP != null)
            bossHP.SetInvincible(true);
    }

    void Update()
    {
        //ステートに応じて処理を切り替え
        switch (currentState)
        {
            case State.Descending:
                Descend();
                break;

            case State.Waiting:
                Wait();
                break;

            case State.Done:
                // 完了済み
                break;
        }
    }

    //降下処理
    void Descend()
    {
        transform.position = Vector2.MoveTowards(transform.position,
            enterPosition, descendSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, enterPosition) < 0.01f)
        {
            currentState = State.Waiting;
            waitTimer = 0f;
        }
    }

    //待機 → 弾幕解放
    void Wait()
    {
        waitTimer += Time.deltaTime;
        if (waitTimer >= waitTime)
        {
            //弾幕切り替え
            patternManager?.SwitchToOmniBarrage();
            //移動再開
            bossMovement.enabled = true;
            //無敵解除
            bossHP?.SetInvincible(false);
            currentState = State.Done;
        }
    }
}