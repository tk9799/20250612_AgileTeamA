using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BarrageController:MonoBehaviour
{
    [SerializeField] private Transform barrageTransfom;
    [SerializeField] private float speed = 4f;
    [SerializeField] BossHP bossHP;
    private Text scoreText;
    private float countTime = 0f;
    private Vector3 startPosition;
    public float MoveSpeed = 20.0f;// 移動値
    private int score = 0;//スコア

    private void Start()
    {
        startPosition = transform.position;
        //scoreText = GameObject.Find("ScereText").GetComponent<Text>();
        //scoreText.text = "";
    }

    private void Update()
    {
        // 位置の更新
        transform.Translate(0, MoveSpeed * Time.deltaTime, 0);//オブジェクトの移動

        //Debug.Log(countTime);
        countTime += Time.deltaTime;

        if (countTime >= 3f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Boss"))
        {
            if(bossHP != null&& bossHP.destroy)
            {
                score += 5000;
                this.scoreText.text = this.score.ToString();
            }
        }
    }
}
