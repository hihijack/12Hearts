using UnityEngine;
using System.Collections;

public class UIDestroyOnEnd : MonoBehaviour {
	void OnAnimEnd(){
		DestroyObject(gameObject);
	}
	
}
