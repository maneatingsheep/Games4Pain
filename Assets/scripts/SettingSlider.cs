using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class SettingSlider : MonoBehaviour {

    private InputField _input;
    private Slider _slider;
    private Button _testButt;

    public UnityEvent OnValueChange;
    public UnityEvent OnTest;

    private bool ignoreChange = false;

    public void SliderValueChanged(float value) {
        if (ignoreChange) return;

        ignoreChange = true;

        _input.text = _slider.value.ToString();
        OnValueChange.Invoke();

        ignoreChange = false;
    }

    public void InputValueChanged() {
        if (ignoreChange) return;

        ignoreChange = true;
        
		byte val = byte.Parse (_input.text);

		val = (byte)Mathf.Max (val, _slider.minValue);
		val = (byte)Mathf.Min (val, _slider.maxValue);

        _input.text = val.ToString ();
        _slider.value = val;


        OnValueChange.Invoke();

        ignoreChange = false;
    }


    public void SetMinMax(byte min, byte max) {
        ignoreChange = true;
        _slider.minValue = min;
        _slider.maxValue = max;
        ignoreChange = false;
    }

    public void SetValue(float value, bool testEnabled) {

        ignoreChange = true;

        _slider.value = value;

        _input.text = value.ToString();
        gameObject.SetActive(true);

        ignoreChange = false;

        _testButt.gameObject.SetActive(testEnabled);
    }

    public void Disabple() {
        gameObject.SetActive(false);
    }

    public void Init() {
        _input = GetComponentInChildren<InputField>();
        _slider = GetComponent<Slider>();
        _testButt = GetComponentInChildren<Button>();
        _slider.onValueChanged.AddListener(SliderValueChanged);
    }
	
    public void TestClicked() {
        OnTest.Invoke();
    }

    public byte value {
        get {
            return (byte)_slider.value;
        }
    }

    
}
