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
    [SerializeField] private float justGuardDistanceRadius = 0f;//オブジェクトの範囲
    [SerializeField] PlayerJudgement playerJudgement;//プレイヤー判定のスクリプト
    [SerializeField] public SpriteRenderer spriteRenderer;//シールドの画像
    [SerializeField] public CircleCollider2D circeCollider;
    [SerializeField] public BulletPoolManager bulletPoolManager;
    [SerializeField] private GameObject successShield;
    public bool judgementEnd = false;
    private bool isBarrier = false;

    private float justRadius = 0f;
    private float justRange = 0.45f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer.enabled = true;
        circeCollider.enabled = true;
        successShield.SetActive(false);
        //Debug.Log(justGuardDistanceRadius);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerJudgement != null && playerJudgement.isGuardJudgement)
        {
            for (int listNum = bulletPoolManager.activeBulletList.Count - 1; listNum >= 0; listNum--)
            {
                GameObject bullet = bulletPoolManager.activeBulletList[listNum];
                //Debug.Log(enemyBulletName.name);
                //enemyBarrageObject = bulletPoolManager.activeBulletList[listNum];
                //enemyBarrage = enemyBulletName.transform;

                float notePos = Vector2.Distance(justGuardTransform.position, bullet.transform.position);//2つのpositionの距離を返す

                float distances = Mathf.Abs(notePos - justRadius);//判定ラインからの距離

                //Debug.Log(notePos);
                if (distances <= justGuardDistanceRadius)
                {
                    //Debug.Log(distances);
                    //Debug.Log("justGuard");
                    bullet.SetActive(false);
                    bulletPoolManager.activeBulletList.RemoveAt(listNum);
                    //Debug.Log(enemyBarrageObject);
                    StartCoroutine(playerJudgement.InvincibleTime());
                    judgementEnd = true;
                    successShield.SetActive(true);
                    isBarrier = true;
                    break;
                    //Debug.Log("InvincibleTime" + judgementEnd);
                }
            }
            if (playerJudgement != null && playerJudgement.isInvincible == false && isBarrier)
            {
                isBarrier = false;
                successShield.SetActive(false);
            }
            judgementEnd = false;
        }
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
