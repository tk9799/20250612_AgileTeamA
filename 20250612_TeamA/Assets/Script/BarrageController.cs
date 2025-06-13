using System.Collections;
using UnityEngine;

public class BarrageController:MonoBehaviour
{
    [SerializeField] private Transform barrageTransfom;
    [SerializeField] private float speed = 4f;
    private float countTime = 0f;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        Vector3 barrageSpeed = Vector3.up * speed * Time.deltaTime;
        Debug.Log(countTime);
        countTime += Time.deltaTime;

        if (countTime >= 3f)
        {
            Destroy(this.gameObject);
        }
    }

    //private IEnumerator BarrageSpeed()
    //{

    //}
}
