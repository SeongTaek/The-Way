using Creataek.TheWay.Common;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

namespace Creataek.TheWay.Curve
{
    public class SplineCurve : MonoBehaviour
    {
        [SerializeField]
        private Knot point0, point1, point2, point3, point4;

        [SerializeField]
        private float _tensionOfCardinalSplines = 0.0f;

        private void Start()
        {
        }

        private void Update()
        {
        }

        private static void DrawCubicHermitSplines(List<Knot> knotList)
        {
            if(knotList.Count < 2)
            {
                return;
            }

            int numPoints = 50;

            for (int i = 0; i < knotList.Count; i += 2)
            {
                var knot0 = knotList[i];
                var knot1 = knotList[i + 1];
                var positions = new Vector3[numPoints];
            }
            /*
            for (int i = 0; i < _numPoints; ++i)
            {
                float t = i / (float)_numPoints;

                _positions[i] = CalculateCubicHermitSplinesPoint(t, point0, point1);
            }

            _lineRenderer[0].SetPositions(_positions);

            _positions = new Vector3[50];

            for (int i = 0; i < _numPoints; ++i)
            {
                float t = i / (float)_numPoints;

                _positions[i] = CalculateCubicHermitSplinesPoint(t, point1, point2);
            }

            _lineRenderer[1].SetPositions(_positions);

            _positions = new Vector3[50];

            for (int i = 0; i < _numPoints; ++i)
            {
                float t = i / (float)_numPoints;

                _positions[i] = CalculateCubicHermitSplinesPoint(t, point2, point3);
            }

            _lineRenderer[2].SetPositions(_positions);

            _positions = new Vector3[50];

            for (int i = 0; i < _numPoints; ++i)
            {
                float t = i / (float)_numPoints;

                _positions[i] = CalculateCubicHermitSplinesPoint(t, point3, point4);
            }

            _lineRenderer[3].SetPositions(_positions);*/
        }

        private static void DrawCatmullRomSplines(List<Knot> knotList)
        {
            UpdateCatmullRomSplinesPoint(knotList);
            DrawCubicHermitSplines(knotList);
        }

        public static void DrawCardinalSplines(List<Knot> knotList, float tension)
        {
            UpdateCardinalSplinesPoint(tension, knotList);
            DrawCubicHermitSplines(knotList);
        }

        public static Vector3 CalculateCubicHermitSplinesPoint(float t, Knot pv0, Knot pv1)
        {
            // s = (1-t)
            // P(t) = s2(1+2t) P0 + t2(1+2s) P1 + s2tU–st2V
            float s = 1 - t;
            float ss = s * s;
            float tt = t * t;

            return (ss * (1 + 2 * t) * pv0.Point) + (tt * (1 + 2 * s) * pv1.Point) + (ss * t * pv0.Vector - s * tt * pv1.Vector);
        }

        public static void UpdateCatmullRomSplinesPoint(List<Knot> knotList)
        {
            for (int i = 1; i < knotList.Count - 1; ++i)
            {
                knotList[i].dir = (knotList[i + 1].Point - knotList[i - 1].Point).normalized;
                knotList[i].magnitude = (knotList[i + 1].Point - knotList[i - 1].Point).magnitude * 0.5f;
            }
        }

        public static void UpdateCardinalSplinesPoint(float tension, List<Knot> knotList)
        {
            if (knotList.Count == 0)
            {
                return;
            }

            knotList[0].dir = knotList[knotList.Count - 1].dir = Vector3.zero;
            knotList[0].magnitude = knotList[knotList.Count - 1].magnitude = 0.0f;

            for (int i = 1; i < knotList.Count - 1; ++i)
            {
                knotList[i].dir = (knotList[i + 1].Point - knotList[i - 1].Point).normalized;
                knotList[i].magnitude = (1.0f - tension) * (knotList[i + 1].Point - knotList[i - 1].Point).magnitude * 0.5f;
            }
        }
    }
}