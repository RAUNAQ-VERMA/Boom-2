using UnityEngine;

public class LoaderCallbackScript : MonoBehaviour
{
    private bool isFirstUpdate = true;
    void Update()
    {
        if(isFirstUpdate){
            isFirstUpdate = false;
            Loader.LoaderCallback();
        }
    }
}
