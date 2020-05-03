using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PointWithVector : MonoBehaviour
{
    public Vector3 dir;
    public float magnitude;

    public Vector3 Point
    {
        get { return transform.position; }
        set { transform.position = value; }
    }
    public Vector3 Vector { get { return dir * magnitude; } }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Point, Point + dir * magnitude);
    }
}
