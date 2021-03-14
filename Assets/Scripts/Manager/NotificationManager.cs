using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour {

    public static NotificationManager manager;

    public Animator notificationBox;
    public Image bg;
    public Image icon;
    public Text text;

    //public float holdDuration = 4f;
    public Sprite defaultIcon;
    public Color defaultColor;

    #region singleton
    // Use this for initialization
    void Awake()
    {
        if (manager == null)
        {
            DontDestroyOnLoad(transform.gameObject);
            manager = this;
        }
        else if (manager != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public void Notify(string message)
    {
        notificationBox.SetTrigger("dropdown");
        text.text = message;
        icon.sprite = defaultIcon;
        bg.color = defaultColor;
        StopAllCoroutines();
        //StartCoroutine(retract(holdDuration));
    }
    public void Notify(string message, Color bgcolor)
    {
        notificationBox.SetTrigger("dropdown");
        text.text = message;
        bg.color = bgcolor;
        icon.sprite = defaultIcon;
        StopAllCoroutines();
        //StartCoroutine(retract(holdDuration));
    }
    public void Notify(string message, Sprite customIcon)
    {
        notificationBox.SetTrigger("dropdown");
        text.text = message;
        icon.sprite = customIcon;
        bg.color = defaultColor;
        StopAllCoroutines();
        //StartCoroutine(retract(holdDuration));
    }
    public void Notify(string message, Color bgcolor, Sprite customIcon)
    {
        notificationBox.SetTrigger("dropdown");
        text.text = message;
        icon.sprite = customIcon;
        bg.color = bgcolor;
        StopAllCoroutines();
        //StartCoroutine(retract(holdDuration));
    }

    //not used anymore because retraction is built into the animation
    IEnumerator retract(float duration)
    {
        yield return new WaitForSeconds(duration);
        notificationBox.SetTrigger("retract");
    }
}
