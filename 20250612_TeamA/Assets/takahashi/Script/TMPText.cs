using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TMPText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text;//操作方法のテキストを書く
    [SerializeField] PlayerController playerController;
    private Gamepad gamepad;
    [SerializeField] public int tutorial = 0;
    public int greenObujectCount = 0;
    [SerializeField] GameObject EnemyPool1;
    [SerializeField] GameObject EnemyPool2;
    [SerializeField] GameObject EnemyWaveManager;
    [SerializeField] GameObject EnemyPoolManager;
    [SerializeField] EnemyWaveManager enemyWaveManager;

    private bool isOneProcess = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //playerController.enabled = false;
        greenObujectCount = 4;
        //EnemyPool1.SetActive(false);
        //EnemyPool2.SetActive(false);
        EnemyWaveManager.SetActive(false);
        EnemyPoolManager.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(tutorial == 0)
        {
            Text.text = "プレイヤーの移動\n左スティックで移動\nXボダンを押しながら左スティックを動かすと低速移動";
            //    "\nBボタンを押すとキャラクター移動ができます";
            //if (Input.GetKey(KeyCode.Joystick1Button1))
            //{
            //    Text.text = "左スティックで移動\nXボダンを押しながら左スティックを動かすと低速移動";
            //}
            if (greenObujectCount == 0)
            {
                tutorial = 1;
            }
        }
        else if(tutorial == 1)
        {
            Text.text = "弾幕の発射\nAボタンを押すと弾を発射できる\n押し続けると連射できる";
            //EnemyPool1.SetActive(true);
            //EnemyPool2.SetActive(true);
            EnemyWaveManager.SetActive(true);
            EnemyPoolManager.SetActive(true);
            if (!isOneProcess)
            {
                StartCoroutine(enemyWaveManager.HandleWaves());
                isOneProcess = true;
            }
            
        }
        else if (tutorial == 2)
        {
            Text.text = "ジャストガード\nタイミング良くRBボタンを押すことでジャストガードができる\n敵弾幕がプレイヤー周りの円状の" +
                "線の上を通過したタイミングがジャストガードの判定になる\nジャストガードが成功するとシールドが現れ、" +
                "一定時間敵弾幕をはじく";
        }
    }
}
