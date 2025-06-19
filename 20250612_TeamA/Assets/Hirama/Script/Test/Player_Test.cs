using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using TMPro;
using System.CodeDom;
using System.Collections;

/// <summary>
/// プレイヤーの簡易移動処理
/// </summary>
public class Player_Test : MonoBehaviour
{
    [Header("移動速度")]
    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private TMP_Text scorePopupText;

    public int score = 0;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2Dコンポーネントが見つかりません。プレイヤーにアタッチしてください。");
        }

        if (scorePopupText != null)
        {
            scorePopupText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // キーボード入力の取得
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(moveX, moveY).normalized;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    public void AddScore(int value)
    {
        score += value;
        Debug.Log("Score: " + score);

        if (scorePopupText != null)
        {
            StartCoroutine(ShowScorePopup("+" + value));
        }
    }

    private IEnumerator ShowScorePopup(string text)
    {
        scorePopupText.text = text;
        scorePopupText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        scorePopupText.gameObject.SetActive(false);
    }
}