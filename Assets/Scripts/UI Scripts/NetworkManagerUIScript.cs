using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUIScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Button serverButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    private void Awake()
    {
        serverButton.onClick.AddListener(() =>
        {
            GameMultiplayerScript.Instance.StartHost();
        });
        hostButton.onClick.AddListener(() =>
        {
            GameMultiplayerScript.Instance.StartHost();
        });
        clientButton.onClick.AddListener(() =>
        {
            GameMultiplayerScript.Instance.StartClient();
        });
    }
}
