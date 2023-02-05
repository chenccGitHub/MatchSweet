using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePanel : View
{
    private Text timeText; //倒计时文本
    private Text scoreText; //分数文本
    private Button gameResetBtn; //游戏重置按钮

    public override void Init()
    {
        timeText = transform.Find("Top/Light_Image/Time_Image/TimeText").GetComponent<Text>();
        scoreText = transform.Find("Top/Score_Image/ScoreValue").GetComponent <Text>();
        gameResetBtn = transform.Find("Top/Quit").GetComponent<Button>();
        gameResetBtn.onClick.AddListener(ResetGame);
    }
    /// <summary>
    /// 重新开始游戏
    /// </summary>
    public void ResetGame()
    {
        GetComponent<AudioSource>().Play();
        GameManager.Instance.ResetGame();
    }
    /// <summary>
    /// 设置分数显示
    /// </summary>
    public void SetScoreText(int value)
    {
        scoreText.text = value.ToString();
    }
    /// <summary>
    /// 设置时间显示
    /// </summary>
    public void SetTimeText(float value)
    {
        timeText.text = value.ToString("0");
    }
    /// <summary>
    /// 设置播放暂停时间动画
    /// </summary>
    /// <param name="isPlayAnimation"></param>
    public void SetTimeAnimation(bool isPlayAnimation)
    {
        timeText.GetComponent<Animator>().enabled = isPlayAnimation;
    }
}
