using UnityEngine;
using System.Collections;

public class TweenSpriteAlpha : UITweener {

#if UNITY_3_5
	public float from = 1f;
	public float to = 1f;
#else
    [Range(0f, 1f)]
    public float from = 1f;
    [Range(0f, 1f)]
    public float to = 1f;
#endif

    SpriteRenderer mSpriteRender;

    public SpriteRenderer _SpriterRender
    {
        get
        {
            if (mSpriteRender == null)
            {
                mSpriteRender = GetComponent<SpriteRenderer>();
            }
            return mSpriteRender;
        }
    }

    [System.Obsolete("Use 'value' instead")]
    public float alpha { get { return this.value; } set { this.value = value; } }

    /// <summary>
    /// Tween's current value.
    /// </summary>

    public float value 
    { get { return _SpriterRender.color.a; } 
      set 
      { 
          _SpriterRender.color = new Color(_SpriterRender.color.r, _SpriterRender.color.g, _SpriterRender.color.b, value); 
      } 
    }

    /// <summary>
    /// Tween the value.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished) { value = Mathf.Lerp(from, to, factor); }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>

    static public TweenSpriteAlpha Begin(GameObject go, float duration, float alpha)
    {
        TweenSpriteAlpha comp = UITweener.Begin<TweenSpriteAlpha>(go, duration);
        comp.from = comp.value;
        comp.to = alpha;

        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }
        return comp;
    }

    public override void SetStartToCurrentValue() { from = value; }
    public override void SetEndToCurrentValue() { to = value; }
}
