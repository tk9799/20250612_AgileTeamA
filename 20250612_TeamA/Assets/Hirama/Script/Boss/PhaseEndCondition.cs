/// <summary>
/// ボスの終了条件を表す
/// これで「HPが指定値以下」「指定秒数経過」「キー入力」を区別できる
/// </summary>
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