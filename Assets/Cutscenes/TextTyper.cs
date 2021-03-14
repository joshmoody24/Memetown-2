using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTyper : MonoBehaviour {

    public Text text;
    public string targetString;
    public float typeSpeed = .05f;
    private new AudioSource audio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        text.text = "";
        audio = GetComponent<AudioSource>();
        StartCoroutine(Type());
    }

    IEnumerator Type()
    {
        for(int i = 0; i < targetString.Length; i++)
        {
            text.text += targetString[i];
            audio.Play();
            yield return new WaitForSeconds(typeSpeed);
        }
    }
}
