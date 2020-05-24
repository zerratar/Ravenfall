using Assets.Scripts.Extensions;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AppearanceSlider : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI lblHeader;
    [SerializeField] private TMPro.TextMeshProUGUI lblSliderValue;
    [SerializeField] private Slider slider;
    private int maxValue;

    public event EventHandler ValueChanged;

    public int MaxValue
    {
        get => maxValue;
        set
        {
            maxValue = value;
            lblHeader.gameObject.SetActiveFast(MaxValue > 0);
            lblSliderValue.gameObject.SetActiveFast(MaxValue > 0);
            slider.gameObject.SetActiveFast(MaxValue > 0);
        }
    }
    public int Value { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        slider.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<float>(SliderValueChanged));
        lblSliderValue.text = Value.ToString();
    }

    private void SliderValueChanged(float arg0)
    {
        Value = Mathf.FloorToInt(arg0 * MaxValue);
        lblSliderValue.text = Value.ToString();
        if (ValueChanged != null) ValueChanged(this, EventArgs.Empty);
    }
}
