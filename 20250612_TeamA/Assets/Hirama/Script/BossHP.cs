using UnityEngine;

public class BossHP : MonoBehaviour
{
    [SerializeField] public float MaxHP = 100f;
    private float currentHP;

    private EnemyBarrageControl barrageControl;

    void Start()
    {
        currentHP = MaxHP;

        barrageControl = GetComponent<EnemyBarrageControl>();
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            BossDie();
        }
    }

    void BossDie()
    {
        if (barrageControl != null)
        {
            barrageControl.OnDeath();
        }

        gameObject.SetActive(false);
    }
}