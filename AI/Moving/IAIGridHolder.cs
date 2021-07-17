public interface IAIGridHolder
{
    Graph _graph { get; }
    int _gridSize { get; }

    void SetGraph(Graph graph, int gridSize);
}

