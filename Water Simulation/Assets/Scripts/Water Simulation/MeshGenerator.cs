using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeshGenerator
{     
    public KeyValuePair<Vector3[], int[]>  GenerateMesh(int xSize, int zSize, float scale)
    {
        Mesh mesh = new Mesh();

        int nxPoints = xSize + 1;
        int nzPoints = zSize + 1;

        Vector3[] vertices = new Vector3[nxPoints * nzPoints];
        int[] triangles = new int[6 * nxPoints * nzPoints];

        for (int x = 0; x < nxPoints; x++)
        {
            for(int z = 0; z < nzPoints; z++)
            {
                vertices[x * nzPoints + z] = new Vector3(x, 0, z) * scale;
            }
        }

        int i = 0;
        for (int x = 0; x < nxPoints - 1; x++)
        {
            for (int z = 0; z < nzPoints - 1; z++)
            {
                triangles[i + 0] = (x + 0) * nzPoints + (z + 0);
                triangles[i + 1] = (x + 1) * nzPoints + (z + 1);
                triangles[i + 2] = (x + 1) * nzPoints + (z + 0);

                triangles[i + 3] = (x + 0) * nzPoints + (z + 0);
                triangles[i + 4] = (x + 0) * nzPoints + (z + 1);
                triangles[i + 5] = (x + 1) * nzPoints + (z + 1);

                i += 6;
            }
        }

        return new KeyValuePair<Vector3[], int[]>(vertices, triangles);
    }

    //public void UpdateMesh(Vector3[] vertices)
    //{
    //    mesh.vertices = vertices;
    //    mesh.RecalculateNormals();

    //    meshFilter.mesh = mesh;
    //}

    //public void UpdateMesh(int[] triangles)
    //{
    //    mesh.triangles = triangles;
    //    mesh.RecalculateNormals();

    //    meshFilter.mesh = mesh;
    //}

    //public void UpdateMesh()
    //{
    //    mesh.RecalculateNormals();

    //    meshFilter.mesh = mesh;
    //}
}
