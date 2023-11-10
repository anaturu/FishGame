using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBadExample : InteractableObject
{
    public override void Interact()
    {
        base.Interact();
        
        Debug.Log("DOOR IS OPEN");
    }
   
}
