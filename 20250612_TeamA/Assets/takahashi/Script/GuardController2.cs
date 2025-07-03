using UnityEngine;

public class GuardController2 : MonoBehaviour
{
    [SerializeField] GameObject enemyBarrageObject;
    [SerializeField] GameObject playerJudgeObject;
    [SerializeField] Transform enemyBarrage;
    [SerializeField] Transform playerJudgeTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 ememyBarragePosition = enemyBarrage.position;
        Vector2 playerPosition = playerJudgeTransform.position;
        Vector2 distance = playerPosition - ememyBarragePosition;
        float d = distance.magnitude;
    }
}
