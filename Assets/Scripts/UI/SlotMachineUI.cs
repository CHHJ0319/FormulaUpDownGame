using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace UI {
    public class SlotMachineUI : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private TextMeshProUGUI resultText;

        [SerializeField] private string animationStateName = "Slotmachine";
        [SerializeField] private float rollingDuration = 1.0f;

        public void Show()
        {
            resultText.text = "";

            gameObject.SetActive(true);
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
            float animLength = animator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(animLength);

            float endTime = Time.time + rollingDuration;
            while (Time.time < endTime)
            {
                resultText.text = UnityEngine.Random.Range(0, 20).ToString();

                yield return new WaitForSeconds(0.05f);
            }

            resultText.text = score.ToString();
            onComplete?.Invoke();

            yield return new WaitForSeconds(0.5f);
            Hide();
        }
    }
}

