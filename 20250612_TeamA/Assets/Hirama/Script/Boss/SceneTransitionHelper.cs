using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class SceneTransitionHelper : MonoBehaviour
{
    public static void DelaySceneLoad(string sceneName, float delay)
    {
        GameObject go = new GameObject("SceneTransitionHelper");
        var helper = go.AddComponent<SceneTransitionHelper>();
        helper.StartCoroutine(helper.LoadSceneCoroutine(sceneName, delay));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
        Destroy(gameObject);
    }
}