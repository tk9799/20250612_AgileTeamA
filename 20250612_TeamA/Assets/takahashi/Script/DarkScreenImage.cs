using UnityEngine;
using UnityEngine.UI;

public class DarkScreenImage : MonoBehaviour
{
    [SerializeField] private Image darkOverlay;
    [Range(0, 1)] public float darkness = 0.5f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetDarkness(darkness);
    }

    public void SetDarkness(float alpha)
    {
        if (darkOverlay != null)
        {
            Color c = darkOverlay.color;
            c.a = Mathf.Clamp01(alpha); // 0〜1で制限
            darkOverlay.color = c;
        }
    }
}
