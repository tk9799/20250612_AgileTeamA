using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GamePlaySceneGuardStamina : MonoBehaviour
{
    [SerializeField] GamaPlayScenePlayerJudgement gamaPlayScenePlayerJudgement;
    [SerializeField] Image staminagage;
    [SerializeField] public float maxStaminaGauge = 10f;
    public bool canStamina = false;
    private bool recoveryStamina = false;
    public float currentStamina;//現在のスタミナ
    private float chargeTime = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentStamina = maxStaminaGauge;
    }

    // Update is called once per frame
    void Update()
    {
        if (gamaPlayScenePlayerJudgement != null && gamaPlayScenePlayerJudgement.isGuardJudgement == true &&
            currentStamina >= 0)
        {
            canStamina = true;
            //currentStamina -= 1;

            if (canStamina)
            {
                currentStamina -= 1;
                canStamina = false;
            }

            if (currentStamina < 0)
            {
                currentStamina = 0;
            }
            staminagage.fillAmount = currentStamina / maxStaminaGauge;
            Debug.Log(currentStamina);
        }
        else if (currentStamina == 0)
        {
            canStamina = false;
            Debug.Log("使用できません");
        }

        chargeTime += Time.deltaTime;
        Debug.Log(canStamina);

        if (chargeTime >= 3.0f)
        {
            Debug.Log("1回復");
            recoveryStamina = true;
            if (currentStamina < maxStaminaGauge)
            {
                currentStamina += 1;

                if (currentStamina > maxStaminaGauge)
                {
                    currentStamina = maxStaminaGauge;
                }


                staminagage.fillAmount = currentStamina / maxStaminaGauge;
                Debug.Log(currentStamina);

                recoveryStamina = false;
            }
            chargeTime = 0f;
        }

        if (currentStamina < maxStaminaGauge)
        {
            //StartCoroutine(ChargeStamina());


        }
    }
}
