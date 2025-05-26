using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ConfirmPopup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageText; // 메시지 텍스트
    [SerializeField] Button yesButton; // 예 버튼
    [SerializeField] Button noButton; // 아니오 버튼

    // 팝업 초기화
    public void Setup(string message, UnityAction onYes, UnityAction onNo)
    {
        messageText.text = message;

        // 버튼 기존 리스너 제거
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        // 새로운 리스너 추가
        yesButton.onClick.AddListener(() =>
        {
            onYes?.Invoke();
            Close();
        });
        noButton.onClick.AddListener(() =>
        {
            onNo?.Invoke();
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
