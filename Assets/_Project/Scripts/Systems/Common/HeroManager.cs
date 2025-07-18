using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using MagicRentalShop.Data;
using MagicRentalShop.Core;

namespace MagicRentalShop.Systems
{
    public class HeroManager : MonoBehaviour
    {
        public static HeroManager Instance { get; private set; }

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
        /// Hero 도감 초기화 (새 게임 시)
        /// </summary>
        public void InitializeHeroCollection()
        {
            PlayerData.heroCollection.Clear();

            var allCustomers = DataManager.Instance.GetAllCustomerData();
            foreach (var customer in allCustomers)
            {
                PlayerData.heroCollection[customer.id] = new HeroCollectionData
                {
                    isAcquired = false,
                    acquiredDay = 0,
                    shouldInherit = false
                };
            }
        }

        /// <summary>
        /// Hero 획득 처리
        /// </summary>
        public void AcquireHero(string customerID)
        {
            if (PlayerData.heroCollection.ContainsKey(customerID))
            {
                PlayerData.heroCollection[customerID].isAcquired = true;
                PlayerData.heroCollection[customerID].acquiredDay = PlayerData.currentDay;
                PlayerData.heroCollection[customerID].shouldInherit = true;

                Debug.Log($"Hero acquired: {customerID}");
            }
        }

        /// <summary>
        /// Hero 부상 처리 (InjuryManager 기능 흡수)
        /// </summary>
        public void InjureHero(string heroInstanceID, InjuryType injury)
        {
            // 기존 부상 제거
            PlayerData.injuredHeroes.RemoveAll(i => i.heroInstanceID == heroInstanceID);

            // 새 부상 추가
            var injuryData = new InjuryData
            {
                heroInstanceID = heroInstanceID,
                injuryType = injury,
                injuryStartDay = PlayerData.currentDay,
                returnDay = PlayerData.currentDay + GetInjuryDuration(injury)
            };

            PlayerData.injuredHeroes.Add(injuryData);
            Debug.Log($"Hero {heroInstanceID} injured until day {injuryData.returnDay}");
        }

        /// <summary>
        /// Hero 가용성 확인
        /// </summary>
        public bool IsHeroAvailable(string heroInstanceID)
        {
            var injury = PlayerData.injuredHeroes.FirstOrDefault(i => i.heroInstanceID == heroInstanceID);
            return injury == null || PlayerData.currentDay >= injury.returnDay;
        }

        /// <summary>
        /// 매일 부상 회복 처리
        /// </summary>
        public void ProcessDailyRecovery()
        {
            var recoveredHeroes = PlayerData.injuredHeroes.Where(i => PlayerData.currentDay >= i.returnDay).ToList();

            foreach (var recovered in recoveredHeroes)
            {
                PlayerData.injuredHeroes.Remove(recovered);
                Debug.Log($"Hero {recovered.heroInstanceID} recovered");
            }
        }

        /// <summary>
        /// 방문 가능한 Hero 필터링
        /// </summary>
        public List<CustomerInstance> GetAvailableHeroes()
        {
            return PlayerData.visitingCustomers.Where(customer =>
                customer.IsHero() && IsHeroAvailable(customer.instanceID)
            ).ToList();
        }

        private int GetInjuryDuration(InjuryType injury)
        {
            return injury switch
            {
                InjuryType.Minor => 3,
                InjuryType.Moderate => 7,
                InjuryType.Severe => 14,
                _ => 0
            };
        }
    }
}