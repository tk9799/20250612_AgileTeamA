using System.Collections;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GuardController2 : MonoBehaviour
{
    [Header("敵弾幕オブジェクト")]
    //[SerializeField] GameObject enemyBarrageObject;
    //[SerializeField] GameObject enemyBarrageObject2;
    //[SerializeField] GameObject enemyBarrageObject3;
    //[SerializeField] GameObject enemyBarrageObject4;
    //[SerializeField] GameObject enemyBarrageObject5;
    ////[Header("プレイヤーオブジェクト")]
    ////[SerializeField] GameObject playerJudgeObject;
    //[SerializeField] Transform enemyBarrage;//敵弾幕のトランスフォーム
    //private float enemyBarrageRadius = 1f;//オブジェクトの範囲
    //[SerializeField] Transform enemyBarrage2;//敵弾幕のトランスフォーム
    //private float enemyBarrage2Radius = 1f;//オブジェクトの範囲
    //[SerializeField] Transform enemyBarrage3;//敵弾幕のトランスフォーム
    //private float enemyBarrage3Radius = 1f;//オブジェクトの範囲
    //[SerializeField] Transform enemyBarrage4;//敵弾幕のトランスフォーム
    //private float enemyBarrage4Radius = 1f;//オブジェクトの範囲
    //[SerializeField] Transform enemyBarrage5;//敵弾幕のトランスフォーム
    //private float enemyBarrage5Radius = 1f;//オブジェクトの範囲
    [SerializeField] Transform justGuardTransform;//プレイヤー判定オブジェクトのトランスフォーム
    [SerializeField] private float justGuardDistanceRadius = 0f;//オブジェクトの範囲
    [SerializeField] PlayerJudgement playerJudgement;//プレイヤー判定のスクリプト
    [SerializeField] public SpriteRenderer spriteRenderer;//シールドの画像
    [SerializeField] public CircleCollider2D circeCollider;
    [SerializeField] public BulletPoolManager bulletPoolManager;
    [SerializeField] private GameObject successShield;
    [SerializeField] private TMPText tmpText;
    //[SerializeField] private GuardStamina guardStamina;
    public bool judgementEnd = false;
    private bool isBarrier = false;
    public int justGuardCount = 0;

    private float justRadius = 2.0f;
    private float justRange = 0.45f;
    public GameObject bullet;

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
        if (playerJudgement != null && playerJudgement.isDead==true)
        {
            spriteRenderer.enabled = false;
        }
        else
        {
            spriteRenderer.enabled = true;
        }

        if (playerJudgement != null && playerJudgement.isGuardJudgement==true)
        {
            
            for (int listNum = bulletPoolManager.activeBulletList.Count - 1; listNum >= 0; listNum--)
            {
                bullet = bulletPoolManager.activeBulletList[listNum];
                float nearest = GetNearestBulletDistance();


                float notePos = Vector2.Distance(bullet.transform.position, justGuardTransform.position);//2つのpositionの距離を返す

                float distances = Mathf.Abs(notePos - justRadius);//判定ラインからの距離
                //Debug.Log("hit");
                Debug.Log("一番近い弾との距離 = " + nearest);
                if (distances <= justGuardDistanceRadius)
                {
                    //Debug.Log(distances);
                    Debug.Log("justGuard");
                    bullet.SetActive(false);
                    bulletPoolManager.activeBulletList.RemoveAt(listNum);
                    //Debug.Log(enemyBarrageObject);
                    StartCoroutine(playerJudgement.InvincibleTime());
                    StartCoroutine(SuccessJustGuad());
                    judgementEnd = true;
                    successShield.SetActive(true);
                    isBarrier = true;
                    if (SceneManager.GetActiveScene().name == "TutorialScene"&&tmpText!=null&&tmpText.tutorial==2)
                    {
                        
                        justGuardCount++;
                        Debug.Log(justGuardCount);
                    }
                    break;
                    //Debug.Log("InvincibleTime" + judgementEnd);
                }
            }
        }
        if (playerJudgement != null && playerJudgement.isInvincible == false && isBarrier)
        {
            isBarrier = false;
            successShield.SetActive(false);
        }
        judgementEnd = false;
    }

    private IEnumerator SuccessJustGuad()
    {
        for (int listNum = bulletPoolManager.activeBulletList.Count - 1; listNum >= 0; listNum--)
        {
            bullet = bulletPoolManager.activeBulletList[listNum];

            float notePos = Vector2.Distance(justGuardTransform.position, bullet.transform.position);//2つのpositionの距離を返す

            float distances = Mathf.Abs(notePos - justRadius);//判定ラインからの距離

            //Debug.Log(notePos);
            if (distances <= justGuardDistanceRadius)
            {
                //Debug.Log(distances);
                //Debug.Log("justGuard");
                bullet.SetActive(false);
                //bulletPoolManager.activeBulletList.RemoveAt(listNum);
                //Debug.Log(enemyBarrageObject);
                judgementEnd = true;
                //successShield.SetActive(true);
                isBarrier = true;
                break;
                //Debug.Log("InvincibleTime" + judgementEnd);
            }
        }

        yield return new WaitForSeconds(playerJudgement.justGuardInvincibleTime);

        yield break;
    }

    void OnDrawGizmosSelected()
    {
        if (justGuardTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(justGuardTransform.position, justGuardDistanceRadius);
        }

    }

    float GetNearestBulletDistance()
    {
        float nearestDistance = Mathf.Infinity; // 大きい数で初期化

        // 弾リストを1つずつ調べる
        foreach (GameObject enemyBullet in bulletPoolManager.activeBulletList)
        {
            // プレイヤーと弾との距離
            float dist = Vector2.Distance(justGuardTransform.position, enemyBullet.transform.position);

            // 今までで一番近ければ更新
            if (dist < nearestDistance)
            {
                nearestDistance = dist;
            }
        }

        return nearestDistance;
    }
}
