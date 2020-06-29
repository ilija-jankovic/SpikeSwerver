using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class MenuManager
{
    private static Canvas[] menus;
    [RuntimeInitializeOnLoadMethod]
    private static void Initialise()
    {
        menus = UnityEngine.Object.FindObjectsOfType<Canvas>();
        //reset game
        GameObject.Find("RestartButton").GetComponent<Button>().onClick.AddListener(() => { 
            GameManager.Reset();
            CloseAllMenus();
        });

        foreach (Canvas canvas in menus)
            canvas.enabled = false;
    }

    public static void OpenMenu(string name)
    {
        foreach (Canvas canvas in menus)
            if (canvas.name == name)
            {
                canvas.enabled = true;
                return;
            }
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
}
