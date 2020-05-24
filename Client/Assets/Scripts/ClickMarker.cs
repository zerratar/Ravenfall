using UnityEngine;

public class ClickMarker : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float animationLength = 1f;
    [SerializeField] private Vector3 offset;

    private float currentAnimationTime = 0f;
    private Material clickMaterial;

    public void Show(Vector3 point)
    {
        currentAnimationTime = 0f;
        gameObject.SetActive(true);
        transform.position = point + offset;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!meshRenderer) meshRenderer = GetComponent<MeshRenderer>();
        if (!meshRenderer) return;
        clickMaterial = meshRenderer.sharedMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if (!clickMaterial) return;
        currentAnimationTime += Time.deltaTime;
        if (currentAnimationTime <= animationLength)
        {
            var percent = currentAnimationTime / animationLength;
            var value = animationCurve.Evaluate(percent);
            clickMaterial.SetFloat("_BorderWidth", value);
            return;
        }

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        currentAnimationTime = 0f;
    }
}
