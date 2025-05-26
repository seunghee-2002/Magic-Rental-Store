using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessagePopup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageText; // 메시지 텍스트
    [SerializeField] Button okButton; // 확인 버튼

    // 팝업 초기화
    public void Setup(string message)
    {
        messageText.text = message;
        okButton.onClick.RemoveAllListeners();
        okButton.onClick.AddListener(() =>
        {
            Close();
        });
    }

    // 팝업 닫기
    void Close()
    {
        gameObject.SetActive(false);
        CommonUI.Instance.overlayBlocker.SetActive(false);
    }
}
