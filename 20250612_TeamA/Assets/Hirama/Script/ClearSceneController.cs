using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearSceneController : MonoBehaviour
{
    [Header("タイトルシーン名")]
    [SerializeField] private string titleSceneName = "GameTitleScene";

    void Update()
    {
        //エンターキー（Returnキー）が押されたらタイトルシーンへ移動
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(titleSceneName);
        }
    }
}