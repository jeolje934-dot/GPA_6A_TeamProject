using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverIndicator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("마우스를 올렸을 때 보여줄 표시 오브젝트")]
    [SerializeField] private GameObject hoverIndicator;

    [Header("오디오 설정")]
    [SerializeField] private AudioSource audioSource;

    [Header("마우스 오버 효과음")]
    [SerializeField] private AudioClip hoverSound;
    [SerializeField][Range(0f, 1f)] private float hoverVolume = 1f;

    [Header("클릭 효과음")]
    [SerializeField] private AudioClip clickSound;
    [SerializeField][Range(0f, 1f)] private float clickVolume = 1f;

    private void Awake()
    {
        if (hoverIndicator != null)
        {
            hoverIndicator.SetActive(false);
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverIndicator != null)
        {
            hoverIndicator.SetActive(true);
        }

        if (audioSource != null && hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound, hoverVolume);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverIndicator != null)
        {
            hoverIndicator.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound, clickVolume);
        }
    }

    private void OnDisable()
    {
        if (hoverIndicator != null)
        {
            hoverIndicator.SetActive(false);
        }
    }
}