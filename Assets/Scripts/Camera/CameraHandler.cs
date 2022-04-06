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

        public static CameraHandler singleton;

        [Space]
        [Header("Lock-on stuff")]
        public LayerMask enemyLayer;
        public Transform lockOnTarget;
        public GameObject lockOnIndicator;
        public float lockOnSpeed;
        public bool stoppedMove = true;
        public float lockOnRange;
        public Collider[] colliders;
        [Space]

        [Space]
        public float targetAngleX;
        public float targetAngleY;
        private bool resetCam;
        public float mouseSens;
        public float lookSpeed = 0.1f;
        public float followSpeed = 0.1f;
        public float pivotSpeed = 0.03f;
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

      


        private void Awake()
        {
            singleton = this;
            myTransform = transform;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
            playerInput = player.gameObject.GetComponent<PlayerInput>();
            lookAngle = targetAngleX = targetAngleY = 0;
        }

        public void FollowTarget(float delta)
        {
            Vector3 targetPosition = Vector3.Lerp(myTransform.position, player.transform.position, delta / followSpeed);
            myTransform.position = targetPosition;
            HandleCameraCollisions(delta);
        }

        public void HandleCameraRotation(float delta)
        {
            targetAngleX += (mouseXInput * lookSpeed) / delta;
            if(targetAngleX >= 360){
                targetAngleX -= 360;
            }else if(targetAngleX <= 0){
                targetAngleX += 360;
            }
            
            Debug.Log(mouseYInput);
            targetAngleY -= (mouseYInput * pivotSpeed) / delta;
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
            lockOnIndicator.SetActive(lockOnTarget != null);
            
            if(lockOnTarget != null){
                Vector3 direction = lockOnTarget.position - player.transform.position;
                
                lockOnIndicator.transform.position = player.transform.position + (direction * .9f);
                lockOnIndicator.transform.position = new Vector3(lockOnIndicator.transform.position.x, lockOnIndicator.transform.position.y + 1, lockOnIndicator.transform.position.z);

                Vector3.Normalize(direction);
                targetAngleX = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
                
                if(lookAngle != targetAngleX){
                    lookAngle = Mathf.LerpAngle(lookAngle, targetAngleX, lookSpeed);
                }
                if(Vector3.Distance(player.transform.position, lockOnTarget.position) > lockOnRange){
                    lockOnTarget = null;
                }
            }
        }

        public void MoveCamera(InputAction.CallbackContext context){
            Vector2 value = context.ReadValue<Vector2>();
            value *= Time.deltaTime * mouseSens;
            
            if(context.performed){
                if(playerInput.currentControlScheme == "Keyboard"){
                    // Debug.Log(value);
                    Cursor.lockState = CursorLockMode.Locked;
                }else{
                    Cursor.lockState = CursorLockMode.None;
                }

                if(lockOnTarget == null){
                    mouseXInput = value.x;
                    mouseYInput = value.y;
                }else{
                    if(playerInput.currentControlScheme == "Keyboard"){
                        float temp = value.x;
                        value.x = value.y;
                        value.y = temp;
                    }
                    if(stoppedMove){
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
            if(context.performed){
                colliders = Physics.OverlapSphere(player.transform.position, lockOnRange, enemyLayer);
                if(lockOnTarget != null){
                    lockOnTarget = null;
                    return;
                }

                if(colliders.Length > 0){
                    lockOnTarget = colliders[0].transform;
                }else{
                    Quaternion angle = player.rb.rotation;
                    // transform.rotation = angle;
                    targetAngleX = angle.eulerAngles.y;
                }
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(player.transform.position, lockOnRange);
        }
    }

}