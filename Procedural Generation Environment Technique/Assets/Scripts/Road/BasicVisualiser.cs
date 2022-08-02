using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicVisualiser : MonoBehaviour
{
    public LsystemGeneration lsystem;
    List<Vector3> positions = new List<Vector3>();
    public GameObject prefab;
    public Material lineMaterial;

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
                    DrawLine(tempPostion, currentPosition, Color.red);
                    Length -= 2;
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

        foreach (var postion in positions)
        {
            Instantiate(prefab, postion, Quaternion.identity);
        }
    }

    private void DrawLine(Vector3 start, Vector3 next, Color colour)
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

    public enum Letters
    { 
        unknown = '1', //1
        save = '[', // [
        load = ']', // [
        draw = 'F', // F
        turnRight = '+', // +
        turnLeft = '-' // -
    }
}
