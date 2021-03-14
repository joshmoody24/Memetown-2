using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineBetweenPoints : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;
    public Transform lineTip;
    private LineRenderer line;
    [Range(0f,2f)] public float fractionFilled = 1f;
    void Start(){
        line = GetComponent<LineRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        //line renderers deal in absolute positions
        line.SetPosition(0,startPos.position);
        Vector3 newPos = Vector3.Lerp(startPos.position,endPos.position, fractionFilled);
        line.SetPosition(1, newPos);
        if(lineTip != null){
            lineTip.position = newPos;
        }
    }
}
