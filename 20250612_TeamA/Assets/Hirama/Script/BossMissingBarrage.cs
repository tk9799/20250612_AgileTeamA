using System.Collections;
using UnityEngine;

public class BossMissingBarrage : MonoBehaviour
{
    [Header("’e‚Ìƒv[ƒ‹(BarragePool)")]
    [SerializeField] private BarragePool bulletPool;
    [Header("")]
    [SerializeField] private Transform[] firePoints;
    [Header("’e‚Ì‘¬“x")]
    [SerializeField] float bulletSpeed = 3f;
    [Header("’e‚Ì¶‘¶ŠÔ(•b)")]
    [SerializeField] float bulletLife = 6f;
    [Header("")]
    [SerializeField] private float mixInterval = 2f;
    [Header("")]
    [SerializeField] private float minInterval = 0.5f;

    void Start()
    {
        //StartCoroutine(FireBarrageSequence());
    }

    //private IEnumerator FireBarrageSequence()
    //{
    //    for(int i = 0; i < firePoints.Length; i++)
    //    {

    //    }
    //}
}