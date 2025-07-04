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
    private float enemyBarrageRadius = 1f;
    [SerializeField] Transform enemyBarrage2;//敵弾幕のトランスフォーム
    private float enemyBarrage2Radius = 1f;
    [SerializeField] Transform enemyBarrage3;//敵弾幕のトランスフォーム
    private float enemyBarrage3Radius = 1f;
    [SerializeField] Transform enemyBarrage4;//敵弾幕のトランスフォーム
    private float enemyBarrage4Radius = 1f;
    [SerializeField] Transform enemyBarrage5;//敵弾幕のトランスフォーム
    private float enemyBarrage5Radius = 1f;
    [SerializeField] Transform justGuardTransform;//プレイヤー判定オブジェクトのトランスフォーム
    [SerializeField] private int justGuardDistance = 0;//ジャストガードの距離
    private float justGuardDistanceRadius = 1f;
    [SerializeField] PlayerJudgement playerJudgement;
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public CircleCollider2D circeCollider;
    public bool judgementEnd = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 enemyBarragePosition = enemyBarrage.position;
        Vector2 enemyBarragePosition2 = enemyBarrage2.position;
        Vector2 enemyBarragePosition3 = enemyBarrage3.position;
        Vector2 enemyBarragePosition4 = enemyBarrage4.position;
        Vector2 enemyBarragePosition5 = enemyBarrage5.position;

        Vector2 guardPosition = justGuardTransform.position;//ガードの中心座標を取得

        Vector2 distance = guardPosition - enemyBarragePosition;
        Vector2 distance2 = guardPosition - enemyBarragePosition;
        Vector2 distance3 = guardPosition - enemyBarragePosition;
        Vector2 distance4 = guardPosition - enemyBarragePosition;
        Vector2 distance5 = guardPosition - enemyBarragePosition;

        float distanceVector = distance.sqrMagnitude;//ベクトルの距離の2乗
        float distanceVector2 = distance.sqrMagnitude;//ベクトルの距離の2乗
        float distanceVector3 = distance.sqrMagnitude;//ベクトルの距離の2乗
        float distanceVector4 = distance.sqrMagnitude;//ベクトルの距離の2乗
        float distanceVector5 = distance.sqrMagnitude;//ベクトルの距離の2乗

        float sumRadius = enemyBarrageRadius + justGuardDistanceRadius;//半径の和
        float sumRadius2 = enemyBarrage2Radius + justGuardDistanceRadius;//半径の和
        float sumRadius3 = enemyBarrage3Radius + justGuardDistanceRadius;//半径の和
        float sumRadius4 = enemyBarrage4Radius + justGuardDistanceRadius;//半径の和
        float sumRadius5 = enemyBarrage5Radius + justGuardDistanceRadius;//半径の和

        float sprSumRadius = sumRadius * sumRadius;//半径の和の2乗
        float sprSumRadius2 = sumRadius2 * sumRadius2;//半径の和の2乗
        float sprSumRadius3 = sumRadius3 * sumRadius3;//半径の和の2乗
        float sprSumRadius4 = sumRadius4 * sumRadius4;//半径の和の2乗
        float sprSumRadius5 = sumRadius5 * sumRadius5;//半径の和の2乗

        if (distanceVector <= sprSumRadius)
        {
            Debug.Log("hit");
            enemyBarrageObject.SetActive(false);
        }
        else if (distanceVector <= sprSumRadius)
        {
            Debug.Log("hit");
            enemyBarrageObject2.SetActive(false);
        }
        else if (distanceVector <= sprSumRadius)
        {
            Debug.Log("hit");
            enemyBarrageObject3.SetActive(false);
        }
        else if (distanceVector <= sprSumRadius)
        {
            Debug.Log("hit");
            enemyBarrageObject4.SetActive(false);
        }
        else if (distanceVector <= sprSumRadius)
        {
            Debug.Log("hit");
            enemyBarrageObject5.SetActive(false);
        }
    }
}
