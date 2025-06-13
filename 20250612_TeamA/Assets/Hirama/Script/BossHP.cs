using UnityEngine;

public class BossHP : MonoBehaviour
{
    [Header("ボスの最大HP")]
    [SerializeField] public float MaxHP = 100f;

    //現在のHPを保持する変数
    private float currentHP;

    //ボスの全方向弾幕発射を制御するスクリプトの参照
    private IBarrageOwner barrageControl;

    void Start()
    {
        //初期HPを最大HPに設定
        currentHP = MaxHP;

        //同じゲームオブジェクトにある弾幕制御スクリプトを取得
        barrageControl = GetComponent<IBarrageOwner>();
    }

    //ボスがダメージを受けると呼ばれる処理
    public void TakeDamage(int damage)
    {
        //現在HPからダメージ値を引く
        currentHP -= damage;

        //HPが0以下ならボス死亡処理を実行
        if (currentHP <= 0)
        {
            BossDie();
        }
    }

    //ボスの死亡処理
    void BossDie()
    {
        //弾幕発射スクリプトが存在すれば、死亡時の処理を呼び出す
        if (barrageControl != null)
        {
            barrageControl.OnDeath();
        }

        //ボスのゲームオブジェクトを非アクティブに
        gameObject.SetActive(false);
    }
}