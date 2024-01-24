using PowerTools;
using UnityEngine;

namespace SchizoQuest.VFX
{
    public class DoorAnimator : MonoBehaviour
    {
        public AnimationClip doorOpen;
        public SpriteAnim animator;
        public bool IsOpen { private set; get; } = false;

        public void PlayOpen()
        {
            animator.Play(doorOpen);
        }

        public void DoorOpened() => IsOpen = true;
    }
}
