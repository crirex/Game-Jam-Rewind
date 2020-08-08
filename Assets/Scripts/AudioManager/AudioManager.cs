using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;

	public AudioMixerGroup mixerGroup;

	public Sound[] sounds;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.outputAudioMixerGroup = mixerGroup;
		}
	}

	void Start()
	{
		//Play("Background");
		//Play("Click");
		//Play("Intro");
	}

	public void Play(string sound) //Use for music loops or single sounds
	{

		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + sound + " not found!");
			return;
        }
		s.source.Play();
	}

	public void Stop(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + sound + " not found!");
			return;
		}

		s.source.Stop();
	}

	public void PauseAll()
	{
		foreach (Sound s in sounds)
		{
			s.source.Pause();
		}
	}

	public void UnPauseAll()
	{
		foreach(Sound s in sounds)
		{
			s.source.UnPause();
		}
	}

	public AudioSource GetSound(string sound) //Use only for spaghetty code
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + sound + " not found!");
			return null;
		}

		return s.source;
	}

	public Sound GetObjectSound(string sound) //Use only for spaghetty code
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + sound + " not found!");
			return null;
		}

		return s;
	}

}
