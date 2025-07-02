using UnityEngine;

/// <summary>
/// 敵の移動を制御するクラス(各移動スクリプトを切り替えて制御)
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    //各移動スクリプト
    private EnemyComplexMovement complex;
    private EnemyCurveMovement curve;
    private EnemyDiagonalMovement diagonal;
    private EnemySideMovement side;
    private EnemysSwayMovement sway;

    private void Awake()
    {
        // GameObject生成時に1回だけ取得
        complex = GetComponent<EnemyComplexMovement>();
        curve = GetComponent<EnemyCurveMovement>();
        diagonal = GetComponent<EnemyDiagonalMovement>();
        side = GetComponent<EnemySideMovement>();
        sway = GetComponent<EnemysSwayMovement>();
    }

    /// <summary>
    /// 移動パターンの初期化（Waveごとに呼ばれる）
    /// </summary>
    public void Initialize(EnemyMovementSettings settings)
    {
        DisableAll(); // 前の設定を無効にする

        // 設定の型によって有効化＆初期化
        if (settings is ComplexMovementSettings c && complex != null)
        {
            complex.enabled = true;
            complex.Initialize(c);
        }
        else if (settings is CurveMovementSettings cu && curve != null)
        {
            curve.enabled = true;
            curve.Initialize(cu);
        }
        else if (settings is DiagonalMovementSettings d && diagonal != null)
        {
            diagonal.enabled = true;
            diagonal.Initialize(d);
        }
        else if (settings is SideMovementSettings s && side != null)
        {
            side.enabled = true;
            side.Initialize(s);
        }
        else if (settings is SwayMovementSettings sw && sway != null)
        {
            sway.enabled = true;
            sway.Initialize(sw);
        }
        else
        {
            Debug.LogWarning($"[EnemyMovement] 未対応または未設定: {settings}");
        }
    }


    private void DisableAll()
    {
        if (complex) complex.enabled = false;
        if (curve) curve.enabled = false;
        if (diagonal) diagonal.enabled = false;
        if (side) side.enabled = false;
        if (sway) sway.enabled = false;
    }
}