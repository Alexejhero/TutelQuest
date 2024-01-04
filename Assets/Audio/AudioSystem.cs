using FMOD.Studio;
using FMODUnity;
using SchizoQuest.Characters;
using SchizoQuest.Game;
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
        public static float SavedVoiceVolume
        {
            get => PlayerPrefs.GetFloat("VoiceVolume", 1f);
            set => PlayerPrefs.SetFloat("VoiceVolume", value);
        }

        private static VCA masterVCA;
        private static VCA musicVCA;
        private static VCA sfxVCA;
        private static VCA voiceVCA;

        private static void Initialize()
        {
            if (!masterVCA.isValid()) masterVCA = RuntimeManager.GetVCA("vca:/master");
            if (!musicVCA.isValid()) musicVCA = RuntimeManager.GetVCA("vca:/music");
            if (!sfxVCA.isValid()) sfxVCA = RuntimeManager.GetVCA("vca:/sfx");
            if (!voiceVCA.isValid()) voiceVCA = RuntimeManager.GetVCA("vca:/voice");
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

        public static void SetVoiceVolume(float volume)
        {
            Initialize();
            voiceVCA.setVolume(volume);
            SavedVoiceVolume = volume;
        }

        public static void UpdateSfxMuteWhileSwitchingCharacters(Player currentPlayer)
        {
            var dist = CameraController.DistanceToActivePlayer;
            RuntimeManager.StudioSystem.setParameterByName("Distance to Active Character", dist);
        }
    }
}
