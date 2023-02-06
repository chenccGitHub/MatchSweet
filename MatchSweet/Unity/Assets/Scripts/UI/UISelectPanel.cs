using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelectPanel : View
{
    private Button closeBtn;
    private Transform grid;
    public override void Init()
    {
        Button[] levelBtn;
        grid = transform.Find("Grid");
        levelBtn = grid.GetComponentsInChildren<Button>();
        for (int i = 0; i < levelBtn.Length; i++)
        {
            int index = i;
            levelBtn[index].onClick.AddListener(() =>
            {
                if (index + 1 == int.Parse(levelBtn[index].transform.Find("Text").GetComponent<Text>().text))
                {
                    GameManager.Instance.ShowSweetsGrid(true);
                    FindObjectOfType<GameManager>().enabled = true;
                    gameObject.SetActive(false);
                    UIManager.Show<UIGamePanel>();
                    GameManager.Instance.ResetGame(index + 1);
                }
            });
        }
        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();
        closeBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.ReturnMain();
        });

    }
}
