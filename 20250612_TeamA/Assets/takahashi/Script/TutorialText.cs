using UnityEngine;
using TMPro;

public class TutorialText : MonoBehaviour
{
    [SerializeField] private TMPText tmpText;
    [SerializeField] public TextMeshProUGUI instructionsText;//指示するテキストを書く
    [SerializeField] GuardController2 guardController2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (tmpText != null && tmpText.tutorial == 0)
        {
            instructionsText.text = "右上にある操作方法を使って緑色のオブジェクトまで移動する";
        }
        else if (tmpText != null && tmpText.tutorial == 1)
        {
            instructionsText.text = "プレイヤーの弾を発射して敵を倒す";
        }
        else if (tmpText != null && tmpText.tutorial == 2)
        {
            instructionsText.text = "ジャストガードを5回成功させる/"+ guardController2.justGuardCount+"回目" +
                "\n５回目以降プレイヤーが攻撃できるようになり、敵を攻撃して次のチュートリアルに進む";
        }
        else if(tmpText != null && tmpText.tutorial == 3)
        {
            instructionsText.text = "";
        }
    }
}