using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(IMoveable))]
public class CordsDetector : MonoBehaviour
{
    private IMoveable moveable;

    private void Start()
    {
        moveable = GetComponent<IMoveable>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AIGridCell aIGridCell = collision.GetComponent<AIGridCell>();
        if(aIGridCell != null)
        {
            moveable.CurrentIndex = aIGridCell.Index;
        }
    }
}
