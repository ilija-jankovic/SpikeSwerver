using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : Entity
{
    List<GameObject> healthPoints = new List<GameObject>();
    protected override void Start()
    {
        base.Start();

        //fill healthPoints and sort
        Transform[] transforms = GetComponentsInChildren<Transform>();
        foreach (Transform t in transforms)
            healthPoints.Add(t.gameObject);
        healthPoints.Sort((x, y) => string.Compare(x.name, y.name));
    }

    public void RemoveHealthPoints(byte amount)
    {
        RemoveHealthPoint(amount);
    }

    private void RemoveHealthPoint(byte step)
    {
        if (step == 0 || healthPoints.Count == 0)
            return;
        healthPoints[0].AddComponent<Rigidbody>();
        healthPoints.RemoveAt(0);
        RemoveHealthPoint((byte)(step - 1));
    }
}
