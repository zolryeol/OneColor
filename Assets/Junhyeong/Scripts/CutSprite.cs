using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSprite
{
    public CutSprite() { }
    CutSprite(Sprite _oriSprite)
    {
        oriSprite = _oriSprite;
    }

    // Start is called before the first frame update
    public Sprite[] cutSprite;
    public Sprite oriSprite;

    public Sprite[] SliceSprite(Texture2D _oriTexture , int rowCount , int colCount)
    {
        Sprite[] cutSprite = new Sprite[rowCount * colCount];

        for (int i = 0; i < rowCount * colCount; i++)
        {
            Texture2D texture = new Texture2D(_oriTexture.width / rowCount, _oriTexture.height / colCount);

            // Sprite를 추출할 영역
            int x = (i % rowCount) * texture.width;
            int y = (i % colCount) * texture.height;

            // Texture2D에서 Sprite 추출
            Color[] pixels = _oriTexture.GetPixels(x, y, texture.width, texture.height);
            texture.SetPixels(pixels);
            texture.Apply();

            cutSprite[i] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        return cutSprite;
    }

}
