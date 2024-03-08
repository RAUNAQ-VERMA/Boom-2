using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class ManageWeaponScript : NetworkBehaviour
{
    [SerializeField] private WeaponSpawnScript weaponSpawner;
    [SerializeField]private List<GameObject> weapons;
    [SerializeField]private Transform weaponHolder;
    [SerializeField]private new Camera camera;
    private bool canEquip = false;
    private int currentWeaponIndex;

    WeaponScript weaponObject;

    private void Start()
    {
        weapons[currentWeaponIndex].GetComponent<Rigidbody>().isKinematic = true;
        
    }
    
    // //this should be in player class
    // private void GameInput_OnPickUpAction(object sender, EventArgs e)
    // {
    //     if(weaponHolder.childCount!=0)
    //     {
    //         UnequipObject();
    //     }
    //     if(canEquip)
    //     {
    //        // weapons[1].GetComponent<WeaponScript>().SetWeaponParent();
    //         WeaponSpawnScript.Instance.GetSpawnedGun().GetComponent<WeaponScript>().SetWeaponParent();
    //         //EquipObject(currentWeaponIndex);
    //     }
        
    // }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Hammer"))
        {
            canEquip = true;
            currentWeaponIndex = 0;
        }
        else if (other.gameObject.CompareTag("Shotgun"))
        {
            canEquip = true;
            currentWeaponIndex = 1;
        }
        else if (other.gameObject.CompareTag("Rpg"))
        {
            canEquip = true;
            currentWeaponIndex = 2;
        }
        else
        {
            canEquip = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        canEquip = false;
    }


    public void Shoot(InputAction.CallbackContext context)
    {
        if(context.performed && weaponHolder.childCount != 0)
        {
            // if (currentWeaponIndex == 0)
            // { 
            //     weaponHolder.gameObject.GetComponentInChildren<HammerScript>().Shoot();
                
            // }
            // else if (currentWeaponIndex == 1)
            // {
            //     weaponHolder.gameObject.GetComponentInChildren<ShotgunScript>().Shoot();
            // }
            // else if (currentWeaponIndex == 2)
            // {
            //     weaponHolder.gameObject.GetComponentInChildren<RpgScript>().Shoot();
            // }
            // else
            // {
            //     Debug.Log("Error in Shooting");
            // }
        }
    }
    private void UnequipObject()
    {
        weaponHolder.DetachChildren();
        weapons[currentWeaponIndex].transform.eulerAngles = new Vector3(weapons[currentWeaponIndex].transform.eulerAngles.x, weapons[currentWeaponIndex].transform.eulerAngles.y, weapons[currentWeaponIndex].transform.eulerAngles.z-45f);
        weapons[currentWeaponIndex].GetComponent<Rigidbody>().isKinematic=false;
    }
    private void EquipObject(int gunIndex)
    {
        currentWeaponIndex = gunIndex;
        weapons[gunIndex].GetComponent <Rigidbody>().isKinematic = true;
        weapons[gunIndex].transform.SetPositionAndRotation(weaponHolder.transform.position, camera.transform.rotation);
      //  weapons[gunIndex].GetComponent<WeaponScript>().SetWeaponParent();
        
    }  
    public Transform GetWeaponHolderTransform(){
        return weaponHolder;
    }
}
