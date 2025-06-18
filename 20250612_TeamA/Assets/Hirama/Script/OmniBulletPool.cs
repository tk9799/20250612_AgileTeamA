using UnityEngine;

public class OmniBulletPool : MonoBehaviour
{
    public static OmniBulletPool Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}