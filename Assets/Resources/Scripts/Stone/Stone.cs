using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour, IHitPointHolder, IScalable
{
    public static Graph graph;
    public int IndexInGraph = -1;

    public int HitPoints { get; private set; }

    public float scaleXOn1920x1080 { get => 1; }

    public float scaleYOn1920x1080 { get => 1; }

    public void TakeDamage(int damage)
    {
        HitPoints -= damage;

        if (HitPoints <= 0)
            Destroy(this.gameObject);
    }

    private void Start()
    {
        HitPoints = 1;
        transform.localScale = SizeManager.GetScaleVector(this);
    }

    private void OnDestroy()
    {
        if(IndexInGraph >= 0 && graph != null)
        {
            graph.RestoreGraphPointByIndex(IndexInGraph, transform.position);
        }
    }

}
