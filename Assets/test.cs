using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    Material mat;
    private void Start()
    {
        mat = GetComponent<MeshRenderer>().sharedMaterial;
        Debug.Log(mat.GetInstanceID());
    }
    //public Transform p0;
    //public Transform p1;
    //public Transform p2;
    //public float resolution;

    //private Vector3 BezierCurvePointQuad(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    //{
    //    float x = QuadFormula(p0.x, p1.x, p2.x, t);
    //    float y = QuadFormula(p0.y, p1.y, p2.y, t);
    //    return new Vector3(x, y, p0.z);
    //}
    //private float QuadFormula(float p0, float p1, float p2, float t)
    //{
    //    return (1-t) * (1-t) * p0 + 2 * t * (1 - t) * p1 + t * t * p2;
    //}

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    for (int i = 0; i < 10; i++)
    //    {
    //        float t = i * (1 / resolution);
    //        Debug.Log(t);
    //        Vector3 p = BezierCurvePointQuad(p0.position, p1.position, p2.position, t);
    //        Gizmos.DrawWireSphere(p, 0.1f);
    //    }
    //}
}
