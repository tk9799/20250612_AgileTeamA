using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [Header("落下速度")]
    [SerializeField] private float fallingSpeed;

    [Header("オブジェクトのHP")]
    [SerializeField] private int myHP=10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0,this.fallingSpeed,0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBarrage"))
        {
            myHP -= 1;
        }
    }
}
