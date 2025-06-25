using System.Collections;
using UnityEngine;

public class RouletteController : MonoBehaviour
{
    [SerializeField] private int awaitTime = 2;
    public float rotateSpeed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float rouletteRotateSpeed = rotateSpeed * Time.deltaTime;
        transform.Rotate(0,0,rouletteRotateSpeed);
        //this.rotateSpeed *= 0.99f;
        StartCoroutine(Roulet());
    }

    private IEnumerator Roulet()
    {
        //Random random = new Random();
        //int count = random.Next(2, 5);

        yield return new WaitForSeconds(awaitTime);

        this.rotateSpeed *= 0.997f;
        if (rotateSpeed < 5f)
        {
            rotateSpeed = 0f;
        }

        yield break;
    }
}
