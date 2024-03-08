using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BasicSetupScript : NetworkBehaviour
{
    [SerializeField] Behaviour[] componentsToDisable;
    private Camera sceneCamera;

    void Start()
    {
        if(!IsLocalPlayer){
            for (int i = 0; i<componentsToDisable.Length; i++){
                componentsToDisable[i].enabled =false;
            }
        }
        else{
            sceneCamera = Camera.main;
            if(sceneCamera!=null){
                sceneCamera.gameObject.SetActive(false);
            }
            
        }
    }
    private void OnDisable() {
        if(sceneCamera!=null){
            sceneCamera.gameObject.SetActive(true);
        }
    }
}
