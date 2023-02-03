using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSweet : MonoBehaviour
{
    private GameSweet gameSweet;
    private IEnumerator moveCoroutine;
    private void Awake()
    {
        gameSweet = GetComponent<GameSweet>();
    }
    public void Move(int newX,int newY,float fillTime)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        StartCoroutine(IMoveCoroutine(newX,newY,fillTime));
    }
    public IEnumerator IMoveCoroutine(int newX, int newY, float fillTime)
    {
        gameSweet.X = newX;
        gameSweet.Y = newY;
        Vector3 startPos = transform.position;
        Vector3 endPos = gameSweet.gameManager.CoreectPosition(newX, newY);
        for (float t = 0; t < fillTime; t += Time.deltaTime)
        {
            gameSweet.transform.position = Vector3.Lerp(startPos, endPos, t / fillTime);
            yield return null;
        }
        gameSweet.transform.position = endPos;
    }
}
