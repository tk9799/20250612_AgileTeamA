using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerJudgement : MonoBehaviour
{
    [Header("‰Šúc‚èc‹@")]
    [SerializeField] private int lives = 3;
    [SerializeField] private Transform myTransform;
    [SerializeField] private Transform playerTransfom;
    private Vector2 myPosition;

    private Text lifeText;

    private void Start()
    {
        lifeText = GameObject.Find("LifeText").GetComponent<Text>();
        lifeText.text = "" + lives;
    }

    new void Update()
    {
        //myTransform.position= playerTransfom.position;
        //myPosition = myTransform.position;
        //myPosition.x = myTransform.position.x + 0.11f;
        //myPosition.y = myTransform.position.y - 0.14f;

        //playerTransfom.position = myPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBarrage"))
        {
            Debug.Log("hit");
            //invincibilityTime -= Time.deltaTime;
            //if(invincibilityTime > 0)
            //{
            //    boxCollider.enabled = false;
            //}
            //else if (invincibilityTime <= 0)
            //{
            //    boxCollider.enabled = true;
            //    invincibilityTime = DefoltInvincibilityTime;
            //}

            if (lives == 0)
            {
                SceneManager.LoadScene("GameOverScene");
            }
            else
            {
                lives -= 1;
                lifeText.text = lives.ToString();
            }
            

        }
    }
}
