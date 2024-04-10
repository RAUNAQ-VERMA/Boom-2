using Unity.Netcode;
using UnityEngine;

public class ManageDamageScript : NetworkBehaviour
{
  [SerializeField] private CharacterController controller;
  private void Start() {
   // PlayerShootScript.OnDamage+= OnDamage;
  }

  private void OnDamage(object sender, DamageEventArgs e)
  {
    if(transform.name == e.playerId){
      Vector3 knockback = transform.forward + (e.bulletTransform * e.gunInfo.damage);
      controller.Move(knockback);
      Debug.Log(transform.name+"||"+transform.position+"||");
    }
  }
    public void ReciveDamage(Vector3 bulletDirection, GunSO gunInfo){
        Vector3 knockback = transform.forward + (bulletDirection * gunInfo.damage);
        controller.Move(knockback);
        Debug.Log(name);
    }
}
