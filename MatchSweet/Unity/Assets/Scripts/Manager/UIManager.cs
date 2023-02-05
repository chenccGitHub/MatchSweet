using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    public UIManager()
    {
        Initilization();
    }
    private static void Initilization()
    {
        foreach (var view in UIRoot.m_views)
        {
            view.Value.Init();
            view.Value.Hide();
        }
        Show<UIMainPanel>();
    }
    public static T GetView<T>() where T : View
    {
        foreach (var view in UIRoot.m_views)
        {
            if (view.Value is T tView)
            {
                return tView;
            }
        }
        return null;
    }
    public static void Show<T>() where T : View
    {
        foreach (var view in UIRoot.m_views)
        {
            if (view.Value is T)
            {
                view.Value.Show();
            }
        }
    }
    public static void Close<T>() where T : View
    {
        foreach (var view in UIRoot.m_views)
        {
            if (view.Value is T)
            {
                view.Value.Hide();
            }
        }
    }
}
