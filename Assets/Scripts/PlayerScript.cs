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
    private WeaponScript availableWeaponObject;

    public static PlayerScript LocalInstance{get;private set;}
       
    [SerializeField] private float gravity = -19.81f;
    private bool isOnGround = false;
    private Vector3 verticalVelocity;
    private float movementSpeed = 10f;
    public float groundDistance = 1f;
    public float jumpHeight = 4f;
    private bool canJump = false;
    public float aimSensitivity = 0.5f;
    private float xClamp = 55f;
    private float xRotation = 0f;
    private float xSensitivity = 8f, ySensitivity = 0.5f;
    private float gamepadRotation=0f;
    private float gamepadXSensitivity = 100f, gamepadYSensitivity = 5f;
    private bool canEquipWeapon = false;
    private static bool isFirstPlayer = true;

    public bool IsLoser = false;

    Vector3 knockback;

    private bool isWalking= false;

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
      //  GameInput.Instance.OnAttackAction += GameInput_OnAttackAction;
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
        if(IsOwner&&currentWeaponObject!=null){
           currentWeaponObject.GetComponent<WeaponScript>().PlayMuzzleFlash();
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
        isWalking = moveDirection!=Vector3.zero;
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
        if(IsOwner){
            currentWeaponObject = availableWeaponObject;
            SetCurrentWeapon(currentWeaponObject);
            Debug.Log("|"+currentWeaponObject + "||"+player+"|");
            currentWeaponObject.SetWeaponParent(player);
        }
    }

    public void SetIdAndTransformOfPlayer(){
        transform.SetPositionAndRotation(new Vector3(-1,6,16),new Quaternion(0,0,0,0));
        playerInfo.id =1;
        if(isFirstPlayer){
            transform.SetPositionAndRotation(new Vector3(-1,-2,-12),new Quaternion(0,0,0,0));
            isFirstPlayer = false;
            playerInfo.id =0;
        }
    }


    //MARK:Do Damage
    public void DoDamage(Vector3 bulletDirection, GunSO gunInfo){
        Vector3 knockback = transform.forward + (bulletDirection * gunInfo.damage);
        controller.Move(knockback);
        knockback = Vector3.zero;
        Debug.Log(name);
    }
    //MARK: GameOver
    public void GameOver(){
        IsLoser = true;
        GameStateManagerScript.Instance.SetGameOver();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("void"))
        {
            GameOver();
            
        }
        canEquipWeapon = true;
        other.TryGetComponent<WeaponScript>(out availableWeaponObject);
        
    }
    private void OnTriggerExit(Collider other)
    {
        canEquipWeapon = false;
        availableWeaponObject = null;
    }

    public int GetPlayerId(){
        return playerInfo.id;
    }
    
    public bool IsPlayerEmptyHanded(){
        return currentWeaponObject==null;
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
        //remove and delete weapon logic
        currentWeaponObject = null;
        //and despawn the object also stop it from following
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

    public bool IsWalking(){
        return isWalking;
    }
    public bool IsHammerSwinging(){
        return currentWeaponObject.gameObject.tag=="Hammer";
    }
    public Transform GetCameraTransform(){
        return cameraTransform;
    }
}
