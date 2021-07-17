using System.Collections;
using UnityEngine;

public class AIGridCell : MonoBehaviour
{
    [SerializeField]
    private int index;
    public int Index { get { return index; } set { index = value; } }
    private void Start()
    {
        Destroy(GetComponent<BoxCollider>());
        StartCoroutine(AddBoxCollider2DWithDelay());
    }

    private IEnumerator AddBoxCollider2DWithDelay()
    {
        yield return new WaitForSeconds(.4f);
        BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.size = new Vector2(2,2);
    }
}
