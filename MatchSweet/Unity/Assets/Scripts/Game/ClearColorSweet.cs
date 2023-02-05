using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearColorSweet : ClearSweet
{
    private ColorType clearColor;

    public ColorType ClearColor { get => clearColor; set => clearColor = value; }

    public override void Clear()
    {
        base.Clear();
        sweet.gameManager.ClearColorSweet(clearColor);
    }
}
