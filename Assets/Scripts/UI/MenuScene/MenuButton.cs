using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.MenuScene
{
    public class MenuButton : MonoBehaviour, IPointerEnterHandler
    {
        public AudioClip hoverSound;
        public AudioClip clickSound;

        private AudioSource audioSource;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            audioSource.time = 0.01f;
            PlaySound(hoverSound);
        }

        public void PlayClickSound()
        {
            PlaySound(clickSound);
        }

        private void PlaySound(AudioClip clip)
        {
            if (audioSource != null && clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
        }
    }
}