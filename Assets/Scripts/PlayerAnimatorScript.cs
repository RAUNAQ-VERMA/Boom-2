using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class PlayerAnimatorScript : MonoBehaviour
{
    public static PlayerAnimatorScript Instance{get;private set;}
    [SerializeField] private Animator animator;
    void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
    }
    void Update()
    {
      if(PlayerScript.LocalInstance!=null){
        animator.SetBool("isWalking",PlayerScript.LocalInstance.IsWalking());
        animator.SetBool("hasWeapon",PlayerScript.LocalInstance.HasWeapon());
      }
    }
    public void HammerSwingAnimation(){
        if(PlayerScript.LocalInstance==null){
            return;
        }
        animator.SetTrigger("hammerSwing");
    }
}
