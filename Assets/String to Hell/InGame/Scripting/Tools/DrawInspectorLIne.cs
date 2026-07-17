using UnityEngine;
using UnityEngine.U2D;

namespace StringToHell.InGame.Tools
{
    public class DrawInspectorLIne : MonoBehaviour
    {
        Collider2D box;
        SpriteShapeRenderer shape;
        void Start()
        {
            shape = GetComponent<SpriteShapeRenderer>();
            shape.color = new Color(1, 1, 1, 0f);
        }
        //private void OnDrawGizmos()
        //{
        //    box = GetComponent<Collider2D>();
        //    Gizmos.color = Color.red;
        //    //Gizmos.matrix = transform.localToWorldMatrix;
        //    Gizmos.DrawWireCube(box.bounds.center, box.bounds.size);
           
        //}
    }
}