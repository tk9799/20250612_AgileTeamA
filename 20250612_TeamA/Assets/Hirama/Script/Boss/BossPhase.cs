using UnityEngine;

/// <summary>
/// ボスの1フェーズを表すデータ
/// </summary>
[CreateAssetMenu(fileName = "BossPhase", menuName = "Barrage/BossPhase")]
public class BossPhase : ScriptableObject
{
    public enum PhaseEndCondition
    {
        //終了条件なし
        None,
        //HPが指定値以下でフェーズ終了
        HP,
        //指定秒数経過でフェーズ終了
        TimeElapsed,
        //キー入力で次のフェーズへ
        KeyInput
    }

    [Header("このフェーズの攻撃パターン")]
    public EnemyAttackPattern attackPattern;

    [Header("このフェーズの移動パターン")]
    public EnemyMovementPattern movementPattern;

    [Header("フェーズ終了条件")]
    public PhaseEndCondition endCondition = PhaseEndCondition.None;

    [Header("終了条件のHP値")]
    public int hpThreshold = 0;

    [Header("終了条件の秒数")]
    public float timeLimit = 0.0f;
}