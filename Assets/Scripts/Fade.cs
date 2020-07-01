using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : ColourTransition
{
    public void Go(Color toColour)
    {
        normal = toColour;
        otherColour = mat.color;
        tTrans = 1f;
    }
}
