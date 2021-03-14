using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager instance = null;
    private Interactable activeObject;
    public GameObject interactionUI;
    public Text interactionUIText;

    void Awake(){
        if(instance == null){
            instance = this;
        }
        else if(instance != this){
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    protected void Start()
    {
        activeObject = null;
    }

    // Update is called once per frame
    protected void Update()
    {
        bool nullifyObject = false;
        if(activeObject != null){
            if(Input.GetButtonDown("Fire1")){
                activeObject.Interact();
                nullifyObject = true;
            }
            if(!activeObject.IsInRange()){
                nullifyObject = true;
            }
        }
        if(nullifyObject){
            setActiveObject(null);
        }
    }

    public bool setActiveObject(Interactable newObject){
        if(newObject == null){
            interactionUI.SetActive(false);
            interactionUIText.text = "";
            activeObject = null;
            return true;
        }
        else if(newObject != activeObject || activeObject == null){
            activeObject = newObject;
            interactionUI.SetActive(true);
            interactionUIText.text = newObject.displayName;
            return true;
        }
        else{
            Debug.Log("there is already an interactable object in range, not changing it");
            return false;
        }
    }
    public Interactable getActiveObject(){
        return activeObject;
    }
}
