using UnityEngine;
using TMPro;

public class TutorialText : MonoBehaviour
{
    [SerializeField] private TMPText tmpText;
    [SerializeField] private TextMeshProUGUI instructionsText;//指示するテキストを書く
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
        else if(tmpText!=null&&tmpText.tutorial == 1)
        {
            instructionsText.text = "プレイヤーの弾を発射して敵を倒す";
        }
        else if (tmpText != null && tmpText.tutorial == 2)
        {
            instructionsText.text = "ジャストガードを5回成功させる";
        }
    }
}
