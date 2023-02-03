using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 甜点颜色类型
/// </summary>
public enum ColorType
{
    BLUE,
    GREEN,
    RED,
    YELLOW,
    PINK,
    PURPLE,
    ANY,
    COUNT

}
public class ColorSweet : MonoBehaviour
{
    private ColorType color;
    public ColorType Color
    { 
        get => color; 
        set => color = value;
    }
    private Dictionary<ColorType, Sprite> colorSpriteDic;
    [System.Serializable]
    public struct ColorSprite
    {
        public ColorType color;
        public Sprite sprite;
    }
    public ColorSprite[] colorSprites;
    private SpriteRenderer sprite;
    public int NumColor
    {
        get { return colorSprites.Length; }
    }
    private void Awake()
    {
        sprite = transform.Find("Sweet").GetComponent<SpriteRenderer>();
        colorSpriteDic = new Dictionary<ColorType, Sprite>();
        for (int i = 0; i < colorSprites.Length; i++)
        {
            if (!colorSpriteDic.ContainsKey(colorSprites[i].color))
            {
                colorSpriteDic.Add(colorSprites[i].color, colorSprites[i].sprite);
            }
        }
    }
    /// <summary>
    /// 设置甜品颜色类型
    /// </summary>
    /// <param name="newColor"></param>
    public void SetColor(ColorType newColor)
    {
        color = newColor;
        if (colorSpriteDic.ContainsKey(newColor))
        {
            sprite.sprite = colorSpriteDic[newColor];
        }
    }

}
