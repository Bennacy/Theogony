// using UnityEngine;
// using UnityEditor;

// namespace Theogony{
//     [CustomEditor(typeof(EnemyController))]
//     [CanEditMultipleObjects]
//     [ExecuteInEditMode]
//     public class EnemyControllerEditor : Editor
//     {
//         EnemyController controller;
//         public override void OnInspectorGUI()
//         {
//             controller = (EnemyController)target;
//             base.OnInspectorGUI();
//         }
        
//         private void OnSceneGUI() {
//             controller = (EnemyController)target;
//             for(int i = 0; i < controller.patrolWaypoints.Length; i++){
//                 EditorGUI.BeginChangeCheck();

//                 Vector3 pos = Handles.PositionHandle(controller.patrolWaypoints[i], Quaternion.identity);
                
//                 if (EditorGUI.EndChangeCheck())
//                 {
//                     Undo.RecordObject(controller, "Change Look At Target Position");
//                     controller.patrolWaypoints[i] = pos;
//                 }
//             }
//         }
//     }
// }