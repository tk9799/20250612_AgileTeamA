using UnityEngine;

public class GuardController2 : MonoBehaviour
{
    [Header("敵弾幕オブジェクト")]
    [SerializeField] GameObject enemyBarrageObject;
    [SerializeField] GameObject enemyBarrageObject2;
    [SerializeField] GameObject enemyBarrageObject3;
    [SerializeField] GameObject enemyBarrageObject4;
    [SerializeField] GameObject enemyBarrageObject5;
    //[Header("プレイヤーオブジェクト")]
    //[SerializeField] GameObject playerJudgeObject;
    [SerializeField] Transform enemyBarrage;//敵弾幕のトランスフォーム
    private float enemyBarrageRadius = 1f;//オブジェクトの範囲
    [SerializeField] Transform enemyBarrage2;//敵弾幕のトランスフォーム
    private float enemyBarrage2Radius = 1f;//オブジェクトの範囲
    [SerializeField] Transform enemyBarrage3;//敵弾幕のトランスフォーム
    private float enemyBarrage3Radius = 1f;//オブジェクトの範囲
    [SerializeField] Transform enemyBarrage4;//敵弾幕のトランスフォーム
    private float enemyBarrage4Radius = 1f;//オブジェクトの範囲
    [SerializeField] Transform enemyBarrage5;//敵弾幕のトランスフォーム
    private float enemyBarrage5Radius = 1f;//オブジェクトの範囲
    [SerializeField] Transform justGuardTransform;//プレイヤー判定オブジェクトのトランスフォーム
    [SerializeField] private float justGuardDistanceRadius = 1f;//オブジェクトの範囲
    [SerializeField] PlayerJudgement playerJudgement;//プレイヤー判定のスクリプト
    [SerializeField] public SpriteRenderer spriteRenderer;//シールドの画像
    [SerializeField] public CircleCollider2D circeCollider;
    public bool judgementEnd = false;

