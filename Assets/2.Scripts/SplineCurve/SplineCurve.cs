using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class SplineCurve : MonoBehaviour
{
    [SerializeField]
    private LineRenderer[] _lineRenderer;

    [SerializeField]
    private PointWithVector point0, point1, point2, point3, point4;

    [SerializeField]
    private float _tensionOfCardinalSplines = 0.0f;

    private int _numPoints = 50;
    private Vector3[] _positions = new Vector3[50];

    private void Start()
    {
        for (int i = 0; i < _lineRenderer.Length; ++i)
        {
            _lineRenderer[i].positionCount = _numPoints;
        }
    }

    private void Update()
    {
        DrawCardinalSplines();
    }

    private void DrawCubicHermitSplines()
    {
        _positions = new Vector3[50];

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

        _lineRenderer[3].SetPositions(_positions);
    }

    private void DrawCatmullRomSplines()
    {
        CalculateCatmullRomSplinesPoint(new PointWithVector[] { point0, point1, point2, point3, point4 });
        DrawCubicHermitSplines();
    }

    private void DrawCardinalSplines()
    {
        CalculateCardinalSplinesPoint(_tensionOfCardinalSplines, new PointWithVector[] { point0, point1, point2, point3, point4 });
        DrawCubicHermitSplines();
    }

    private Vector3 CalculateCubicHermitSplinesPoint(float t, PointWithVector pv0, PointWithVector pv1)
    {
        // s = (1-t)
        // P(t) = s2(1+2t) P0 + t2(1+2s) P1 + s2tU–st2V
        float s = 1 - t;
        float ss = s * s;
        float tt = t * t;

        return (ss * (1 + 2 * t) * pv0.Point) + (tt * (1 + 2 * s) * pv1.Point) + (ss * t * pv0.Vector - s * tt * pv1.Vector);
    }

    private void CalculateCatmullRomSplinesPoint(PointWithVector[] pvArray)
    {
        for (int i = 1; i < pvArray.Length - 1; ++i)
        {
            pvArray[i].dir = (pvArray[i + 1].Point - pvArray[i - 1].Point).normalized;
            pvArray[i].magnitude = (pvArray[i + 1].Point - pvArray[i - 1].Point).magnitude * 0.5f;
        }
    }

    private void CalculateCardinalSplinesPoint(float tension, PointWithVector[] pvArray)
    {
        for (int i = 1; i < pvArray.Length - 1; ++i)
        {
            pvArray[i].dir = (pvArray[i + 1].Point - pvArray[i - 1].Point).normalized;
            pvArray[i].magnitude = (1.0f - tension) * (pvArray[i + 1].Point - pvArray[i - 1].Point).magnitude * 0.5f;
        }
    }
}
