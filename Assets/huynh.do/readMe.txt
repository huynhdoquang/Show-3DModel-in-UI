in this extension we use have new layer "3D Model", 
pls check on your layer, if dont have layer "3D Model" pls create new one and add it for:
- 3d model u want renderer 
- adjust on camera setting of 3DModelUICamera : culling mask must have layer "3D Model"
OK set-up have done

==== USEAGE ====
Model3DVisionManager this class is instance: pls create new gameobject with this script
- Model 3D Camera: u can set it null and it will get camera prefab

Create new Raw Image with attach Showing3DModelInUIHelper.cs :
- IsEnableRotate: enable rotate helper  (bool)
- AngularSpeed: Angular Speed of rorate (int)
- IsEnanbleTouch: is enable callback when touch on raw-img  (bool);

So.. UI Set-up done

=== IMPLEMENT IN CODE ===
public function:
- Init(GameObject loadedModel, float scale = 1, int antiAliasing = 0)
