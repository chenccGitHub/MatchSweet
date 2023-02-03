using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRoot : MonoBehaviour
{
    public static Dictionary<string, View> m_views = new Dictionary<string, View>(); //����һ���ֵ�洢����UI����
    private void Awake()
    {
        var prefabs = Resources.LoadAll<Transform>("UIPrefab");//����UIPrefab�ļ����µ�����UIԤ����
        foreach (Transform view in prefabs)
        {
            if (!m_views.ContainsKey(view.name))
            {
                Transform prefab = Instantiate(view, transform);
                prefab.name = prefab.name.Replace("(Clone)", "");//ȥ��ʵ���������(Clone)��׺����Ԥ��������ͳһ
                m_views.Add(prefab.name, prefab.GetComponent<View>());//�����ֵ�
            }
        }
        new UIManager();
    }
}
