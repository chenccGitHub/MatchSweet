using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearLineSweet : ClearSweet
{
    public bool isRow;
    public override void Clear()
    {
        base.Clear();
        if (isRow)
        {
            sweet.gameManager.ClearRowSweet(sweet.Y);
        }
        else
        {
            sweet.gameManager.ClearLineSweet(sweet.X);
        }
    }
}
