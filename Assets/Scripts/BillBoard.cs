using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public Transform canvas;
     
    void LateUpdate()
    {
        canvas.transform.forward = Camera.main.transform.forward;
    }
}
