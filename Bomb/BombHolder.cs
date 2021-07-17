using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
class BombHolder : MonoBehaviour
{
    private int _bombsCounter;
    private BombSpawner _bombSpawner;

    private void Awake()
    {
        StartCoroutine(LoadClasses());
    }

    private IEnumerator LoadClasses()
    {
        while (true)
        {
            if (!(_bombSpawner = FindObjectOfType<BombSpawner>()))
                yield return new WaitForSeconds(0.2f);
            else yield break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bomb bomb = collision.GetComponent<Bomb>();
        if(bomb != null && !bomb._bombSet )
        {
            _bombsCounter++;
            Destroy(collision.gameObject);
        }
    }

    public void PlantBomb()
    {
        if (_bombsCounter <= 0) return;

        if (_bombSpawner)
        {
            _bombsCounter--;
            GameObject go = Instantiate(_bombSpawner.GetBombPrefab(), transform.position, Quaternion.identity);
            go.GetComponent<Bomb>().BombSet();
        }
    }
}