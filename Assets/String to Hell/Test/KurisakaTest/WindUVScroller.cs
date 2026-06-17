using UnityEngine;

public class WindUVScroller : MonoBehaviour
{
    [Header("AreaEffector2DのforceMagnitudeに対する比率")]
    [SerializeField, Range(0.001f, 0.04f)] private float windPowerRate = 0.01f;
    [SerializeField] private QuadWind[] winds;
    [Header("風の縦振幅量")]
    [SerializeField, Range(0, 0.01f)] private float amplitude = 0.001f;
    [Header("風の縦振幅速度")]
    [SerializeField, Range(0, 10f)] private float amplitudeSpeed = 1f;

    private float windPower;
    private Vector2 windDir;
    private AreaEffector2D wind;
    private float passingTime;
    private float difference;

    void Start()
    { 
        wind = GetComponent<AreaEffector2D>();
        windDir = AngleToDir(wind);
        windPower = wind.forceMagnitude * windPowerRate;

        for (int i = 0; i < winds.Length; i++)
        {
            winds[i].Mat = winds[i].quad.material;
            winds[i].Mat.color = winds[i].color;
        }

        difference = Random.value;
    }

    void Update()
    {
        passingTime += Time.deltaTime;
        var move = windDir * windPower * Time.deltaTime;
        move.y = Mathf.Sin(passingTime * amplitudeSpeed + difference) * 0.001f;
        //Debug.Log("windDir" + windDir);
        for (int i = 0; i < winds.Length; i++)
        {
            winds[i].Mat.mainTextureOffset += move * winds[i].moveRate;
        }
    }

    Vector2 AngleToDir(AreaEffector2D ae2d)
    {
        float angle = ae2d.forceAngle;
        float rad = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * -1;
    }
}

[System.Serializable]
public class QuadWind
{
    [Header("項目名(動作には影響なし)")]
    public string elementName;
    [Header("Quadオブジェクトをセット")]
    public MeshRenderer quad;
    public Material Mat{ get; set; }
    [Header("Wind Powerに対する比率")]
    public float moveRate = 0.5f;
    [Header("α値をゼロにしないよう注意")]
    public Color color = Color.gray3;
}
