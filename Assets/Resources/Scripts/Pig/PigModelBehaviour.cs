using System.Collections;
using UnityEngine;

class PigModelBehaviour : MonoBehaviour, IHitPointHolder, IScalable
{
    private Sprite[] _sprites;
    private SpriteRenderer _spriteRenderer;
    private PlayersMoving _pm;

    public int HitPoints { get; private set; }

    public float scaleXOn1920x1080 { get => 0.25f; }
    public float scaleYOn1920x1080 { get => 0.25f; }

    private void Start()
    {
        HitPoints = 1;

        _spriteRenderer = GetComponent<SpriteRenderer>();

        transform.localScale = SizeManager.GetScaleVector(this);

        StartCoroutine(LoadClasses());
    }

    private IEnumerator LoadClasses()
    {
        SpritesHolder _sprs = null;

        while (true)
        {
            if (_sprs == null)
                _sprs = FindObjectOfType<SpritesHolder>();

            if (_pm == null)
                _pm = GetComponentInParent<PlayersMoving>();


            if (_sprs == null || _pm == null)
            {
                yield return new WaitForSeconds(.2f);
            }
            //complete
            else 
            {
                _sprites = _sprs.GetPigSprites();
                yield break;
            }
            
        }

    }

    private void Update()
    {
        if(_pm && _spriteRenderer)
        {
            Vector3 moveVector = _pm.GetMovePoint();
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

        if (HitPoints <= 0)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dog"))
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CompareTag(collision.gameObject.tag).Equals("Dog"))
        {
            Destroy(this.gameObject);
        }
    }




}
