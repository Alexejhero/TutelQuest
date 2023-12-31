using SchizoQuest.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace SchizoQuest.Menu
{
    public class OptionsBehaviour : MonoBehaviour
    {
        public Slider masterVolumeSlider;
        public Slider musicVolumeSlider;
        public Slider sfxVolumeSlider;

        public void Start()
        {
            masterVolumeSlider.value = AudioSystem.SavedMasterVolume;
            musicVolumeSlider.value = AudioSystem.SavedMusicVolume;
            sfxVolumeSlider.value = AudioSystem.SavedSfxVolume;

            masterVolumeSlider.onValueChanged.AddListener(AudioSystem.SetMasterVolume);
            musicVolumeSlider.onValueChanged.AddListener(AudioSystem.SetMusicVolume);
            sfxVolumeSlider.onValueChanged.AddListener(AudioSystem.SetSfxVolume);
        }
    }
}
