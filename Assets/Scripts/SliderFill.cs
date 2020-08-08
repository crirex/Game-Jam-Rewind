using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderFill : MonoBehaviour
{
    private Slider _slider;
    private Image _image;

    public bool isDecreasing = true;
    public float timeToWait;

    public float value = 0f;

    void Start()
    {
        _slider = gameObject.GetComponent<Slider>();

        if (_slider == null)
        {
            _image = gameObject.GetComponent<Image>();
            _image.fillAmount = isDecreasing ? 1f : 0f;
        }
        else
        {
            _slider.value = isDecreasing ? 1f : 0f;
        }


    }

    void Update()
    {
        if (timeToWait == -1)
        {
            if (_slider == null)
            {
                _image.fillAmount = value;
            }
            else
            {
                _slider.value = value;
            }
        }
        else
        {
            if (_slider == null)
            {
                _image.fillAmount += Time.deltaTime / timeToWait * (isDecreasing ? -1 : 1);
                value = _image.fillAmount;
            }
            else
            {
                _slider.value += Time.deltaTime / timeToWait * (isDecreasing ? -1 : 1);
                value = _slider.value;
            }
        }
    }

    public void ResetSlider()
    {
        if (_slider == null)
        {
            _image.fillAmount = isDecreasing ? 1f : 0f;
        }
        else
        {
            _slider.value = isDecreasing ? 1f : 0f;
        }
    }
}
