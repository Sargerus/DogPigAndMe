using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CordsDetector))]
public class RandomMoving : MonoBehaviour, IMoveable, IAIGridHolder
{
    private int currentIndex = -1;
    public int CurrentIndex { get { return currentIndex; } set { if (value >= _gridSize) currentIndex = _gridSize - 1; else currentIndex = value; } }

    public Graph _graph { get; private set; }
    public int _gridSize { get; private set; }

    private int moveIndex;
    private List<Vector3> way;
    private Vector3 movePoint;

    void Awake()
    {
        GameManager.OnGraphCreated += SetGraph;
    }

    public void SetGraph(Graph graph, int gridSize)
    {
        _graph = graph;
        _gridSize = gridSize;

        GameManager.OnGraphCreated -= SetGraph;

        transform.position = _graph.GetRandomPosition();
    }

    void Start()
    {
        way = new List<Vector3>();
    }

    void Update()
    {
        if (_graph != null && CurrentIndex >= 0)
        {
            if (transform.localPosition == movePoint || moveIndex == 0)
            {
                moveIndex++;

                if (moveIndex >= way.Count)
                {
                    moveIndex = 0;
                    int randomValue = Random.Range(0, _gridSize);
                    way = _graph.AStar(CurrentIndex, randomValue);
                }

                movePoint = way[moveIndex];
            }

            transform.position = Vector3.MoveTowards(transform.localPosition, movePoint, Time.deltaTime * 1.0f);
        }
    }

    public Vector3 GetMovePoint() => movePoint;
}
