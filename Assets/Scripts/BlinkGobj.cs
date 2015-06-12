using UnityEngine;
using System.Collections;

public class BlinkGobj : MonoBehaviour {

    public float showDur = 0.8f;
    public float hideDur = 0.2f;

    public bool random = false;
    public bool auto = false;

    bool blinking = false;

    void Start() 
    {
        if (auto)
        {
            StartBlink();
        }
    }

    public void StartBlink() 
    {
        blinking = true;
        StartCoroutine(CoBlink());
    }

    public void StopBlink() 
    {
        blinking = false;
    }

    IEnumerator CoBlink() 
    {
        while (true)
        {
            if (!blinking)
            {
                break;
            }
            renderer.enabled = true;
            if (random)
            {
                RandomShowTime();
            }
            yield return new WaitForSeconds(showDur);
            if (blinking)
            {
                renderer.enabled = false;
            }
            else
            {
                break;
            }

            if (random)
            {
                RandomHideTime();
            }
            yield return new WaitForSeconds(hideDur);

            if (blinking)
            {
                renderer.enabled = true;
            }
            else
            {
                renderer.enabled = true;
                break;
            }
        }
    }

    void RandomShowTime()
    {
        float oriVal = showDur;
        showDur += UnityEngine.Random.Range(-0.5f, 0.5f);
        if (showDur <= 0)
        {
            showDur = oriVal;
        }
    }

    void RandomHideTime() 
    {
        float oriVal = hideDur;
        hideDur += UnityEngine.Random.Range(-0.1f, 0.3f);
        if (hideDur <= 0)
        {
            hideDur = oriVal;
        }
    }
}
