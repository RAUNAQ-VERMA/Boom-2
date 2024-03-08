using UnityEngine;


public class FollowPlayerScript : MonoBehaviour
{
    private Transform targetTransform;

    public void SetTargetTransform(Transform targetTransform){
        this.targetTransform = targetTransform;
    }
    void LateUpdate()
    {
        if(targetTransform == null){
            return;
        }
        transform.position = targetTransform.position;
        transform.rotation =  targetTransform.rotation;
    }
}
