using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace SchizoQuest.Audio
{
    public static class AudioSystem
    {
        public static float SavedMasterVolume
        {
            get => PlayerPrefs.GetFloat("MasterVolume", 1f);
            set => PlayerPrefs.SetFloat("MasterVolume", value);
        }
        public static float SavedMusicVolume
        {
            get => PlayerPrefs.GetFloat("MusicVolume", 1f);
            set => PlayerPrefs.SetFloat("MusicVolume", value);
        }
        public static float SavedSfxVolume
        {
            get => PlayerPrefs.GetFloat("SfxVolume", 1f);
            set => PlayerPrefs.SetFloat("SfxVolume", value);
        }

        private static VCA masterVCA;
        private static VCA musicVCA;
        private static VCA sfxVCA;

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
    }
}
