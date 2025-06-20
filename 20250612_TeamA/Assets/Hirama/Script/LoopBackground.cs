using System.Collections.Generic;
using UnityEngine;

public class LoopBackground : MonoBehaviour
{
    [Header("ƒXƒNƒ[ƒ‹‚·‚é”wŒii•¡”j")]
    [SerializeField] public Transform[] backgrounds;

    [Header("ƒXƒNƒ[ƒ‹‘¬“x")]
    [SerializeField] public float scrollSpeed = 2f;

    [Header("”wŒi1–‡‚Ì‚‚³")]
    [SerializeField] public float bgHeight = 10f;

    public static readonly List<LoopBackground> AllBackgrounds = new();

    //–ˆƒtƒŒ[ƒ€”wŒi‚ğ‰º‚ÉˆÚ“®‚µA‰º‚Éo‚½‚çã‚ÉÄ”z’u
    void Update()
    {
        //Še”wŒi‚ğ‡‚Éˆ—
        foreach (Transform bg in backgrounds)
        {
            //”wŒi‚ğ‰º•ûŒü‚ÉˆÚ“®
            bg.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

            //‰æ–ÊŠO‚Éo‚½‚çÄ”z’u
            if (bg.position.y < -bgHeight)
            {
                //ˆê”Ôã‚É‚ ‚é”wŒi‚ÌYÀ•W‚ğæ“¾
                float highestY = FindHighestBackgroundY();

                //Œ»İ‚Ì”wŒi‚ğ‚»‚Ìã‚ÉÄ”z’u
                bg.position = new Vector3(bg.position.x, highestY + bgHeight, bg.position.z);
            }
        }
    }

    public void StopScroll()
    {
        enabled = false;
    }

    void OnEnable()
    {
        AllBackgrounds.Add(this);
    }

    void OnDisable()
    {
        AllBackgrounds.Remove(this);
    }

    //Œ»İ‚Ì”wŒi‚Ì’†‚Åˆê”Ôã‚É‚ ‚éYÀ•W‚ğ•Ô‚·
    float FindHighestBackgroundY()
    {
        //Å‰‚Ì”wŒi‚ÌYÀ•W‚ğ‰Šú’l‚Æ‚µ‚Äİ’è
        float maxY = backgrounds[0].position.y;

        //‘¼‚Ì”wŒi‚Æ”äŠr‚µ‚Äˆê”Ô‚‚¢YÀ•W‚ğ’T‚·
        for (int i = 1; i < backgrounds.Length; i++)
        {
            if (backgrounds[i].position.y > maxY)
                maxY = backgrounds[i].position.y;
        }

        //ˆê”Ô‚‚¢YÀ•W‚ğ•Ô‚·
        return maxY;
    }
}