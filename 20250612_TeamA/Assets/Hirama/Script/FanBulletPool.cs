using UnityEngine;

public class FanBulletPool : BulletPool
{
    public static FanBulletPool Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}