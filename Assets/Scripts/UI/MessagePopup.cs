using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessagePopup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] Button okButton;

    /// <summary>
    /// 메시지 팝업 세팅
    /// </summary>
    /// <param name="message">보여줄 메시지</param>
    public void Setup(string message)
    {
        messageText.text = message;
        okButton.onClick.RemoveAllListeners();
        okButton.onClick.AddListener(() =>
        {
            Close();
        });
    }

    void Close()
    {
        gameObject.SetActive(false);
        CommonUI.Instance.overlayBlocker.SetActive(false);
    }
}
