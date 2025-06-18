using UnityEngine;

public class LoopBackground : MonoBehaviour
{
    [Header("スクロールする背景（複数）")]
    [SerializeField] public Transform[] backgrounds;

    [Header("スクロール速度")]
    [SerializeField] public float scrollSpeed = 2f;

    [Header("背景1枚の高さ")]
    [SerializeField] public float bgHeight = 10f;

    //毎フレーム背景を下に移動し、下に出たら上に再配置
    void Update()
    {
        //各背景を順に処理
        foreach (Transform bg in backgrounds)
        {
            //背景を下方向に移動
            bg.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

            //画面外に出たら再配置
            if (bg.position.y < -bgHeight)
            {
                //一番上にある背景のY座標を取得
                float highestY = FindHighestBackgroundY();

                //現在の背景をその上に再配置
                bg.position = new Vector3(bg.position.x, highestY + bgHeight, bg.position.z);
            }
        }
    }

    //現在の背景の中で一番上にあるY座標を返す
    float FindHighestBackgroundY()
    {
        //最初の背景のY座標を初期値として設定
        float maxY = backgrounds[0].position.y;

        //他の背景と比較して一番高いY座標を探す
        for (int i = 1; i < backgrounds.Length; i++)
        {
            if (backgrounds[i].position.y > maxY)
                maxY = backgrounds[i].position.y;
        }

        //一番高いY座標を返す
        return maxY;
    }
}