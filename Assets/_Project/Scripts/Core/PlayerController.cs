using UnityEngine;

using MagicRentalShop.Data;
using MagicRentalShop.Systems;

namespace MagicRentalShop.Core
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set; }

        private PlayerData PlayerData => GameController.Instance.PlayerData;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// 골드 추가
        /// </summary>
        public void AddGold(int amount)
        {
            PlayerData.gold += amount;
            Debug.Log($"Gold added: +{amount}, Total: {PlayerData.gold}");
        }

        /// <summary>
        /// 골드 차감
        /// </summary>
        public bool SpendGold(int amount)
        {
            if (PlayerData.gold >= amount)
            {
                PlayerData.gold -= amount;
                Debug.Log($"Gold spent: -{amount}, Remaining: {PlayerData.gold}");
                return true;
            }

            Debug.LogWarning($"Not enough gold: {amount} required, {PlayerData.gold} available");
            return false;
        }

        /// <summary>
        /// 월세 납부
        /// </summary>
        public bool PayRent(int rentAmount)
        {
            if (SpendGold(rentAmount))
            {
                PlayerData.lastRentPaymentDay = PlayerData.currentDay;
                PlayerData.hasShownRentWarning = false;
                Debug.Log($"Rent paid: {rentAmount} gold");
                return true;
            }
            return false;
        }

        /// <summary>
        /// 무기 판매
        /// </summary>
        public bool SellWeapon(WeaponInstance weapon, int sellPrice)
        {
            if (PlayerData.ownedWeapons.Remove(weapon))
            {
                AddGold(sellPrice);
                Debug.Log($"Weapon sold for {sellPrice} gold");
                return true;
            }
            return false;
        }

        /// <summary>
        /// 다음 날로 진행
        /// </summary>
        public void AdvanceDay()
        {
            PlayerData.currentDay++;
            Debug.Log($"Advanced to day {PlayerData.currentDay}");
        }

        /// <summary>
        /// 페이즈 변경
        /// </summary>
        public void SetPhase(GamePhase phase)
        {
            PlayerData.currentPhase = phase;
            Debug.Log($"Phase changed to {phase}");
        }
    }
}