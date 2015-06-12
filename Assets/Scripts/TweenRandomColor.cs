using UnityEngine;
using System.Collections;

/// <summary>
/// 随机颜色渐变
/// </summary>
public class TweenRandomColor : MonoBehaviour {

    public float idleTime;
    public float fadeTime;
	// Use this for initialization
	void Start () {
        StartCoroutine(Co());
	}

    IEnumerator Co() 
    {
        while (true)
        {
            yield return new WaitForSeconds(idleTime);
            Color nextColor = GetNextColor();
            iTween.ColorTo(gameObject, nextColor, fadeTime);
            yield return new WaitForSeconds(fadeTime);
        }
    }

    Color GetNextColor() 
    {
        return Tools.GetRandomColor();
    }
}
