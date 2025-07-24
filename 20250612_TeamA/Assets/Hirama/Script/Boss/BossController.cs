using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("ボス設定データ")]
    public BossData bossData;

    private BossBase bossBase;
    private BossPhase currentPhase;
    private int currentPhaseIndex = 0;

    private BossAttackPattern currentAttackPattern;
    private BossMovementPattern currentMovementPattern;

    private float phaseTimer = 0f;

    private void Awake()
    {
        bossBase = GetComponent<BossBase>();
        if (bossBase == null)
        {
            Debug.LogError("BossBase コンポーネントが見つかりません");
        }
    }

    private void OnEnable()
    {
        if (bossData != null)
        {
            bossBase.Init(bossData);
            StartPhase(0);
        }
    }

    private void Update()
    {
        if (bossBase == null || bossBase.IsDead) return;

        phaseTimer += Time.deltaTime;

        currentAttackPattern?.UpdateAttack(this);
        currentMovementPattern?.UpdateMovement(this);

        if (CheckPhaseEndCondition(currentPhase))
        {
            GoToNextPhase();
        }
    }

    private void StartPhase(int index)
    {
        if (index < 0 || index >= bossData.phases.Count)
        {
            Debug.Log("すべてのフェーズが終了しました");
            bossBase.TakeDamage(bossBase.CurrentHP);
            return;
        }

        currentPhaseIndex = index;
        currentPhase = bossData.phases[index];
        phaseTimer = 0f;

        currentAttackPattern = currentPhase.attackPattern as BossAttackPattern;
        currentAttackPattern?.Initialize(this);

        currentMovementPattern = currentPhase.movementPattern as BossMovementPattern;
        currentMovementPattern?.Initialize(this);

        Debug.Log($"ボスフェーズ {index + 1} 開始");
    }

    private void GoToNextPhase()
    {
        currentAttackPattern?.OnExit(this);
        currentPhaseIndex++;
        StartPhase(currentPhaseIndex);
    }

    private bool CheckPhaseEndCondition(BossPhase phase)
    {
        switch (phase.endCondition)
        {
            case BossPhaseEndCondition.HP:
                return bossBase.CurrentHP <= phase.hpThreshold;
            case BossPhaseEndCondition.TimeElapsed:
                return phaseTimer >= phase.timeLimit;
            case BossPhaseEndCondition.KeyInput:
                return Input.GetKeyDown(KeyCode.Space);
            case BossPhaseEndCondition.None:
            default:
                return false;
        }
    }

    public Vector3 GetBossPosition() => transform.position;
    public void SetBossPosition(Vector3 pos) => transform.position = pos;
    public BossBase GetBossBase() => bossBase;
}