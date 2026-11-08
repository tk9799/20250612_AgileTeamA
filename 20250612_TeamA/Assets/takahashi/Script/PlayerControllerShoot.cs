using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerControllerShoot : MonoBehaviour
{
    [Header("連射間隔（秒）")]
    public float fireInterval = 0.05f;

    [Header("使用する弾のデータ")]
    public BulletBase playerBulletData;

    [Header("発射位置（複数ショット対応）")]
    public Transform[] shotPoints;

    private bool isFiring = false;
    private SpriteRenderer sr;
    [SerializeField] TMPText tmpText;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(FireRoutine());
    }

    private void Update()
    {
        if (tmpText != null && tmpText.tutorial != 2||tmpText.isAttack)
        {
            isFiring = Input.GetKey(KeyCode.JoystickButton0);
        }
        else if (SceneManager.GetActiveScene().name == "TutorialScene" && tmpText != null && tmpText.tutorial == 2
            && tmpText.isAttack==false)
        {
            //Debug.Log("発射できない");
            isFiring = false;
        }
    }

    private IEnumerator FireRoutine()
    {
        while (true)
        {
            if (isFiring && playerBulletData != null && sr != null && sr.enabled)
            {
                foreach (var point in shotPoints)
                {
                    Vector2 direction = Vector2.up;

                    //弾発射（BulletPoolManager経由）
                    BulletPoolManager.Instance.FireBullet(playerBulletData, point.position, direction);

                    //効果音再生
                    if (playerBulletData.shotSE != null)
                        AudioSource.PlayClipAtPoint(playerBulletData.shotSE, point.position);
                }
            }

            yield return new WaitForSeconds(fireInterval);
        }
    }
    public void StopShooting()
    {
        isFiring = false;
    }
}
