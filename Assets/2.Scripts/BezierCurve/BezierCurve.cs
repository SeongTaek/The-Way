using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creataek.TheWay.Curve
{
    public class BezierCurve : MonoBehaviour
    {
        [SerializeField]
        private LineRenderer _lineRenderer;

        [SerializeField]
        private Transform point0, point1, point2, point3;

        private int _numPoints = 50;
        private Vector3[] _positions = new Vector3[50];

        private void Start()
        {
            _lineRenderer.positionCount = _numPoints;
        }

        private void Update()
        {
            DrawCubicCurve();
        }

        private void DrawLinearCurve()
        {
            for (int i = 1; i < _numPoints + 1; ++i)
            {
                float t = i / (float)_numPoints;

                _positions[i - 1] = CalculateLinearBezierPoint(t, point0.position, point1.position);
            }

            _lineRenderer.SetPositions(_positions);
        }

        private void DrawQuadraticCurve()
        {
            for (int i = 1; i < _numPoints + 1; ++i)
            {
                float t = i / (float)_numPoints;

                _positions[i - 1] = CalcuateQuadraticBezierPoint(t, point0.position, point1.position, point2.position);
            }

            _lineRenderer.SetPositions(_positions);
        }

        private void DrawCubicCurve()
        {
            for (int i = 1; i < _numPoints + 1; ++i)
            {
                float t = i / (float)_numPoints;

                _positions[i - 1] = CalculateCubicBezierPoint(t, point0.position, point1.position, point2.position, point3.position);
            }

            _lineRenderer.SetPositions(_positions);
        }

        private Vector3 CalculateLinearBezierPoint(float t, Vector3 p0, Vector3 p1)
        {
            // P = P0 + t(P1 – P0)
            return p0 + t * (p1 - p0);
        }

        private Vector3 CalcuateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            // B(t) = (1-t)2P0 + 2(1-t)tP1 + t2P2
            //         uu           u        tt
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            return (uu * p0) + (2 * u * t * p1) + (tt * p2);
        }

        private Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            // B(t) = (1-t)3 P0 + 3(1-t)2 tP1 + 3(1-t) t2 P2 + t3 P3 
            //         uuu          uu             u   tt      ttt
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            return (uuu * p0) + (3 * uu * t * p1) + (3 * u * tt * p2) + (ttt * p3);
        }
    }
}