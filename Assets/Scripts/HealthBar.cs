using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : Entity
{
    List<GameObject> healthPoints = new List<GameObject>();
    Vector3[] healthPositions;
    protected override void Start()
    {
        base.Start();

        //fill healthPoints and sort
        Transform[] transforms = GetComponentsInChildren<Transform>();
        foreach (Transform t in transforms)
            if (t.gameObject != gameObject)
                healthPoints.Add(t.gameObject);
        healthPoints.Sort((x, y) => string.Compare(x.name, y.name));

        //store health positions
        healthPositions = new Vector3[healthPoints.Count];
        for (byte i = 0; i < healthPoints.Count; i++)
            healthPositions[i] = healthPoints[i].transform.position;
    }

    public void RemoveHealthPoints(byte amount)
    {
        RemoveHealthPoint(amount);
    }

    private void RemoveHealthPoint(byte step)
    {
        if (step == 0 || healthPoints.Count == 0)
            return;
        healthPoints[0].AddComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
        healthPoints.RemoveAt(0);
        RemoveHealthPoint((byte)(step - 1));
    }

    public void AddHealth()
    {
        if (Health >= 3 || Health <= 0)
            return;

        //position new health
        GameObject health = Instantiate(Resources.Load("Health") as GameObject);
        healthPoints.Insert(0, health);

        health.transform.SetParent(transform);
        Vector3 pos = healthPositions[healthPositions.Length - healthPoints.Count];
        health.transform.position = pos;

        //flash
        Flash f = health.GetComponent<Flash>();
        if (f)
            f.Go();
    }


    public byte Health
    {
        get { return (byte)healthPoints.Count; }
    }

    public byte MaxHealth
    {
        get { return (byte)healthPositions.Length; }
    }
}
