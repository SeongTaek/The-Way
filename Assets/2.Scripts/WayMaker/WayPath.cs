using Creataek.TheWay.Common;
using Creataek.TheWay.Curve;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Creataek.TheWay
{
    public class WayPath
    {
        public int NumPoints { get { return localPoints.Length; } }

        protected Vector3[] localPoints;
        protected Vector3[] localTangents;
        protected Vector3[] localNormals;
        protected float[] times;

        public void Init()
        {
        }

        public void SetKnots(Knot start, Knot end)
        {
            int distance = Math.Max(1, (int)(end.Point - start.Point).magnitude);
            int numVerts = distance * 10;

            localPoints = new Vector3[numVerts];
            localTangents = new Vector3[numVerts];
            localNormals = new Vector3[numVerts];
            times = new float[numVerts];

            float timeScale = 1.0f / (numVerts - 1);

            for (int i = 0; i < numVerts; ++i)
            {
                times[i] = (i != numVerts - 1) ? timeScale * i : 1.0f;
                localPoints[i] = SplineCurve.CalculateCubicHermitSplinesPoint(times[i], start, end);
            }

            for (int i = 0; i < numVerts - 1; ++i)
            {
                Vector3 forward = (localPoints[i + 1] - localPoints[i]).normalized;

                localTangents[i] = Vector3.Cross(Vector3.up, forward).normalized;
                localNormals[i] = Vector3.Cross(forward, localTangents[i]).normalized;
            }

            localTangents[numVerts - 1] = localTangents[numVerts - 2];
            localNormals[numVerts - 1] = localNormals[numVerts - 2];
        }

        public Vector3 GetTangent(int idx)
        {
            if (localTangents == null || idx < 0 || idx >= localTangents.Length)
            {
                return Vector3.zero;
            }

            return localTangents[idx];
        }

        public Vector3 GetNormal(int idx)
        {
            if (localNormals == null || idx < 0 || idx >= localNormals.Length)
            {
                return Vector3.zero;
            }

            return localNormals[idx];
        }

        public Vector3 GetPoint(int idx)
        {
            if (localPoints == null || idx < 0 || idx >= localPoints.Length)
            {
                return Vector3.zero;
            }

            return localPoints[idx];
        }

        public float GetTime(int idx)
        {
            if (times == null || idx < 0 || idx >= times.Length)
            {
                return 0.0f;
            }

            return times[idx];
        }
    }
}