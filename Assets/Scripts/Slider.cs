using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slider : MonoBehaviour
{
    private UnityEngine.UI.Slider _slider;
    private Image _image;

    public bool isDecreasing = true;
    public float timeToWait;

    void Start()
    {
        _slider = gameObject.GetComponent<UnityEngine.UI.Slider>();

        if (_slider == null)
        {
            _image = gameObject.GetComponent<Image>();
            _image.fillAmount = isDecreasing ? 1f : 0f;
        }
        else
            _slider.value = isDecreasing ? 1f : 0f;


    }

    void Update()
    {
        if (_slider == null)
        {
           _image.fillAmount += Time.deltaTime / timeToWait * (isDecreasing ? -1 : 1);
        }
       else
           _slider.value +=  Time.deltaTime/timeToWait * (isDecreasing ? -1 : 1);


    }
}
