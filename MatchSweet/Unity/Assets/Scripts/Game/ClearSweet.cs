using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearSweet : MonoBehaviour
{
    public AnimationClip clearAnimation; //清除动画

    protected GameSweet sweet;

    private bool isClearing;

    public bool IsClearing { get => isClearing;}
    /// <summary>
    /// 消除甜品
    /// </summary>
    public virtual void Clear()
    {
        isClearing = true;
        StartCoroutine(IClearCoroutine());

    }
    /// <summary>
    /// 播放清除动画、声音、以及移除方块
    /// </summary>
    /// <returns></returns>
    public IEnumerator IClearCoroutine()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play(clearAnimation.name);
            //等分+1，播放清除声音
            yield return new WaitForSeconds(clearAnimation.length);
            Destroy(gameObject);
            isClearing=false;
        }
    }
}
