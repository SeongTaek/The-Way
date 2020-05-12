﻿using Creataek.TheWay.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creataek.TheWay
{
    public class WayMesh : MonoBehaviour
    {
        [Header("Way settings")]
        [SerializeField]
        protected float wayWidth = 1.0f;
        [SerializeField]
        [Range(0, .5f)]
        protected float thickness = .15f;
        [SerializeField]
        protected bool flattenSurface;

        [Header("Material settings")]
        [SerializeField]
        protected Material roadMaterial;
        [SerializeField]
        protected Material undersideMaterial;
        [SerializeField]
        protected float textureTiling = 1;

        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private Mesh _mesh;

        protected void Awake()
        {
            AssignMeshComponents();
            AssignMaterials();
        }

        public void CreateWayMesh(WayPath wayPath)
        {
            Vector3[] verts = new Vector3[wayPath.NumPoints * 8];
            Vector2[] uvs = new Vector2[verts.Length];
            Vector3[] normals = new Vector3[verts.Length];

            int numTris = 2 * (wayPath.NumPoints - 1) + (false/*(path.isClosedLoop)*/ ? 2 : 0);
            int[] roadTriangles = new int[numTris * 3];
            int[] underRoadTriangles = new int[numTris * 3];
            int[] sideOfRoadTriangles = new int[numTris * 2 * 3];

            int vertIndex = 0;
            int triIndex = 0;

            // Vertices for the top of the road are layed out:
            // 0  1
            // 8  9
            // and so on... So the triangle map 0,8,1 for example, defines a triangle from top left to bottom left to bottom right.
            int[] triangleMap = { 0, 8, 1, 1, 8, 9 };
            int[] sidesTriangleMap = { 4, 6, 14, 12, 4, 14, 5, 15, 7, 13, 15, 5 };

            bool usePathNormals = true;// !(path.space == PathSpace.xyz && flattenSurface);

            for (int i = 0; i < wayPath.NumPoints; i++)
            {
                Vector3 localUp = wayPath.GetNormal(i);// Vector3.up;// (usePathNormals) ? Vector3.Cross(wayPath.GetTangent(i), wayPath.GetNormal(i)) : Vector3.up/*path.up*/;
                Vector3 localRight = wayPath.GetTangent(i);// Vector3.right;// (usePathNormals) ? wayPath.GetNormal(i) : Vector3.Cross(localUp, wayPath.GetTangent(i));

                // Find position to left and right of current path vertex
                Vector3 vertSideA = wayPath.GetPoint(i) - localRight * Mathf.Abs(wayWidth);
                Vector3 vertSideB = wayPath.GetPoint(i) + localRight * Mathf.Abs(wayWidth);

                // Add top of road vertices
                verts[vertIndex + 0] = vertSideA;
                verts[vertIndex + 1] = vertSideB;
                // Add bottom of road vertices
                verts[vertIndex + 2] = vertSideA - localUp * thickness;
                verts[vertIndex + 3] = vertSideB - localUp * thickness;

                // Duplicate vertices to get flat shading for sides of road
                verts[vertIndex + 4] = verts[vertIndex + 0];
                verts[vertIndex + 5] = verts[vertIndex + 1];
                verts[vertIndex + 6] = verts[vertIndex + 2];
                verts[vertIndex + 7] = verts[vertIndex + 3];

                // Set uv on y axis to path time (0 at start of path, up to 1 at end of path)
                uvs[vertIndex + 0] = new Vector2(0, wayPath.GetTime(i));
                uvs[vertIndex + 1] = new Vector2(1, wayPath.GetTime(i));

                // Top of road normals
                normals[vertIndex + 0] = localUp;
                normals[vertIndex + 1] = localUp;
                // Bottom of road normals
                normals[vertIndex + 2] = -localUp;
                normals[vertIndex + 3] = -localUp;
                // Sides of road normals
                normals[vertIndex + 4] = -localRight;
                normals[vertIndex + 5] = localRight;
                normals[vertIndex + 6] = -localRight;
                normals[vertIndex + 7] = localRight;

                // Set triangle indices
                if (i < wayPath.NumPoints - 1 || false/*path.isClosedLoop*/)
                {
                    for (int j = 0; j < triangleMap.Length; j++)
                    {
                        roadTriangles[triIndex + j] = (vertIndex + triangleMap[j]) % verts.Length;
                        // reverse triangle map for under road so that triangles wind the other way and are visible from underneath
                        underRoadTriangles[triIndex + j] = (vertIndex + triangleMap[triangleMap.Length - 1 - j] + 2) % verts.Length;
                    }
                    for (int j = 0; j < sidesTriangleMap.Length; j++)
                    {
                        sideOfRoadTriangles[triIndex * 2 + j] = (vertIndex + sidesTriangleMap[j]) % verts.Length;
                    }

                }

                vertIndex += 8;
                triIndex += 6;
            }

            _mesh.Clear();
            _mesh.vertices = verts;
            _mesh.uv = uvs;
            _mesh.normals = normals;
            _mesh.subMeshCount = 3;
            _mesh.SetTriangles(roadTriangles, 0);
            _mesh.SetTriangles(underRoadTriangles, 1);
            _mesh.SetTriangles(sideOfRoadTriangles, 2);
            _mesh.RecalculateBounds();
        }

        private void AssignMeshComponents()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _mesh = new Mesh();

            _meshFilter.sharedMesh = _mesh;
        }

        private void AssignMaterials()
        {
            if (roadMaterial != null && undersideMaterial != null)
            {
                _meshRenderer.sharedMaterials = new Material[] { roadMaterial, undersideMaterial, undersideMaterial };
                _meshRenderer.sharedMaterials[0].mainTextureScale = new Vector3(1, textureTiling);
            }
        }
    }
}