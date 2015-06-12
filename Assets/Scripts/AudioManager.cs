using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
//	public static AudioManager Init(){
//		GameObject gobj = new GameObject();
//		gobj.name = "AudioManager";
//		return gobj.AddComponent<AudioManager>();
//		DontDestroyOnLoad(gobj);
//	}

    private static AudioManager instance;

    public static AudioManager _Instance
    {
        get 
        {
            if (AudioManager.instance == null)
            {
                GameObject gobj = new GameObject();
                AudioManager.instance = gobj.AddComponent<AudioManager>();
                DontDestroyOnLoad(gobj);
            }
            return AudioManager.instance; 
        }
    }

	public  void PlaySound(string path, bool loop = false, string name = "AudioSource"){
		AudioClip ac = Resources.Load(path) as AudioClip;
		PlaySound(ac, loop, name);
	}
	public  GameObject PlaySound(AudioClip ac, bool loop = false, string name = "AudioSource"){
		if(ac == null){
			Debug.LogWarning("Play Sound AC Is Null");
			return null;
		}
		GameObject gobjSound = new GameObject();
		gobjSound.name = name;
		AudioSource audioS = gobjSound.AddComponent<AudioSource>();
		audioS.clip = ac;
		audioS.loop = loop;

		if((loop && GameManager.enableMusic) || (!loop && GameManager.enableSound)){
			audioS.Play();
		}

		if(!loop){
			Destroy (gobjSound, ac.length);
		}

		return gobjSound;
	}

	public  void StopSound(string name){
		GameObject gobjSound = GameObject.Find(name);
		if(gobjSound != null){
			AudioSource audio = gobjSound.GetComponent<AudioSource>();
			audio.Stop();
		}
	}

    public void PauseSound(string name) 
    {
        GameObject gobjSound = GameObject.Find(name);
        if (gobjSound != null)
        {
            AudioSource audio = gobjSound.GetComponent<AudioSource>();
            audio.Pause();
        }
    }

	public  bool ContineSound(string name){
		bool success = false;
		GameObject gobjSound = GameObject.Find(name);
		if(gobjSound != null){
			AudioSource audio = gobjSound.GetComponent<AudioSource>();
			if(!audio.isPlaying){
				audio.Play();
			}
			success = true;
		}

		return success;
	}

    /// <summary>
    /// 在durTime内淡出音乐
    /// </summary>
    /// <param name="name"></param>
    /// <param name="durTime"></param>
    public  void FadeOutSound(string name, float durTime) 
    {
        GameObject gobjSound = GameObject.Find(name);
        if (gobjSound != null)
        {
            AudioSource audio = gobjSound.GetComponent<AudioSource>();
            if (audio.isPlaying)
            {
                TweenVolume tv = gobjSound.AddComponent<TweenVolume>();
                tv.from = audio.volume;
                tv.to = 0f;
                tv.duration = durTime;
                tv.PlayForward();
            }
        }
    }

    
}
