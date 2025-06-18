using UnityEngine;

public class Player_Test : MonoBehaviour
{
    [Header("�ړ����x")]
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D�R���|�[�l���g��������܂���B�v���C���[�ɃA�^�b�`���Ă��������B");
        }
    }

    private void Update()
    {
        // �����E���������̓��͂��擾
        float moveX = Input.GetAxisRaw("Horizontal"); // �� ���E�̓��́iA/D, ��/���j
        float moveY = Input.GetAxisRaw("Vertical");   // �� �㉺�̓��́iW/S, ��/���j

        moveInput = new Vector2(moveX, moveY).normalized; // ���K�����Ď΂߈ړ�����葬�x��
    }

    private void FixedUpdate()
    {
        // Rigidbody2D�̑��x�𒼐ڐݒ肵�Ĉړ�
        rb.linearVelocity = moveInput * moveSpeed;
    }
}
