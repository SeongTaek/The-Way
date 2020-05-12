using Creataek.TheWay.Common;
using Creataek.TheWay.Curve;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager.UI;
using UnityEngine;

namespace Creataek.TheWay
{
    public class WayMaker : MonoBehaviour
    {
        // TODO : 테스트를 위해 추가한거여서, 비지니스 코드 작성시 제거 필요.
        public List<Knot> sampleKnotList = new List<Knot>();

        [SerializeField]
        protected Way wayPrefab;
        [SerializeField]
        protected Transform wayParent;

        private List<Knot> _knotList = new List<Knot>();
        private List<Way> _wayList = new List<Way>();

        protected void Update()
        {
            UpdatePath(0, _knotList.Count);

            if (Input.GetKeyDown(KeyCode.A))
            {
                AddKnot(sampleKnotList[_knotList.Count]);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                RemoveKnote(sampleKnotList[_knotList.Count - 1]);
            }
        }

        public void AddKnot(Knot knot)
        {
            if (!_knotList.Contains(knot))
            {
                _knotList.Add(knot);
                UpdatePath(_knotList.Count - 2, _knotList.Count);
            }
            else
            {
                Debug.LogWarning("knot is already added.");
            }
        }

        public void RemoveKnote(Knot knot)
        {
            if (_knotList.Contains(knot))
            {
                var knotIdx = _knotList.IndexOf(knot);

                _knotList.RemoveAt(knotIdx);
                UpdatePath(knotIdx - 2, knotIdx);
                RemoveUnnecessaryWay(_knotList);
            }
            else
            {
                Debug.LogWarning("knot not found and cannot be removed.");
            }
        }

        protected void UpdatePath(int startIdx, int endIdx)
        {
            if (startIdx < 0 || startIdx > _knotList.Count || endIdx < 0 || endIdx > _knotList.Count)
            {
                Debug.LogWarning(string.Format("startIdx : {0}, endIdx {1} - invalid index", startIdx, endIdx));
                return;
            }

            var updateKnotList = _knotList.GetRange(startIdx, (endIdx - startIdx));

            SplineCurve.UpdateCardinalSplinesPoint(0.0f, updateKnotList);

            for (int i = startIdx; i < endIdx - 1; ++i)
            {
                var knot0 = _knotList[i];
                var knot1 = _knotList[i + 1];
                var way = GetWay(i);

                if (way == null)
                {
                    way = CreateWay();
                }

                way.SetKnots(knot0, knot1);
            }
        }

        protected Way GetWay(int idx)
        {
            if (idx < 0 || idx >= _wayList.Count)
            {
                Debug.LogWarning(string.Format("idx : {0} - invalid index", idx));
                return null;
            }

            return _wayList[idx];
        }

        protected Way CreateWay()
        {
            _wayList.Add(Instantiate<Way>(wayPrefab, wayParent));

            return _wayList[_wayList.Count - 1];
        }

        protected void RemoveUnnecessaryWay(List<Knot> knotList)
        {
            for (int i = _wayList.Count - 1; i >= 0; --i)
            {
                if (knotList.Count == 1 || i >= (knotList.Count + 1) / 2)
                {
                    Destroy(_wayList[i].gameObject);
                    _wayList.RemoveAt(i);
                }
            }
        }
    }
}