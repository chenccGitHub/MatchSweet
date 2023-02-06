using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainPanel : View
{
    private Button startBtn;
    private Button QuitGameBtn;
    public override void Init()
    {
        startBtn = transform.Find("Start_Button").GetComponent<Button>();
        QuitGameBtn = transform.Find("Quit_Button").GetComponent<Button>();
        startBtn.onClick.AddListener(StartNewGame);
        QuitGameBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
    public void StartNewGame()
    {
        gameObject.SetActive(false);
        UIManager.Show<UISelectPanel>();
    }
}
