using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MagicRentalShop.Data;

namespace MagicRentalShop.Systems
{
    /// <summary>
    /// Hero 부상 시스템을 전담 관리하는 클래스
    /// </summary>
    public class InjuryManager : MonoBehaviour
    {
        [Header("부상 데이터")]
        [SerializeField] private List<InjuryData> injuredHeroes = new List<InjuryData>();
        
        // 싱글톤 패턴
        public static InjuryManager Instance { get; private set; }

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
        /// Hero를 부상 상태로 설정
        /// </summary>
        public void InjureHero(string heroInstanceID, InjuryType injury, int currentDay)
        {
            // 기존 부상 기록 제거
            injuredHeroes.RemoveAll(i => i.heroInstanceID == heroInstanceID);
            
            // 새 부상 기록 추가
            var injuryData = new InjuryData
            {
                heroInstanceID = heroInstanceID,
                injuryType = injury,
                injuryStartDay = currentDay,
                returnDay = currentDay + GetInjuryDuration(injury)
            };
            
            injuredHeroes.Add(injuryData);
            Debug.Log($"Hero {heroInstanceID} injured ({injury}) until day {injuryData.returnDay}");
        }
        
        /// <summary>
        /// Hero가 방문 가능한 상태인지 확인
        /// </summary>
        public bool IsHeroAvailableToVisit(string heroInstanceID, int currentDay)
        {
            var injury = injuredHeroes.FirstOrDefault(i => i.heroInstanceID == heroInstanceID);
            return injury == null || currentDay >= injury.returnDay;
        }
    
        /// <summary>
        /// 매일 호출 - 회복된 Hero들 정리
        /// </summary>
        public void ProcessDailyRecovery(int currentDay)
        {
            var recoveredHeroes = injuredHeroes.Where(i => currentDay >= i.returnDay).ToList();
            
            foreach (var recovered in recoveredHeroes)
            {
                injuredHeroes.Remove(recovered);
                Debug.Log($"Hero {recovered.heroInstanceID} recovered from injury");
            }
        }
        
        /// <summary>
        /// 방문 가능한 Hero들만 필터링
        /// </summary>
        public List<CustomerInstance> FilterAvailableHeroes(List<CustomerInstance> allHeroes, int currentDay, PlayerData playerData)
        {
            return allHeroes.Where(hero => 
                hero.IsHero(playerData) && IsHeroAvailableToVisit(hero.instanceID, currentDay)
            ).ToList();
        }
        
        /// <summary>
        /// 부상 기간 계산
        /// </summary>
        private int GetInjuryDuration(InjuryType injury)
        {
            return injury switch
            {
                InjuryType.Minor => 3,    // 경상: 3일
                InjuryType.Moderate => 7, // 중상: 7일
                InjuryType.Severe => 14,  // 중증: 14일
                _ => 0
            };
        }
        
        /// <summary>
        /// 저장용 데이터 반환
        /// </summary>
        public List<InjuryData> GetSaveData()
        {
            return new List<InjuryData>(injuredHeroes);
        }
        
        /// <summary>
        /// 저장 데이터로부터 복원
        /// </summary>
        public void RestoreFromSaveData(List<InjuryData> saveData)
        {
            injuredHeroes = saveData ?? new List<InjuryData>();
            Debug.Log($"Restored {injuredHeroes.Count} injured heroes");
        }
    }
    
    /// <summary>
    /// 부상 데이터 클래스
    /// </summary>
    [System.Serializable]
    public class InjuryData
    {
        public string heroInstanceID;    // 부상당한 Hero ID
        public InjuryType injuryType;    // 부상 종류
        public int injuryStartDay;       // 부상 시작 날짜
        public int returnDay;            // 복귀 예정 날짜
    }
}