using UnityEngine.Audio;
using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

	public static AudioManager Instance;

	public Sound[] sounds;

	void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = s.mixerGroup;
			s.lastVolume = s.volume;
		}
	}

	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}

	public void Stop(string sound) 
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		s.source.Stop();
	}

	public float GetVolume(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return -1;
		}
		return s.source.volume;
	}
	public float GetVolumeDefault(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return -1;
		}
		return s.volume;
	}

	public void SetVolume(string sound, float vol)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		s.source.volume=vol;
	}

	public void FadeIn(string sound, float dur)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		//if(s.lastVolume>s.source.volume)
			//return;

		s.source.pitch = s.pitch;
		StartCoroutine(Transition(s,s.source.volume,s.volume, dur));
	}
	public void FadeOut(string sound, float dur)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		//if(s.lastVolume<s.source.volume)
			//return;
		s.source.pitch = s.pitch;
		StartCoroutine(Transition(s,s.source.volume,0, dur));
	}

	IEnumerator Transition(Sound s, float initVol, float finalVol, float dur)
	{
		s.lastVolume = s.source.volume;
		s.source.volume = initVol;
		while (s.source.volume<finalVol && finalVol-initVol>0)
		{
			s.lastVolume = s.source.volume;
			s.source.volume += Time.deltaTime/dur;
			yield return new WaitForSecondsRealtime(Time.deltaTime);
		}
		while (s.source.volume>finalVol && finalVol-initVol<0)
		{
			s.lastVolume = s.source.volume;
			s.source.volume -= Time.deltaTime/dur;
			yield return new WaitForSecondsRealtime(Time.deltaTime);
		}
	}

	public void StopAllSounds()
	{
		
	}


}
