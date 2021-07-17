using System.Collections;
using UnityEngine;

public class DogModelBehaviour : MonoBehaviour, IHitPointHolder, IScalable
{
    private Sprite[] _sprites;
    private RandomMoving _rm;
    private SpriteRenderer _spriteRenderer;
    private SpritesHolder _sprs;

    public int HitPoints { get; private set; }

    public float scaleXOn1920x1080 { get => 0.25f; }

    public float scaleYOn1920x1080 { get => 0.25f; }

    void Start()
    {
        HitPoints = 2;

        _spriteRenderer = GetComponent<SpriteRenderer>();

        transform.localScale = SizeManager.GetScaleVector(this);

        StartCoroutine(LoadClasses());
    }

    private IEnumerator LoadClasses()
    {
        while (true)
        {
            if (_rm == null)
                _rm = GetComponentInParent<RandomMoving>();

            if (_sprs == null)
            {
                _sprs = FindObjectOfType<SpritesHolder>();
                if (_sprs)
                    _sprites = _sprs.GetDogSprites();
            }

            if (_rm == null || _sprs == null)
            {
                yield return new WaitForSeconds(.2f);
            }
            else yield break;
        }

    }

    void Update()
    {
        if (_rm != null)
        {
            Vector3 moveVector = _rm.GetMovePoint();

            //right
            if (transform.localPosition.x < moveVector.x)
            {
                _spriteRenderer.sprite = _sprites[0];
            }

            //left
            if (transform.localPosition.x > moveVector.x)
            {
                _spriteRenderer.sprite = _sprites[1];
            }

            //bottom
            if (transform.localPosition.y > moveVector.y)
            {
                _spriteRenderer.sprite = _sprites[2];
            }

            //top
            if (transform.localPosition.y < moveVector.y)
            {
                _spriteRenderer.sprite = _sprites[3];
            }
        }
    }

    public void TakeDamage(int damage)
    {
        HitPoints -= damage;

        switch (HitPoints)
        {
            case 1:
                {
                    _sprites = _sprs.GetAngryDogSprites();
                    break;
                }
            default:
                {
                    Destroy(this.gameObject);
                    break;
                }
        }
    }
}
