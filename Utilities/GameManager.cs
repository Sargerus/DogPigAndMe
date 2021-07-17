using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private ImageProccessing _imageProccessing;
    private Graph _graph;
    private SpritesHolder _spritesHolder;
    public GameObject stonePrefab;

    public delegate void GraphNotifier(Graph graph, int gridSize);
    public static event GraphNotifier OnGraphCreated;

    private void Awake()
    {
        StartCoroutine(LoadClasses());
    }

    private IEnumerator LoadClasses()
    {
        while (true)
        {
            if (_imageProccessing == null)
                _imageProccessing = FindObjectOfType<ImageProccessing>();

            if (_spritesHolder == null)
                _spritesHolder = FindObjectOfType<SpritesHolder>();

            if (_imageProccessing && _spritesHolder)
            {
                CreateAIGrid();
                yield break;
            }

            yield return new WaitForSeconds(.2f);
        }
    }

    private void CreateAIGrid()
    {
        StartCoroutine(ProccessBlocks());
    }

    private IEnumerator ProccessBlocks()
    {
        SpriteRenderer spr;
        Stone stoneScript;
        GameObject go;
        Sprite stone = _spritesHolder.GetStoneSprite();
        List<int> graphDots = new List<int>();

        while (true)
        {
            if (_imageProccessing == null)
            {
                yield return new WaitForSeconds(0.2f);
                continue;
            }                

            List<(Vector3, Vector3)> cellCoords = ProccessImage();

            for (int i = 0; i < cellCoords.Count; i++)
            {
                graphDots.Add(0);
            }

            _graph = new Graph(graphDots, cellCoords, ImageProccessing.gridRowSize, ImageProccessing.gridColumnSize);

            for (int i = 1; i < ImageProccessing.gridRowSize; i+=2)
            {
            
                for (int j = 1; j < ImageProccessing.gridColumnSize; j+=2)
                {            
                    go = Instantiate(stonePrefab, _imageProccessing.centers[i * ImageProccessing.gridColumnSize + j].transform.localPosition, Quaternion.identity);
                    
                    if((spr = go.GetComponent<SpriteRenderer>()) != null)
                        spr.sprite = stone;

                    if((stoneScript = go.GetComponent<Stone>()) != null)
                        stoneScript.IndexInGraph = i * ImageProccessing.gridColumnSize + j;

                    _graph.RemoveVertices(i * ImageProccessing.gridColumnSize + j);
                }
            }

            Stone.graph = _graph;
            yield break;
        }
    }

    private List<(Vector3, Vector3)> ProccessImage()
    {
        List<(Vector3, Vector3)> rectangles = new List<(Vector3, Vector3)>();

        if (_imageProccessing)
            rectangles = _imageProccessing.ProccessImage();

        return rectangles;
    }

    private void FixedUpdate()
    {
        OnGraphCreated?.Invoke(_graph, ImageProccessing.gridColumnSize * ImageProccessing.gridRowSize);
    }

}
