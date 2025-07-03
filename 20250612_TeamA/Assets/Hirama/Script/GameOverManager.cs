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
        //ƒ{ƒX‚ÌˆÚ“®’â~
        foreach (var mover in FindObjectsByType<BossMovement>(FindObjectsSortMode.None))
        {
            mover.enabled = false;
        }

        //EnemyMovement‚Ì’â~
        foreach (var enemyMove in FindObjectsByType<EnemyMovement>(FindObjectsSortMode.None))
        {
            enemyMove.enabled = false;
        }

        //G‹›“G‚Ì“®‚«’â~
        foreach (var c in FindObjectsByType<EnemyComplexMovement>(FindObjectsSortMode.None))
        {
            c.enabled = false;
        }

        foreach (var c in FindObjectsByType<EnemyCurveMovement>(FindObjectsSortMode.None))
        {
            c.enabled = false;
        }

        foreach (var d in FindObjectsByType<EnemyDiagonalMovement>(FindObjectsSortMode.None))
        {
            d.enabled = false;
        }

        foreach (var s in FindObjectsByType<EnemySideMovement>(FindObjectsSortMode.None))
        {
            s.enabled = false;
        }

        foreach (var sw in FindObjectsByType<EnemysSwayMovement>(FindObjectsSortMode.None))
        {
            sw.enabled = false;
        }

        //’e‚Ì–³Œø‰»
        foreach (var bulletObj in GameObject.FindGameObjectsWithTag("EnemyBullet"))
        {
            var bullet = bulletObj.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.isStopped = true;
            }
        }

        //ƒ{ƒX‚Ì’e–‹’â~
        foreach (var barrage in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
        {
            if (barrage is BossOmnidirectionalBarrage || barrage is BossFanBarrage || barrage is PatternManager)
            {
                ((MonoBehaviour)barrage).enabled = false;
            }
        }

        //G‹›“G‚Ì©‹@‘_‚¢‚Ì’â~
        foreach (var aimsBarrage in FindObjectsByType<EnemyAimsBarrage>(FindObjectsSortMode.None))
        {
            aimsBarrage.enabled = false;
            aimsBarrage.StopAllCoroutines();
        }

        //G‹›“G‚Ì‘S•ûˆÊ’e–‹‚Ì’â~
        foreach (var omniBarrage in FindObjectsByType<EnemyOmnidirectionalBarrage>(FindObjectsSortMode.None))
        {
            omniBarrage.enabled = false;
            omniBarrage.StopAllCoroutines();
        }

        //”wŒi‚ÌƒXƒNƒ[ƒ‹’â~
        foreach (var bg in new List<LoopBackground>(LoopBackground.AllBackgrounds))
        {
            bg.StopScroll();
        }

        Debug.Log("[GameOverManager] “GA’e–‹A”wŒi‚ğ‚·‚×‚Ä’â~‚µ‚Ü‚µ‚½B");
    }
}