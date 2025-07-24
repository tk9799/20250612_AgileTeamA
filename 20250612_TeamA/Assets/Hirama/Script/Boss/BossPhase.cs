using UnityEngine;

/// <summary>
/// ボスの1フェーズを表すデータ
/// </summary>
[CreateAssetMenu(fileName = "BossPhase", menuName = "Barrage/Boss/BossPhase")]
public class BossPhase : ScriptableObject
{
    [Header("このフェーズの攻撃パターン")]
    public BossAttackPattern attackPattern;

    [Header("このフェーズの移動パターン")]
    public BossMovementPattern movementPattern;

    [Header("フェーズ終了条件")]
    public BossPhaseEndCondition endCondition = BossPhaseEndCondition.None;

    [Header("終了条件のHP値")]
    public int hpThreshold = 0;

    [Header("終了条件の秒数")]
    public float timeLimit = 0.0f;
}