using UnityEngine;

/// <summary>
/// G‹›“G‚ÌŠî–{İ’è
/// </summary>
[CreateAssetMenu(fileName = "EnemyData", menuName = "Barrage/Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("Œ©‚½–Ú‚Ì‰æ‘œ")]
    public Sprite enemySprite;

    [Header("G‹›“G•\¦–¼")]
    public string EnemydisplayName = "";

    [Header("G‹›“G‚ÌPrefab")]
    public GameObject EnemyPrefab;

    [Header("’e«”\ƒf[ƒ^")]
    public BulletBase bulletBase;

    [Header("Œ©‚½–Ú‚Ì‘å‚«‚³B(1, 1)‚ª“™”{")]
    public Vector2 scale = Vector2.one;

    [Header("Œ©‚½–Ú‚ÌF")]
    public Color enemyColor = Color.white;

    [Header("Å‘åHP")]
    public int maxHP = 10;

    [Header("ˆÚ“®‘¬“x")]
    public float moveSpeed = 2.0f;

    [Header("UŒ‚ƒpƒ^[ƒ“")]
    public AttackPattern attackPattern;

    [Header("ˆÚ“®ƒpƒ^[ƒ“")]
    public MovementPattern movementPattern;

    [Header("Œ‚”j‚É‰ÁZ‚·‚éƒXƒRƒA")]
    public int score = 0;
}