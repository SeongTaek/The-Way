using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creataek.TheWay.Common
{
    public class Knot : MonoBehaviour
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
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(Point, 1.0f);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(Point, Point + dir * magnitude);
        }
    }
}