using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Schizo.Audio;

public class AudioExample : MonoBehaviour
{

	[SerializeField]
	private AudioClip[] clips;

    // This script is only for example purposes and consists of a crime against humanity
    void Start()
    {
		AudioSystem.Instance.Play(clips[0], ClipType.Music);
		StartCoroutine(TimedClips());
	}

	// Play 3 sfx
	private IEnumerator TimedClips()
	{
		yield return new WaitForSeconds(0.5f);
		AudioSystem.Instance.Play(clips[1], ClipType.Sfx);

		yield return new WaitForSeconds(0.5f);
		AudioSystem.Instance.Play(clips[2], ClipType.Sfx);

		yield return new WaitForSeconds(0.5f);
		AudioSystem.Instance.Play(clips[3], ClipType.Sfx);
	}

}
