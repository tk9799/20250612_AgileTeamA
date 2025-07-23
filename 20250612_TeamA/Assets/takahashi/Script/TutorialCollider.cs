using UnityEngine;

public class tutorialCollider : MonoBehaviour
{
    [SerializeField] private GameObject myGameObject;
    [SerializeField] private TMPText tmpText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            tmpText.greenObujectCount--;
            Destroy(myGameObject);
        }
    }
}
