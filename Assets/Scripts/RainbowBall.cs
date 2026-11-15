
using UnityEngine;

public class RainbowBall : MonoBehaviour
{
    private Renderer ballRenderer;
    private Material ballMaterial;

    [Header("Glow Settings")]
    public float emissionPower = 3f;

    void Start()
    {
        ballRenderer = GetComponent<Renderer>();

        if (ballRenderer == null)
            ballRenderer = GetComponentInChildren<Renderer>();

        if (ballRenderer == null)
        {
            Debug.LogError("공의 Renderer를 찾을 수 없습니다.");
            return;
        }

        ballMaterial = ballRenderer.material;

        // 공의 기본 색상을 흰색으로 고정
        ballMaterial.SetColor("_BaseColor", Color.white);

        // Emission 활성화
        ballMaterial.EnableKeyword("_EMISSION");

        // 흰색 발광 적용
        ballMaterial.SetColor(
            "_EmissionColor",
            Color.white * emissionPower
        );
    }

    void OnDestroy()
    {
        if (ballMaterial != null)
        {
            Destroy(ballMaterial);
        }
    }
}
