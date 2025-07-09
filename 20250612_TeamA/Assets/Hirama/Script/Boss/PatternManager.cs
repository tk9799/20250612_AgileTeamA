using UnityEngine;

public class PatternManager : MonoBehaviour
{
    [Header("切り替え条件：全方位弾の発射回数")]
    public int maxOmniShots = 5;

    [Header("全方位弾幕")]
    public BossOmnidirectionalBarrage omniBarrage;

    [Header("プレイヤーに向けて撃つ扇形弾幕")]
    public BossFanBarrage fanBarrage;

    private int currentOmniShots = 0;
    private enum PatternState { Omni, Fan }

    private PatternState currentState = PatternState.Omni;

    void Awake()
    {
        enabled = false;
    }

    public void ReportOmniBarrageFired()
    {
        if (currentState != PatternState.Omni) return;

        currentOmniShots++;
        if (currentOmniShots >= maxOmniShots)
        {
            SwitchToFanBarrage();
        }
    }

    public void ReportFanBarrageFired()
    {
        if (currentState != PatternState.Fan) return;

        SwitchToOmniBarrage();
    }

    public void SwitchToOmniBarrage()
    {
        currentState = PatternState.Omni;
        currentOmniShots = 0;

        omniBarrage.enabled = true;
        fanBarrage.enabled = false;
    }

    public void SwitchToFanBarrage()
    {
        currentState = PatternState.Fan;
        currentOmniShots = 0;

        omniBarrage.enabled = false;
        fanBarrage.enabled = true;
    }
}