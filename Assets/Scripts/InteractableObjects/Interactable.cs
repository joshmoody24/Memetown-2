using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    private Transform player;
    public string displayName = "Interact";
    public float interactionRadius = 1f;
    public bool isInteractable = true;
    public UnityEvent onInteract;

    // Start is called before the first frame update
    protected void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    protected void Update()
    {
        if(IsInRange() && isInteractable){
            if(InteractionManager.instance.getActiveObject() != this){
                InteractionManager.instance.setActiveObject(this);
            }
        }
    }

    protected void OnDrawGizmosSelected(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }

    public bool IsInRange(){
        if(Vector3.Distance(player.position, transform.position) < interactionRadius){
            return true;
        }
        else{
            return false;
        }
    }

    public virtual void Interact(){
        onInteract.Invoke();
    }
}
