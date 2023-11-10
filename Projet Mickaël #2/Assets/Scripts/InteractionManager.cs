using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private Transform mouseWorldPosGizmo;
    [SerializeField] private float maxRange = 100;

    private Vector3 mouseScreenPos;
    private Vector3 mouseWorldPos;
    private Vector3 origin;
    private Vector3 dir;
    
    void Update()
    {
        GetMouseScreenPos();
    }

    void GetMouseScreenPos()
    {
        mouseScreenPos = Input.mousePosition; //Donne les posX + posY de la souris en pixel
        mouseScreenPos.z = -transform.position.z; //Ajout de la profondeur

        mouseWorldPos = mainCam.ScreenToWorldPoint(mouseScreenPos); //Conversion Pixel en WorldPos

        mouseWorldPosGizmo.position = mouseWorldPos; //follow le transform de l'objet sur la mousePosWorld

        Vector3 origin = transform.position;
        Vector3 dir = mouseWorldPos - origin; //Distance entre transform.position - mousePosWorld
        
        Debug.DrawRay(origin, dir.normalized * maxRange, Color.cyan);


        // if (Input.GetMouseButtonDown(0))
        // {
        //     RaycastHit hit; //Variable qui va stocker toutes les infos du Game Object touché
        //
        //     if (Physics.Raycast(origin, dir, out hit, maxRange))
        //     {
        //         if (hit.transform.GetComponent<DoorBadExample>() != null) //Si l'objet touché a le script "DoorBadExample"
        //         {
        //             hit.transform.GetComponent<DoorBadExample>().OpenDoor();
        //         }
        //     }
        //     
        //     if (Physics.Raycast(origin, dir, out hit, maxRange))
        //     {
        //         if (hit.transform.GetComponent<ChestBadExample>() != null) //Si l'objet touché a le script "ChestBadExample"
        //         {
        //             hit.transform.GetComponent<ChestBadExample>().OpenChest();
        //         }
        //     }
        //     
        //     if (Physics.Raycast(origin, dir, out hit, maxRange))
        //     {
        //         if (hit.transform.GetComponent<DoorBadExample>() != null) //Si l'objet touché a le script "DoorBadExample"
        //         {
        //             hit.transform.GetComponent<DoorBadExample>().OpenDoor();
        //         }
        //     }
        // }
        
    }
}
