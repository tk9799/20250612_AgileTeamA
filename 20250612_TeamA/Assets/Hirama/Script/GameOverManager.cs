using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StopAllEnemies()
    {
        // ’e–‹’â~
        foreach (var barrage in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
        {
            if (barrage is BossOmnidirectionalBarrage || barrage is BossFanBarrage || barrage is PatternManager)
            {
                ((MonoBehaviour)barrage).enabled = false;
            }
        }

        // “G‚ÌˆÚ“®’â~
        foreach (var mover in FindObjectsByType<BossMovement>(FindObjectsSortMode.None))
        {
            mover.enabled = false;
        }

        // ’e‚Ì–³Œø‰»
        foreach (var bulletObj in GameObject.FindGameObjectsWithTag("EnemyBullet"))
        {
            var bullet = bulletObj.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.isStopped = true;
            }
        }

        //”wŒi‚ÌƒXƒNƒ[ƒ‹’â~
        foreach (var bg in new List<LoopBackground>(LoopBackground.AllBackgrounds))
        {
            bg.StopScroll();
        }

        Debug.Log("[GameOverManager] “G‚Æ’e–‹‚Æ”wŒi‚ğ‚·‚×‚Ä’â~‚µ‚Ü‚µ‚½B");
    }
}