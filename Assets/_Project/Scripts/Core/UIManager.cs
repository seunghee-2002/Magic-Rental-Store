using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

using MagicRentalShop.Data;
using MagicRentalShop.Systems;

namespace MagicRentalShop.Core
{
    /// <summary>
    /// UI 시스템의 중앙 관리자 - 모든 UI 패널과 팝업의 생성, 표시, 소멸을 전담
    /// 싱글톤 패턴으로 구현되어 게임 전체에서 하나의 인스턴스만 존재
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("=== 공통 UI 레이아웃 ===")]
        [SerializeField] private Transform phaseContentArea; // 페이즈 패널이 들어갈 영역 (3/5)
        [SerializeField] private StatusBarView statusBar; // 상태바 (중간 구분선)
        [SerializeField] private InventoryView inventory; // 인벤토리 (2/5, 항상 활성)

        [Header("=== 페이즈 패널 프리팹 ===")]
        [SerializeField] private GameObject morningViewPrefab;
        [SerializeField] private GameObject dayViewPrefab;
        [SerializeField] private GameObject nightViewPrefab;

        [Header("=== 공통 팝업 프리팹 ===")]
        [SerializeField] private AlertPopup alertPopupPrefab;
        [SerializeField] private LoadingPopup loadingPopupPrefab;
        [SerializeField] private SellWeaponPanel sellWeaponPanelPrefab;
        [SerializeField] private RentWarningPanel rentWarningPanelPrefab;

        [Header("=== UI 설정 ===")]
        [SerializeField] private Canvas mainCanvas;
        [SerializeField] private Transform popupParent; // 팝업들이 생성될 부모
        [SerializeField] private float animationDuration = 0.3f;

        // 현재 활성화된 UI 상태
        private GameObject currentPhasePanel;
        private GamePhase currentPhase = GamePhase.Morning;

        // 팝업 관리
        private AlertPopup currentAlertPopup;
        private LoadingPopup currentLoadingPopup;
        private List<GameObject> activePopups = new List<GameObject>();

        // 패널 캐싱 (성능 최적화)
        private Dictionary<GamePhase, GameObject> phasePanelCache = new Dictionary<GamePhase, GameObject>();

        #region Unity Lifecycle

        private void Awake()
        {
            // 싱글톤 패턴 구현
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeUIManager();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            SetupCommonUI();
        }

        #endregion

        #region Initialization

        /// <summary>
        /// UIManager 초기화
        /// </summary>
        private void InitializeUIManager()
        {
            // popupParent가 설정되지 않았다면 mainCanvas를 사용
            if (popupParent == null)
                popupParent = mainCanvas.transform;

            // 팝업 프리팹들 미리 생성 및 비활성화 (오브젝트 풀링)
            if (alertPopupPrefab != null)
            {
                currentAlertPopup = Instantiate(alertPopupPrefab, popupParent);
                currentAlertPopup.gameObject.SetActive(false);
            }

            if (loadingPopupPrefab != null)
            {
                currentLoadingPopup = Instantiate(loadingPopupPrefab, popupParent);
                currentLoadingPopup.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 공통 UI 설정 (게임 시작 시 호출)
        /// </summary>
        public void SetupCommonUI()
        {
            // StatusBar 초기화
            if (statusBar != null)
            {
                statusBar.gameObject.SetActive(true);
                // GameController에서 PlayerData가 로드된 후 UpdateStatusBar 호출됨
            }

            // Inventory 초기화
            if (inventory != null)
            {
                inventory.gameObject.SetActive(true);
                // inventory.SetMode(InventoryMode.Normal);
            }

            Debug.Log("[UIManager] 공통 UI 설정 완료");
        }

        #endregion

        #region Phase Panel Management

        /// <summary>
        /// 페이즈에 따른 패널 전환
        /// </summary>
        /// <param name="phase">전환할 페이즈</param>
        public void SetPhasePanel(GamePhase phase)
        {
            if (currentPhase == phase && currentPhasePanel != null)
                return; // 이미 같은 페이즈면 무시

            StartCoroutine(TransitionPhasePanel(phase));
        }

        /// <summary>
        /// 페이즈 패널 전환 코루틴 (부드러운 애니메이션)
        /// </summary>
        private IEnumerator TransitionPhasePanel(GamePhase newPhase)
        {
            // 기존 패널 페이드 아웃
            if (currentPhasePanel != null)
            {
                yield return StartCoroutine(FadeOut(currentPhasePanel));
                currentPhasePanel.SetActive(false);
            }

            // 새 패널 로드 또는 캐시에서 가져오기
            GameObject newPanel = GetOrCreatePhasePanel(newPhase);

            if (newPanel != null)
            {
                newPanel.SetActive(true);
                newPanel.transform.SetParent(phaseContentArea, false);

                // 새 패널 페이드 인
                yield return StartCoroutine(FadeIn(newPanel));

                currentPhasePanel = newPanel;
                currentPhase = newPhase;

                Debug.Log($"[UIManager] {newPhase} 패널로 전환 완료");
            }
            else
            {
                Debug.LogError($"[UIManager] {newPhase} 패널을 찾을 수 없습니다!");
            }
        }

        /// <summary>
        /// 페이즈 패널 가져오기 또는 생성
        /// </summary>
        private GameObject GetOrCreatePhasePanel(GamePhase phase)
        {
            // 캐시에서 먼저 확인
            if (phasePanelCache.ContainsKey(phase))
            {
                return phasePanelCache[phase];
            }

            // 새로 생성
            GameObject prefab = GetPhasePrefab(phase);
            if (prefab != null)
            {
                GameObject panel = Instantiate(prefab);
                phasePanelCache[phase] = panel;
                return panel;
            }

            return null;
        }

        /// <summary>
        /// 페이즈에 맞는 프리팹 반환
        /// </summary>
        private GameObject GetPhasePrefab(GamePhase phase)
        {
            return phase switch
            {
                GamePhase.Morning => morningViewPrefab,
                GamePhase.Day => dayViewPrefab,
                GamePhase.Night => nightViewPrefab,
                _ => null
            };
        }

        #endregion

        #region Common Popups

        /// <summary>
        /// 알림 팝업 표시
        /// </summary>
        /// <param name="title">제목</param>
        /// <param name="message">메시지</param>
        /// <param name="onOk">확인 버튼 콜백</param>
        public void ShowAlert(string title, string message, Action onOk = null)
        {
            if (currentAlertPopup == null)
            {
                Debug.LogError("[UIManager] AlertPopup이 없습니다!");
                return;
            }

            //currentAlertPopup.Show(title, message, onOk);
            AddToActivePopups(currentAlertPopup.gameObject);
        }

        /// <summary>
        /// 로딩 팝업 표시
        /// </summary>
        /// <param name="message">로딩 메시지</param>
        public void ShowLoading(string message = "로딩 중...")
        {
            if (currentLoadingPopup == null)
            {
                Debug.LogError("[UIManager] LoadingPopup이 없습니다!");
                return;
            }

            //currentLoadingPopup.Show(message);
            AddToActivePopups(currentLoadingPopup.gameObject);
        }

        /// <summary>
        /// 로딩 팝업 숨기기
        /// </summary>
        public void HideLoading()
        {
            if (currentLoadingPopup != null && currentLoadingPopup.gameObject.activeInHierarchy)
            {
                //currentLoadingPopup.Hide();
                RemoveFromActivePopups(currentLoadingPopup.gameObject);
            }
        }

        /// <summary>
        /// 월세 경고 팝업 표시
        /// </summary>
        /// <param name="rentAmount">월세 금액</param>
        /// <param name="currentGold">현재 골드</param>
        public void ShowRentWarning(int rentAmount, int currentGold)
        {
            if (rentWarningPanelPrefab == null)
            {
                Debug.LogError("[UIManager] RentWarningPanel 프리팹이 없습니다!");
                return;
            }

            var rentWarning = Instantiate(rentWarningPanelPrefab, popupParent);
            //rentWarning.Show(rentAmount, currentGold, () =>
            // {
            //     RemoveFromActivePopups(rentWarning.gameObject);
            //     Destroy(rentWarning.gameObject);
            // });

            AddToActivePopups(rentWarning.gameObject);
        }

        /// <summary>
        /// 무기 판매 팝업 표시
        /// </summary>
        /// <param name="weapon">판매할 무기</param>
        public void ShowSellWeaponPanel(WeaponInstance weapon)
        {
            if (sellWeaponPanelPrefab == null)
            {
                Debug.LogError("[UIManager] SellWeaponPanel 프리팹이 없습니다!");
                return;
            }

            var sellPanel = Instantiate(sellWeaponPanelPrefab, popupParent);
            //sellPanel.Show(weapon, () =>
            // {
            //     RemoveFromActivePopups(sellPanel.gameObject);
            //     Destroy(sellPanel.gameObject);
            // });

            AddToActivePopups(sellPanel.gameObject);
        }

        #endregion

        #region Status Update

        /// <summary>
        /// StatusBar 업데이트 (GameController에서 호출)
        /// </summary>
        /// <param name="playerData">플레이어 데이터</param>
        public void UpdateStatusBar(PlayerData playerData)
        {
            if (statusBar != null)
            {
                //statusBar.UpdateDisplay(playerData);
            }
        }

        /// <summary>
        /// 인벤토리 모드 설정
        /// </summary>
        /// <param name="mode">설정할 모드</param>
        public void SetInventoryMode(InventoryMode mode)
        {
            if (inventory != null)
            {
                //inventory.SetMode(mode);
            }
        }

        /// <summary>
        /// 무기 선택을 위한 인벤토리 하이라이트
        /// </summary>
        /// <param name="enable">하이라이트 활성화 여부</param>
        public void HighlightInventoryForSelection(bool enable)
        {
            if (inventory != null)
            {
                //inventory.HighlightForSelection(enable);
            }
        }

        #endregion

        #region Popup Stack Management

        /// <summary>
        /// 활성화된 팝업 목록에 추가
        /// </summary>
        private void AddToActivePopups(GameObject popup)
        {
            if (!activePopups.Contains(popup))
            {
                activePopups.Add(popup);
            }
        }

        /// <summary>
        /// 활성화된 팝업 목록에서 제거
        /// </summary>
        private void RemoveFromActivePopups(GameObject popup)
        {
            activePopups.Remove(popup);
        }

        /// <summary>
        /// 모든 팝업 닫기 (긴급 상황용)
        /// </summary>
        public void CloseAllPopups()
        {
            for (int i = activePopups.Count - 1; i >= 0; i--)
            {
                if (activePopups[i] != null)
                {
                    Destroy(activePopups[i]);
                }
            }
            activePopups.Clear();

            // 공통 팝업들도 숨기기
            HideLoading();
            if (currentAlertPopup != null)
                currentAlertPopup.gameObject.SetActive(false);
        }

        #endregion

        #region Animation Helpers

        /// <summary>
        /// 오브젝트 페이드 인 애니메이션
        /// </summary>
        private IEnumerator FadeIn(GameObject target)
        {
            CanvasGroup canvasGroup = target.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = target.AddComponent<CanvasGroup>();

            canvasGroup.alpha = 0f;
            float elapsedTime = 0f;

            while (elapsedTime < animationDuration)
            {
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / animationDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = 1f;
        }

        /// <summary>
        /// 오브젝트 페이드 아웃 애니메이션
        /// </summary>
        private IEnumerator FadeOut(GameObject target)
        {
            CanvasGroup canvasGroup = target.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                yield break;

            float elapsedTime = 0f;
            float startAlpha = canvasGroup.alpha;

            while (elapsedTime < animationDuration)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / animationDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = 0f;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// 현재 활성화된 페이즈
        /// </summary>
        public GamePhase CurrentPhase => currentPhase;

        /// <summary>
        /// 현재 활성화된 팝업 수
        /// </summary>
        public int ActivePopupCount => activePopups.Count;

        /// <summary>
        /// 인벤토리 뷰 레퍼런스
        /// </summary>
        public InventoryView InventoryView => inventory;

        /// <summary>
        /// 상태바 뷰 레퍼런스
        /// </summary>
        public StatusBarView StatusBarView => statusBar;

        #endregion

        #region Editor Debug

#if UNITY_EDITOR
        [Header("=== 디버그 정보 (Editor Only) ===")]
        [SerializeField] private bool showDebugInfo = true;

        private void OnGUI()
        {
            if (!showDebugInfo) return;

            GUILayout.BeginArea(new Rect(10, 10, 300, 200));
            GUILayout.Label("=== UIManager Debug ===");
            GUILayout.Label($"Current Phase: {currentPhase}");
            GUILayout.Label($"Active Popups: {activePopups.Count}");
            GUILayout.Label($"Phase Panel Cache: {phasePanelCache.Count}");

            if (inventory != null)
                //GUILayout.Label($"Inventory Mode: {inventory.CurrentMode}");

            GUILayout.EndArea();
        }
#endif

        #endregion
    }
}