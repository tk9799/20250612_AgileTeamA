using UnityEngine;

/// <summary>
/// ’e‚ÌŠî–{İ’è‚ğ ScriptableObject ‚ÅŠÇ—
/// </summary>
[CreateAssetMenu(fileName = "BulletBase", menuName = "Danmaku/BulletBase")]

public class BulletBase : ScriptableObject
{
    [Header("’e‚ÌŒ©‚½–Ú‚ğ•\‚·‰æ‘œ(ƒXƒvƒ‰ƒCƒg‰æ‘œ)")]
    //[SerializeField] public Sprite sprite;

    [Header("’e‘¬")]
    [SerializeField] public float speed = 5f;

    [Header("¶‘¶ŠÔ")]
    [SerializeField] public float lifetime = 5f;

    [Header("“–‚½‚è”»’è‚Ì”¼Œa")]
    [SerializeField] public float hitboxRadius = 0.1f;
}