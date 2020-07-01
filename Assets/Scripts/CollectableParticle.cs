using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableParticle : Entity
{
    [SerializeField]
    public byte type;
    float speed;
    protected override void Start()
    {
        base.Start();
        speed = Random.Range(0.2f, 0.8f);
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        if (!Collected)
        {
            transform.RotateAround(transform.parent.position, Vector3.up, Time.deltaTime * speed*450f);
            return;
        }
        switch (type)
        {
            case (byte)Collectable.Types.points:
                Vector3 pos = transform.position;
                Vector3 pointsTextPos = GameManager.PointsText.transform.position;

                //move towards points text
                transform.position = Vector3.MoveTowards(pos, pointsTextPos, speed);

                //check if close enough to points text
                if (Vector3.Distance(pos, pointsTextPos) < 0.1f)
                {
                    //add points if player still alive
                    if (GameManager.Player)
                        GameManager.AddPoints(1);
                    Destroy(gameObject);
                }
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }

    public void Collect()
    {
        transform.SetParent(null);
    }

    bool Collected
    {
        get { return transform.parent == null; }
    }
}
