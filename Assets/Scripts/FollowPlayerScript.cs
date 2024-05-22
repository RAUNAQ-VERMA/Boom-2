using UnityEngine;


public class FollowPlayerScript : MonoBehaviour
{
    private Transform targetTransform;
    private Transform cameraTransform;

    public void SetTargetTransform(Transform targetTransform,Transform cameraTransform){
        this.targetTransform = targetTransform;
        this.cameraTransform = cameraTransform;
    }
    void LateUpdate()
    {
        if(targetTransform == null){
            return;
        }
        transform.position = targetTransform.position;
        transform.rotation =  cameraTransform.rotation;
    }
}
