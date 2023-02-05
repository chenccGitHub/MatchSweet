using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOverPanel : View
{
    private Text overScoreText;
    private Button resetGameBtn;
    private Button returnMainBtn;
    public override void Init()
    {
        overScoreText = transform.Find("OverText/OverScoreText").GetComponent<Text>();
        resetGameBtn = transform.Find("ResetGameBtn").GetComponent<Button>();
        returnMainBtn = transform.Find("ReturnMainBtn").GetComponent<Button>();
        resetGameBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.ResetGame();
        });
        returnMainBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.ReturnMain();
        });
    }
    public void ResultScore(int value)
    {
        overScoreText.text = value.ToString();
    }
}
