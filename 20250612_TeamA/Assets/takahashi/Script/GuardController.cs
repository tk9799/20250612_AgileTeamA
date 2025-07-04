using UnityEngine;

public class GuardController : MonoBehaviour
{
    [SerializeField] PlayerJudgement playerJudgement;
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform myTransform;
    [SerializeField] GameObject enemyBarrage;
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public CircleCollider2D circeCollider;
    public bool isHit = false;
    public bool judgementEnd = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //this.gameObject.SetActive(false);
        spriteRenderer.enabled = true;
        circeCollider.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        myTransform.position = playerTransform.position;
        //if (playerJudgement != null && playerJudgement.isInput == false)
        //{
        //    gameObject.SetActive(false);
        //}
        //else if (judgementEnd)
        //{
        //    gameObject.SetActive(false);
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("EnemyBullet"))
        {
            Debug.Log("衝突判定");
            judgementEnd = false;
            //isHit = true;
            //表示非表示でオブジェクトを管理している
            Debug.Log(enemyBarrage);
            collision.gameObject.SetActive(false);
            //他のスクリプトのコルーチンの処理する
            StartCoroutine(playerJudgement.InvincibleTime());
            judgementEnd = true;
            Debug.Log("InvincibleTime" + judgementEnd);
        }
    }
}
