
using UnityEngine;

public class ObstacleGlitch : MonoBehaviour
{
    private Renderer[] renderers;
    private Material[][] materials;

    [Header("Glitch Settings")]
    public float minInterval = 0.12f;
    public float maxInterval = 0.3f;
    public float emissionPower = 1.2f;

    [Header("Glitch Colors")]
    public Color blueColor = new Color(0.35f, 0.75f, 0.9f);
    public Color purpleColor = new Color(0.65f, 0.45f, 0.8f);

    [Header("Flicker Settings")]
    [Range(0f, 1f)]
    public float offChance = 0.2f;

    private float timer;
    private float nextGlitch;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();

        materials = new Material[renderers.Length][];

        for (int i = 0; i < renderers.Length; i++)
        {
            // 원본 Material을 복제하여 장애물별로 독립 적용
            materials[i] = renderers[i].materials;

            foreach (Material mat in materials[i])
            {
                mat.EnableKeyword("_EMISSION");
            }
        }

        SetGlitchColor(blueColor, 0.5f);
        SetNextGlitch();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= nextGlitch)
        {
            timer = 0f;

            SetNextGlitch();

            // 가끔 빛이 약해지거나 꺼지는 효과
            if (Random.value < offChance)
            {
                SetGlitchColor(Color.black, 0f);
            }
            else
            {
                // 청록색 또는 보라색 중 무작위 선택
                Color glitchColor =
                    Random.value > 0.5f ? blueColor : purpleColor;

                // 발광 강도를 불규칙하게 변경
                float intensity = Random.Range(
                    emissionPower * 0.35f,
                    emissionPower
                );

                SetGlitchColor(glitchColor, intensity);
            }
        }
    }

    void SetGlitchColor(Color color, float intensity)
    {
        if (materials == null) return;

        foreach (Material[] group in materials)
        {
            foreach (Material mat in group)
            {
                if (mat != null)
                {
                    mat.SetColor(
                        "_EmissionColor",
                        color * intensity
                    );
                }
            }
        }
    }

    void SetNextGlitch()
    {
        nextGlitch = Random.Range(minInterval, maxInterval);
    }

    void OnDestroy()
    {
        if (materials == null) return;

        foreach (Material[] group in materials)
        {
            foreach (Material mat in group)
            {
                if (mat != null)
                {
                    Destroy(mat);
                }
            }
        }
    }
}
