using UnityEngine;
using System.Collections;

public class GobjLife : MonoBehaviour {

    public float lifeTime;
	// Use this for initialization
	void Start () {
        if (lifeTime > 0)
        {
            StartCoroutine(CoTime());
        }
	}

    IEnumerator CoTime()
    {
        yield return new WaitForSeconds(lifeTime);
        DestroyObject(gameObject);
    }
}
