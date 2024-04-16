using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool canMove { get; private set; } = true;
    private bool isSprinting => canSprint && Input.GetKey(sprintKey);
    private bool shouldJump => Input.GetKeyDown(jumpKey) && characterController.isGrounded;
    private bool shouldCrouch => Input.GetKeyDown(crouchKey) && !duringCrouchAnimation && characterController.isGrounded;

    [Header(("Options fonctionnelles - SlideSlope est buggé"))]
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canUseHeadBob = true;
    [SerializeField] private bool willSlideOnSlopes = false;
    [SerializeField] private bool useStamina = true;
    [SerializeField] private bool canInteract = true;

    [Header(("Controles"))]
    [SerializeField] private KeyCode fowardInput = KeyCode.Z;
    [SerializeField] private KeyCode backwardInput = KeyCode.S;
    [SerializeField] private KeyCode leftInput = KeyCode.Q;
    [SerializeField] private KeyCode rightInput = KeyCode.D;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header(("Parametres de mouvement"))]
    [SerializeField] private float walkSpeed = 10.0f;
    [SerializeField] private float sprintSpeed = 20.0f;
    [SerializeField] private float crouchSpeed = 5.0f;
    [SerializeField] private float slopeSpeed = 8f;

    [Header(("Parametres de vision"))]
    [SerializeField, Range(1,10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1, 180)] private float upperLookLimit = 80.0f;
    [SerializeField, Range(1, 180)] private float lowerLookLimit = 80.0f;

    [Header(("Parametres de stamina"))]
    [SerializeField] public float maxStamina = 100;
    [SerializeField] public float staminaUseMultiplier = 5;
    [SerializeField] public float timeBeforeStaminaRegenStart = 5;
    [SerializeField] public float staminaValueIncrement = 2;
    [SerializeField] public float staminaTimeIncrement = 0.1f;
    public static float currentStamina;
    public Coroutine regeneratingStamina;
    public static Action<float> onStaminaChange;

    [Header(("Parametres de saut"))]
    [SerializeField] private float jumpForce = 10.0f;
    [SerializeField] private float gravity = 30.0f;

    [Header(("Parametres d'accroupissement"))]
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float standingHeight = 2.0f;
    [SerializeField] private float timeToCrouch = 0.25f;
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private Vector3 standingCenter = new Vector3(0, 0, 0);
    private bool isCrouching;
    private bool duringCrouchAnimation;
    
    [Header(("Parametres d'Headbob"))]
    [SerializeField] private float walkBobSpeed = 14.0f;
    [SerializeField] private float walkBobAmount = 0.05f;
    [SerializeField] private float sprintBobSpeed = 18.0f;
    [SerializeField] private float sprintBobAmount = 0.11f;
    [SerializeField] private float crouchBobSpeed = 8.0f;
    [SerializeField] private float couchBobAmount = 0.025f;
    private float defaultYPos = 0;
    private float timer;

    [Header(("-------Ne pas toucher a ça -----"))]
    public Animator anim;
    //Parametre des powerUps
        //Vitesse
    public bool shouldHaveSpeed = false;
    public float speedPower;
    

    //Parametres de slides sur les pentes
    private Vector3 hitPointNormal;
    private bool isSliding{
        get{
            if(characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2f)){
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > characterController.slopeLimit;
            }
            else{
                return false;
            }
        }
    }

    [Header("Interaction")]
    [SerializeField] private Vector3 interactionRayPoint = default;
    [SerializeField] private float interactionDistance = default;
    [SerializeField] private LayerMask interactionLayer = default;
    private interactable currentInteractable;

    private Camera playerCamera;
    private CharacterController characterController;

    private Vector3 moveDirection;
    private Vector2 currentInput;

    private float rotationX = 0;
    
    void Awake() {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        defaultYPos = playerCamera.transform.localPosition.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentStamina=maxStamina;
    }
    
    void Start(){
        currentStamina = maxStamina;
        
    }

    void Update() {
        if (canMove) {
            HandleMovementInput();
            HandleMouseLook();

            if(canJump){
                HandleJump();
            }

            if(canCrouch){
                HandleCrouch();
            }

            if(canUseHeadBob){
                HandleHeadBob();
            }

            if(useStamina){
                HandleStamina();
            }

            if(canInteract){
                HandleInteractionCheck();
                HandleInteractionInput();
            }

            ApplyFinalMovement();
        }
    }



    private void HandleMovementInput() {
        float inputX = 0f;
        float inputY = 0f;
        if (Input.GetKey(fowardInput))
            inputX = 1f;
        else if (Input.GetKey(backwardInput))
            inputX = -1f;
        if (Input.GetKey(leftInput))
            inputY = -1f;
        else if (Input.GetKey(rightInput))
            inputY = 1f;
        
        //Si powerup speed, active la vitesse speciale
        if(shouldHaveSpeed){
            currentInput = new Vector2(speedPower * inputX, speedPower * inputY);
        }
        else{
            currentInput = new Vector2((isSprinting ? sprintSpeed : isCrouching ? crouchSpeed : walkSpeed) * inputX, (isSprinting ? sprintSpeed : isCrouching ? crouchSpeed : walkSpeed) * inputY);
        }

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }

    private void HandleMouseLook() {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);
    }

    private void HandleJump() {
        if (shouldJump) {
            moveDirection.y = jumpForce;

        }
    }

    private void HandleCrouch() {
       if(shouldCrouch) {
            StartCoroutine(CrouchStand());
        }
    }

    private void HandleHeadBob() {
        if(!characterController.isGrounded) {
            return;
        }

        if(Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f) {
            timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : isSprinting ? sprintBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                defaultYPos + Mathf.Sin(timer) * (isCrouching ? couchBobAmount : isSprinting ? sprintBobAmount : walkBobAmount), 
                playerCamera.transform.localPosition.z);
        }
    }

    private void HandleStamina(){
        if(isSprinting && currentInput != Vector2.zero){

            if(regeneratingStamina != null){
                StopCoroutine(regeneratingStamina);
                regeneratingStamina = null;
            }

            currentStamina -= staminaUseMultiplier * Time.deltaTime;

            if(currentStamina < 0)
                currentStamina = 0;
            
            onStaminaChange?.Invoke(currentStamina);

            if(currentStamina <= 0)
                canSprint = false;
        }

        if(!isSprinting && currentStamina < maxStamina && regeneratingStamina == null){
            regeneratingStamina = StartCoroutine(regenerateStamina());
        }
    }

    private void HandleAnimation(){
        anim.SetBool("jump", !characterController.isGrounded);
        anim.SetFloat("vertical", currentInput.x);
        anim.SetFloat("horizontal", currentInput.y);
    }

    private void ApplyFinalMovement() {
        if(!characterController.isGrounded) {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        if(willSlideOnSlopes && isSliding){
            moveDirection += new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z);
            moveDirection *= slopeSpeed;
        }

        HandleAnimation();
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void HandleInteractionCheck() {
        if(Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance))
        {
            if(hit.collider.gameObject.layer == 6 && (currentInteractable == null || hit.collider.gameObject.GetInstanceID() != currentInteractable.GetInstanceID()))
            {
                hit.collider.TryGetComponent(out currentInteractable);

                if(currentInteractable)
                    currentInteractable.OnFocus();
            }
        }
        else if (currentInteractable)
        {
            currentInteractable.OnLoseFocus();
            currentInteractable = null;
        }
    }

    private void HandleInteractionInput() {
        if(Input.GetKeyDown(interactKey) && currentInteractable != null && Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance, interactionLayer))
        {
            currentInteractable.OnInteract();
        }
    }

    private IEnumerator CrouchStand(){

        if (isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1f)){
            yield break;
        }

        duringCrouchAnimation = true;

        float timeElapsed = 0;
        float targetHeight = isCrouching ? standingHeight : crouchHeight;
        float currentHeight = characterController.height;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = characterController.center;

        while(timeElapsed < timeToCrouch){
            characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        characterController.height = targetHeight;
        characterController.center = targetCenter;

        isCrouching = !isCrouching;

        duringCrouchAnimation = false;
    }

    public IEnumerator regenerateStamina(){
        yield return new WaitForSeconds(timeBeforeStaminaRegenStart);
        WaitForSeconds timeToWait = new WaitForSeconds(staminaTimeIncrement);

        while(currentStamina < maxStamina){
            if(currentStamina > 0)
                canSprint = true;
            
            currentStamina += staminaValueIncrement;

            if(currentStamina > maxStamina)
                currentStamina = maxStamina;
            
            onStaminaChange?.Invoke(currentStamina);

            yield return timeToWait;
        }

        regeneratingStamina = null;
    }

    //Powerup actions
    //Activate vitesse
    public void setSpeedPower(float powerUpSpeed, float resetTime){
        speedPower = powerUpSpeed;
        shouldHaveSpeed = true;

        Invoke("resetPower", resetTime);
    }
    //Stop vitesse
    public void resetPower(){
        if(shouldHaveSpeed){
            shouldHaveSpeed = !shouldHaveSpeed;
        }
    }
}
