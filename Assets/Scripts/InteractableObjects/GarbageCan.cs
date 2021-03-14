using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCan : Interactable
{
    public bool empty;

    public void Fill(){
        empty = false;
    }
    public void Empty(){
        empty = true;
    }
    public override void Interact(){
        base.Interact();
        Empty();
        JanitorManager.instance.UpdateTodoList();
        isInteractable = false;
    }
    public bool isEmpty(){
        return empty;
    }


}
