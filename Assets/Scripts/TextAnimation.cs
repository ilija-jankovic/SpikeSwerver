using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TMPro.TextMeshPro))]
public class TextAnimation : MonoBehaviour
{
    bool _active;

    TMPro.TextMeshPro textComp;
    string target, curText = "";

    const float TIME_BETWEEN_LETTERS = 0.15f;
    float time;

    void Awake()
    {
        textComp = GetComponent<TMPro.TextMeshPro>();
    }

    void Update()
    {
        textComp.text = GameManager.Points + "/" + GameManager.POINTS_TO_ADVANCE_LEVEL[GameManager.Level - 1] + "\n" + curText;
        if (!_active)
            return;
        if (time >= TIME_BETWEEN_LETTERS * 10)
        {
            time = 0;
            _active = false;
            return;
        }

        time += Time.deltaTime;

        if (target.Length == curText.Length)
            return;
        if (time >= TIME_BETWEEN_LETTERS)
        {
            time = 0;
            curText = target.Substring(0, curText.Length + 1);
        }
    }

    public void Write(string text)
    {
        _active = true;
        target = text;
        curText = "";
    }

    public bool Active
    {
        get { return _active; }
    }
}
