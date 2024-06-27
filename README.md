# Unity-Hyerarchy-Cleaner
/*
 * This script is designed to handle complex object hierarchies where useful objects with MeshRenderers 
 * are nested under multiple levels of empty objects. The script covers two main steps:
 * 
 * Step 1: Identify and Transfer Objects with MeshRenderers
 * - 'original' refers to the original object hierarchy where the objects with MeshRenderers are nested.
 * - Fill the variables 'original' and 'newTransform' with the appropriate Transforms.
 * - Set 'transferMeshes' to true to initiate the transfer process. This will move all objects with 
 *   MeshRenderers from the 'original' Transform to the 'newTransform', maintaining their world coordinates.
 * 
 * Step 2: Organize and Move Objects with Meshes that have 0 Vertices
 * - 'empty' is used to store objects with 0-vertex meshes.
 * - Fill the variables 'newTransform' and 'empty' with newly created empty objects.
 * - Set 'organizeEmptyMeshes' to true to initiate the organization process. This will move all objects 
 *   with MeshRenderers that have meshes with 0 vertices from the 'newTransform' to the 'empty' Transform.
 * 
 * How to Use:
 * - Attach this script to a GameObject in your scene.
 * - Assign the 'original', 'newTransform', and 'empty' Transforms in the Inspector.
 * - To ensure the results are visible in the scene, set the scale of 'newTransform' to 100.
 * - To transfer MeshRenderers, set 'transferMeshes' to true in the Inspector.
 * - To organize and move empty meshes, set 'organizeEmptyMeshes' to true in the Inspector.
 * - The script will automatically reset the boolean flags after performing the respective operations.
 */
