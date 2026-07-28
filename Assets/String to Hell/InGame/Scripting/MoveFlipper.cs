using UnityEngine;

public class MoveFlipper : MonoBehaviour
{
    [Header("左向きのスプライトか否か")]
    [SerializeField] private bool facingLeftImage = true;
    private Transform tf;
    private float preX;
    private float startScaleX;
    private bool cooling = false;
    [SerializeField] private float coolTime = 0.2f;

    void Start()
    {
        tf = GetComponent<Transform>();
        preX = tf.position.x;
        startScaleX = tf.localScale.x;
    }

    void Update()
    {
        var movedX = tf.position.x - preX;
        var scale = tf.localScale;
        if(preX != tf.position.x && !cooling)
        {
            int sign = facingLeftImage ? -1 : 1;
            scale.x = movedX > 0 ? sign * startScaleX : -sign * startScaleX;
            cooling = true;
            Invoke(nameof(FinishCool), coolTime);
        }      
        tf.localScale = scale;
        preX = tf.position.x;
    }

    void FinishCool()
    {
        cooling = false;
    }
}
