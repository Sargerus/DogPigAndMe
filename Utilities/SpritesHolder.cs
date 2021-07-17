using UnityEngine;

public class SpritesHolder : MonoBehaviour
{
    [SerializeField]
    private Sprite[] _dogSprites; //4 - right, left, bottom, top

    [SerializeField]
    private Sprite[] _angryDogSprites; //4 - right, left, bottom, top

    [SerializeField]
    private Sprite[] _pigSprites; //4 - right, left, bottom, top

    [SerializeField]
    private Sprite _stoneSprite;

    [SerializeField]
    private Sprite _bombSprite;

    public Sprite[] GetDogSprites() => _dogSprites;
    public Sprite[] GetAngryDogSprites() => _angryDogSprites;
    public Sprite[] GetPigSprites() => _pigSprites;
    public Sprite GetStoneSprite() => _stoneSprite;
    public Sprite GetBombSprite() => _bombSprite;
}
