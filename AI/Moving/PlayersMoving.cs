using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersMoving : MonoBehaviour, IMoveable, IAIGridHolder
{
    public int CurrentIndex { get; set; }
    public Graph _graph { get; private set; }
    public int _gridSize { get; private set; }

    private bool _directionChanged;
    private Edge.Direction moveDirection = Edge.Direction.Stop;

    private int moveIndex;
    private List<Vector3> way;
    private Vector3 movePoint;

    void Awake()
    {
        GameManager.OnGraphCreated += SetGraph;
    }

    public void SetGraph(Graph graph, int gridSize) {
        _graph = graph;
        _gridSize = gridSize;

        movePoint = transform.position = _graph.GetRandomPosition();

        GameManager.OnGraphCreated -= SetGraph;
    } 

    void Start()
    {
        SwipeDetector.OnSwipe += setMoveDirection;
        way = new List<Vector3>();
        CurrentIndex = -1;
    }

    void Update()
    {
        if (_graph != null && CurrentIndex >= 0)
        {
            if (transform.localPosition == movePoint || _directionChanged)
            {
                moveIndex++;

                if (moveIndex >= way.Count)
                {
                    moveIndex = 0;
                    way = _graph.AStar(CurrentIndex, GetEndPoint());
                }

                movePoint = way[moveIndex];
                _directionChanged = false;
            }

            transform.position = Vector3.MoveTowards(transform.localPosition, movePoint, Time.deltaTime * 1.0f);
        }
    }

    public Vector3 GetMovePoint() => movePoint;

    private void setMoveDirection(SwipeData sd)
    {
        moveDirection = sd.Direction;
        _directionChanged = true;

        if(sd.Direction == Edge.Direction.Stop)
        {
            BombHolder h = GetComponent<BombHolder>();
            if (h != null){
                h.PlantBomb();
            }
        }
    }

    private int GetEndPoint() => _graph.GetNextPointByDirection(CurrentIndex, moveDirection);

}
