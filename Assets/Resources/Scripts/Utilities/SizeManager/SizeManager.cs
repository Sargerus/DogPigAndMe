using UnityEngine;

public class SizeManager
{
    public static Vector2 GetScaleVector(IScalable obj)
    {
        return new Vector2(obj.scaleXOn1920x1080 / (1920f / Screen.width), obj.scaleYOn1920x1080 / (1920f / Screen.width));
    }
}
