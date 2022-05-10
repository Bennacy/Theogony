using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        private LayerMask ignoreLayers;
        private PlayerInput playerInput;
        private GlobalInfo globalInfo;
        public CustomSlider sensSlider;
        public CustomSlider pivotSlider;

        public static CameraHandler singleton;

        [Space]
        [Header("Lock-on stuff")]
        public LayerMask enemyLayer;
        public Transform lockOnTarget;
        public GameObject lockOnIndicator;
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
        private Quaternion origRot;
        private float rotDuration;
        private float xRot;
        private float yRot;
        private float zRot;


        private void Awake()
        {
            singleton = this;
            myTransform = transform;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
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
            Vector3 targetPosition = Vector3.Lerp(myTransform.position, player.transform.position, delta / followSpeed);
            myTransform.position = targetPosition;
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
            
            targetAngleY -= (mouseYInput * (pivotSensitivity / 2000)) / delta;
            targetAngleY = Mathf.Clamp(targetAngleY, minimumPivot, maximumPivot);

            Vector3 rotation = Vector3.zero;
            rotation.y = lookAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            myTransform.rotation = targetRotation;

            if(lookAngle != targetAngleX){
                lookAngle = Mathf.LerpAngle(lookAngle, targetAngleX, lookSpeed);
            }
            rotation = Vector3.zero;
            rotation.x = pivotAngle;

            if(pivotAngle != targetAngleY){
                pivotAngle = Mathf.LerpAngle(pivotAngle, targetAngleY, lookSpeed);
            }


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
            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
            cameraTransform.localPosition = cameraTransformPosition;
            
        }

        void LateUpdate()
        {
            if(sensSlider.OnValueChanged()){
                globalInfo.sensitivityX = sensitivity = sensSlider.GetValue();
            }
            if(pivotSlider.OnValueChanged()){
                globalInfo.sensitivityY = pivotSensitivity = pivotSlider.GetValue();
            }
            lockOnIndicator.SetActive(lockOnTarget != null);
            
            if(lockOnTarget != null){
                lookSpeed = 0.01f;
                targetAngleY = 19;
                
                Vector3 direction = lockOnTarget.position - player.transform.position;
                
                Vector3 indicatorPos = player.transform.position + (direction * .8f);
                indicatorPos.y = lockOnTarget.position.y;
                lockOnIndicator.transform.position = indicatorPos;

                Vector3.Normalize(direction);
                targetAngleX = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
                
                if(lookAngle != targetAngleX){
                    lookAngle = Mathf.LerpAngle(lookAngle, targetAngleX, lookSpeed);
                }
                if(Vector3.Distance(player.transform.position, lockOnTarget.position) > lockOnRange){
                    lockOnTarget = null;
                    previouslyLocked = false;
                }
            }else if(previouslyLocked){
                lockOnTarget = GetClosestEnemy();
                lookSpeed = 0.1f;
            }else{
                lookSpeed = 0.1f;
            }

            CameraShake();
        }

        #region Camera Controls
        public void MoveCamera(InputAction.CallbackContext context){
            Vector2 value = context.ReadValue<Vector2>();
            value *= Time.deltaTime * mouseSens;
            
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
                    Debug.Log("Not Sitting - " + targetAngleX);
                }
            }
        }
        private void LockOn(float waitTime){
            float timer = 0;
            while(timer < waitTime){
                Debug.Log(timer);
                timer += Time.deltaTime;
            }
            if(timer > waitTime){
                Quaternion angle = player.rb.rotation;
                targetAngleX = angle.eulerAngles.y;
                Debug.Log("Sitting - " + targetAngleX);
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
            // LockOn(.5f);
            
            targetAngleX = Vector3.SignedAngle(Vector3.forward, target.position - player.transform.position, Vector3.up);
        }
        #endregion

        #region Camera Shake
        public void CameraShake(){
            if(posDuration > 0){
                posDuration -= Time.deltaTime;
                Vector3 random = Random.insideUnitSphere;
                float newX = random.x * xPos;
                float newY = random.y * yPos;
                float newZ = random.z * zPos;
                cameraTransform.localPosition = origPos + new Vector3(newX, newY, newZ);
            }else{
                cameraTransform.localPosition = origPos;
                xPos = yPos = zPos = 0;
            }

            if(rotDuration > 0){
                rotDuration -= Time.deltaTime;
                Vector3 random = Random.insideUnitSphere;
                float newX = random.x * xRot;
                float newY = random.y * yRot;
                float newZ = random.z * zRot;
                cameraTransform.localRotation = Quaternion.Euler(origRot.eulerAngles + new Vector3(newX, newY, newZ));
            }else{
                cameraTransform.localRotation = origRot;
                xRot = yRot = zRot = 0;
            }
        }

        public void ShakePosition(float xMag, float yMag, float zMag, float duration){
            xPos = xMag;
            yPos = yMag;
            zPos = zMag;
            posDuration = duration;
        }

        public void ShakeRotation(float xMag, float yMag, float zMag, float duration){
            xRot = xMag;
            yRot = yMag;
            zRot = zMag;
            rotDuration = duration;
        }
        #endregion

        void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(player.transform.position, lockOnRange);
        }
    }
}