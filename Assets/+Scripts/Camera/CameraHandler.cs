using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;

namespace Theogony
{
    public class CameraHandler : MonoBehaviour
    {
        public PlayerControllerScript player;
        public Transform cameraTransform;
        public Transform cameraPivotTransform;
        private Transform myTransform;
        private Vector3 cameraTransformPosition;
        public LayerMask ignoreLayers;
        private PlayerInput playerInput;
        private GlobalInfo globalInfo;
        private Camera mainCam;
        public CustomSlider sensSlider;
        public CustomSlider pivotSlider;
        public float maxDistance;
        private Vector3 cameraPositionGoal;

        public static CameraHandler singleton;
        private bool previouslyPaused;

        [Space]
        [Header("Lock-on stuff")]
        public LayerMask enemyLayer;
        public float reticleSize;
        public Texture2D reticleTexture;
        public Transform lockOnTarget;
        public float lockOnSpeed;
        public bool stoppedMove = true;
        public float lockOnRange;
        public bool previouslyLocked;
        public Collider[] colliders;
        [Space]

        [Space]
        public float targetAngleX;
        public float targetAngleY;
        private bool resetCam;
        public float mouseSens;
        public float lookSpeed = 0.1f;
        [Range(0,100)]
        public float sensitivity;
        public float followSpeed = 0.1f;
        [Range(0,100)]
        public float pivotSensitivity;
        private float mouseXInput;
        private float mouseYInput;

        private float targetPosition;
        private float defaultPosition;
        public float lookAngle;
        private float pivotAngle;
        public float minimumPivot = -35;
        public float maximumPivot = 35;

        public float cameraSpheresRadius = 0.2f;
        public float cameraCollisionOffset = 0.2f;
        public float minimumCollisionOffset = 0.2f;

        // Camera shake variables
        private Vector3 origPos;
        private float posDuration;
        private float xPos;
        private float yPos;
        private float zPos;
        private bool resetPos;
        private Quaternion origRot;
        private float rotDuration;
        private float xRot;
        private float yRot;
        private float zRot;
        private bool resetRot;


        private void Awake()
        {
            singleton = this;
            mainCam = Camera.main;
            myTransform = transform;
            defaultPosition = cameraTransform.localPosition.z;
            // ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerScript>();
            playerInput = player.gameObject.GetComponent<PlayerInput>();
            lookAngle = targetAngleX = targetAngleY = 0;
            globalInfo = GlobalInfo.GetGlobalInfo();
            sensSlider.SetValue(globalInfo.sensitivityX);
            pivotSlider.SetValue(globalInfo.sensitivityY);
            sensitivity = globalInfo.sensitivityX;
            pivotSensitivity = globalInfo.sensitivityY;
            origPos = cameraTransform.localPosition;
            origRot = cameraTransform.localRotation;
        }

        public void FollowTarget(float delta)
        {
            Vector3 targetPosition = Vector3.Lerp(myTransform.position, player.rb.position, delta / followSpeed);
            myTransform.position = player.transform.position;
            HandleCameraCollisions(delta);
        }

        public void HandleCameraRotation(float delta)
        {
            targetAngleX += (mouseXInput * (sensitivity / 1000)) / delta;
            if(targetAngleX >= 360){
                targetAngleX -= 360;
            }else if(targetAngleX <= 0){
                targetAngleX += 360;
            }
            
            if(playerInput.currentControlScheme != "Keyboard"){
                pivotSensitivity = globalInfo.sensitivityY * 3;
                sensitivity = globalInfo.sensitivityX * 3;
            }else{
                sensitivity = globalInfo.sensitivityX;
                pivotSensitivity = globalInfo.sensitivityY;
            }
            targetAngleY -= (mouseYInput * (pivotSensitivity / 1000)) / delta;
            targetAngleY = Mathf.Clamp(targetAngleY, minimumPivot, maximumPivot);


            if(lookAngle != targetAngleX){
                lookAngle = Mathf.LerpAngle(lookAngle, targetAngleX, lookSpeed);
                // lookAngle = targetAngleX;
            }
            Vector3 rotation = Vector3.zero;
            rotation.y = lookAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            myTransform.rotation = targetRotation;


            if(pivotAngle != targetAngleY){
                pivotAngle = Mathf.LerpAngle(pivotAngle, targetAngleY, lookSpeed*1.5f);
                // pivotAngle = targetAngleY;
            }
            rotation = Vector3.zero;
            rotation.x = pivotAngle;
            targetRotation = Quaternion.Euler(rotation);
            cameraPivotTransform.localRotation = targetRotation;
        }
     
