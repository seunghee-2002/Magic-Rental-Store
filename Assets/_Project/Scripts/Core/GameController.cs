using UnityEngine;

using MagicRentalShop.Data;
using MagicRentalShop.Systems;

namespace MagicRentalShop.Core
{
    /// <summary>
    /// 게임의 전체 흐름과 상태를 관리하는 메인 컨트롤러
    /// </summary>
    public class GameController : MonoBehaviour
    {
        [Header("게임 데이터")]
        [SerializeField] private PlayerData playerData;

        [Header("게임 설정")]
        [SerializeField] private GameConfig gameConfig;

        // 싱글톤 패턴
        public static GameController Instance { get; private set; }

        // 다른 클래스에서 접근할 속성들
        public PlayerData PlayerData => playerData;
        public int CurrentDay => playerData?.currentDay ?? 1;
        public GamePhase CurrentPhase => playerData?.currentPhase ?? GamePhase.Morning;
        public int CurrentGold => playerData?.gold ?? 0;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeGame();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #region 초기화

        /// <summary>
        /// 게임 초기화
        /// </summary>
        private void InitializeGame()
        {
            // GameConfig 로드
            if (gameConfig == null)
            {
                gameConfig = Resources.Load<GameConfig>("GameConfig");
            }

            // 새 게임 또는 로드된 게임 확인
            if (playerData == null)
            {
                StartNewGame();
            }
        }

        #endregion

        #region 게임 상태 관리

        /// <summary>
        /// 새 게임 시작
        /// </summary>
        public void StartNewGame()
        {
            playerData = new PlayerData();

            // Hero 도감 초기화
            HeroManager.Instance?.InitializeHeroCollection();

            Debug.Log("New game started");
        }

        /// <summary>
        /// 다음 날로 진행
        /// </summary>
        public void AdvanceToNextDay()
        {
            PlayerController.Instance.AdvanceDay();

            // Hero 부상 회복 처리
            HeroManager.Instance?.ProcessDailyRecovery();

            // 아침 페이즈로 설정
            PlayerController.Instance.SetPhase(GamePhase.Morning);

            Debug.Log($"Advanced to day {CurrentDay}");
        }

        /// <summary>
        /// 페이즈 전환
        /// </summary>
        public void ChangePhase(GamePhase newPhase)
        {
            PlayerController.Instance.SetPhase(newPhase);
            Debug.Log($"Phase changed to {newPhase}");
        }

        #endregion

        #region 게임 상태 저장 및 로드

        /// <summary>
        /// 게임 저장
        /// </summary>
        public void SaveGame()
        {
            // 실제 저장 로직은 PersistenceController에서 처리
            Debug.Log("Game saved");
        }

        /// <summary>
        /// 게임 로드
        /// </summary>
        public void LoadGame(PlayerData loadedData)
        {
            playerData = loadedData;

            // 각 매니저들에게 복원 알림
            HeroManager.Instance?.ProcessDailyRecovery(); // 부상 상태 업데이트

            Debug.Log($"Game loaded - Day {CurrentDay}");
        }

        #endregion

        #region 디버깅

        /// <summary>
        /// 게임 상태 유효성 검사
        /// </summary>
        public bool IsGameValid()
        {
            return playerData != null && playerData.IsValid();
        }

        /// <summary>
        /// 디버그 정보 출력
        /// </summary>
        [ContextMenu("Debug Game State")]
        public void DebugGameState()
        {
            if (playerData != null)
            {
                playerData.DebugInfo();
            }
            else
            {
                Debug.Log("PlayerData is null");
            }
        }
        
        #endregion
    }
}