using UnityEngine;
using System.Collections;

public class TriChangeBgColor : ITrigger {

    public GameObject gobj;
    public Color toColor;

    public override void OnTri()
    {
        gobj.renderer.material.SetColor("_ColorA", toColor);
    }
}
