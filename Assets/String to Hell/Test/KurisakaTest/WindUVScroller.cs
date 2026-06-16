using UnityEngine;
using static UnityEngine.InputManagerEntry;

public class WindUVScroller : MonoBehaviour
{
    [SerializeField] private Color windColor = Color.white;
    [SerializeField, Range(0.001f, 0.04f)] private float windPowerRate = 0.01f;
    [SerializeField] private QuadWind[] winds;
    //private Material[] mats;

    private float windPower;
    private Vector2 windDir;
    //private Material mat;
    private AreaEffector2D wind;

    void Start()
    {
        //mat = GetComponent<Renderer>().material;   
        wind = GetComponent<AreaEffector2D>();
        windDir = AngleToDir(wind);
        windPower = wind.forceMagnitude * windPowerRate;

        //mats = new Material[winds.Length];
        for (int i = 0; i < winds.Length; i++)
        {
            winds[i].Mat = winds[i].quad.material;
            winds[i].color = winds[i].color;
        }
    }

    void Update()
    {
        var move = windDir * windPower * Time.deltaTime;

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
    public string elementName;
    public MeshRenderer quad;
    public Material Mat{ get; set; }
    public float moveRate = 0.5f;
    [Header("α値をゼロにしないよう注意")]
    public Color color = Color.gray3;
}
