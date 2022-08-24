using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code is very messy
public class PlanetFlatCrust : MonoBehaviour
{
    public int seed = 20;
    public int numbPlate = 30;
    public float radius = 8;
    public float amplitude = 1f;

    public FlatCrust crust;
    public List<FlatPlate> plates;

    private MeshFilter meshFilter;
    private Mesh mesh;

    //if i want to change colour
    private Color landColour;
    private Color waterColour;

    //Starts crust value and change vertices  
    void OnValidate()
    {
        meshFilter = GetComponent<MeshFilter>();
        if (!mesh)
        {
            mesh = Instantiate<Mesh>(meshFilter.sharedMesh);
        }
        crust = new FlatCrust(numbPlate, radius, mesh, seed);
        ChangeVertices(crust);
    }

    //Instantiate sometimes instantiate the instantiate?
    //replace with planet again and it seem it will fix itself for now
    private void Awake()
    {
        mesh = Instantiate<Mesh>(meshFilter.sharedMesh);
        ChangeVertices(crust);
    }

    // Changes the vertex pos to make water depth
    private void ChangeVertices(FlatCrust crust)
    {
        List<FlatPlate> plates = crust.plates;
        Vector3[] vertices = mesh.vertices;
        Vector3[] newVertices = new Vector3[vertices.Length];

        foreach (FlatPlate plate in plates)
        {
            foreach (int vertexID in plate.vertices)
            {
                Vector3 vertex = vertices[vertexID];
                if (plate.plateType == FlatPlate.PlateType.water)
                {
                    newVertices[vertexID] = vertex - vertex.normalized;
                }
                else
                {
                    //small bit of Perlin Noise random
                    float height = Perlin.Noise(vertex.normalized * radius);
                    float roughness = amplitude * 0.01f;
                    newVertices[vertexID] = vertex + (vertex.normalized * height * roughness);
                }
            }
        }

        //NEEDS TO BE MESH not shared mesh. It will break prefab mesh.
        //need to try to fix this bug
        meshFilter.mesh.vertices = newVertices;
        meshFilter.mesh.RecalculateNormals();
    }
}
