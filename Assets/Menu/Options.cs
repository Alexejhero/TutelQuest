using SchizoQuest.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace SchizoQuest.Menu
{
    public class Options : MonoBehaviour
    {
        public Slider masterVolumeSlider;
        public Slider musicVolumeSlider;
        public Slider sfxVolumeSlider;

        public Button backButton;

        public void Start()
        {
            masterVolumeSlider.value = AudioSystem.SavedMasterVolume;
            musicVolumeSlider.value = AudioSystem.SavedMusicVolume;
            sfxVolumeSlider.value = AudioSystem.SavedSfxVolume;

            masterVolumeSlider.onValueChanged.AddListener(AudioSystem.SetMasterVolume);
            musicVolumeSlider.onValueChanged.AddListener(AudioSystem.SetMusicVolume);
            sfxVolumeSlider.onValueChanged.AddListener(AudioSystem.SetSfxVolume);
        }

        public void OnCancel()
        {
            backButton.onClick.Invoke();
        }
    }
}
