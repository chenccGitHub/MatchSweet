using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameVictoryPanel : View
{
    private Text victoryText;
    private Button resetGameBtn;
    private Button returnMainBtn;
    private Button againGameBtn;
    private Image star1;
    private Image star2;
    private Image star3;
    public override void Init()
    {
        victoryText = transform.Find("VictoryText/OverScoreText").GetComponent<Text>();
        resetGameBtn = transform.Find("ResetGameBtn").GetComponent<Button>();
        returnMainBtn = transform.Find("ReturnMainBtn").GetComponent<Button>();
        againGameBtn = transform.Find("AgainGameBtn").GetComponent<Button>();
        star1 = transform.Find("Star/star1").GetComponent<Image>();
        star2 = transform.Find("Star/star2").GetComponent<Image>();
        star3 = transform.Find("Star/star3").GetComponent<Image>();
        resetGameBtn.onClick.AddListener(() =>
        {
            UIManager.GetView<UIGamePanel>().ResetGame();
        });
        returnMainBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.ReturnMain();
        });
        againGameBtn.onClick.AddListener(() =>
        {
            //ÏÂÒ»¹Ø
            GameManager.Instance.AgainGame();
        });
    }
    public void ResultScore(int value)
    {
        victoryText.text = value.ToString();
    }
    public void SetStarCount(float starNum)
    {
        switch (starNum)
        {
            case 1:
                star1.color = Color.yellow;
                star2.color = Color.white;
                star3.color = Color.white;
                break;
            case 2:
                star1.color = Color.yellow;
                star2.color = Color.yellow;
                star3.color = Color.white;
                break;
            case 3:
                star1.color = Color.yellow;
                star2.color = Color.yellow;
                star3.color = Color.yellow;
                break;
            default:
                break;
        }
    }
}
