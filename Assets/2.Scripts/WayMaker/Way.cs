using Creataek.TheWay.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creataek.TheWay
{
    public class Way : MonoBehaviour
    {
        protected WayPath wayPath;
        [SerializeField]
        protected WayMesh wayMesh;

        private Knot _startKnot;
        private Knot _endKnot;

        protected void Awake()
        {
            wayPath = new WayPath();
        }

        public void Init()
        {
        }

        public void SetKnots(Knot start, Knot end)
        {
            /*bool updateKnots = (_startKnot == null || _startKnot.Point != start.Point || _startKnot.dir != start.dir || _startKnot.magnitude != start.magnitude)
                || (_startKnot == null || _endKnot.Point != end.Point || _endKnot.dir != end.dir || _endKnot.magnitude != end.magnitude);

            if (!updateKnots)
            {
                return;
            }*/

            _startKnot = start;
            _endKnot = end;

            wayPath.SetKnots(start, end);
            wayMesh.CreateWayMesh(wayPath);
        }

        public void Draw()
        {
        }

        protected void OnDrawGizmos()
        {

            for(int i = 0; i < wayPath.NumPoints; ++i)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(wayPath.GetPoint(i), wayPath.GetPoint(i) + wayPath.GetNormal(i) * 10.0f);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(wayPath.GetPoint(i), wayPath.GetPoint(i) + wayPath.GetTangent(i) * 10.0f);
            }
        }
    }
}