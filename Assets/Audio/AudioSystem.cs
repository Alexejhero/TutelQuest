using UnityEngine;

namespace SchizoQuest.Audio
{
	/// <summary>
	/// Type of audio clip, corresponds to the track being used
	/// Sfx exclusively can handle multiple sounds at once through its own set of tracks
	/// </summary>
	public enum ClipType { Sfx, Voice, Background, Music };
	public class AudioSystem : MonoBehaviour
	{
		private static AudioSystem instance;
		public static AudioSystem Instance => instance;

		/// <summary>
		/// Count of main audio tracks (Sound effects)
		/// </summary>
		[SerializeField]
		private int sfxTrackCount = 5;

		[SerializeField]
		[Range(0f, 1.0f)]
		private float masterVolume = 1.0f;

		[SerializeField]
		[Range(0f, 1.0f)]
		private float sfxVolume = 0.8f;

		[SerializeField]
		[Range(0f, 1.0f)]
		private float voiceVolume = 1.0f;

		[SerializeField]
		[Range(0f, 1.0f)]
		private float backgroundVolume = 0.6f;

		[SerializeField]
		[Range(0f, 1.0f)]
		private float musicVolume = 0.75f;

		/// <summary>
		/// Main tracks array, used for sound effects
		/// </summary>
		private AudioSource[] sfxTracks;
		/// <summary>
		/// Lead audio track, used for prompts and voice overs
		/// </summary>
		private AudioSource voiceTrack;
		/// <summary>
		/// Sub track used for background ambient noise
		/// </summary>
		private AudioSource backgroundTrack;
		/// <summary>
		/// Music track
		/// </summary>
		private AudioSource musicTrack;

		private int currentSfx = 0;

		private void Awake()
		{
			DontDestroyOnLoad(this.gameObject);

			if (instance != null && instance != this)
			{
				Destroy(this);
			}
			else
			{
				instance = this;
			}

			CreateSources();
		}

		private void CreateSources()
		{
			sfxTracks = new AudioSource[sfxTrackCount];

			for (int i = 0; i < sfxTrackCount; i++)
			{
				AudioSource audioSource = gameObject.AddComponent<AudioSource>();
				sfxTracks[i] = audioSource;
			}

			voiceTrack = gameObject.AddComponent<AudioSource>();
			backgroundTrack = gameObject.AddComponent<AudioSource>();
			musicTrack = gameObject.AddComponent<AudioSource>();

			ResetVolumes();
		}

		/// <summary>
		/// Call to reload volume values
		/// To be called after volume value changes
		/// </summary>
		public void ResetVolumes()
		{
			for (int i = 0; i < sfxTrackCount; i++)
			{
				SetupSource(sfxTracks[i], sfxVolume);
			}

			SetupSource(voiceTrack, voiceVolume);
			SetupSource(backgroundTrack, backgroundVolume);
			SetupSource(musicTrack, musicVolume, true);
		}


		/// <summary>
		/// Play a sound effect; loops through the entirety of available tracks and cycles back to the first track when it runs out
		/// Will allow to play N sound effects at a time <see cref="sfxTrackCount"/>
		/// </summary>
		/// <param name="clip">Audio clip to play</param>
		private void PlaySfx(AudioClip clip)
		{
			sfxTracks[currentSfx].Stop(); // Kill whatever this track was playing
			sfxTracks[currentSfx].clip = clip;
			sfxTracks[currentSfx].Play();

			currentSfx++;
			if (currentSfx > sfxTrackCount) { currentSfx = 0; }
		}

		/// <summary>
		/// Play a clip from the main audio sources
		/// Depending on clip type, the corresponding volume/track will be respected
		/// Sfx can play with a certain amount of overlap
		/// </summary>
		/// <param name="clip">Clip to play</param>
		/// <param name="at">Type of clip <see cref="ClipType"/></param>
		public void Play(AudioClip clip, ClipType at)
		{
			switch (at)
			{
				case ClipType.Sfx:
					PlaySfx(clip);
					return;
				case ClipType.Voice:
					PlaySource(voiceTrack, clip);
					break;
				case ClipType.Background:
					PlaySource(backgroundTrack, clip);
					break;
				case ClipType.Music:
					PlaySource(musicTrack, clip);
					break;
			}
		}

		/// <summary>
		/// Play a clip from a custom audio source
		/// Good for when you want to play audio from your own audio source but want to respect volume settings
		/// </summary>
		/// <param name="source">Source to play from</param>
		/// <param name="clip">Clip to play</param>
		/// <param name="at">Audio type <see cref="ClipType"/></param>
		public void PlayCustom(AudioSource source, AudioClip clip, ClipType at)
		{
			switch (at)
			{
				case ClipType.Sfx:
					SetupSource(source, sfxVolume, false, false);
					break;
				case ClipType.Voice:
					SetupSource(source, voiceVolume, false, false);
					break;
				case ClipType.Background:
					SetupSource(source, backgroundVolume, false, false);
					break;
				case ClipType.Music:
					SetupSource(source, musicVolume, false, false);
					break;
			}
			PlaySource(source, clip);
		}

		/// <summary>
		/// Get the volume for a given clip type, or the master volume
		/// </summary>
		/// <param name="clipType">Optional, Type of clip <see cref="ClipType"/> you want volume for, pass nothing for master volume</param>
		/// <returns></returns>
		public float GetVolume(ClipType? clipType = null)
		{
			float rv = masterVolume;
			switch (clipType)
			{
				case ClipType.Sfx:
					rv = sfxVolume;
					break;
				case ClipType.Voice:
					rv = voiceVolume;
					break;
				case ClipType.Background:
					rv = backgroundVolume;
					break;
				case ClipType.Music:
					rv = musicVolume;
					break;
			}
			return rv;
		}

		/// <summary>
		/// Set the volume of a given clip type or set the master volume
		/// </summary>
		/// <param name="volume">New volume to set</param>
		/// <param name="clipType">Optional, Type of clip <see cref="ClipType"/> to set volume for, none for master volume</param>
		public void SetVolume(float volume, ClipType? clipType = null)
		{
			switch (clipType)
			{
				default:
					masterVolume = volume;
					break;
				case ClipType.Sfx:
					sfxVolume = volume;
					break;
				case ClipType.Voice:
					voiceVolume = volume;
					break;
				case ClipType.Background:
					backgroundVolume = volume;
					break;
				case ClipType.Music:
					musicVolume = volume;
					break;
			}

			ResetVolumes();
		}

		/// <summary>
		/// Warpper that stops an audio source, changes its current clip and plays
		/// </summary>
		/// <param name="source">Source to play from</param>
		/// <param name="clip">Clip to play</param>
		private void PlaySource(AudioSource source, AudioClip clip)
		{
			source.Stop();
			source.clip = clip;
			source.Play();
		}

		/// <summary>
		/// Compounds a desired volume by the value of <see cref="masterVolume"/>
		/// </summary>
		/// <param name="audioSource">Source to affect</param>
		/// <param name="volume">Value of this source's volume</param>
		/// <param name="loop"></param>
		/// <param name="force2d"></param>
		private void SetupSource(AudioSource audioSource, float volume, bool loop = false, bool force2d=true)
		{
			audioSource.volume = volume * masterVolume;
			if (force2d) audioSource.spatialBlend = 0.0f;
			audioSource.loop = loop;
		}

	}
}
