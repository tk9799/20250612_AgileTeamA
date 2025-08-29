using UnityEngine;

public class UIRotateLoop : MonoBehaviour
{
    [Header("‰ñ“]‘¬“x (ZŽ²‚ªUI—p)")]
    public float rotationSpeed = 20f;

    void Update()
    {
        //ZŽ²‚Å‰ñ“]
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}