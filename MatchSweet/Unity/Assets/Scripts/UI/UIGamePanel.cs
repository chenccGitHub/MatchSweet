using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePanel : View
{
    private Text timeText; //����ʱ�ı�
    private Text scoreText; //�����ı�
    private Button gameResetBtn; //��Ϸ���ð�ť

    public override void Init()
    {
        timeText = transform.Find("Top/Light_Image/Time_Image/TimeText").GetComponent<Text>();
        scoreText = transform.Find("Top/Score_Image/ScoreValue").GetComponent <Text>();
        gameResetBtn = transform.Find("Top/Quit").GetComponent<Button>();
        gameResetBtn.onClick.AddListener(ResetGame);
    }
    /// <summary>
    /// ���¿�ʼ��Ϸ
    /// </summary>
    public void ResetGame()
    {
        GameManager.Instance.ResetGame();
    }
    /// <summary>
    /// ���÷�����ʾ
    /// </summary>
    public void SetScoreText(int value)
    {
        scoreText.text = value.ToString();
    }
    /// <summary>
    /// ����ʱ����ʾ
    /// </summary>
    public void SetTimeText(float value)
    {
        timeText.text = value.ToString("0");
    }
    /// <summary>
    /// ���ò�����ͣʱ�䶯��
    /// </summary>
    /// <param name="isPlayAnimation"></param>
    public void SetTimeAnimation(bool isPlayAnimation)
    {
        timeText.GetComponent<Animator>().enabled = isPlayAnimation;
    }
}
