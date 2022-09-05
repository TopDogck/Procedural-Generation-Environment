using Habrador_Computational_Geometry;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class TectonicPlateEdge
    {
        public Vector3 start;
        public Vector3 end;

        //2 plates which share the edge together.
        public TectonicPlate[] parents = new TectonicPlate[2];

        public TectonicPlateEdge(Vector3 start, Vector3 end, TectonicPlate parent)
        {
            this.start = start;
            this.end = end;
            parents[0] = parent;
        }
    }