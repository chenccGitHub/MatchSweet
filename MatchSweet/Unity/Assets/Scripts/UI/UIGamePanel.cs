using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePanel : View
{
    private Text timeText; //����ʱ�ı�
    private float time = 10f;    //ʱ��
    private Text scoreText; //�����ı�
    private int score = 0;  //����
    private Button gameResetBtn; //��Ϸ���ð�ť
    private float scoreTime; //�ӷ�����ʱ��
    private float currentScore = 0; //��ǰ����
    public override void Init()
    {
        timeText = transform.Find("Top/Light_Image/Time_Image/TimeText").GetComponent<Text>();
        scoreText = transform.Find("Top/Score_Image/ScoreValue").GetComponent <Text>();
        gameResetBtn = transform.Find("Top/Quit").GetComponent<Button>();
        gameResetBtn.onClick.AddListener(ResetGame);
    }
    private void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            time = 0;
            GameManager.Instance.isGameOver = true;
            UIManager.Show<UIGameOverPanel>();
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
                scoreText.text = currentScore.ToString();
                scoreTime = 0;
            }
        }
        timeText.text = time.ToString("0");
    }
    public void AddScore(int value)
    {
        score += value;
    }
    /// <summary>
    /// ���¿�ʼ��Ϸ
    /// </summary>
    public void ResetGame()
    {
        GameManager.Instance.isGameOver = false;
        time = 10f;
    }
}
