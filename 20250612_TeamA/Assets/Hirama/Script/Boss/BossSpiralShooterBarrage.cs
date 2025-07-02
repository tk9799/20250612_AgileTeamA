using UnityEngine;

public class BossSpiralShooterBarrage : MonoBehaviour
{
    [Header("弾プレハブ")]
    public GameObject bulletPrefab;

    [Header("弾を発射する間隔（秒）")]
    public float fireInterval = 0.05f;

    [Header("スパイラル回転速度")]
    public float spiralSpeed = 360f;

    [Header("スパイラルの回転回数")]
    public int totalRotations = 3;

    [Header("1回転あたりの弾数")]
    public int bulletsPerRotation = 100;

    //現在の角度
    private float currentAngle = 0f;

    private void Start()
    {
        //ゲーム開始時にスパイラル発射コルーチンを開始
        StartCoroutine(ShootSpiral());
    }

    System.Collections.IEnumerator ShootSpiral()
    {
        int totalBullets = totalRotations * bulletsPerRotation;
        float angleStep = 360f / bulletsPerRotation;
        float angleIncrementPerShot = spiralSpeed * fireInterval;

        for (int i = 0; i < totalBullets; i++)
        {
            float angle = currentAngle * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = dir * 5f;

            currentAngle += angleIncrementPerShot;
            yield return new WaitForSeconds(fireInterval);
        }
    }
}