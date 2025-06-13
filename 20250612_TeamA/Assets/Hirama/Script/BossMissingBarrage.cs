using System.Collections;
using UnityEngine;

public class BossMissingBarrage : MonoBehaviour
{
    [Header("íeÇÃÉvÅ[Éã(BarragePool)")]
    [SerializeField] private BarragePool barragePool;
    [Header("")]
    [SerializeField] private Transform[] firePoints;
    [Header("íeÇÃë¨ìx")]
    [SerializeField] float bulletSpeed = 3f;
    [Header("íeÇÃê∂ë∂éûä‘(ïb)")]
    [SerializeField] float bulletLifeTime = 6f;
    [Header("")]
    [SerializeField] private float mixInterval = 2f;
    [Header("")]
    [SerializeField] private float minInterval = 0.5f;

    void Start()
    {
        StartCoroutine(FireBarrageSequence());
    }

    private IEnumerator FireBarrageSequence()
    {
        for (int i = 0; i < firePoints.Length; i++)
        {
            Transform firePoint = firePoints[i];

            Vector2 direction = Vector2.down;

            GameObject bullet = barragePool.GetBullet(firePoint.position, Quaternion.identity);
            Barrage barrage = bullet.GetComponent<Barrage>();
            barrage.Initialize(direction, bulletSpeed, bulletLifeTime, barragePool, this);

            float waitTime = Random.Range(minInterval, mixInterval);
            yield return new WaitForSeconds(waitTime);
        }
    }

    public void AddBullet(Barrage bullet)
    {

    }

    public void RemoveBullet(Barrage bullet)
    {

    }
}