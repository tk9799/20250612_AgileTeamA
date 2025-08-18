using UnityEngine;

/// <summary>
/// 雑魚敵の行動(移動や攻撃など)を制御
/// EnemyDataに設定されたパターンなどを実行する役割
/// </summary>
//[RequireComponent(typeof(EnemyBase))]
public class EnemyController : MonoBehaviour
{
    [Header("雑魚敵設定データ")]
    public EnemyData enemyData;

    //同じGameObjectにアタッチされたEnemyBaseを取得
    private EnemyBase enemyBase;

    //インスタンス化された移動パターン
    private MovementPattern movementPatternInstance;
    //インスタンス化された攻撃パターン
    private AttackPattern attackPatternInstance;

    /// <summary>
    /// 初期化処理。EnemyBaseを取得しておく
    /// </summary>
    void Awake()
    {
        enemyBase = GetComponent<EnemyBase>();
    }

    /// <summary>
    /// EnemyDataに基づく初期化処理。外部(EnemyPoolなど)から呼び出される想定
    /// </summary>
    public void Init(EnemyData data)
    {
        enemyData = data;

        if (movementPatternInstance != null)
        {
            //古いパターンがあれば破棄
            movementPatternInstance = null;
        }

        if (enemyData.movementPattern != null)
        {
            //Debug.Log($"Instantiating movement pattern: {enemyData.movementPattern.name}");
            //ScriptableObjectは共有されるのでインスタンス化して使うのが安全
            movementPatternInstance = Instantiate(enemyData.movementPattern);
            movementPatternInstance.Initialize(this);
        }
        else
        {
            //Debug.LogWarning("movementPattern is null in EnemyData");
        }

        if (attackPatternInstance != null)
        {
            attackPatternInstance = null;
        }

        if (enemyData.attackPattern != null)
        {
            attackPatternInstance = Instantiate(enemyData.attackPattern);
            attackPatternInstance.Initialize(this);
        }
    }

    /// <summary>
    /// 毎フレームの更新処理。移動パターンがあれば実行
    /// </summary>
    void Update()
    {
        if (movementPatternInstance != null)
        {
            movementPatternInstance.UpdateMovement(this);
        }

        if (attackPatternInstance != null)
        {
            attackPatternInstance.UpdateAttack(this);
        }
    }
}