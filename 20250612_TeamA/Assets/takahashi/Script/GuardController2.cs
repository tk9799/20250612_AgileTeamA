using UnityEngine.SceneManagement;
using UnityEngine;

public class GuardController2 : MonoBehaviour
{
    [Header("敵弾幕オブジェクト")]
    [SerializeField] Transform justGuardTransform;//プレイヤー判定オブジェクトのトランスフォーム
    [SerializeField] private float justGuardDistanceRadius = 0f;//オブジェクトの範囲
    [SerializeField] PlayerJudgement playerJudgement;//プレイヤー判定のスクリプト
    [SerializeField] public SpriteRenderer spriteRenderer;//シールドの画像
    [SerializeField] public CircleCollider2D circeCollider;
    [SerializeField] public BulletPoolManager bulletPoolManager;
    [SerializeField] private GameObject successShield;
    [SerializeField] private TMPText tmpText;
    public bool judgementEnd = false;
    private bool isBarrier = false;
    public int justGuardCount = 0;
    private float nearestDistance = 0.0f;

    private float justRadius = 2.0f;
    private float justRange = 0.45f;
    public GameObject bullet;

    void Start()
    {
        spriteRenderer.enabled = true;
        circeCollider.enabled = true;
        successShield.SetActive(false);
        //Debug.Log(justGuardDistanceRadius);
    }

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
                GetNearestBulletDistance();
                bullet = bulletPoolManager.activeBulletList[listNum];

                float notePos = Vector2.Distance(bullet.transform.position, justGuardTransform.position);//2つのpositionの距離を返す

                float distances = Mathf.Abs(notePos - justRadius);//判定ラインからの距離
                //Debug.Log("hit");
                Debug.Log(nearestDistance);
                if (nearestDistance <= justGuardDistanceRadius)
                {
                    Debug.Log("justGuard");
                    bullet.SetActive(false);

                    bulletPoolManager.activeBulletList.RemoveAt(listNum);

                    StartCoroutine(playerJudgement.InvincibleTime());
                    
                    judgementEnd = true;
                    
                    successShield.SetActive(true);
                    isBarrier = true;
                    if (SceneManager.GetActiveScene().name == "TutorialScene"&&tmpText!=null&&tmpText.tutorial==2)
                    {
                        
                        justGuardCount++;
                        Debug.Log(justGuardCount);
                    }
                    break;
                }
                bulletPoolManager.activeBulletList.Remove(bullet);
            }
        }
        if (playerJudgement != null && playerJudgement.isInvincible == false && isBarrier)
        {
            isBarrier = false;
            successShield.SetActive(false);
        }
        judgementEnd = false;
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
        nearestDistance = Mathf.Infinity; // 大きい数で初期化

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
