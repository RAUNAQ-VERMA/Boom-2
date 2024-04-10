using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerScript : NetworkBehaviour,IWeaponParent
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform groundDetector;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private PlayerSO playerInfo;
    
    
    private WeaponScript currentWeaponObject;

    public static PlayerScript LocalInstance{get;private set;}
       
    [SerializeField] private float gravity = -19.81f;
    private bool isOnGround = false;
    private Vector3 verticalVelocity;
    private float movementSpeed = 10f;
    public float groundDistance = 0.4f;
    public float jumpHeight = 4f;
    private bool canJump = false;
    public float aimSensitivity = 0.5f;
    private float xClamp = 85f;
    private float xRotation = 0f;
    private float xSensitivity = 8f, ySensitivity = 0.5f;
    private float gamepadRotation=0f;
    private float gamepadXSensitivity = 100f, gamepadYSensitivity = 5f;
    private bool canEquipWeapon = false;
    private static bool isFirstPlayer = true;

    Vector3 knockback;

    public override void OnNetworkSpawn()
    {
        LocalInstance =this;
        SetIdAndTransformOfPlayer();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
       // GameInput.Instance.OnAttackAction += GameInput_OnAttackAction;
        GameInput.Instance.OnJumpAction += GameInput_OnJumpAction;
        GameInput.Instance.OnPickUpAction += GameInput_OnPickUpAction;
      //  PlayerShootScript.OnDamage+= OnDamage;
    }

    private void OnDamage(object sender, DamageEventArgs e)
    {
        if(transform.name == e.playerId){
            knockback = transform.forward + (e.bulletTransform * e.gunInfo.damage);
            controller.Move(knockback*Time.deltaTime);
            Debug.Log(transform.name);
        }
    }

    private void GameInput_OnPickUpAction(object sender, EventArgs e)
    {
        if(canEquipWeapon){
            Interact(this);
        }
    }

    private void GameInput_OnJumpAction(object sender, EventArgs e)
    {
        canJump = true;
    }

    private void GameInput_OnAttackAction(object sender, EventArgs e)
    {
        if(IsOwner){
           // currentWeaponObject.Shoot(cameraTransform);
           gameObject.GetComponent<PlayerShootScript>().Shoot(cameraTransform);
        }
    }

    public void Update()
    {
        if(!GameStateManagerScript.Instance.IsGamePlaying()){
            return;
        }
        if (!IsOwner) return;

        isOnGround = Physics.CheckSphere(groundDetector.position, groundDistance, layerMask);

        ManageMovement();

        ManageJump();
        
        if (GameInput.Instance.GetControlScheme().Equals("Keyboard"))
        {
            Vector3 mouseInput = GameInput.Instance.GetMosuseInput();
            ManageCameraMovement(mouseInput);
        }

        if(GameInput.Instance.GetControlScheme().Equals("Gamepad"))
        {    
            Vector2 gamepadInput = GameInput.Instance.GetGamepadInput();
            ManageGamepadLook(gamepadInput);
        }
    }
    
    private void ManageCameraMovement(Vector2 mouseInput)
    {
        transform.Rotate(Vector3.up, mouseInput.x * xSensitivity * Time.deltaTime);
        xRotation -= mouseInput.y*ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);
        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = xRotation;
        cameraTransform.eulerAngles = targetRotation;
    }
    private void ManageMovement()
    {
        Vector3 moveDirection = GameInput.Instance.GetMoveDirections();
        Vector3 velocity = (transform.right * moveDirection.x + transform.forward * moveDirection.y) * movementSpeed;
        controller.Move(velocity  * Time.deltaTime);
    }

    private void ManageJump() {
        
        if (isOnGround && verticalVelocity.y<0)
        {
            verticalVelocity.y = -2f;
        }
        
        if (canJump)
        {
            if (isOnGround)
            {
                verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity );                
            }
            canJump = false;  
        }
        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity*Time.deltaTime);
    }
    private void ManageGamepadLook(Vector2 gamepadInput)
    {
        transform.Rotate(Vector3.up, gamepadInput.x * gamepadXSensitivity * Time.deltaTime);
        gamepadRotation -= gamepadInput.y * gamepadYSensitivity;
        gamepadRotation = Mathf.Clamp(gamepadRotation,-xClamp,xClamp);
        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = gamepadRotation;
        cameraTransform.eulerAngles = targetRotation;
    }

    private void Interact(PlayerScript player){
        // weapon object is blank for client
        if(IsOwner){
            SetCurrentWeapon(currentWeaponObject);
            Debug.Log("|"+currentWeaponObject + "||"+player+"|");
            currentWeaponObject.SetWeaponParent(player);
        }
    }

    public void SetIdAndTransformOfPlayer(){
        transform.SetPositionAndRotation(new Vector3(1,2,-16),new Quaternion(0,0,0,0));
        playerInfo.id =1;
        if(isFirstPlayer){
            transform.SetPositionAndRotation(new Vector3(-1,-2,-16),new Quaternion(0,0,0,0));
            isFirstPlayer = false;
            playerInfo.id =0;
        }
    }

    public void DoDamage(Vector3 bulletDirection, GunSO gunInfo){
        Vector3 knockback = transform.forward + (bulletDirection * gunInfo.damage);
        controller.Move(knockback);
        knockback = Vector3.zero;
        Debug.Log(name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<WeaponScript>(out currentWeaponObject))
        {
            canEquipWeapon = true;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        canEquipWeapon = false;
    }

    public int GetPlayerId(){
        return playerInfo.id;
    }
    
    public Transform GetWeaponHolderTransform()
    {
        return weaponHolder;
    }

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }

    public void ClearWeapon()
    {
        throw new NotImplementedException();
    }

    public WeaponScript GetCurrentWeapon()
    {
        return currentWeaponObject;
    }

    public bool HasWeapon()
    {
        return currentWeaponObject!=null;
    }

    public void SetCurrentWeapon(WeaponScript weaponObject)
    {
        currentWeaponObject = weaponObject;
    }
}
