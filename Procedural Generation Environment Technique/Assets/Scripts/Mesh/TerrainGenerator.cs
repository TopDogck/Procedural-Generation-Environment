using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class TerrainGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    Color[] colors;

    public int xSize = 40;
    public int zSize = 40;
    public float MountainFrequencies = 4;
    public float MountainAmplitude = 2;

    public float octaveLayers = 1;
    public float layer1Frequencies = 1;
    public float layer1Amplitude = 1;
    public float layer2Frequencies = 1;
    public float layer2Amplitude = 1;

    public Gradient gradient;

    float minTerrainHeight;
    float maxTerrainHeight;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        
    }

    public void UpdateIsland(int x, int z, float frequency, float amplitude, float octave, float l1frequency, float l1amplitude, float l2frequency, float l2amplitude)
    {
        xSize = x;
        zSize = z;
        MountainFrequencies = frequency;
        MountainAmplitude = amplitude;

        octaveLayers = octave;
        layer1Frequencies = l1frequency;
        layer1Amplitude = l1amplitude;
        layer2Frequencies = l2frequency;
        layer2Amplitude = l2amplitude;
}

    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y_noise = XZOctave(x, z);
                vertices[i] = new Vector3(x, y_noise, z);

                if(y_noise > maxTerrainHeight)
                {
                    maxTerrainHeight = y_noise;
                }
                
                if(y_noise < minTerrainHeight)
                {
                    minTerrainHeight = y_noise;
                }

                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

       for (int vert = 0, tris = 0, z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        colors = new Color[vertices.Length];
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }
    }
    
    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.colors = colors;
        cosntraintParam();
    }

    // Update is called once per frame
    void Update()
    {
        CreateShape();
        UpdateMesh();
        //Debug.Log("Do I even need visual studio?");
    }

    float XZOctave(float x, float z)
    {
        //float xCoord = (float)x / xSize;
        //float zCoord = (float)z / zSize;
        float xCoord = (float)x/xSize;
        float zCoord = (float)z/zSize;
        //float noise = 0f;
        float noise = Mathf.PerlinNoise(xCoord * MountainFrequencies, zCoord * MountainFrequencies) * MountainAmplitude;

        if (octaveLayers >= 1)
        {
            noise += Mathf.PerlinNoise(xCoord * layer1Frequencies, zCoord * layer1Frequencies) * layer1Amplitude;
        }

        if (octaveLayers >= 2) {
            noise += Mathf.PerlinNoise(xCoord * layer2Frequencies, zCoord * layer2Frequencies) * layer2Amplitude;
        }

        return noise;
    }

    void cosntraintParam(){
        if (xSize > 200){
            xSize = 200;
        }

        if (zSize > 200){
            zSize = 200;
        }

        if (xSize < 10){
            xSize = 10;
        }

        if (zSize < 10){
            zSize = 10;
        }
    }
}
