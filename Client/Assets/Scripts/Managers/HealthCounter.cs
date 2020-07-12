using UnityEngine;
using UnityEngine.UI;

public class HealthCounter : MonoBehaviour
{
    [SerializeField] private PlayerCamera playerCamera;

    [SerializeField] private TMPro.TextMeshProUGUI lblCount;
    [SerializeField] private Image imgBackground;

    [SerializeField] private Color negativeColor = Color.red;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color positiveColor = Color.green;

    [SerializeField] private float flyUpLength = 1f;
    [SerializeField] private float lifeLength = 1f;
    [SerializeField] private float offsetY = 1f;

    [SerializeField] private AnimationCurve movementSpeedCurve;
    [SerializeField] private AnimationCurve fadeCurve;

    private float minOffsetY = 0f;
    private float lifeSpan = -1f;

    private Transform target;

    private void Start()
    {
        if (!imgBackground) imgBackground = GetComponentInChildren<Image>();
        if (!lblCount) lblCount = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (!playerCamera) playerCamera = FindObjectOfType<PlayerCamera>();
        this.transform.localPosition = Vector3.zero;
        this.SetToDefault();
    }

    private void Update()
    {
        if (lifeSpan < 0)
            return;

        lifeSpan += Time.deltaTime;
        var progress = lifeSpan / lifeLength;
        var opacity = fadeCurve.Evaluate(progress);

        SetOpacity(opacity);
        transform.position = target.position + (Vector3.up * GetPositionY(progress));
        transform.LookAt(playerCamera.transform);

        if (progress >= 1)
        {
            gameObject.SetActive(false);
            lifeSpan = -1;
        }
    }

    private void SetOpacity(float value)
    {
        if (imgBackground)
        {
            var bgColor = imgBackground.color;
            imgBackground.color = new Color(bgColor.r, bgColor.g, bgColor.b, value);
        }

        lblCount.faceColor = new Color(lblCount.faceColor.r, lblCount.faceColor.g, lblCount.faceColor.b, value);
    }

    private float GetPositionY(float progress)
    {
        var distance = movementSpeedCurve.Evaluate(progress) * flyUpLength;
        return minOffsetY + offsetY + distance;
    }

    public void SetValue(Transform targetTransform, int count)
    {
        target = targetTransform;

        SetToDefault();

        lblCount.text = count.ToString();
        lifeSpan = 0;

        var collider = target.GetComponentInChildren<CapsuleCollider>();
        if (collider)
        {
            minOffsetY = collider.height;
        }

        if (count > 0)
        {
            imgBackground.color = positiveColor;
        }
        else if (count < 0)
        {
            imgBackground.color = negativeColor;
        }
        else
        {
            imgBackground.color = defaultColor;
        }
    }
    private void SetToDefault()
    {
        transform.position = target.position + (Vector3.up * GetPositionY(0));
        SetOpacity(0);
        gameObject.SetActive(true);
    }
}
