using StringToHell.InGame;
using UnityEngine;

public class WindScroller : MonoBehaviour
{
    [System.Serializable]
    public class WindLayer
    {
        public SpriteRenderer renderer;
        public float moveRate = 1f;
        [HideInInspector] public Material mat;
    }

    public WindLayer[] winds;
    public Color gizmosColor = Color.green;

    // 風エフェクター（Area Effector 2D）
    private IWind windEffector;

    [Header("エフェクターのForceMagnitudeに対するスクロール速度比率")]
    public float ratioSpeedToMagnitude = 0.02f;

    void Start()
    {
        windEffector = GetComponentInChildren<IWind>();

        foreach (var w in winds)
        {
            if (w.renderer != null)
            {
                w.mat = w.renderer.material;
            }
        }
    }

    void Update()
    {
        if (windEffector == null) return;

        // Effector の角度（degree）をラジアンに変換
        //float rad = windEffector.forceAngle * Mathf.Deg2Rad;

        // Effector の方向ベクトルを作成
        Vector2 forceDir = windEffector.WindDirection;//new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;

        // スクロール方向を決定
        Vector2 scrollDir = -forceDir * ratioSpeedToMagnitude * windEffector.WindForce * Time.deltaTime;

        foreach (var w in winds)
        {
            if (w.mat != null)
            {
                w.mat.mainTextureOffset += scrollDir * w.moveRate;
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = gizmosColor;
    //    var sr = GetComponentInChildren<SpriteRenderer>();
    //    var size = sr == null? Vector3.zero : sr.sprite.bounds.size;
    //    size.x *= transform.localScale.x;
    //    size.y *= transform.localScale.y;
    //    size.z *= transform.localScale.z;
    //    Gizmos.DrawWireCube(transform.position, size);
    //}
}
