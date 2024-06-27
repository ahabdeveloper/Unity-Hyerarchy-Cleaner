using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/*
 * AhabDev. 2024.
 * 
 * This script handles complex object hierarchies where objects with MeshRenderers are nested under multiple levels of empty objects. It covers two main steps:
 * 
 * Step 1: Identify and Transfer Objects with MeshRenderers
 * - 'original' refers to the original object hierarchy with nested objects containing MeshRenderers.
 * - Assign 'original' and 'newTransform' with appropriate Transforms.
 * - Use the "STEP 1. Transfer Meshes" button in the Inspector to initiate the transfer process. This moves all objects with MeshRenderers from the 'original' Transform to the 'newTransform', maintaining their world coordinates.
 * 
 * Step 2: Organize and Move Objects with Meshes that have 0 Vertices
 * - 'empty' stores objects with 0-vertex meshes.
 * - Assign 'newTransform' and 'empty' with newly created empty objects.
 * - Use the "STEP 2. Organize Empty Meshes" button in the Inspector to initiate the organization process. This moves all objects with MeshRenderers that have meshes with 0 vertices from the 'newTransform' to the 'empty' Transform.
 * 
 * How to Use:
 * - Attach this script to a GameObject in your scene.
 * - Assign the 'original', 'newTransform', and 'empty' Transforms in the Inspector.
 * - To quickly create and assign the necessary empty objects, use the "STEP 0. Fill in Automatically Necessary Variables" button.
 * - Use the "STEP 1. Transfer Meshes" button to transfer MeshRenderers.
 * - Use the "STEP 2. Organize Empty Meshes" button to move empty meshes.
 * - The script automatically resets the boolean flags after performing the respective operations.
 */

namespace AhabTools
{
    [ExecuteInEditMode]
    public class TransferMeshRenderer : MonoBehaviour
    {
        public Transform original;
        public Transform newTransform;
        public Transform empty;

        private void Transfer()
        {
            if (original == null || newTransform == null)
            {
                Debug.LogWarning("Original or New Transform is not assigned.");
                return;
            }

            MoveMeshRenderers(original);
        }

        private void MoveMeshRenderers(Transform parent)
        {
            foreach (Transform child in parent)
            {
                // Check if the child has a MeshRenderer component
                if (child.GetComponent<MeshRenderer>() != null)
                {
                    // Move the child to newTransform, preserving world coordinates
                    child.SetParent(newTransform, true);
                }

                // Recursively call MoveMeshRenderers for each child
                MoveMeshRenderers(child);
            }
        }

        private void OrganizeEmptyMeshRenderers()
        {
            if (newTransform == null || empty == null)
            {
                Debug.LogWarning("New Transform or Empty Transform is not assigned.");
                return;
            }

            MoveEmptyMeshes(newTransform);
        }

