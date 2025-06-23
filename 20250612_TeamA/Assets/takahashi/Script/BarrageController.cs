using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BarrageController : MonoBehaviour
{
    BalletPoolPlayer poolPlayer;
    [SerializeField] private float speed = 4f;
    BossHP bossHP;
    private Text scoreText;
    private float countTime = 0f;
    private Vector3 startPosition;
    public float MoveSpeed = 20.0f;
    private int score = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        poolPlayer = transform.parent.GetComponent<BalletPoolPlayer>();
        gameObject.SetActive(false);
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, MoveSpeed * Time.deltaTime, 0);
        //countTime += Time.deltaTime;
        //if (countTime >= 3.0f)
        //{
        //    //オブジェクトプールのReturnBullet関数を呼び出し自身を回収
        //    poolPlayer.ReturnBullet(this);
        //}
    }

    //private void OnEnable()
    //{
    //    Invoke("Disable", 3f);
    //}

    //private void Disable()
    //{
    //    gameObject.SetActive(false);
    //}

    //void OnDisable()
    //{
    //    CancelInvoke(); // 前回の Invoke をキャンセル
    //}

    void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false); // ぶつかったら消す（戻す）
    }

    private void OnBecameInvisible()
    {
        //追記　下の回収処理を呼び出す
        poolPlayer.ReturnBullet(this);
    }


    public void ShowInStage(Vector3 _pos)
    {
        //追記　positionを渡された座標に設定
        transform.position = _pos;
    }

    public void HideFromStage()
    {
        //オブジェクトプールのReturnBullet関数を呼び出し自身を回収
        poolPlayer.ReturnBullet(this);
    }
}
