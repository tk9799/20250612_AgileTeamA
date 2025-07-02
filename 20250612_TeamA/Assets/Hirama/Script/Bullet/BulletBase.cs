using UnityEngine;

/// <summary>
/// 弾の見た目・性能・挙動などを設定する ScriptableObject
/// プレイヤー弾、雑魚弾、ボス弾すべて共通で使用可能
/// </summary>
[CreateAssetMenu(fileName = "BulletBase", menuName = "Danmaku/BulletBase")]

public class BulletBase : ScriptableObject
{
    //[Header("弾の見た目となるスプライト画像")]
    //public Sprite sprite;

    [Header("弾の色")]
    public Color color = Color.white;

    [Header("弾のサイズ。Vector2(1, 1)が等倍")]
    public Vector2 scale = Vector2.one;

    [Header("弾が与えるダメージ量")]
    public int damage = 1;

    [Header("弾の初速")]
    public float speed = 3f;

    [Header("弾の寿命(秒)。これを過ぎると自動で消滅")]
    public float lifetime = 10f;

    [Header("当たり判定の半径。小さくするほど避けやすくなる")]
    public float hitboxRadius = 0.1f;

    [Header("弾が自動回転する速度(度/秒)")]
    public float rotationSpeed = 0f;

    [Header("弾の加速度。時間経過で速度が加算される")]
    public float acceleration = 0f;

    [Header("自機狙いにするか(発射時に自機方向へ角度補正)")]
    public bool homing = false;

    [Header("ホーミングの強さ(角度の補正速度など)※未使用時は0")]
    public float homingStrength = 0f;

    [Header("貫通弾かどうか。trueなら敵に当たっても消えない")]
    public bool piercing = false;

    [Header("敵やプレイヤーに命中したときに再生されるエフェクト")]
    public GameObject hitEffect;

    [Header("寿命切れや画面外消滅時のエフェクト")]
    public GameObject deathEffect;

    [Header("弾を発射したときに鳴らす効果音")]
    public AudioClip shotSE;

    [Header("命中時に鳴らす効果音")]
    public AudioClip hitSE;

    [Header("弾のID。識別用にスクリプト側で参照する文字列")]
    public string bulletId;

    [Header("この弾の属性：プレイヤー／敵／ボスなど")]
    public BulletType type;
}