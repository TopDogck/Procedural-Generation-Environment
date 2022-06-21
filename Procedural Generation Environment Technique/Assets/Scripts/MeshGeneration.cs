using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGeneration : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    public int xSize = 20;
    public int zSize = 20;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        //StartCoroutine(CreateShape());
        CreateShape();
    }

    void Update()
    {
        UpdateMesh(); 
    }

    void CreateShape() //IEnumerator 
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 3f;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vertlook = 0;
        int trilook = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[trilook + 0] = vertlook + 0;
                triangles[trilook + 1] = vertlook + xSize + 1;
                triangles[trilook + 2] = vertlook + 1;

                triangles[trilook + 3] = vertlook + 1;
                triangles[trilook + 4] = vertlook + xSize + 1;
                triangles[trilook + 5] = vertlook + xSize + 2;

                vertlook++;
                trilook += 6;

                //yield return new WaitForSeconds(.3f);
            }
            vertlook++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    //private void OnDrawGizmos()
    //{
    //    if (vertices == null) return;

    //    for (int d = 0; d < vertices.Length; d++)
    //    {
    //        Gizmos.DrawSphere(vertices[d], .1f);
    //    }
    //}
}
