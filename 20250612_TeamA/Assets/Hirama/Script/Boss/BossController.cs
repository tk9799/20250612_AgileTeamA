using System.Collections;
using UnityEngine;

/// <summary>
/// ボスのフェーズ制御や攻撃・移動の管理を行うコンポーネント
/// </summary>
public class BossController : MonoBehaviour
{
    private BossBase bossBase;
    private BossData bossData;

    private int currentPhaseIndex = 0;
    private BossPhase currentPhase;
    private float phaseTimer = 0f;

    private void Start()
    {
        bossBase = GetComponent<BossBase>();
        bossData = bossBase.bossData;

        StartPhase(currentPhaseIndex);
    }

    private void Update()
    {
        if (currentPhase == null)
        {
            return;
        }

        phaseTimer += Time.deltaTime;

        //パターンの更新
        currentPhase.attackPattern?.UpdateAttack(this);
        currentPhase.movementPattern?.UpdateMovement(this);

        //フェーズ終了条件チェック
        CheckPhaseEndCondition();
    }

    /// <summary>
    /// 指定フェーズの開始処理
    /// </summary>
    private void StartPhase(int index)
    {
        if (index >= bossData.phases.Count)
        {
            Debug.Log("全フェーズ終了。ボスを撃破します");
            bossBase.ForceDie();
            return;
        }

        currentPhase = bossData.phases[index];
        bossBase.SetPhaseHP(currentPhase.phaseMaxHP);
        phaseTimer = 0f;

        OnPhaseStart();
    }

    /// <summary>
    /// 現在のフェーズの初期化処理
    /// </summary>
    private void OnPhaseStart()
    {
        currentPhase.attackPattern?.Initialize(this);
        currentPhase.movementPattern?.Initialize(this);

        Debug.Log($"フェーズ開始: {currentPhase.PhaseDisplayName}");
    }

    /// <summary>
    /// HP0のときBossBaseから呼ばれる
    /// </summary>
    public void OnBossHPDepleted()
    {
        if ((currentPhase.endCondition & BossPhaseEndCondition.HP) != 0)
        {
            if (bossBase.CurrentHP <= currentPhase.hpThreshold)
            {
                EndCurrentPhase();
            }
        }
        else
        {
            bossBase.ForceDie();
        }
    }

    /// <summary>
    /// 現在のフェーズの終了条件をすべてチェック
    /// </summary>
    private void CheckPhaseEndCondition()
    {
        bool end = false;
        var cond = currentPhase.endCondition;

        if ((cond & BossPhaseEndCondition.HP) != 0)
        {
            if (bossBase.CurrentHP <= currentPhase.hpThreshold)
            {
                end = true;
            }
        }

        if ((cond & BossPhaseEndCondition.TimeElapsed) != 0)
        {
            if (phaseTimer >= currentPhase.timeLimit)
            {
                end = true;
            }
        }

        if ((cond & BossPhaseEndCondition.KeyInput) != 0)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                end = true;
            }
        }

        if (end)
        {
            EndCurrentPhase();
        }
    }

    /// <summary>
    /// 現在のフェーズを終了し次へ進む
    /// </summary>
    private void EndCurrentPhase()
    {
        OnPhaseEnd();

        currentPhase = null;
        currentPhaseIndex++;
        StartPhase(currentPhaseIndex);
    }

    /// <summary>
    /// 現在のフェーズ終了時のクリーンアップ処理
    /// </summary>
    private void OnPhaseEnd()
    {
        currentPhase.attackPattern?.OnExit(this);
        currentPhase.movementPattern?.OnExit(this);

        //弾の一斉削除
        ClearAllEnemyBullets();

        Debug.Log($"フェーズ終了: {currentPhase.PhaseDisplayName}");
    }

    /// <summary>
    /// 画面上のすべての敵・ボス弾を削除
    /// </summary>
    private void ClearAllEnemyBullets()
    {
        var bullets = BulletPoolManager.Instance.GetAllActiveBullets();
        foreach (var bullet in bullets)
        {
            if (bullet.bulletBase.type == BulletType.Enemy || bullet.bulletBase.type == BulletType.Boss)
            {
                bullet.ReturnToPool();
            }
        }
    }

    /// <summary>
    /// 今のフェーズ名を取得
    /// </summary>
    public string GetCurrentPhaseName()
    {
        return currentPhase != null ? currentPhase.PhaseDisplayName : "未設定";
    }
}