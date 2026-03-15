using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace UI.GameScene
{
    public class SlotMachineUI : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private TextMeshProUGUI resultText;

        [Header("Audio Clips")]
        public AudioClip leverSound;
        public AudioClip slotSound;
        public AudioClip stopSound;

        [SerializeField] private float rollingDuration = 1.0f;

        private RectTransform rect;
        private AudioSource audioSource;

        private string animationStateName = "SlotMachine";

        void Awake()
        {
            rect = GetComponent<RectTransform>();
            audioSource = GetComponent<AudioSource>();
        }

        public void Show()
        {
            gameObject.SetActive(true);

            rect.anchoredPosition = new Vector2(0, 0);
            rect.sizeDelta = new Vector2(70, 70);

            resultText.text = "";
            resultText.fontSize = 20;
            resultText.GetComponent<RectTransform>().anchoredPosition = new Vector2(28, 3);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void PlaySlot(int targetScore, Action onComplete = null)
        {
            Show();
            StartCoroutine(SlotRoutine(targetScore, onComplete));
        }

        private IEnumerator SlotRoutine(int score, Action onComplete)
        {
            animator.Play(animationStateName, 0, 0f);

            yield return null;

            audioSource.clip = slotSound;
            audioSource.loop = true;
            audioSource.Play();

            float animLength = animator.GetCurrentAnimatorStateInfo(0).length;

            if (leverSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(leverSound);
            }

            yield return new WaitForSeconds(animLength);

            float endTime = Time.time + rollingDuration;
            while (Time.time < endTime)
            {
                resultText.text = UnityEngine.Random.Range(0, 20).ToString();

                yield return new WaitForSeconds(0.05f);
            }

            audioSource.Stop();

            resultText.text = score.ToString();
            onComplete?.Invoke();

            yield return new WaitForSeconds(0.5f);

            SetTargetScore();
        }

        public void SetTargetScore()
        {
            rect.sizeDelta = new Vector2(25, 25);
            rect.anchoredPosition = new Vector2(600, 30);

            resultText.fontSize = 8;
            resultText.GetComponent<RectTransform>().anchoredPosition = new Vector2(28, -2.3f);
        }
    }
}

