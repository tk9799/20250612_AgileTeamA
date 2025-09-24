using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TMPText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text;//操作方法のテキストを書く
    //[SerializeField] PlayerController playerController;
    private Gamepad gamepad;
    [SerializeField] public int tutorial = 0;
    public int greenObujectCount = 0;
    private bool isSelect = false;
    [SerializeField] GameObject BulletPool;
    [SerializeField] GameObject BulletPool2;
    [SerializeField] GameObject EnemyWaveManager;
    [SerializeField] GameObject EnemyPoolManager;
    [SerializeField] GameObject BulletPoolManager;
    //[SerializeField] EnemyWaveManager enemyWaveManager;
    [SerializeField] EnemyWaveManagerTutorial enemyWaveManagerTutorial;
    [SerializeField] GuardController2 guardController2;
    [SerializeField] TutorialText tutorialText;
    [SerializeField] GameObject imageObject;
    [SerializeField] GameObject selectButton;
    public List<GameObject> enemyList = new List<GameObject>();
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] BossBase bossBase;
    [SerializeField] GameManager gameManager;
    //public int dieCount = 0;
    private bool isOneProcess = false;
    public bool isAttack = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //playerController.enabled = false;
        greenObujectCount = 4;
        imageObject.SetActive(false);
        selectButton.SetActive(false);
        BulletPool.SetActive(false);
        BulletPool2.SetActive(false);
        EnemyWaveManager.SetActive(false);
        EnemyPoolManager.SetActive(false);
        BulletPoolManager.SetActive(false);
        //GameObject obj= Instantiate(bossPrefab, Vector2.zero, Quaternion.identity);
        //bossBase=obj.GetComponent<BossBase>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (tutorial == 0)
        {
            Text.text = "プレイヤーの移動\n左スティックで移動\nXボダンを押しながら左スティックを動かすと低速移動" +
                "\nXボタンを押しながらスティック入力をすると低速移動";
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
        else if (tutorial == 1)
        {
            //Debug.Log(dieCount);
            Text.text = "弾幕の発射\nAボタンを押すと弾を発射できる\n押し続けると連射できる";
            if (BulletPool != null)
            {
                BulletPool.SetActive(true);
            }
            if (BulletPool2 != null)
            {
                BulletPool2.SetActive(true);
            }

            EnemyWaveManager.SetActive(true);
            EnemyPoolManager.SetActive(true);
            if (BulletPoolManager != null)
            {
                BulletPoolManager.SetActive(true);
            }
            if (enemyWaveManagerTutorial != null && enemyWaveManagerTutorial.waveList.Count == 1)
            {
                tutorial = 2;
            }
            //if (!isOneProcess)
            //{
            //    //StartCoroutine(enemyWaveManager.HandleWaves());
            //    isOneProcess = true;
            //}

        }
        else if (tutorial == 2)
        {
            Text.text = "ジャストガード\nタイミング良くRBボタンを押すことでジャストガードができる\n敵弾幕がプレイヤー周りの円状の" +
                "線の上を通過したタイミングがジャストガードの判定になる\nジャストガードが成功するとシールドが現れ、" +
                "一定時間敵弾幕をはじく";

            if (guardController2 != null && guardController2.justGuardCount >= 5)
            {
                isAttack = true;
                //enemyWaveManager.SkipToBoss();
                if (enemyWaveManagerTutorial != null && enemyWaveManagerTutorial.isWaveRunning == false)
                {
                    tutorial = 3;
                }
                //guardController2.justGuardCount = 0;
            }
        }
        else if (tutorial == 3)
        {
            Text.text = "これまでの操作方法でボスを倒す";

            if(bossBase == null)
            {
                Debug.LogError("ボスがnullになっている！！！！！！！！！！！！！");
            }

            //Debug.Log($"ボスが生きているか(TMPText)：{bossBase.IsBossDestroy} ");
            //if(gameManager!=null&&gameManager.IsGameClear == true)
            //{
            //    Debug.Log("Clear");
            //    tutorial = 4;
            //}

            if (gameManager != null && gameManager.isClear == true)
            //if (bossBase.IsDead)
            {
                Debug.Log("Clear");
                tutorial = 4;
            }
            else if (bossBase == null)
            {
                Debug.LogError("bossBaseがnullです");
            }
        }
        else if (tutorial == 4)
        {
            Text.text = "";
            tutorialText.instructionsText.text = "チュートリアルクリア\nBボタン入力でタイトルに戻る";
            imageObject.SetActive(true);
            StartCoroutine(StargeSelect());
            if (isSelect && Input.GetKey(KeyCode.JoystickButton1))
            {
                SceneManager.LoadScene("GameTitleScene");
            }
        }
    }

    IEnumerator StargeSelect()
    {
        yield return new WaitForSeconds(2f);
        selectButton.SetActive(true);
        isSelect = true;
    }
}
