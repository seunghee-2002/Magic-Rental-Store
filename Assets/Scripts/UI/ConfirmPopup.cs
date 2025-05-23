using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ConfirmPopup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;

    /// <summary>
    /// 팝업 세팅
    /// </summary>
    /// <param name="message">보여줄 메시지</param>
    /// <param name="onYes">예를 눌렀을 때 실행될 콜백</param>
    /// <param name="onNo">아니오를 눌렀을 때 실행될 콜백</param>
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

    void Close()
    {
        gameObject.SetActive(false);
        CommonUI.Instance.overlayBlocker.SetActive(false);
    }
}
