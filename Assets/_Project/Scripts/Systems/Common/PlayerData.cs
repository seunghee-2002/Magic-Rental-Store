using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using MagicRentalShop.Data;

namespace MagicRentalShop.Systems
{
    /// <summary>
    /// 게임의 모든 동적 상태를 저장하는 메인 데이터 클래스
    /// JSON 직렬화 가능하며, 저장/로드 시 이 클래스 하나만으로 게임 전체 상태를 복원 가능
    /// </summary>
    [System.Serializable]
    public class PlayerData
    {
        [Header("기본 게임 정보")]
        public int gold = 5000;
        public int currentDay = 1;
        public GamePhase currentPhase = GamePhase.Morning;
        public float phaseRemainingTime = 0f;

        [Header("게임 진행 상황")]
        public List<string> unlockedRecipeIDs = new List<string>();
        public List<string> purchasedWeaponIDs = new List<string>();
        public int lastShopRefreshDay = 0;

        [Header("인벤토리 시스템")]
        public List<WeaponInstance> ownedWeapons = new List<WeaponInstance>();
        public List<MaterialInstance> ownedMaterials = new List<MaterialInstance>();

        [Header("고객 시스템")]
        public List<CustomerInstance> visitingCustomers = new List<CustomerInstance>();

        [Header("Hero 시스템")]
        public Dictionary<string, HeroCollectionData> heroCollection = new Dictionary<string, HeroCollectionData>();
        public List<InjuryData> injuredHeroes = new List<InjuryData>();

        [Header("모험 시스템")]
        public List<AdventureInstance> ongoingAdventures = new List<AdventureInstance>();
        public List<AdventureResultData> completedAdventures = new List<AdventureResultData>();

        [Header("제작 시스템")]
        public List<CraftingInstance> ongoingCrafting = new List<CraftingInstance>();

        [Header("월세 시스템")]
        public int lastRentPaymentDay = 0;
        public bool hasShownRentWarning = false;

        /// <summary>
        /// 데이터 유효성 검사
        /// </summary>
        public bool IsValid()
        {
            if (gold < 0)
            {
                Debug.LogError("Invalid PlayerData: Negative gold");
                return false;
            }
            
            if (currentDay < 1)
            {
                Debug.LogError("Invalid PlayerData: Invalid current day");
                return false;
            }
            
            return true;
        }

        /// <summary>
        /// 디버그 정보 출력
        /// </summary>
        public void DebugInfo()
        {
            Debug.Log($"=== PlayerData Debug Info ===");
            Debug.Log($"Gold: {gold}, Day: {currentDay}, Phase: {currentPhase}");
            Debug.Log($"Weapons: {ownedWeapons.Count}, Materials: {ownedMaterials.Count}");
            Debug.Log($"Heroes: {heroCollection.Count(kvp => kvp.Value.isAcquired)}");
            Debug.Log($"Injured Heroes: {injuredHeroes.Count}");
            Debug.Log($"Ongoing Adventures: {ongoingAdventures.Count}");
}
    }
}