        private void HandleCameraCollisions(float delta)
        {
            
            targetPosition = defaultPosition;
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();
            if (Physics.SphereCast(cameraPivotTransform.position, cameraSpheresRadius, direction, out hit, Mathf.Abs(targetPosition), ignoreLayers))
            {
                float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetPosition = -(dis - cameraCollisionOffset);

            }
            if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
            {
                targetPosition = -minimumCollisionOffset;
            }
            // cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
            // cameraTransformPosition.z = targetPosition;
            // cameraTransform.localPosition = cameraTransformPosition;
        }

        void LateUpdate()
        {
            if(sensSlider.OnValueChanged()){
                globalInfo.sensitivityX = sensitivity = sensSlider.GetValue();
            }
            if(pivotSlider.OnValueChanged()){
                globalInfo.sensitivityY = pivotSensitivity = pivotSlider.GetValue();
            }
            
            if(lockOnTarget != null){
                // lookSpeed = 0.03f;
                
                Vector3 direction = lockOnTarget.position - player.transform.position;
                
                Vector3 indicatorPos = player.transform.position + (direction * .8f);
                indicatorPos.y = lockOnTarget.position.y;
                
                LookAt(lockOnTarget);

                if(Vector3.Distance(player.transform.position, lockOnTarget.position) > lockOnRange){
                    lockOnTarget = null;
                    previouslyLocked = false;
                }

            }else if(previouslyLocked){
                lockOnTarget = GetClosestEnemy();
                // lookSpeed = 0.03f;
            }else{
                // lookSpeed = 0.1f;
            }

            CameraShake();

            Vector3 playerToCam = cameraTransform.TransformPoint(cameraTransform.localPosition) - player.transform.position;
            RaycastHit hit;
            Physics.Raycast(player.transform.position, playerToCam, out hit, 10, ~ignoreLayers);
            if(hit.collider){
                float desiredZ = Vector3.Distance(player.transform.position, hit.point);
                cameraPositionGoal = origPos;
                cameraPositionGoal.z = -(desiredZ - 01f);
            }else{
                cameraPositionGoal = origPos;
            }

            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, cameraPositionGoal, Time.deltaTime * 4);
            
            previouslyPaused = globalInfo.paused;
        }

        void FixedUpdate()
        {
        }

        #region Camera Controls
        public void MoveCamera(InputAction.CallbackContext context){
            Vector2 value = context.ReadValue<Vector2>();
            value *= Time.deltaTime * mouseSens;
            if(!previouslyPaused && globalInfo.paused){
                Debug.Log("Paused");
                mouseXInput = 0;
                mouseYInput = 0;
                return;
            }
            
            if(context.performed && !(playerInput.currentControlScheme == "Keyboard" && globalInfo.paused)){
                if(playerInput.currentControlScheme == "Keyboard"){
                    Cursor.lockState = CursorLockMode.Locked;
                }else{
                    Cursor.lockState = CursorLockMode.None;
                }

                if(lockOnTarget == null){
                    mouseXInput = value.x;
                    mouseYInput = value.y;
                }else{
                    if(stoppedMove){
                        if(playerInput.currentControlScheme == "Keyboard"){
                            float temp = value.x;
                            value.x = value.y;
                            value.y = temp;
                            if(Mathf.Abs(value.x) < 50f){
                                value.x = 0;
                            }
                        }
                        colliders = Physics.OverlapSphere(player.transform.position, lockOnRange, enemyLayer);
                        int index = 0;
                        for(int i = 0; i < colliders.Length; i++){
                            if(colliders[i].transform == lockOnTarget){
                                index = i;
                            }
                        }

                        if(value.x > 0.5f){
                            index++;
                            if(index < 0){
                                index = colliders.Length - 1;
                            }
                            if(index >= colliders.Length){
                                index = 0;
                            }
                            stoppedMove = false;
                            lockOnTarget = colliders[index].transform;
                        }else if(value.x < -0.5f){
                            index--;
                            if(index < 0){
                                index = colliders.Length - 1;
                            }
                            if(index >= colliders.Length){
                                index = 0;
                            }
                            stoppedMove = false;
                            lockOnTarget = colliders[index].transform;
                        }else{
                            stoppedMove = true;
                        }
                    }

                    if(value.x == 0){
                        stoppedMove = true;
                    }
                }
            }
        }

