
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AchievementPopupController : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private RectTransform popupRect;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;

    [Header("애니메이션 설정")]
    [SerializeField] private float slideDistance = 500f;
    [SerializeField] private float slideDuration = 0.4f;
    [SerializeField] private float displayDuration = 3f;

    private Vector2 targetPosition;

    private Queue<int> achievementQueue = new Queue<int>();
    private Coroutine popupCoroutine;

    private void Awake()
    {
        targetPosition = popupRect.anchoredPosition;

        // 시작 시 화면 왼쪽 바깥으로 이동
        popupRect.anchoredPosition =
            targetPosition + Vector2.left * slideDistance;

        // 완전히 숨기기
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    // 새 업적 알림 요청
    public void ShowAchievement(int achievementIndex)
    {
        achievementQueue.Enqueue(achievementIndex);

        if (popupCoroutine == null)
        {
            popupCoroutine = StartCoroutine(ShowQueue());
        }
    }

    // 업적 알림을 순서대로 표시
    private IEnumerator ShowQueue()
    {
        while (achievementQueue.Count > 0)
        {
            int index = achievementQueue.Dequeue();

            titleText.text = AchievementSystem.GetName(index);
            descriptionText.text =
                AchievementSystem.GetDescription(index);

            // 등장
            yield return MovePopup(targetPosition, 1f);

            // 표시 시간
            yield return new WaitForSecondsRealtime(displayDuration);

            // 퇴장
            yield return MovePopup(
                targetPosition + Vector2.left * slideDistance,
                0f
            );
        }

        popupCoroutine = null;
    }

    // 위치와 투명도 애니메이션
    private IEnumerator MovePopup(
        Vector2 destination,
        float targetAlpha
    )
    {
        Vector2 startPosition = popupRect.anchoredPosition;
        float startAlpha = canvasGroup.alpha;

        float elapsed = 0f;

        while (elapsed < slideDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(elapsed / slideDuration);
            t = Mathf.SmoothStep(0f, 1f, t);

            popupRect.anchoredPosition =
                Vector2.Lerp(startPosition, destination, t);

            canvasGroup.alpha =
                Mathf.Lerp(startAlpha, targetAlpha, t);

            yield return null;
        }

        popupRect.anchoredPosition = destination;
        canvasGroup.alpha = targetAlpha;
    }
}
