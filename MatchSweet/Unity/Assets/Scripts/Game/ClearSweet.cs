using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearSweet : MonoBehaviour
{
    public AnimationClip clearAnimation; //�������

    protected GameSweet sweet;

    private bool isClearing;

    public bool IsClearing { get => isClearing;}
    /// <summary>
    /// ������Ʒ
    /// </summary>
    public virtual void Clear()
    {
        isClearing = true;
        StartCoroutine(IClearCoroutine());

    }
    /// <summary>
    /// ��������������������Լ��Ƴ�����
    /// </summary>
    /// <returns></returns>
    public IEnumerator IClearCoroutine()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play(clearAnimation.name);
            //�ȷ�+1�������������
            yield return new WaitForSeconds(clearAnimation.length);
            Destroy(gameObject);
            isClearing=false;
        }
    }
}
