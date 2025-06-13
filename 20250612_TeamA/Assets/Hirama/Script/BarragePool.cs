using UnityEngine;
using System.Collections.Generic;

public class BarragePool : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int poolSize = 200;

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            pool.Enqueue(bullet);
        }
    }

    public GameObject GetBullet(Vector3 position, Quaternion rotaion)
    {
        GameObject bullet = pool.Count > 0 ? pool.Dequeue() : Instantiate(bulletPrefab);
        bullet.transform.position = position;
        bullet.transform.rotation = rotaion;
        bullet.SetActive(true);
        return bullet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        pool.Enqueue(bullet);
    }
}