using System;
using System.Collections;
using UnityEngine;

public class BombSpawner : MonoBehaviour, IAIGridHolder
{
    private static BombSpawner _instance;
    public static int bombCounter = 0;
    private const int bombLimit = 1;

    [SerializeField]
    private GameObject _bombPrefab;

    private BombSpawner()
    {
        GameManager.OnGraphCreated += SetGraph;
    }

    public Graph _graph { get; private set; }

    public int _gridSize { get; private set; }

    public static BombSpawner GetInstance()
    {
        if (_instance == null)
            _instance = new BombSpawner();

        return _instance;
    }

    public void SetGraph(Graph graph, int gridSize)
    {
        _graph = graph;
        _gridSize = gridSize;

        GameManager.OnGraphCreated -= SetGraph;
    }

    private void Update()
    {
        if(_graph != null)
        {
            SpawnBombInRandomPosition();
        }
    }

    private void SpawnBombInRandomPosition()
    {
        if(BombSpawner.bombCounter < bombLimit)
        {
            Instantiate(_bombPrefab, _graph.GetRandomPosition(), Quaternion.identity);
            bombCounter++;
        }
    }

    public GameObject GetBombPrefab() => _bombPrefab;
}

