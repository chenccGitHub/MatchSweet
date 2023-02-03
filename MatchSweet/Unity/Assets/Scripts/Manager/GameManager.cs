using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 甜点枚举
/// </summary>
public enum SweetsType
{
    EMPTY,  //空类型
    NORMAL, //普通类型
    BARRIER,//障碍类型
    ROWCLEAR, //行清除
    COLUMNCLEAR, //列清除
    RAINBOWCANDY, //彩虹糖类型
    COUNT,         //计数类型
}
/// <summary>
/// 算法类型
/// </summary>
public enum MatchType
{
    COLUMN_ROW, //行列匹配类型
    ROW_COLUMN, //列行匹配类型
}
public class GameManager : Singleton<GameManager>
{
    [Header("游戏行、列")]
    public int xColumn; //列
    public int yRow;    //行
    [SerializeField]
    private float fillTime = 0.1f; //填充时间
    [Header("网格相关")]
    public GameObject girdPrefab; //网格预制体

    public bool isGameOver; //游戏结束
    private float gameTime = 60; //游戏时间
    private float scoreTime ; //加分数的时间
    private int score = 0;
    private int currentScore = 0;//当前分数


    private void Update()
    {
        gameTime -= Time.deltaTime;
        if (gameTime <= 0)
        {
            gameTime = 0;
            //游戏结束
            GameOver();
            return;
        }
        if (scoreTime <= 0.05f)
        {
            scoreTime += Time.deltaTime;
        }
        else
        {
            if (currentScore < score)
            {
                currentScore += 50;
                UIManager.GetView<UIGamePanel>().SetScoreText(currentScore);
                scoreTime = 0;
            }
        }
        UIManager.GetView<UIGamePanel>().SetTimeText(gameTime);
    }

