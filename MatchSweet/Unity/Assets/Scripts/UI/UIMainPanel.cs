using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainPanel : View
{
    private Button startBtn;
    public override void Init()
    {
        startBtn = transform.Find("Start_Button").GetComponent<Button>();
        startBtn.onClick.AddListener(StartNewGame);
    }
    public void StartNewGame()
    {
        gameObject.SetActive(false);
        UIManager.Show<UIGamePanel>();
        FindObjectOfType<GameManager>().enabled = true;
    }
}
