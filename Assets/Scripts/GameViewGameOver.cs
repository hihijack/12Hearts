using UnityEngine;
using System.Collections;

public class GameViewGameOver : MonoBehaviour {

    public AudioClip bgm;
    // Use this for initialization
	void Start () {
        AudioManager._Instance.PlaySound(bgm, true, "bgm");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
