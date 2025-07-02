using UnityEngine;

public class GuardController : MonoBehaviour
{
    [SerializeField] PlayerJudgement playerJudgement;
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform myTransform;
    [SerializeField] GameObject enemyBarrage;
    public bool isHit = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        myTransform.position = playerTransform.position;
        //if (playerJudgement != null && playerJudgement.isInvincible)
        //{

        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("EnemyBullet"))
        {
            Debug.Log("衝突判定");
            isHit = true;
            //表示非表示でオブジェクトを管理している
            enemyBarrage.SetActive(false);
            //他のスクリプトのコルーチンの処理する
            StartCoroutine(playerJudgement.InvincibleTime());
        }
    }
}
