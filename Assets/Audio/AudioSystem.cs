using FMOD.Studio;
using FMODUnity;
using SchizoQuest.Characters;
using UnityEngine;

namespace SchizoQuest.Audio
{
    public static class AudioSystem
    {
        public const float DEFAULT_VOLUME_MASTER = 1f;
        public const float DEFAULT_VOLUME_MUSIC = 0.69f;
        public const float DEFAULT_VOLUME_SFX = 1f;
        public static float SavedMasterVolume
        {
            get => PlayerPrefs.GetFloat("MasterVolume", DEFAULT_VOLUME_MASTER);
            set => PlayerPrefs.SetFloat("MasterVolume", value);
        }
        public static float SavedMusicVolume
        {
            get => PlayerPrefs.GetFloat("MusicVolume", DEFAULT_VOLUME_MUSIC);
            set => PlayerPrefs.SetFloat("MusicVolume", value);
        }
        public static float SavedSfxVolume
        {
            get => PlayerPrefs.GetFloat("SfxVolume", DEFAULT_VOLUME_SFX);
            set => PlayerPrefs.SetFloat("SfxVolume", value);
        }

        private static VCA masterVCA;
        private static VCA musicVCA;
        private static VCA sfxVCA;
        private static Bus sfxBus;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            if (!masterVCA.isValid())
            {
                masterVCA = RuntimeManager.GetVCA("vca:/master");
                masterVCA.setVolume(SavedMasterVolume);
            }

            if (!musicVCA.isValid())
            {
                musicVCA = RuntimeManager.GetVCA("vca:/music");
                musicVCA.setVolume(SavedMusicVolume);
            }

            if (!sfxVCA.isValid())
            {
                sfxVCA = RuntimeManager.GetVCA("vca:/sfx");
                sfxVCA.setVolume(SavedSfxVolume);
            }

            if (!sfxBus.isValid())
            {
                sfxBus = RuntimeManager.GetBus("bus:/master/sfx");
            }
        }

        public static void SetMasterVolume(float volume)
        {
            Initialize();
            masterVCA.setVolume(volume);
            SavedMasterVolume = volume;
        }

        public static void SetMusicVolume(float volume)
        {
            Initialize();
            musicVCA.setVolume(volume);
            SavedMusicVolume = volume;
        }

        public static void SetSfxVolume(float volume)
        {
            Initialize();
            sfxVCA.setVolume(volume);
            SavedSfxVolume = volume;
        }

        public static void PauseSfx(bool paused)
        {
            Initialize();
            sfxBus.setPaused(paused);
        }

        public static void ResetVolume()
        {
            SetMasterVolume(DEFAULT_VOLUME_MASTER);
            SetMusicVolume(DEFAULT_VOLUME_MUSIC);
            SetSfxVolume(DEFAULT_VOLUME_SFX);
        }

        public static void SetCharacter(PlayerType character)
        {
            RuntimeManager.StudioSystem.setParameterByName("Character", character.ValueIndex());
        }
    }
}
