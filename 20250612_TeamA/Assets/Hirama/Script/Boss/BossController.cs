//using System.Collections;
//using UnityEngine;

///// <summary>
///// ボスの動作を制御するクラス
///// </summary>
//public class BossController : BossBase, IDanmakuEnemy
//{
//    private BossPhase currentPhase;
//    private int currentPhaseIndex = 0;
//    private float phaseTimer = 0f;

//    private AttackPattern currentAttackPattern;
//    private MovementPattern currentMovementPattern;

//    private bool isPhaseRunning = false;

//    Transform IDanmakuEnemy.transform => base.transform;
//    int IDanmakuEnemy.CurrentHP => this.CurrentHP;

//    void IDanmakuEnemy.TakeDamage(int damage)
//    {
//        base.TakeDamage(damage);
//    }

//    protected override void OnEnable()
//    {
//        base.OnEnable();
//        StartPhase(currentPhaseIndex);
//    }

//    private void Update()
//    {
//        if (isDead || bossData == null || !isPhaseRunning)
//            return;

//        //時間経過更新
//        phaseTimer += Time.deltaTime;

//        //移動・攻撃更新
//        currentAttackPattern?.UpdateAttack(this);
//        currentMovementPattern?.UpdateMovement(this);

//        //フェーズ終了条件チェック
//        CheckPhaseEndCondition();
//    }

//    /// <summary>
//    /// フェーズを開始する
//    /// </summary>
//    private void StartPhase(int index)
//    {
//        if (index >= bossData.attackPhases.Count)
//        {
//            Debug.Log("すべてのフェーズが終了しました。ボス撃破扱い。");
//            Die();
//            return;
//        }

//        currentPhase = bossData.attackPhases[index];
//        currentAttackPattern = currentPhase.attackPattern;
//        currentMovementPattern = currentPhase.movementPattern;

//        //タイマーリセット
//        phaseTimer = 0f;

//        //攻撃・移動パターン初期化
//        currentAttackPattern?.Initialize(this);
//        currentMovementPattern?.Initialize(this);

//        Debug.Log($"フェーズ {index + 1} 開始：終了条件 {currentPhase.endCondition}");

//        isPhaseRunning = true;
//    }

//    /// <summary>
//    /// フェーズ終了条件をチェック
//    /// </summary>
//    private void CheckPhaseEndCondition()
//    {
//        switch (currentPhase.endCondition)
//        {
//            case PhaseEndCondition.HP:
//                if (CurrentHP <= currentPhase.hpThreshold)
//                {
//                    NextPhase();
//                }
//                break;

//            case PhaseEndCondition.TimeElapsed:
//                if (phaseTimer >= currentPhase.timeLimit)
//                {
//                    NextPhase();
//                }
//                break;

//            case PhaseEndCondition.KeyInput:
//                if (Input.GetKeyDown(KeyCode.X))
//                {
//                    NextPhase();
//                }
//                break;

//            case PhaseEndCondition.None:
//                //常に継続。外部から強制で切り替える必要あり。
//                break;
//        }
//    }

//    /// <summary>
//    /// 次のフェーズに進む
//    /// </summary>
//    private void NextPhase()
//    {
//        isPhaseRunning = false;
//        currentPhaseIndex++;
//        StartPhase(currentPhaseIndex);
//    }

//    /// <summary>
//    /// 撃破処理（基底クラスのDieをオーバーライド可能）
//    /// </summary>
//    protected override void Die()
//    {
//        base.Die();

//        //撃破演出、スコア加算、BGM停止など
//        //ScoreManager.Instance.AddScore(bossData.score);
//        //AudioManager.Instance.StopBGM();
//    }
//}