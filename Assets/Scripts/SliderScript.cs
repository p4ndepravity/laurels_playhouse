using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderScript : MonoBehaviour
{
    public GameObject slider;
    public GameObject text;

    public void ChangeValue()
    {
        string value = slider.GetComponent<Slider>().value.ToString("0");
        text.GetComponent<Text>().text = value;
    }
}