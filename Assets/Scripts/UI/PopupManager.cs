using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public GameObject confirmPopupPrefab;
    public GameObject messagePopupPrefab;

    public void ShowConfirmation(string message, UnityAction onYes, UnityAction onNo)
    {
        GameObject go = Instantiate(confirmPopupPrefab, transform);
        ConfirmPopup popup = go.GetComponent<ConfirmPopup>();
        popup.Setup(message, onYes, onNo);
    }
    public void ShowMessage(string message)
    {
        var go = Instantiate(messagePopupPrefab, transform);
        go.GetComponent<MessagePopup>().Setup(message);
    }
}
