using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class MenuManager
{
    private static Canvas[] menus;
    private static Button _restart;

    public static bool helpDisplayDisabled;

    [RuntimeInitializeOnLoadMethod]
    private static void Initialise()
    {
        menus = UnityEngine.Object.FindObjectsOfType<Canvas>();
        _restart = GameObject.Find("RestartButton").GetComponent<Button>();

        //reset game
        Restart.onClick.AddListener(() => { 
            GameManager.Reset();
            CloseAllMenus();
        });

        foreach (Canvas canvas in menus)
            canvas.enabled = false;
    }

    public static Canvas FindMenu(string name)
    {
        foreach (Canvas canvas in menus)
            if (canvas.name == name)
                return canvas;
        return null;
    }

    public static void OpenMenu(string name)
    {
        FindMenu(name).enabled = true;
    }

    public static void FadeInMenu(string name)
    {
        FadeUI f = FindMenu(name).GetComponent<FadeUI>();
        if (f)
            f.FadeIn();
    }

    public static void FadeOutMenu(string name)
    {
        FadeUI f = FindMenu(name).GetComponent<FadeUI>();
        if (f)
            f.FadeOut();
    }

    public static void CloseAllMenus()
    {
        SwitchToMenu(null);
    }

    public static void SwitchToMenu(string name)
    {
        foreach (Canvas canvas in menus)
            canvas.enabled = canvas.name == name ? true : false;
    }

    public static Button Restart
    {
        get { return _restart; }
    }
}
