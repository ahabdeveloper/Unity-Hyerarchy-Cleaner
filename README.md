# Unity-Hyerarchy-Cleaner

## Description

This script handles complex object hierarchies where objects with MeshRenderers are nested under multiple levels of empty objects. It covers two main steps:

## Step 1: Identify and Transfer Objects with MeshRenderers
- 'original' refers to the original object hierarchy with nested objects containing MeshRenderers.
- Assign **'original'** and **'newTransform'** with appropriate Transforms.
- Use the **"STEP 1. Transfer Meshes"** button in the Inspector to initiate the transfer process. This moves all objects with MeshRenderers from the **'original'** Transform to the **'newTransform'**, maintaining their world coordinates.

## Step 2: Organize and Move Objects with Meshes that have 0 Vertices
- **'empty'** stores objects with 0-vertex meshes.
- Assign **'newTransform'** and **'empty'** with newly created empty objects.
- Use the **"STEP 2. Organize Empty Meshes"** button in the Inspector to initiate the organization process. This moves all objects with MeshRenderers that have meshes with 0 vertices from the **'newTransform'** to the **'empty'** Transform.

## How to Use:
1. Attach this script to a GameObject in your scene.
2. Assign the **'original'**, **'newTransform'**, and **'empty'** Transforms in the Inspector.
3. To quickly create and assign the necessary empty objects, use the **"STEP 0. Fill in Automatically Necessary Variables"** button.
4. Use the **"STEP 1. Transfer Meshes"** button to transfer MeshRenderers.
5. Use the **"STEP 2. Organize Empty Meshes"** button to move empty meshes.

The script automatically resets the boolean flags after performing the respective operations.
