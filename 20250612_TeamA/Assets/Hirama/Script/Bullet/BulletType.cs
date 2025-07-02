/// <summary>
/// 弾の種類を表す
/// これにより、弾が「プレイヤー弾」「雑魚敵弾」「ボス弾」など区別できる
/// </summary>
public enum BulletType
{
    /// <summary>
    /// プレイヤーの弾
    /// </summary>
    Player,

    /// <summary>
    /// >雑魚敵の弾
    /// </summary>
    Enemy,

    /// <summary>
    /// ボスの弾
    /// </summary>
    Boss
}