        public void LockOn(InputAction.CallbackContext context){
            if(context.performed && !globalInfo.paused){
                GetClosestEnemy();

                if(lockOnTarget != null){
                    previouslyLocked = false;
                    lockOnTarget = null;
                    return;
                }

                if(colliders.Length > 0){
                    previouslyLocked = true;
                    lockOnTarget = GetClosestEnemy();
                }else{
                    Quaternion angle = player.rb.rotation;
                    targetAngleX = angle.eulerAngles.y;
                }
            }
        }
        public void LoseLockOn(){
            previouslyLocked = false;
            lockOnTarget = null;
        }

        private Transform GetClosestEnemy(){
            colliders = Physics.OverlapSphere(player.transform.position, lockOnRange, enemyLayer);

            if(colliders.Length > 0){
                float smallestDist = Mathf.Infinity;
                Transform closest = null;
                foreach(Collider coll in colliders){
                    float dist = Vector3.Distance(player.transform.position, coll.transform.position);
                    if(dist < smallestDist){
                        smallestDist = dist;
                        closest = coll.transform;
                    }
                }
            
                return closest;
            }

            return null;
        }

        public void LookAt(Transform target){       
            Vector3 angle = target.position - player.transform.position;
            targetAngleX = Vector3.SignedAngle(Vector3.forward, angle, Vector3.up);
            targetAngleY = Functions.MapValues(Vector3.Distance(player.transform.position, target.position), 0, lockOnRange, 25, 35);
        }
        #endregion

        #region Camera Shake
        public void CameraShake(){
            if(posDuration > 0){
                resetPos = false;
                posDuration -= Time.deltaTime;
                Vector3 random = Random.insideUnitSphere;
                float newX = random.x * xPos;
                float newY = random.y * yPos;
                float newZ = random.z * zPos;
                cameraTransform.localPosition = origPos + new Vector3(newX, newY, newZ);
            }else if(!resetPos){
                resetPos = true;
                cameraTransform.localPosition = origPos;
                xPos = yPos = zPos = 0;
            }

            if(rotDuration > 0){
                resetRot = false;
                rotDuration -= Time.deltaTime;
                Vector3 random = Random.insideUnitSphere;
                float newX = random.x * xRot;
                float newY = random.y * yRot;
                float newZ = random.z * zRot;
                cameraTransform.localRotation = Quaternion.Euler(origRot.eulerAngles + new Vector3(newX, newY, newZ));
            }else if(!resetRot){
                resetRot = true;
                cameraTransform.localRotation = origRot;
                xRot = yRot = zRot = 0;
            }
        }

        public void ShakePosition(float xMag, float yMag, float zMag, float duration){
            origPos = cameraTransform.localPosition;
            xPos = xMag;
            yPos = yMag;
            zPos = zMag;
            posDuration = duration;
        }

        public void ShakeRotation(float xMag, float yMag, float zMag, float duration){
            origRot = cameraTransform.localRotation;
            xRot = xMag;
            yRot = yMag;
            zRot = zMag;
            rotDuration = duration;
        }
        #endregion

        void OnDrawGizmos()
        {
            Transform parent = Selection.activeTransform;
            if(parent){
                if(parent.parent){
                    while(parent.parent.parent != null){
                        parent = parent.parent;
                    }
                    if(parent == transform){
                        Gizmos.DrawWireSphere(player.transform.position, lockOnRange);
                        Gizmos.DrawSphere(cameraPivotTransform.position, .5f);
                        Gizmos.DrawSphere(transform.position, .5f);
                    }
                }
            }
        }

        void OnGUI()
        {
            if(lockOnTarget != null){
                Vector3 reticlePos= mainCam.WorldToScreenPoint(lockOnTarget.transform.position);
                GUI.DrawTexture(new Rect((reticlePos.x - (reticleSize/2f)), (Screen.height - (reticlePos.y + (reticleSize/2f))), reticleSize, reticleSize), reticleTexture);
            }
        }
    }
}