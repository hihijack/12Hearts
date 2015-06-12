using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

    public GameObject gobj;
	// Use this for initialization
	void Start () {
        Animator anim = gobj.AddComponent<Animator>();
        RuntimeAnimatorController rac = Resources.Load("Cube") as RuntimeAnimatorController;
        anim.runtimeAnimatorController = rac;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
