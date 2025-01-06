using UnityEngine;
using TMPro;

public class TextPulse : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textElement;
    [SerializeField] private Color pulseColor = Color.red;
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float minAlpha = 0.2f;
    [SerializeField] private bool scaleWithPulse = true;
    [SerializeField] private float minScale = 0.9f;
    [SerializeField] private float maxScale = 1.1f;

    private Color originalColor;
    private Vector3 originalScale;

    private void Start()
    {
        if (textElement == null)
        {
            textElement = GetComponent<TextMeshProUGUI>();
        }

        originalColor = textElement.color;
        originalScale = textElement.transform.localScale;
    }

    private void Update()
    {
        // Calculate pulse value using a sine wave (0 to 1)
        float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;

        // Interpolate between colors
        Color currentColor = Color.Lerp(
            new Color(pulseColor.r, pulseColor.g, pulseColor.b, minAlpha),
            pulseColor,
            pulse
        );
        textElement.color = currentColor;

        // Scale the text if enabled
        if (scaleWithPulse)
        {
            float currentScale = Mathf.Lerp(minScale, maxScale, pulse);
            textElement.transform.localScale = originalScale * currentScale;
        }
    }

    // Call this to reset the text to its original state
    public void ResetText()
    {
        textElement.color = originalColor;
        textElement.transform.localScale = originalScale;
    }

    // Call this to start/stop the pulse effect
    public void SetPulseActive(bool active)
    {
        enabled = active;
        if (!active)
        {
            ResetText();
        }
    }
}


