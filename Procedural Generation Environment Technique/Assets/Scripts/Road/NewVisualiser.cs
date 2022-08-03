using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BasicVisualiser;

public class NewVisualiser : MonoBehaviour
{
    public LsystemGeneration lsystem;
    List<Vector3> positions = new List<Vector3>();
    //public GameObject prefab; //for testing
    public Material lineMaterial; // for testing
    public int customLength = 2;
    public Roads roads;
    public bool drawLine = false;

    private int length = 8;
    private float angle = 90;

    public int Length
    {
        get //=> length; 
        {
            if (length > 0)
            {
                return length;
            }
            else
            {
                return 1;
            }
        }
        set => length = value;
    }

    private void Start()
    {
        var sequence = lsystem.GenerateAxiom();
        VisualiseSequence(sequence);
    }

    private void VisualiseSequence(string sequence)
    {
        Stack<AgentParameters> savePoints = new Stack<AgentParameters>();

        var currentPosition = Vector3.zero;
        Vector3 direction = Vector3.forward;
        Vector3 tempPostion = Vector3.zero;

        positions.Add(currentPosition);

        foreach (var word in sequence)
        {
            Letters symbol = (Letters)word;
            switch (symbol)
            {
                case Letters.save:
                    savePoints.Push(new AgentParameters
                    {
                        position = currentPosition,
                        direction = direction,
                        length = Length
                    });
                    break;
                case Letters.load:
                    if (savePoints.Count > 0) // incase error
                    {
                        var agentParameter = savePoints.Pop();
                        currentPosition = agentParameter.position;
                        direction = agentParameter.direction;
                        length = agentParameter.length;
                    }
                    else
                    {
                        throw new System.Exception("No save points in the stack");
                    }
                    break;
                case Letters.draw:
                    tempPostion = currentPosition;
                    currentPosition += direction * Length;
                    roads.PlaceStreetPos(tempPostion, Vector3Int.RoundToInt(direction), length);
                    if (drawLine == true)
                    {
                        DrawLine(tempPostion, currentPosition, Color.white); //for testing
                    }
                    Length -= customLength;
                    positions.Add(currentPosition);
                    break;
                case Letters.turnRight:
                    direction = Quaternion.AngleAxis(angle, Vector3.up) * direction; //z postion
                    break;
                case Letters.turnLeft:
                    direction = Quaternion.AngleAxis(-angle, Vector3.up) * direction; //z postion
                    break;
                default:
                    break;
            }
        }
        roads.FixRoad();

        //foreach (var postion in positions) // draw the orbs
        //{
        //    Instantiate(prefab, postion, Quaternion.identity);
        //}
    }

    private void DrawLine(Vector3 start, Vector3 next, Color colour) // for testing
    {
        GameObject line = new GameObject("line");
        line.transform.position = start;
        var lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startColor = colour;
        lineRenderer.endColor = colour;
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, next);
    }
}
