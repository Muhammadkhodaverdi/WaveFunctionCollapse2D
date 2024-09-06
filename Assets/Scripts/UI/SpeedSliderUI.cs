using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedSliderUI : MonoBehaviour
{
    public static SpeedSliderUI Instance { get; private set; }

    public event EventHandler<OnSliderValueChanageEventArgs> OnSliderValueChanage;
    public class OnSliderValueChanageEventArgs : EventArgs
    {
        public float value;
    }

    [SerializeField] private Slider slider;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        slider.onValueChanged.AddListener((value) =>
        {
            OnSliderValueChanage?.Invoke(this, new OnSliderValueChanageEventArgs { value = value });
        });
    }
}
