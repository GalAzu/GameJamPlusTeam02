﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMechanics : MonoBehaviour
{

    //REFACTOR OBJECTS POOLING FOR BUILDING, HOW THE BUILD MECHANICS WORKS AND WHY? GET DEEPER INTO THE MECHANICS, ITS USE, AND ITS WAY OF INETRACTING
    // REFACTOR AUDIO
    private ThirdPersonCharacter player;
    [SerializeField]
    private float buildDistance;
    private Inventory inventory;
    [SerializeField] private int objBuildID;
    [SerializeField]
    private GameObject MetalWall;
    public float buildTime;
    public WasteCollection wasteCol;
    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        player = FindObjectOfType<ThirdPersonCharacter>();
        wasteCol = FindObjectOfType<WasteCollection>();
    }
    private void Update()
    {

        if (Input.inputString != "")
        {
            int number;
            bool is_a_number = int.TryParse(Input.inputString, out number);
            if (is_a_number && number >= 0 && number < 10)
            {
                objBuildID = number;
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            switch(objBuildID)
            {
                case (1):
                    if (inventory.inventory.Contains(GetWasteInventoryItem(WasteData.wasteType.MetalSheet))
                         &&
                        GetWasteInventoryItem(WasteData.wasteType.MetalSheet).stackSize >= 3)
                    {
                        StartCoroutine(BuildStart()) ;
                    }
                    else Debug.Log("Not enough resources");
                    break;
            }
        }
    }
    private WasteInventory GetWasteInventoryItem(WasteData.wasteType waste)
    {
        WasteInventory wasteToUse = inventory.inventory.Find(wasteParam => wasteParam.wasteData._wasteType == waste);
        return wasteToUse;
    }
    private int GetWasteInventoryItemQuantity(WasteData.wasteType waste)
    {
        WasteInventory wasteToCheck = inventory.inventory.Find(wasteParam => wasteParam.wasteData._wasteType == waste);
        return wasteToCheck.stackSize;
    }
    public void BuildObject(GameObject objToBuild)
    {
        Instantiate(objToBuild, transform.position + transform.forward * buildDistance, player.transform.rotation);
    }
    private IEnumerator BuildStart(Waste wasteToBuild)
    {
        //move isBuilding to this class
        if(!player.isBuilding)
        {
            //BUILD START
            Debug.Log("start build corutine");
            FMODUnity.RuntimeManager.PlayOneShot("event:/Build");
            player.isBuilding = true;
            player.rb.constraints = RigidbodyConstraints.FreezeAll;
            yield return new WaitForSeconds(buildTime);
            BuildObject(MetalWall);
            player.isBuilding = false;
            player.rb.constraints = RigidbodyConstraints.None;
            Debug.Log("finish build corutine");
            FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Wall", gameObject);
        }
    }
    private IEnumerator BuildStart()
    {
        //move isBuilding to this class
        if (!player.isBuilding)
        {
            //BUILD START
            Debug.Log("start build corutine");
            FMODUnity.RuntimeManager.PlayOneShot("event:/Build");
            player.isBuilding = true;
            player.rb.constraints = RigidbodyConstraints.FreezeAll;
            yield return new WaitForSeconds(buildTime);
            BuildObject(MetalWall);
            player.isBuilding = false;
            player.rb.constraints = RigidbodyConstraints.None;
            Debug.Log("finish build corutine");
            FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Wall", gameObject);
        }
    }

}

