using UnityEngine;
using System.Collections;

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

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(FireRoutine());
    }

    private void Update()
    {
        // Zキー押しっぱなしで連射
        isFiring = Input.GetKey(KeyCode.Z);
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
