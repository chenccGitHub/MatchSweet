using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSweet : MonoBehaviour
{
    [HideInInspector]
    public GameManager gameManager;
    private int x;
    public int X
    {
        get => x;
        set
        {
            if (CanMove())
            {
                x = value;
            }
        }
    }
    private int y;
    public int Y 
    { 
        get => y;
        set 
        {
            if (CanMove())
            {
                y = value;
            }  
        }
    }
    private SweetsType type;
    public SweetsType Type { get => type; }
    private MoveSweet movedComponent;
    public MoveSweet MovedComponent { get => movedComponent; }
    public bool CanMove()
    {
        return movedComponent != null;
    }

    private ColorSweet coloredComponent;
    public ColorSweet ColoredComponent { get => coloredComponent; }

    public bool CanColor()
    {
        return coloredComponent != null;
    }

    private ClearSweet clearedComponent;
    public ClearSweet ClearedComponent { get => clearedComponent;}
    public bool CanClear()
    {
        return clearedComponent != null;
    }
    private void Awake()
    {
        movedComponent = GetComponent<MoveSweet>();
        coloredComponent = GetComponent<ColorSweet>();
        clearedComponent = GetComponent<ClearSweet>();
    }
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
    /// <param name="_gameManager"></param>
    /// <param name="_type"></param>
    public void Init(int _x,int _y,GameManager _gameManager,SweetsType _type)
    {
        x = _x;
        y = _y;
        gameManager = _gameManager;
        type = _type;
    }
    private void OnMouseEnter()
    {
        gameManager.EnteredSweet(this);
    }
    private void OnMouseDown()
    {
        gameManager.PressedSweet(this);
    }
    private void OnMouseUp()
    {
        gameManager.ReleaseSweet();
    }
}
