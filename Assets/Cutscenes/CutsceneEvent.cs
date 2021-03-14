using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CutsceneEvent : MonoBehaviour {

    public UnityEvent OnEnabled;

    public UnityEvent OnDisabled;

    private void OnEnable()
    {
        OnEnabled.Invoke();
    }

    private void OnDisable()
    {
        OnDisabled.Invoke();
    }
}