    public void AddScore(int value)
    {
        score += value;
        UIManager.GetView<UIGamePanel>().SetScoreText(score);
    }
    #region
    [Header("甜品元素相关类型")]
    public Dictionary<SweetsType, GameObject> sweetPrefabDic; //甜品预制体字典（包含类型以及实例化物体）
    [System.Serializable]
    public struct SweetPrefab
    {
        public SweetsType type;  //甜品类型
        public GameObject prefab; //甜品预制体
    }
    public SweetPrefab[] sweetPrefabs; //甜品结构体数组（用于初始化赋值）
    private GameSweet[,] sweets; //甜品二维数组（用于生成甜品）
    private GameSweet pressedSweet; //鼠标按下的甜品
    private GameSweet enteredSweet; //鼠标进入的甜品
    #endregion
    private void Start()
    {
        Init();
        TestCreatBarrier();
        StartCoroutine(IAllFill());
        UIManager.Show<UIGamePanel>();
    }
    /// <summary>
    /// 测试生成障碍饼干
    /// </summary>
    private void TestCreatBarrier()
    {
        Destroy(sweets[4, 4].gameObject);
        CreateNewSweet(4, 4, SweetsType.BARRIER);
    }
    private void Init()
    {
        sweetPrefabDic = new Dictionary<SweetsType, GameObject>(); //实例化字典
        for (int i = 0; i < sweetPrefabs.Length; i++)
        {
            if (!sweetPrefabDic.ContainsKey(sweetPrefabs[i].type))
            {
                sweetPrefabDic.Add(sweetPrefabs[i].type, sweetPrefabs[i].prefab);
            }
        }
        //生成网格
        for (int x = 0; x < xColumn; x++)
        {
            for (int y = 0; y < yRow; y++)
            {
                GameObject chocolate = Instantiate(girdPrefab, CoreectPosition(x, y), Quaternion.identity);
                chocolate.transform.SetParent(transform);
            }
        }
        //GameSweet二维数组，作用于具体游戏位置
        sweets = new GameSweet[xColumn, yRow];
        for (int x = 0; x < xColumn; x++)
        {
            for (int y = 0; y < yRow; y++)
            {
                //游戏开始生成空类型的甜品用于做检测当前位置是否有元素
                CreateNewSweet(x, y, SweetsType.EMPTY);
            }
        }
    }
    /// <summary>
    /// 巧乐力生成的位置坐标
    /// </summary>
    public Vector3 CoreectPosition(int x, int y)
    {
        //巧克力x轴坐标计算：GameManager的X轴坐标 - 网格长度的一半 + 列对应的x坐标
        //巧克力Y轴坐标计算：GameManager的Y轴坐标 + 网格高度的一半 - 行对应的y坐标
        return new Vector3(transform.position.x - xColumn / 2f + x, transform.position.y + yRow / 2f - y);
    }
    /// <summary>
    /// 生成甜品
    /// </summary>
    /// <param name="生成坐标x"></param>
    /// <param name="生成坐标y"></param>
    /// <param name="生成甜品类型"></param>
    /// <returns></returns>
    public GameSweet CreateNewSweet(int x, int y, SweetsType type)
    {
        GameObject newSweet = Instantiate(sweetPrefabDic[type], CoreectPosition(x, y), Quaternion.identity);
        newSweet.transform.SetParent(transform);
        sweets[x, y] = newSweet.GetComponent<GameSweet>();
        sweets[x, y].Init(x, y, this, type);
        //随机生成甜品类型
        if (sweets[x, y].CanColor())
        {
            sweets[x, y].ColoredComponent.SetColor((ColorType)Random.Range(0, sweets[x, y].ColoredComponent.NumColor));
        }
        return sweets[x, y];
    }
    /// <summary>
    /// 全部填充
    /// </summary>
    public IEnumerator IAllFill()
    {
        bool needFill = true; //是否需要继续填充
        while (needFill)
        {
            yield return new WaitForSeconds(fillTime);
            while (Fill()) //如果填充还没有完成的话就继续填充
            {
                yield return new WaitForSeconds(fillTime);
            }
            needFill = ClearAllMatchedSweet();
        }
    }
    /// <summary>
    /// 分部填充
    /// </summary>
    public bool Fill()
    {
        bool filledNotFinished = false; //是否没有填充完成
        //从上往下遍历
        for (int y = yRow - 2; y >= 0; y--)
        {
            for (int x = 0; x < xColumn; x++)
            {
                GameSweet sweet = sweets[x, y]; //得到当前甜品元素的位置对象
                if (!sweet.CanMove()) continue;
                //如果甜品可以移动的话就需要填充
                GameSweet sweetBelow = sweets[x, y + 1];
                if (sweetBelow.Type == SweetsType.EMPTY) //垂直填充
                {
                    VerticalFill(sweetBelow, sweet, x, y);
                    filledNotFinished = true;
                }
                else
                {
                    //斜向填充
                    filledNotFinished = NotVerticalFill(sweet, x, y);
                }

            }
        }
        filledNotFinished = SpecialFill();
        return filledNotFinished;
    }
    /// <summary>
    /// 最上排特殊填充
    /// </summary>
    /// <returns></returns>
    private bool SpecialFill()
    {
        bool isFillFinish = false;
        //最上面的特殊情况
        for (int x = 0; x < xColumn; x++)
        {
            GameSweet sweet = sweets[x, 0]; //获取最上排甜品位置的元素
            if (sweet.Type == SweetsType.EMPTY) //如果甜品类型为空的话就生成
            {
                GameObject newSweet = Instantiate(sweetPrefabDic[SweetsType.NORMAL], CoreectPosition(x, -1), Quaternion.identity);
                newSweet.transform.parent = transform;
                sweets[x, 0] = newSweet.GetComponent<GameSweet>();
                sweets[x, 0].Init(x, -1, this, SweetsType.NORMAL);
                sweets[x, 0].MovedComponent.Move(x, 0, fillTime);
                sweets[x, 0].ColoredComponent.SetColor((ColorType)Random.Range(0, sweets[x, 0].ColoredComponent.NumColor));
                isFillFinish = true;
            }
        }
        return isFillFinish;
    }
    /// <summary>
    /// 垂直填充
    /// </summary>
    private void VerticalFill(GameSweet sweetBelow, GameSweet sweet, int x, int y)
    {
        Destroy(sweetBelow.gameObject);
        sweet.MovedComponent.Move(x, y + 1, fillTime);
        sweets[x, y + 1] = sweet;
        CreateNewSweet(x, y, SweetsType.EMPTY);
    }
    /// <summary>
    /// 斜向填充
    /// </summary>
    private bool NotVerticalFill(GameSweet sweet, int x, int y)
    {
        bool isFillFinish = false;
        //斜向填充
        for (int down = -1; down <= 1; down++)
        {
            //等于0是垂直填充
            if (down == 0) continue;
            //偏移
            int downX = x + down;
            //超过边界限制
            if (downX < 0 || downX >= xColumn) continue;
            GameSweet downSweet = sweets[downX, y + 1];
            //检测是否可以填充
            if (downSweet.Type != SweetsType.EMPTY) continue;
            //判断垂直填充是否满足要求
            bool canfill = true;
            //遍历甜食正上方是否可以往下填充，如果可以的话就不需要斜向填充
            for (int aboveY = y; aboveY >= 0; aboveY--)
            {
                GameSweet aboveSweet = sweets[downX, aboveY];
                if (aboveSweet.CanMove())
                {
                    break;
                }
                else if (!aboveSweet.CanMove() && aboveSweet.Type != SweetsType.EMPTY) //不能移动且上面不是空物体 
                {
                    canfill = false;
                    break;
                }
            }
            if (!canfill) //填充元素
            {
                Destroy(downSweet.gameObject);
                sweet.MovedComponent.Move(downX, y + 1, fillTime);
                sweets[downX, y + 1] = sweet;
                CreateNewSweet(x, y, SweetsType.EMPTY);
                isFillFinish = true;
                break;
            }
        }
        return isFillFinish;
    }
    /// <summary>
    /// 甜品是否相邻
    /// </summary>
    /// <returns></returns>
    public bool IsSweetAdjacent(GameSweet sweet1, GameSweet sweet2)
    {
        return (sweet1.X == sweet2.X && Mathf.Abs(sweet1.Y - sweet2.Y) == 1) || (sweet1.Y == sweet2.Y && Mathf.Abs(sweet1.X - sweet2.X) == 1);
    }
    /// <summary>
    /// 交换甜品
    /// </summary>
    public void ExchangeSweets(GameSweet sweet1, GameSweet sweet2)
    {
        //检测交换是否满足条件
        if (!sweet1.CanMove() || !sweet2.CanMove() || !IsSweetAdjacent(sweet1, sweet2))
        {
            return;
        }
        //位置调换
        sweets[sweet1.X, sweet1.Y] = sweet2;
        sweets[sweet2.X, sweet2.Y] = sweet1;
        //检测匹配元素是否为空
        if (MatchSweets(sweet1, sweet2.X, sweet2.Y) != null || MatchSweets(sweet2, sweet1.X, sweet1.Y) != null)
        {
            int tempX = sweet1.X;
            int tempY = sweet1.Y;
            sweet1.MovedComponent.Move(sweet2.X, sweet2.Y, fillTime);
            sweet2.MovedComponent.Move(tempX, tempY, fillTime);
            ClearAllMatchedSweet();
            StartCoroutine(IAllFill());
        }
        else
        {
            sweets[sweet1.X, sweet1.Y] = sweet1;
            sweets[sweet2.X, sweet2.Y] = sweet2;
        }
       
        
    }
    /// <summary>
    /// 鼠标按下甜品
    /// </summary>
    public void PressedSweet(GameSweet sweet)
    {
        if (isGameOver)
        {
            return;
        }
        pressedSweet = sweet;
    }
    /// <summary>
    /// 鼠标进入甜品
    /// </summary>
    public void EnteredSweet(GameSweet sweet)
    {
        enteredSweet = sweet;
    }
    /// <summary>
    /// 鼠标松开甜品
    /// </summary>
    public void ReleaseSweet()
    {
        ExchangeSweets(pressedSweet, enteredSweet);
    }
    /// <summary>
    /// 匹配甜品算法
    /// </summary>
    /// <param name="sweet"></param>
    /// <param name="newX"></param>
    /// <param name="newY"></param>
    /// <returns></returns>
    public List<GameSweet> MatchSweets(GameSweet sweet, int newX, int newY)
    {
        List<GameSweet> tempList;
        //有颜色的才可以匹配
        if (!sweet.CanColor())
        {
            return null;
        }
        //获取匹配元素的颜色类型
        ColorType color = sweet.ColoredComponent.Color;
        //行匹配列表
        List<GameSweet> matchRowSweets = new List<GameSweet>();
        //列匹配列表
        List<GameSweet> matchLineSweets = new List<GameSweet>();
        //完成匹配列表
        List<GameSweet> matchFinishedSweets = new List<GameSweet>();

        //行匹配
        matchRowSweets.Add(sweet);
        MatchAlgorithm(newX, 100, newY, matchRowSweets, color, xColumn);
        tempList = LTMatchSweet(MatchType.COLUMN_ROW, matchRowSweets, matchLineSweets, matchFinishedSweets, newY, color, xColumn);
        if (tempList != null)
        {
            return tempList;
        }
        matchRowSweets.Clear();
        matchLineSweets.Clear();
        matchFinishedSweets.Clear();

        //列匹配
        matchLineSweets.Add(sweet);
        //i=0 往上匹配，i=1 往下匹配
        MatchAlgorithm(newY, newX, 100, matchLineSweets, color, yRow);
        tempList = LTMatchSweet(MatchType.ROW_COLUMN, matchLineSweets, matchRowSweets, matchFinishedSweets, newX, color, yRow);
        return tempList;
    }
    /// <summary>
    /// 清除甜品
    /// </summary>
    /// <returns></returns>
    public bool ClearSweet(int x, int y)
    {
        //检测当前位置的甜品元素是否可以清除且没有在清除中
        if (sweets[x, y].CanClear() && !sweets[x, y].ClearedComponent.IsClearing)
        {
            sweets[x, y].ClearedComponent.Clear();
            CreateNewSweet(x, y, SweetsType.EMPTY);
            return true;
        }
        return false;
    }
    /// <summary>
    /// 清除所有完成匹配的甜品元素
    /// </summary>
    /// <returns></returns>
    public bool ClearAllMatchedSweet()
    {
        bool needRefill = false;
        for (int y = 0; y < yRow; y++)
        {
            for (int x = 0; x < xColumn; x++)
            {
                if (!sweets[x, y].CanClear()) continue;
                List<GameSweet> matchList = MatchSweets(sweets[x, y], x, y);
                if (matchList == null) continue;
                foreach (var tempSweet in matchList)
                {
                    if (ClearSweet(tempSweet.X, tempSweet.Y))
                    {
                        needRefill = true;
                    }
                }
            }
        }
        return needRefill;
    }
    /// <summary>
    /// 匹配算法
    /// </summary>
    private void MatchAlgorithm(int pos, int newX, int newY, List<GameSweet> matchSweets, ColorType color, int distance)
    {
        //i=0 往左匹配，i=1 往右匹配
        for (int i = 0; i <= 1; i++)
        {
            for (int rank = 1; rank < distance; rank++)
            {
                int m;
                if (i == 0)
                {
                    m = pos - rank;
                }
                else
                {
                    m = pos + rank;
                }
                //限制边界
                if (m < 0 || m >= distance)
                {
                    continue;
                }
                int x = (newX == 100) ? m : newX;
                int y = (newY == 100) ? m : newY;
                //满足条件的加入行列表匹配
                if (sweets[x, y].CanColor() && sweets[x, y].ColoredComponent.Color == color)
                {
                    if (!matchSweets.Contains(sweets[x, y]))
                    {
                        matchSweets.Add(sweets[x, y]);
                    } 
                }
                else
                {
                    break;
                }
            }
        }
    }
    /// <summary>
    /// LT匹配糖果
    /// </summary>
    private List<GameSweet> LTMatchSweet(MatchType matchType, List<GameSweet> matchSweet1, List<GameSweet> matchSweet2, List<GameSweet> matchFinishedSweets, int pos, ColorType color, int distance)
    {
        if (matchSweet1.Count >= 3)
        {
            for (int i = 0; i < matchSweet1.Count; i++)
            {
                if (!matchFinishedSweets.Contains(matchSweet1[i]))
                {
                    matchFinishedSweets.Add(matchSweet1[i]);
                }
                int x = matchSweet1[i].X;
                int y = 100;     //100表示不用传进来的参数
                if (matchType != MatchType.COLUMN_ROW) 
                {
                    x = 100;
                    y = matchSweet1[i].Y;
                }
                MatchAlgorithm(pos, x, y, matchSweet2, color, xColumn);
                if (matchSweet2.Count < 2)
                {
                    matchSweet2.Clear();
                }
                else
                {
                    for (int j = 0; j < matchSweet2.Count; j++)
                    {
                        if (!matchFinishedSweets.Contains(matchSweet2[j]))
                        {
                            matchFinishedSweets.Add(matchSweet2[j]);
                        }
                    }
                }
            }
        }

        if (matchFinishedSweets.Count >= 3)
        {
            return matchFinishedSweets;
        }
        return null;
    }
    /// <summary>
    /// c重新开始游戏
    /// </summary>
    public void ResetGame()
    {
        gameTime = 60;
        isGameOver = false;
        UIManager.GetView<UIGamePanel>().SetTimeAnimation(true);
        UIManager.Close<UIGameOverPanel>();

    }
    public void GameOver()
    {
        GameManager.Instance.isGameOver = true;
        UIManager.Show<UIGameOverPanel>();
        UIManager.GetView<UIGameOverPanel>().ResultScore(score);
        UIManager.GetView<UIGamePanel>().SetTimeAnimation(false);
    }
}
