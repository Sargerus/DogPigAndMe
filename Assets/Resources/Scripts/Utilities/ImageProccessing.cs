using System;
using System.Collections.Generic;
using UnityEngine;

public class ImageProccessing : MonoBehaviour
{
    public struct ColorRectangle
    {
        public float xLeftTop, xRightTop, xLeftBottom, xRightBottom, yTop, yBottom;
        public Color color;

        public ColorRectangle(int x, int y, Color color)
        {
            this.color = color;
            xLeftTop = xLeftBottom = xRightTop = xRightBottom = x;
            yTop = yBottom = y;
        }
        
        public void findCenter(out Vector2 center)
        {
            Vector2 top = new Vector2( (xRightTop + xLeftTop) / 2, yTop);
            Vector2 bottom = new Vector2( (xRightBottom + xLeftBottom) / 2, yBottom);

            center = new Vector2( (top.x + bottom.x) / 2, (top.y + bottom.y) / 2);
        }

        public void flip()
        {
            float yTopBuf = yTop;
            yTop = yBottom;
            yBottom = yTopBuf;
        }
    }

    public const int gridRowSize = 9;
    public const int gridColumnSize = 17;
    
    [SerializeField]
    private Texture2D _AIGrid;

    [SerializeField]
    private Texture2D _background;

    public GameObject BackroundHolder;
    public List<GameObject> centers;

    List<ColorRectangle> vertices = new List<ColorRectangle>();
    List<(Vector3, Vector3)> verticesVectors = new List<(Vector3, Vector3)>();

    public List<(Vector3, Vector3)> ProccessImage()
    {
        int istep;
        int jstep;

        float height = Camera.main.orthographicSize * 2;
        float width = height * Screen.width / Screen.height;

        Vector3 _cameraTopLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 10f));
        Vector3 _cameraBottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 10f));

        Vector3 left = Camera.main.WorldToScreenPoint(_cameraTopLeft);
        Vector3 right = Camera.main.WorldToScreenPoint(_cameraBottomRight);

        Texture2D texture2D = ScaleTexture(_background, Convert.ToInt32(right.x), Convert.ToInt32(left.y));
        BackroundHolder.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));

        SpriteRenderer spr = BackroundHolder.GetComponent<SpriteRenderer>();
        Camera.main.orthographicSize = spr.bounds.size.x * Screen.height / Screen.width * 0.5f;

        Texture2D scaledGridTexture = ScaleTexture(_AIGrid, Convert.ToInt32(right.x), Convert.ToInt32(left.y));
        istep = scaledGridTexture.height / 25;//36;
        jstep = scaledGridTexture.width / 43;//51;
  
       Color ignoreColor = new Color();
       Color currentColor = new Color();
       Color previousColor = new Color();
       List<ColorRectangle> verticesLine = new List<ColorRectangle>();
  
       for(int i = scaledGridTexture.height; i > 0; i-=istep)
       {
            if (vertices.Count == gridColumnSize * gridRowSize) break;
            int cell = -1;
           ColorRectangle colorRectangle = new ColorRectangle();
  
           for (int j = 0; j < scaledGridTexture.width; j+=jstep)
           {
               currentColor = scaledGridTexture.GetPixel(j, i);
  
               if (currentColor.a == 0f) continue;
  
               //find background ignoring color, only once
               if (ignoreColor.ColorIsEmpty())
               {
                   ignoreColor = currentColor;
                   previousColor = currentColor;
                   continue;
               }
  
               if (currentColor.CompareColors(ignoreColor)) 
               {
                   previousColor = currentColor;
                   continue;
               } 
  
               //proccessing
               if (currentColor.CompareColors(previousColor))
               {
                   if(verticesLine[cell].xRightTop < j)
                   {
                        if (cell < 0 && cell >= verticesLine.Count) continue;

                       colorRectangle = verticesLine[cell];
                       colorRectangle.xRightTop = j;
                       verticesLine[cell] = colorRectangle;
                   }
               }
               //found new rectangle
               if ((!currentColor.CompareColors(ignoreColor) && !currentColor.CompareColors(previousColor)) || j >= scaledGridTexture.width - jstep)
               {
                   //new rectangles line
                   if(cell == -1 && verticesLine.Count > 0 && !verticesLine[0].color.CompareColors(currentColor) && !currentColor.CompareColors(ignoreColor))
                   {
                       vertices.AddRange(verticesLine);
                       verticesLine.Clear();
                   }
  
                   //new rectangle in row
                   cell++;
  
                   if(cell >= verticesLine.Count && verticesLine.Count != gridColumnSize)
                   {
                        verticesLine.Add(new ColorRectangle(j, i, currentColor));
                    }
                    else
                   {
                       int index;
                       if (cell == 0) //to check whether we on left board or right board
                           index = 0;
                       else index = cell - 1;
  
                       colorRectangle = verticesLine[index];
                       colorRectangle.xLeftBottom = colorRectangle.xLeftBottom > j ? j : colorRectangle.xLeftBottom;
                       colorRectangle.xRightBottom = j;
                       colorRectangle.yBottom = colorRectangle.yBottom > i ? i : colorRectangle.yBottom;
                       verticesLine[index] = colorRectangle;
                   }
  
                   if (cell == gridColumnSize)
                   {
                       previousColor = currentColor;
                       break;
                   }
                       
               }
               previousColor = currentColor;
           }
       }
  
       vertices.AddRange(verticesLine);

        for (int i = 0; i < vertices.Count; i++)
        {

            var scyTop = scaledGridTexture.height - ((vertices[i].yTop + vertices[i].yBottom) / 2);
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            float topMiddleX = ((float)vertices[i].xRightTop + (float)vertices[i].xLeftTop) / 2;
            float bottomMiddleX = ((float)vertices[i].xRightBottom + (float)vertices[i].xLeftBottom) / 2;
            cube.transform.position = Camera.main.ScreenToWorldPoint(new Vector3((topMiddleX + bottomMiddleX) / 2, (float)Screen.height - scyTop, 9));
            centers.Add(cube);
            cube.transform.localScale = new Vector3(0.2f, 0.2f, 1f);
            Destroy(cube.GetComponent<MeshRenderer>());

            verticesVectors.Add((Camera.main.ScreenToWorldPoint(new Vector3(vertices[i].xLeftTop, (float)scaledGridTexture.height - vertices[i].yTop, 9)),
                                 Camera.main.ScreenToWorldPoint(new Vector3(vertices[i].xRightBottom, (float)scaledGridTexture.height - vertices[i].yBottom, 9))));

            AIGridCell aicell = cube.AddComponent<AIGridCell>();
            aicell.Index = i;
        }

        for(int i = 0; i < verticesVectors.Count; i++)
        {
            verticesVectors[i] = (new Vector3(verticesVectors[i].Item1.x, -verticesVectors[i].Item1.y, verticesVectors[i].Item1.z), 
                                    new Vector3(verticesVectors[i].Item2.x, -verticesVectors[i].Item2.y, verticesVectors[i].Item2.z));
        }

        return verticesVectors;
    }

    private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
        Color[] rpixels = result.GetPixels(0);
        float incX = (1.0f / (float)targetWidth);
        float incY = (1.0f / (float)targetHeight);
        for (int px = 0; px < rpixels.Length; px++)
        {
            rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor(px / targetWidth)));
        }
        result.SetPixels(rpixels, 0);
        result.Apply();
        return result;
    }
}
