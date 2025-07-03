using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
[RequireComponent(typeof(LineRenderer))]

public class JustGuard : MonoBehaviour
{
    [SerializeField] private EdgeCollider2D edgeCollider2D;
    [SerializeField] private LineRenderer lineRenderer;
    public int segments = 32;//線の分割数大きいほど滑らかな円になる
    public float radius = 2.0f;//円の半径の長さ
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //頂点の配列を作る。配列の中の＋１は最後の点で線を閉じるため
        Vector3[] points3D = new Vector3[segments + 1];
        Vector2[] points2D = new Vector2[segments + 1];

        for(int i = 0; i <= segments; i++)//円周上の点を計算
        {
            //円周上の各店の角度を計算
            //2*PIは360度、i/segmrntsで分割して角度を求める
            float angle = 2 * Mathf.PI * i / segments;
            //cosとsinを使ってX座標とY座標を求める
            //*radiusで半径にスケール
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            //points[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            points3D[i] = new Vector3(x, y, 0);
            points2D[i] = new Vector2(x, y);
            //edgeCollider2D.points= points;
        }
        edgeCollider2D.points = points2D;

        lineRenderer.positionCount = points3D.Length;
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = false; // ローカル座標で描画する
        lineRenderer.SetPositions(points3D);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
