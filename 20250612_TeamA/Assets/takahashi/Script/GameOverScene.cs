using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOverScene : MonoBehaviour
{
    private Gamepad gamepad;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.JoystickButton0))
        {
            SceneManager.LoadScene("GameTitleScene");
        }
    }
}
