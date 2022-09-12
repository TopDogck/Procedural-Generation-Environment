using Habrador_Computational_Geometry;
using System.Collections.Generic;
using UnityEngine;

    public class PlanetCrustGenerator : MonoBehaviour
    {
        public int seed = 20;
        public int plateAmount = 30;
        public float radius = 8; //Ridged
        public float amplitude = 1f; //Height,Depth
        public PlanetCrust crust;
        public PlanetUI planetUI;
        public bool randomColour = true;
        public HashSet<Vector3> points;
        public List<TectonicPlate> plates;

        private MeshFilter meshFilter;
        private Mesh originalMesh;

        private Color landColor;
        private Color waterColor;
        

        // Update is called once per frame
        void OnValidate()
        {

            meshFilter = GetComponent<MeshFilter>();
            if (!originalMesh)
            {
                originalMesh = Instantiate<Mesh>(meshFilter.sharedMesh);
            }
        }

        public void UpdatePlanet(int seedUI, int plateUI, float radiusUI, float amplitudeUI, bool randomColour)
        {
            Random.InitState(seedUI); //If removed it will keep randomising
            if (randomColour == true)
            {
                //Random colour
                MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
                meshRenderer.material.color = Random.ColorHSV(0, 1, 0.75f, 1f);
                MeshRenderer childMeshRenderer = GetComponentsInChildren<MeshRenderer>()[1];
                childMeshRenderer.material.color = Random.ColorHSV(0, 1, 0.75f, 1f);
                //Debug.Log(childMeshRenderer.material.name);
            }
            else
            {
                //else Earth colours
                MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
                meshRenderer.material.color = Color.green;
                MeshRenderer childMeshRenderer = GetComponentsInChildren<MeshRenderer>()[1];
                childMeshRenderer.material.color = Color.blue;
            }
            seed = seedUI;
            crust = new PlanetCrust(plateUI, radiusUI, originalMesh, randomSeed: seedUI, amplitudeUI);
            ChangeVertices(crust);
        }

        private void Awake()
        {
            //Needed or will break the mesh again
            originalMesh = Instantiate<Mesh>(meshFilter.sharedMesh);
            ChangeVertices(crust);
        }
        
        // Changes vertex position on mesh to represent ocean depth.
        private void ChangeVertices(PlanetCrust crust)
        {
            List<TectonicPlate> plates = crust.plates;
            Vector3[] vertices = originalMesh.vertices;
            Vector3[] newVertices = new Vector3[vertices.Length];

            foreach(TectonicPlate plate in plates)
            {
                foreach(int vertexID in plate.vertices)
                {

                    Vector3 vertex = vertices[vertexID];
                    if(plate.plateType == TectonicPlate.PlateType.oceanic)
                    {
                        newVertices[vertexID] = vertex - vertex.normalized;
                    } else
                    {
                    //Perlin Noise
                        float height = Perlin.Noise(vertex.normalized * planetUI.radiusSize); //radius // planetUI.radiusSize
                    float roughness = planetUI.amplitude * 0.1f;
                        newVertices[vertexID] = vertex + (vertex.normalized * height * roughness);
                    }
                }
            }
            meshFilter.mesh.vertices = newVertices;
            meshFilter.mesh.RecalculateNormals();
        }
    }