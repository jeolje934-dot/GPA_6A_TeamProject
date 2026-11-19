using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIGlitchEffect : MonoBehaviour
{
    [Header("흔들림 강도")]
    [SerializeField] private float maxOffsetX = 10f;
    [SerializeField] private float maxOffsetY = 3f;
    [SerializeField] private float glitchDuration = 0.08f;
    [SerializeField] private float minInterval = 1.5f;
    [SerializeField] private float maxInterval = 3.5f;

    [Header("선택")]
    [SerializeField] private Graphic targetGraphic;
    [SerializeField] private bool useFlash = true;
    [SerializeField] private Color flashColor = new Color(0.75f, 1f, 0.3f, 0.7f);

    private RectTransform _rectTransform;
    private Vector2 _originPos;
    private Color _originColor;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _originPos = _rectTransform.anchoredPosition;

        if (targetGraphic == null)
        {
            targetGraphic = GetComponent<Graphic>();
        }

        if (targetGraphic != null)
        {
            _originColor = targetGraphic.color;
        }
    }

    private void OnEnable()
    {
        if (_rectTransform != null)
        {
            _rectTransform.anchoredPosition = _originPos;
        }

        if (targetGraphic != null)
        {
            targetGraphic.color = _originColor;
        }

        StopAllCoroutines();
        StartCoroutine(GlitchRoutine());
    }

    private IEnumerator GlitchRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            float elapsed = 0f;
            while (elapsed < glitchDuration)
            {
                float offsetX = Random.Range(-maxOffsetX, maxOffsetX);
                float offsetY = Random.Range(-maxOffsetY, maxOffsetY);
                _rectTransform.anchoredPosition = _originPos + new Vector2(offsetX, offsetY);

                if (useFlash && targetGraphic != null)
                {
                    targetGraphic.color = (Random.value > 0.45f) ? flashColor : _originColor;
                }

                yield return new WaitForSeconds(0.02f);
                elapsed += 0.02f;
            }

            _rectTransform.anchoredPosition = _originPos;
            if (targetGraphic != null)
            {
                targetGraphic.color = _originColor;
            }
        }
    }
}