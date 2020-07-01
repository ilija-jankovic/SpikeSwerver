
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScorePanel : MonoBehaviour
{
    TMPro.TextMeshPro highScoreText;

    const float TARGET_HEIGHT = -2f;
    float startHeight;

    [SerializeField]
    float speed;

    bool moving;

    void Awake()
    {
        startHeight = transform.position.y;
        highScoreText = GameObject.Find("HighScore").GetComponent<TMPro.TextMeshPro>();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        moving = true;
        highScoreText.text = "Highscore: \n" + GameManager.HighScore;
    }

    void Update()
    {
        if (moving)
        {
            Vector3 pos = transform.position;
            Vector3 target = new Vector3(pos.x, TARGET_HEIGHT, pos.z);
            transform.position = Vector3.MoveTowards(pos, target, speed);
            if(Vector3.Distance(pos, target) < 0.1f)
            {
                moving = false;
            }
        }
    }

    public void Reset()
    {
        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, startHeight, pos.z);
        gameObject.SetActive(false);
    }
}
