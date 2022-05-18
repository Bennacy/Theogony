using UnityEngine;
using UnityEditor;

namespace Theogony{
    [CustomEditor(typeof(BossBarrier))]
    [CanEditMultipleObjects]
    [ExecuteInEditMode]
    public class BossBarrierEditor : Editor
    {
        BossBarrier controller;
        public override void OnInspectorGUI()
        {
            controller = (BossBarrier)target;
            base.OnInspectorGUI();
        }
        
        private void OnSceneGUI() {
            controller = (BossBarrier)target;

            for(int i = 0; i < controller.traversePoints.Length; i++){
                EditorGUI.BeginChangeCheck();

                Vector3 pointPos = controller.traversePoints[i] + controller.transform.position;
                pointPos.y = 0;
                Vector3 pos = Handles.PositionHandle(pointPos, Quaternion.identity);
                
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(controller, "Change Look At Target Position");
                    pos.y += controller.transform.position.y;
                    controller.traversePoints[i] = pos - controller.transform.position;
                }
            }
        }
    }
}