        private void MoveEmptyMeshes(Transform parent)
        {
            foreach (Transform child in parent)
            {
                MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    MeshFilter meshFilter = child.GetComponent<MeshFilter>();
                    if (meshFilter != null && meshFilter.sharedMesh != null)
                    {
                        if (meshFilter.sharedMesh.vertexCount == 0)
                        {
                            // Move the child to empty transform, preserving world coordinates
                            child.SetParent(empty, true);
                            continue; // Skip to the next child since this one has been moved
                        }
                    }
                }

                // Recursively call MoveEmptyMeshes for each child
                MoveEmptyMeshes(child);
            }
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(TransferMeshRenderer))]
        public class TransferMeshRendererEditor : Editor
        {
            #region Foldout booleans
            private bool foldoutVariables = true;
            #endregion

            #region GUI Style variables
            private GUIStyle foldoutHeaderStyle;
            private GUIStyle foldoutContentStyle;
            #endregion

            public override void OnInspectorGUI()
            {
                var transferMeshRenderer = (TransferMeshRenderer)target;

                SetFoldoutStyle();
                serializedObject.Update();

                #region Hyperlink
                GUIStyle hyperlinkStyle = new GUIStyle();
                hyperlinkStyle.normal.textColor = Color.gray;
                hyperlinkStyle.fontStyle = FontStyle.Italic;
                Rect linkRect = EditorGUILayout.GetControlRect();
                if (GUI.Button(linkRect, "Click here visit this tool Github repository for its latest version.", hyperlinkStyle))
                {
                    Application.OpenURL("https://github.com/ahabdeveloper/Unity-Hyerarchy-Cleaner/tree/main");
                }
                #endregion
                DrawInfoBoxes(transferMeshRenderer);
                DrawFoldouts(transferMeshRenderer);
                DrawButtons(transferMeshRenderer);
                serializedObject.ApplyModifiedProperties();
            }

            #region Drawing Foldouts
            private void SetFoldoutStyle()
            {
                if (foldoutHeaderStyle == null)
                {
                    foldoutHeaderStyle = new GUIStyle("ShurikenModuleTitle")
                    {
                        border = new RectOffset(2, 2, 2, 2),
                        fixedHeight = 22,
                        contentOffset = new Vector2(20f, -2f)
                    };
                }

                if (foldoutContentStyle == null)
                {
                    foldoutContentStyle = new GUIStyle(GUI.skin.box)
                    {
                        padding = new RectOffset(1, 1, 1, 1),
                        margin = new RectOffset(1, 1, 1, 1)
                    };
                }
            }

            private void DrawFoldout(ref bool foldoutBool, string title, System.Action drawMethod)
            {
                Rect backgroundRect = GUILayoutUtility.GetRect(1f, 22f, GUILayout.ExpandWidth(true));
                GUI.Box(backgroundRect, GUIContent.none, foldoutHeaderStyle);
                foldoutBool = EditorGUI.Foldout(backgroundRect, foldoutBool, title, true, foldoutHeaderStyle);
                if (foldoutBool)
                {
                    EditorGUI.indentLevel++;
                    EditorGUI.DrawRect(EditorGUILayout.BeginVertical(EditorStyles.helpBox), new Color(0.18f, 0.18f, 0.18f, 1f));
                    drawMethod();
                    EditorGUILayout.EndVertical();
                    EditorGUI.indentLevel--;
                }
            }

            private void DrawFoldouts(TransferMeshRenderer transferMeshRenderer)
            {
                DrawFoldout(ref foldoutVariables, "Control Variables", () =>
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("original"), new GUIContent("Original", "The original object hierarchy where the objects with MeshRenderers are nested."));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("newTransform"), new GUIContent("New Transform", "The new Transform where objects with MeshRenderers will be moved."));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("empty"), new GUIContent("Empty", "The Transform to store objects with 0-vertex meshes."));
                });
            }
            #endregion

            #region Draw Buttons
            private void DrawButtons(TransferMeshRenderer transferMeshRenderer)
            {
                GUILayout.Space(10);

                if (GUILayout.Button(new GUIContent(" STEP 0. Fill in Automatically Necessary Variables", "Creates 'Meshes' and '0 Vertex - Delete' objects in the scene and assigns them to the appropriate variables.")))
                {
                    GameObject meshesObject = new GameObject("Meshes");
                    GameObject zeroVertexObject = new GameObject("0 Vertex - Delete");

                    meshesObject.transform.localScale = new Vector3(100, 100, 100);
                    transferMeshRenderer.newTransform = meshesObject.transform;
                    transferMeshRenderer.empty = zeroVertexObject.transform;

                    Debug.Log("Created 'Meshes' and '0 Vertex - Delete' objects. Don't forget to set the main parent of your current object hierarchy to be fixed and cleaned in the 'Original' slot.");
                }

                if (GUILayout.Button(new GUIContent("STEP 1. Transfer Meshes", "Transfer all objects with MeshRenderers from the 'Original' to the 'New Transform'")))
                {
                    transferMeshRenderer.Transfer();
                }

                if (GUILayout.Button(new GUIContent("STEP 2. Organize Empty Meshes", "Move all objects with MeshRenderers that have meshes with 0 vertices from the 'New Transform' to the 'Empty' Transform")))
                {
                    transferMeshRenderer.OrganizeEmptyMeshRenderers();
                }
            }

            private void DrawInfoBoxes(TransferMeshRenderer transferMeshRenderer)
            {
                EditorGUILayout.HelpBox("Open the script in your default editor to read the full instructions for this Editor Tool.", MessageType.Info);

                if (transferMeshRenderer.original == null)
                {
                    EditorGUILayout.HelpBox("'Original' is empty or null. Please assign the main parent of your current object hierarchy.", MessageType.Warning);
                }

                if (transferMeshRenderer.newTransform == null)
                {
                    EditorGUILayout.HelpBox("'New Transform' is empty or null. Please assign a Transform to move the objects with MeshRenderers.", MessageType.Warning);
                }

                if (transferMeshRenderer.empty == null)
                {
                    EditorGUILayout.HelpBox("'Empty' is empty or null. Please assign a Transform to store objects with 0-vertex meshes.", MessageType.Warning);
                }
            }
            #endregion
        }
#endif
    }

}
