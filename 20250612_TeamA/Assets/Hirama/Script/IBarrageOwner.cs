using UnityEngine;

public class IBarrageOwner : MonoBehaviour
{
    public interface BarrageOwner
    {
        void AddBullet(Barrage bullet);
        void RemoveBullet(Barrage bullet);
    }
}