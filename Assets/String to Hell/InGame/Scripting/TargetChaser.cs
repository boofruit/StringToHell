using UnityEngine;

public class TargetChaser : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float speed = 1f;
    private Transform tf;

    void Start()
    {
        tf = GetComponent<Transform>();
    }

    void Update()
    {
        var move = target.position - tf.position;
        tf.Translate(move * speed * Time.deltaTime);
    }
}