    private float justRadius = 1f;
    private float justRange = 0.45f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer.enabled = true;
        circeCollider.enabled = true;

    }

    // Update is called once per frame
    void Update()
    {
        float notePos=Vector2.Distance(justGuardTransform.position,enemyBarrage.position);
        float notePos2=Vector2.Distance(justGuardTransform.position,enemyBarrage2.position);
        float notePos3=Vector2.Distance(justGuardTransform.position,enemyBarrage3.position);
        float notePos4=Vector2.Distance(justGuardTransform.position,enemyBarrage4.position);
        float notePos5=Vector2.Distance(justGuardTransform.position,enemyBarrage5.position);
        float distances = Mathf.Abs(notePos-justRadius);//判定ラインからの距離
        float distances2 = Mathf.Abs(notePos2-justRadius);//判定ラインからの距離
        float distances3 = Mathf.Abs(notePos3-justRadius);//判定ラインからの距離
        float distances4 = Mathf.Abs(notePos4-justRadius);//判定ラインからの距離
        float distances5 = Mathf.Abs(notePos5-justRadius);//判定ラインからの距離
        if (distances <= justGuardDistanceRadius)
        {
            Debug.Log(distances);
            Debug.Log("justGuard");
            enemyBarrageObject.SetActive(false);
            StartCoroutine(playerJudgement.InvincibleTime());
            judgementEnd = true;
            Debug.Log("InvincibleTime" + judgementEnd);
        }
        if (distances2 <= justGuardDistanceRadius)
        {
            Debug.Log(distances2);
            Debug.Log("justGuard");
            enemyBarrageObject2.SetActive(false);
            StartCoroutine(playerJudgement.InvincibleTime());
            judgementEnd = true;
            Debug.Log("InvincibleTime" + judgementEnd);
        }
        if (distances3 <= justGuardDistanceRadius)
        {
            Debug.Log(distances3);
            Debug.Log("justGuard");
            enemyBarrageObject3.SetActive(false);
            StartCoroutine(playerJudgement.InvincibleTime());
            judgementEnd = true;
            Debug.Log("InvincibleTime" + judgementEnd);
        }
        if (distances4 <= justGuardDistanceRadius)
        {
            Debug.Log(distances4);
            Debug.Log("justGuard");
            enemyBarrageObject4.SetActive(false);
            StartCoroutine(playerJudgement.InvincibleTime());
            judgementEnd = true;
            Debug.Log("InvincibleTime" + judgementEnd);
        }
        if (distances5 <= justGuardDistanceRadius)
        {
            Debug.Log(distances5);
            Debug.Log("justGuard");
            enemyBarrageObject5.SetActive(false);
            StartCoroutine(playerJudgement.InvincibleTime());
            judgementEnd = true;
            Debug.Log("InvincibleTime" + judgementEnd);
        }

        //Vector2 enemyBarragePosition = enemyBarrage.position;//敵弾幕のトランスフォームポジション
        //Vector2 enemyBarragePosition2 = enemyBarrage2.position;
        //Vector2 enemyBarragePosition3 = enemyBarrage3.position;
        //Vector2 enemyBarragePosition4 = enemyBarrage4.position;
        //Vector2 enemyBarragePosition5 = enemyBarrage5.position;

        //Vector2 guardPosition = justGuardTransform.position;//ガードの中心座標を取得

        //Vector2 distance = guardPosition - enemyBarragePosition;//弾幕からプレイヤーへの距離
        //Vector2 distance2 = guardPosition - enemyBarragePosition2;
        //Vector2 distance3 = guardPosition - enemyBarragePosition3;
        //Vector2 distance4 = guardPosition - enemyBarragePosition4;
        //Vector2 distance5 = guardPosition - enemyBarragePosition5;

        //float distanceVector = distance.sqrMagnitude;//ベクトルの距離の2乗
        //float distanceVector2 = distance.sqrMagnitude;//ベクトルの距離の2乗
        //float distanceVector3 = distance.sqrMagnitude;//ベクトルの距離の2乗
        //float distanceVector4 = distance.sqrMagnitude;//ベクトルの距離の2乗
        //float distanceVector5 = distance.sqrMagnitude;//ベクトルの距離の2乗

        //float sumRadius = enemyBarrageRadius + justGuardDistanceRadius;//半径の和
        //float sumRadius2 = enemyBarrage2Radius + justGuardDistanceRadius;//半径の和
        //float sumRadius3 = enemyBarrage3Radius + justGuardDistanceRadius;//半径の和
        //float sumRadius4 = enemyBarrage4Radius + justGuardDistanceRadius;//半径の和
        //float sumRadius5 = enemyBarrage5Radius + justGuardDistanceRadius;//半径の和

        //float sprSumRadius = sumRadius * sumRadius;//半径の和の2乗
        //float sprSumRadius2 = sumRadius2 * sumRadius2;//半径の和の2乗
        //float sprSumRadius3 = sumRadius3 * sumRadius3;//半径の和の2乗
        //float sprSumRadius4 = sumRadius4 * sumRadius4;//半径の和の2乗
        //float sprSumRadius5 = sumRadius5 * sumRadius5;//半径の和の2乗

        //if (distanceVector <= sprSumRadius)//距離の2乗が半径の和の2乗以下なら
        //{
        //    Debug.Log("hit");
        //    gameObject.SetActive(false);
        //    StartCoroutine(playerJudgement.InvincibleTime());
        //    judgementEnd = true;
        //    Debug.Log("InvincibleTime" + judgementEnd);
        //}
        //else if (distanceVector2 <= sprSumRadius2)
        //{
        //    Debug.Log("hit");
        //    gameObject.SetActive(false);
        //    StartCoroutine(playerJudgement.InvincibleTime());
        //    judgementEnd = true;
        //    Debug.Log("InvincibleTime" + judgementEnd);
        //}
        //else if (distanceVector3 <= sprSumRadius3)
        //{
        //    Debug.Log("hit");
        //    gameObject.SetActive(false);
        //    StartCoroutine(playerJudgement.InvincibleTime());
        //    judgementEnd = true;
        //    Debug.Log("InvincibleTime" + judgementEnd);
        //}
        //else if (distanceVector4 <= sprSumRadius4)
        //{
        //    Debug.Log("hit");
        //    gameObject.SetActive(false);
        //    StartCoroutine(playerJudgement.InvincibleTime());
        //    judgementEnd = true;
        //    Debug.Log("InvincibleTime" + judgementEnd);
        //}
        //else if (distanceVector5 <= sprSumRadius5)
        //{
        //    Debug.Log("hit");
        //    gameObject.SetActive(false);
        //    StartCoroutine(playerJudgement.InvincibleTime());
        //    judgementEnd = true;
        //    Debug.Log("InvincibleTime" + judgementEnd);
        //}


    }
    void OnDrawGizmosSelected()
    {
        if (justGuardTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(justGuardTransform.position, justGuardDistanceRadius);
        }

    }
}
