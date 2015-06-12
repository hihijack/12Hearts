using UnityEngine;
using System.Collections;

/// <summary>
/// 触发时淡出
/// </summary>
public class TriFadeOut : ITrigger {

    public float fadeTime;

    public override void OnTri()
    {
        TweenSpriteAlpha.Begin(gameObject, fadeTime, 0f);
    }
}
