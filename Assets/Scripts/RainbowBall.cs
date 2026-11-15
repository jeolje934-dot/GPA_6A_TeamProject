


using UnityEngine;

public class RainbowBall : MonoBehaviour
{
    private Renderer ballRenderer;
    private Material ballMaterial;

    public float colorSpeed = 0.5f;
    public float emissionPower = 3f;

    void Start()
    {
        ballRenderer = GetComponent<Renderer>();
        ballMaterial = ballRenderer.material;

        ballMaterial.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        float t = (Mathf.Sin(Time.time * colorSpeed) + 1f) / 2f;

        Color blue = new Color(0f, 0.2f, 1f);
        Color cyan = new Color(0f, 1f, 1f);
        Color white = Color.white;

        Color neonColor;

        if (t < 0.5f)
        {
            neonColor = Color.Lerp(blue, cyan, t * 2f);
        }
        else
        {
            neonColor = Color.Lerp(cyan, white, (t - 0.5f) * 2f);
        }

        ballMaterial.SetColor("_BaseColor", neonColor);
        ballMaterial.SetColor(
            "_EmissionColor",
            neonColor * emissionPower
        );
    }

    void OnDestroy()
    {
        if (ballMaterial != null)
            Destroy(ballMaterial);
    }
}
