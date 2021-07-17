using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
class Bomb : MonoBehaviour
{
    private const float timeBeforeBlow = 3f;
    public bool _bombSet { get; private set; }
    private BoxCollider2D _boxCollider2D;

    private void OnDestroy()
    {
        if(!_bombSet)
            BombSpawner.bombCounter--;
    }

    private void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private IEnumerator Flickering()
    {
        Vector3 originalScale = transform.localScale;
        float time = 0f;
        float timePeriod = 0.2f;
        bool lowsize = true;

        while (true)
        {
            if (time >= timeBeforeBlow)
                yield break;

            lowsize = !lowsize;
            time += timePeriod;

            if (lowsize)
                transform.localScale = originalScale;
            else transform.localScale = transform.localScale * 1.2f;

            yield return new WaitForSeconds(timePeriod);
        }
    }
    private IEnumerator StartBlowCounter()
    {
        while (true)
        {
            yield return new WaitForSeconds(3.0f);
            break;
        }

        _boxCollider2D.enabled = true;
        _boxCollider2D.size *= 1.5f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IHitPointHolder hitPointHolder = collision.GetComponent<IHitPointHolder>();

        if (hitPointHolder == null)
            return;

        switch (_bombSet)
        {
            case true:
                {
                    hitPointHolder.TakeDamage(1);
                    Destroy(this.gameObject);
                    break;
                }
        }
    }

    public void BombSet()
    {
        _bombSet = true;
        StartCoroutine(Flickering());
        StartCoroutine(StartBlowCounter());
    }

    private void Update()
    {
        if (_boxCollider2D.enabled == false && _bombSet == false)
            _boxCollider2D.enabled = true;
    }
}
