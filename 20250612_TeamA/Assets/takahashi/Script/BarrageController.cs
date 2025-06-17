using System.Collections;
using UnityEngine;

public class BarrageController:MonoBehaviour
{
    [SerializeField] private Transform barrageTransfom;
    [SerializeField] private float speed = 4f;
    private float countTime = 0f;
    private Vector3 startPosition;
    public float MoveSpeed = 20.0f;         // 移動値

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // 位置の更新
        transform.Translate(0, MoveSpeed * Time.deltaTime, 0);//オブジェクトの移動

        //Debug.Log(countTime);
        countTime += Time.deltaTime;

        if (countTime >= 3f)
        {
            Destroy(this.gameObject);
        }
    }

}
