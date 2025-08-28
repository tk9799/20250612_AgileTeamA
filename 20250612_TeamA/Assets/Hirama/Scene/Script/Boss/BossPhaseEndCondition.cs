using System;
/// <summary>
/// ボスの終了条件を表す
/// これで「HPが指定値以下」「指定秒数経過」「キー入力」を区別できる
/// </summary>
[Flags]
public enum BossPhaseEndCondition
{
    /// <summary>
    /// 終了条件なし
    /// </summary>
    None = 0,

    /// <summary>
    /// HPが指定値以下でフェーズ終了
    /// </summary>
    HP = 1 << 0,

    /// <summary>
    /// 指定秒数経過でフェーズ終了
    /// </summary>
    TimeElapsed = 1 << 1,

    /// <summary>
    /// キー入力で次のフェーズへ
    /// </summary>
    KeyInput = 1 << 2
}