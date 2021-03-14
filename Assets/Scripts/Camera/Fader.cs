using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour {

    public static Fader fader;

    public Image panel;
    public new CanvasRenderer renderer;

    public float fadeInSpeed = 1f;
    public float fadeOutSpeed = 1f;
    public float holdTime = 1f;

    #region singleton
    private void Awake()
    {
        if (fader == null)
        {
            //TODO only works on root
            //DontDestroyOnLoad(transform.gameObject);
            fader = this;
        }
        else if (fader != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private void Start()
    {
        renderer = GetComponent<CanvasRenderer>();
        renderer.SetAlpha(0);
    }
    /*
    private void Update()
    {
        if (isFadingIn)
        {
            if (fader.color.a < 0)
            {
            }
            else
            {
                isFadingIn = false;

            }
        }
        else if (isFadingOut)
        {
            if (fader.color.a > 0)
            {
                fader.color = new Color(fader.color.r, fader.color.g, fader.color.b, fader.color.a - (Time.deltaTime * fadeSpeed);
            }
            else
            {
                isFadingOut = false;
                fader.color = Color.clear;
            }
        }
    }
    */

    private void FadeIn()
    {
        panel.enabled = true;
        renderer.SetAlpha(0f);
        panel.CrossFadeAlpha(1f, fadeInSpeed, true);
    }

    private void FadeOut()
    {
        renderer.SetAlpha(1f);
        panel.CrossFadeAlpha(0f, fadeOutSpeed, true);
    }

    private IEnumerator FadeInOut()
    {
        FadeIn();
        yield return new WaitForSeconds(fadeInSpeed + holdTime);
        FadeOut();
        yield return new WaitForSeconds(fadeOutSpeed);
        panel.enabled = false;
    }

    public void ScreenTransition()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInOut());
    }
}
