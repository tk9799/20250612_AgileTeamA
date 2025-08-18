using UnityEngine;

/// <summary>
/// ボスの1フェーズを表すデータ
/// </summary>
[CreateAssetMenu(fileName = "BossPhase", menuName = "Barrage/Boss/BossPhase")]
public class BossPhase : ScriptableObject
{
    [Header("このフェーズの表示名")]
    public string PhaseDisplayName = "";

    [Header("このフェーズの最大HP")]
    public int phaseMaxHP = 0;

    [Header("このフェーズの攻撃パターン")]
    public BossAttackPattern attackPattern;

    [Header("このフェーズの移動パターン")]
    public BossMovementPattern movementPattern;

    [Header("フェーズ終了条件(複数可)")]
    public BossPhaseEndCondition endCondition = BossPhaseEndCondition.None;

    [Header("終了条件_HP")]
    public int hpThreshold = 0;

    [Header("終了条件_制限時間")]
    public float timeLimit = 0f;
}