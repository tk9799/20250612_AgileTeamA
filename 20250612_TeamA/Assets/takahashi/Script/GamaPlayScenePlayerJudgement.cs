using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GamaPlayScenePlayerJudgement : MonoBehaviour
{
    [Header("初期残り残機")]
    [SerializeField] public int lives = 3;
    [Header("復活までの待ち時間")]
    [SerializeField] public float respawnDelay = 2f;

    [Header("復活時の無敵時間")]
    [SerializeField] public float invincibleTime = 2f;

    [Header("ジャストガード時の無敵時間")]
    [SerializeField] public float justGuardInvincibleTime = 2f;

    [Header("リスポーン位置")]
    [SerializeField] public Vector2 respawnPosition = new Vector2(0f, 0f);

    [SerializeField] private Transform myTransform;
    [SerializeField] private Transform playerTransfom;
    //[SerializeField] public PlayerController playerController;
    [SerializeField] public GamePlaySceneGuardController gamePlaySceneGuardController;
    [SerializeField] private GameObject guradObject;
    [SerializeField] public float invincible = 1.0f;//ジャストガード時の無敵時間
    [SerializeField] private GamePlaySceneGuardStamina gamePlaySceneGuardStamina;
    [SerializeField] public GamePlayScenePlayerControllerHP gamePlayScenePlayerControllerHP;


    public event Action<int> OnLifeChanged;
    public event Action OnPlayerDied;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] public BoxCollider2D hitCollider;

    private Vector2 myPosition;
    public bool isDead = false;
    public bool isInput = false;
    public bool isInvincible = false;//無敵判定
    public bool isGuardJudgement = false;//ジャストガード判定
    private Text lifeText;
    //private float chargeTime = 0f;

    private bool oneProcess = false;
    private bool isGenerateGuard = false;//シールドを表示非表示判定


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button5) && !isInvincible && !isInput)
        {
            isGuardJudgement = true;
            if (gamePlaySceneGuardStamina != null && gamePlaySceneGuardStamina.canStamina == true)
            {
                isInput = true;

                StartCoroutine(GeneratorGuard());
            }
        }
        else if (gamePlaySceneGuardStamina != null && gamePlaySceneGuardStamina.canStamina == false)
        {
            isGuardJudgement = false;
        }
        
        if (hitCollider.enabled == true && spriteRenderer.color == Color.red)
        {
            spriteRenderer.color = Color.white;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("hit");
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss") ||
            collision.gameObject.CompareTag("Bullet"))
        {
            gamePlayScenePlayerControllerHP.TakeDamage();
            hitCollider.enabled = false;
        }
    }


    private IEnumerator GeneratorGuard()
    {
        Debug.Log("ガード");
        isGenerateGuard = true;
        //一定時間シールドを出現する
        //guradObject.SetActive(true);
        gamePlaySceneGuardController.spriteRenderer.enabled = true;
        gamePlaySceneGuardController.circeCollider.enabled = true;
        yield return new WaitForSeconds(0.05f);
        Debug.Log("ガード非表示");

        //シールドを消す
        isInput = false;
        
        isGenerateGuard = false;
    }

    public IEnumerator InvincibleTime()
    {
        if (oneProcess)
        {
            yield break;
        }

        oneProcess = true;
        Debug.Log("無敵時間開始");
        hitCollider.enabled = false;
        isInvincible = true;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(justGuardInvincibleTime);

        Debug.Log("無敵時間終了");
        spriteRenderer.color = Color.white;
        hitCollider.enabled = true;
        oneProcess = false;
        isInvincible = false;
    }
}
