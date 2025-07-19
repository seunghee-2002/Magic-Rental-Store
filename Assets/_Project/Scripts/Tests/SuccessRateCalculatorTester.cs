using UnityEngine;
using MagicRentalShop.Systems;
using MagicRentalShop.Data;
using MagicRentalShop.Core;
using System.Collections.Generic;
using System.Text;

namespace MagicRentalShop.Tests
{
    /// <summary>
    /// 읽기 전용 필드를 위한 어트리뷰트
    /// </summary>
    public class ReadOnlyAttribute : PropertyAttribute { }

    /// <summary>
    /// SuccessRateCalculator 수동 테스트 클래스
    /// 인스펙터의 버튼을 통해 직접 실행하거나 컨텍스트 메뉴로 호출 가능
    /// </summary>
    [System.Serializable]
    public class SuccessRateCalculatorTester : MonoBehaviour
    {
        [Header("테스트 설정")]
        [SerializeField] private bool showDetailedLogs = true;
        [SerializeField] private bool runPerformanceTests = false;
        [SerializeField] private int performanceTestIterations = 1000;
        
        [Header("테스트할 시나리오")]
        [SerializeField] private bool testBasicRates = true;
        [SerializeField] private bool testElementAdvantage = true;
        [SerializeField] private bool testGradeBonus = true;
        [SerializeField] private bool testHeroBonus = true;
        [SerializeField] private bool testEdgeCases = true;
        [SerializeField] private bool testScenarios = true;
        
        [Header("빠른 계산기")]
        [Space(10)]
        [SerializeField] [Range(1, 100)] private int quickAdvLevel = 10;
        [SerializeField] private Element quickAdvElement = Element.Fire;
        [SerializeField] private Grade quickAdvGrade = Grade.Common;
        [SerializeField] private Element quickWeaponElement = Element.Fire;
        [SerializeField] private Grade quickWeaponGrade = Grade.Common;
        [SerializeField] [Range(1, 100)] private int quickDungeonLevel = 10;
        [SerializeField] private Element quickDungeonElement = Element.Ice;
        [SerializeField] private Grade quickDungeonGrade = Grade.Common;
        [SerializeField] private bool quickIsHero = false;
        
        [Space(5)]
        [SerializeField] [ReadOnly] private float lastCalculatedRate = 0f;
        [SerializeField] private bool autoCalculateOnChange = true;
        
        private SuccessRateCalculator calculator;
        private int testsPassed = 0;
        private int testsFailed = 0;
        private List<string> testResults = new List<string>();

        private void Start()
        {
            // 런타임에서 자동 초기화
            InitializeCalculator();
        }

        private void InitializeCalculator()
        {
            if (calculator == null)
            {
                calculator = new SuccessRateCalculator();
                Debug.Log("SuccessRateCalculator 초기화 완료");
            }
        }

        #region 공개 메서드 (인스펙터 버튼용)
        
        [ContextMenu("빠른 성공률 계산")]
        public void QuickCalculate()
        {
            InitializeCalculator();
            
            lastCalculatedRate = calculator.CalculateFinalRate(
                quickAdvLevel, quickAdvElement, quickAdvGrade,
                quickWeaponElement, quickWeaponGrade,
                quickDungeonLevel, quickDungeonElement, quickDungeonGrade,
                quickIsHero);
            
            Debug.Log($"<color=cyan><b>빠른 계산 결과: {lastCalculatedRate:F1}%</b></color>");
            
            var breakdown = calculator.GetDetailedBreakdown(
                quickAdvLevel, quickAdvElement, quickAdvGrade,
                quickWeaponElement, quickWeaponGrade,
                quickDungeonLevel, quickDungeonElement, quickDungeonGrade,
                quickIsHero);
            
            Debug.Log($"조건: Lv.{quickAdvLevel} {quickAdvElement} {quickAdvGrade} + {quickWeaponElement} {quickWeaponGrade} " +
                     $"vs Lv.{quickDungeonLevel} {quickDungeonElement} {quickDungeonGrade} ({(quickIsHero ? "Hero" : "Customer")})");
            Debug.Log($"계산: {breakdown.baseRate:F1}% × {breakdown.elementBonus:F2} × {breakdown.gradeBonus:F2} + {breakdown.specialBonus:F1}% = {breakdown.finalRate:F1}%");
        }
        
        private void OnValidate()
        {
            if (autoCalculateOnChange && Application.isPlaying && calculator != null)
            {
                QuickCalculate();
            }
        }
        [ContextMenu("모든 테스트 실행")]
        public void RunAllTests()
        {
            InitializeCalculator();
            
            Debug.Log("=== SuccessRateCalculator 테스트 시작 ===");
            testsPassed = 0;
            testsFailed = 0;
            testResults.Clear();
            
            var startTime = System.DateTime.Now;
            
            if (testBasicRates) RunBasicRateTests();
            if (testElementAdvantage) RunElementAdvantageTests();
            if (testGradeBonus) RunGradeBonusTests();
            if (testHeroBonus) RunHeroBonusTests();
            if (testEdgeCases) RunEdgeCaseTests();
            if (testScenarios) RunScenarioTests();
            if (runPerformanceTests) RunPerformanceTests();
            
            var endTime = System.DateTime.Now;
            var duration = endTime - startTime;
            
            // 최종 결과 출력
            LogTestSummary(duration);
        }
        
        [ContextMenu("기본 성공률 테스트")]
        public void RunBasicRateTestsOnly()
        {
            InitializeCalculator();
            ResetTestCounters();
            RunBasicRateTests();
            LogTestSummary();
        }
        
        [ContextMenu("속성 상성 테스트")]
        public void RunElementTestsOnly()
        {
            InitializeCalculator();
            ResetTestCounters();
            RunElementAdvantageTests();
            LogTestSummary();
        }
        
        [ContextMenu("Hero 보정 테스트")]
        public void RunHeroTestsOnly()
        {
            InitializeCalculator();
            ResetTestCounters();
            RunHeroBonusTests();
            LogTestSummary();
        }
        
        [ContextMenu("실제 시나리오 테스트")]
        public void RunScenarioTestsOnly()
        {
            InitializeCalculator();
            ResetTestCounters();
            RunScenarioTests();
            LogTestSummary();
        }

        #endregion

        #region 테스트 구현

        private void RunBasicRateTests()
        {
            LogTestCategory("기본 성공률 테스트");
            
            // 테스트 1: 동일 레벨 = 50% 성공률
            TestEqualLevels();
            
            // 테스트 2: 높은 레벨 = 높은 성공률
            TestHigherLevel();
            
            // 테스트 3: 범위 제한 (1~100%)
            TestRangeLimit();
        }
        
        private void RunElementAdvantageTests()
        {
            LogTestCategory("속성 상성 테스트");
            
            // 테스트 1: Fire vs Ice 상성
            TestFireVsIce();
            
            // 테스트 2: 같은 속성 페널티
            TestSameElementPenalty();
            
            // 테스트 3: Light/Dark 특수 보너스
            TestLightDarkBonus();
            
            // 테스트 4: 속성 순환 상성
            TestElementCycle();
        }
        
        private void RunGradeBonusTests()
        {
            LogTestCategory("등급 보정 테스트");
            
            // 테스트 1: 고급 등급 vs 일반 등급
            TestHigherGradeAdvantage();
        }
        
        private void RunHeroBonusTests()
        {
            LogTestCategory("Hero 보정 테스트");
            
            // 테스트 1: Hero vs Customer 차이
            TestHeroVsCustomer();
            
            // 테스트 2: Hero 보정 정확도 (+10%)
            TestHeroBonusAccuracy();
        }
        
        private void RunEdgeCaseTests()
        {
            LogTestCategory("엣지 케이스 테스트");
            
            // 테스트 1: 0 레벨 처리
            TestZeroLevels();
            
            // 테스트 2: 음수 레벨 처리
            TestNegativeLevels();
            
            // 테스트 3: 극단적 조건
            TestExtremeConditions();
        }
        
        private void RunScenarioTests()
        {
            LogTestCategory("실제 시나리오 테스트");
            
            // 시나리오 1: 최적 조건
            TestOptimalScenario();
            
            // 시나리오 2: 최악 조건
            TestWorstScenario();
            
            // 시나리오 3: 균형 조건
            TestBalancedScenario();
            
            // 시나리오 4: 신규 플레이어
            TestNewPlayerScenario();
            
            // 시나리오 5: 고레벨 플레이어
            TestHighLevelScenario();
        }
        
        private void RunPerformanceTests()
        {
            LogTestCategory("성능 테스트");
            
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            for (int i = 0; i < performanceTestIterations; i++)
            {
                calculator.CalculateFinalRate(
                    Random.Range(1, 100), GetRandomElement(), GetRandomGrade(),
                    GetRandomElement(), GetRandomGrade(),
                    Random.Range(1, 100), GetRandomElement(), GetRandomGrade(),
                    Random.value > 0.5f);
            }
            
            stopwatch.Stop();
            
            bool passed = stopwatch.ElapsedMilliseconds < 1000;
            LogTestResult("성능 테스트", 
                $"{performanceTestIterations}번 계산을 {stopwatch.ElapsedMilliseconds}ms에 완료", 
                passed);
        }

        #endregion

        #region 개별 테스트 메서드

        private void TestEqualLevels()
        {
            float result = calculator.CalculateFinalRate(
                10, Element.None, Grade.Common,
                Element.None, Grade.Common,
                10, Element.None, Grade.Common,
                false);
            
            bool passed = Mathf.Approximately(result, 50f);
            LogTestResult("동일 레벨 50% 테스트", $"결과: {result:F1}%", passed);
        }
        
        private void TestHigherLevel()
        {
            float highResult = calculator.CalculateFinalRate(
                20, Element.None, Grade.Common,
                Element.None, Grade.Common,
                10, Element.None, Grade.Common,
                false);
                
            float lowResult = calculator.CalculateFinalRate(
                10, Element.None, Grade.Common,
                Element.None, Grade.Common,
                20, Element.None, Grade.Common,
                false);
            
            bool passed = highResult > lowResult;
            LogTestResult("높은 레벨 우위 테스트", 
                $"고레벨: {highResult:F1}%, 저레벨: {lowResult:F1}%", passed);
        }
        
        private void TestRangeLimit()
        {
            // 극단적 조건들로 테스트
            var testCases = new[]
            {
                new { adv = 1, dung = 100, desc = "최악 조건" },
                new { adv = 100, dung = 1, desc = "최적 조건" },
                new { adv = 50, dung = 50, desc = "균형 조건" }
            };
            
            bool allPassed = true;
            foreach (var testCase in testCases)
            {
                float result = calculator.CalculateFinalRate(
                    testCase.adv, Element.Fire, Grade.Legendary,
                    Element.Fire, Grade.Legendary,
                    testCase.dung, Element.Ice, Grade.Common,
                    true);
                
                bool inRange = result >= 1f && result <= 100f;
                if (!inRange) allPassed = false;
                
                if (showDetailedLogs)
                {
                    Debug.Log($"  {testCase.desc}: {result:F1}% (범위 내: {inRange})");
                }
            }
            
            LogTestResult("범위 제한 테스트", "1~100% 범위 준수", allPassed);
        }
        
        private void TestFireVsIce()
        {
            float fireVsIce = calculator.CalculateFinalRate(
                10, Element.Fire, Grade.Common,
                Element.Fire, Grade.Common,
                10, Element.Ice, Grade.Common,
                false);
                
            float iceVsFire = calculator.CalculateFinalRate(
                10, Element.Ice, Grade.Common,
                Element.Ice, Grade.Common,
                10, Element.Fire, Grade.Common,
                false);
            
            bool passed = fireVsIce > iceVsFire;
            LogTestResult("Fire vs Ice 상성", 
                $"Fire→Ice: {fireVsIce:F1}%, Ice→Fire: {iceVsFire:F1}%", passed);
        }
        
        private void TestSameElementPenalty()
        {
            float sameElement = calculator.CalculateFinalRate(
                10, Element.Fire, Grade.Common,
                Element.Fire, Grade.Common,
                10, Element.Fire, Grade.Common,
                false);
                
            float neutralElement = calculator.CalculateFinalRate(
                10, Element.None, Grade.Common,
                Element.None, Grade.Common,
                10, Element.None, Grade.Common,
                false);
            
            bool passed = sameElement < neutralElement;
            LogTestResult("같은 속성 페널티", 
                $"같은 속성: {sameElement:F1}%, 중립: {neutralElement:F1}%", passed);
        }
        
        private void TestLightDarkBonus()
        {
            float lightAdvantage = calculator.CalculateFinalRate(
                10, Element.Light, Grade.Common,
                Element.Light, Grade.Common,
                10, Element.Fire, Grade.Common,
                false);
                
            float normalElement = calculator.CalculateFinalRate(
                10, Element.Water, Grade.Common,
                Element.Water, Grade.Common,
                10, Element.Fire, Grade.Common,
                false);
            
            bool passed = lightAdvantage > normalElement;
            LogTestResult("Light/Dark 특수 보너스", 
                $"Light: {lightAdvantage:F1}%, 일반: {normalElement:F1}%", passed);
        }
        
        private void TestElementCycle()
        {
            // Fire → Ice → Air → Earth → Thunder → Water → Fire 순환 테스트
            var cycles = new[]
            {
                new { attacker = Element.Fire, defender = Element.Ice, name = "Fire→Ice" },
                new { attacker = Element.Ice, defender = Element.Air, name = "Ice→Air" },
                new { attacker = Element.Air, defender = Element.Earth, name = "Air→Earth" },
                new { attacker = Element.Earth, defender = Element.Thunder, name = "Earth→Thunder" },
                new { attacker = Element.Thunder, defender = Element.Water, name = "Thunder→Water" },
                new { attacker = Element.Water, defender = Element.Fire, name = "Water→Fire" }
            };
            
            bool allPassed = true;
            foreach (var cycle in cycles)
            {
                float advantage = calculator.CalculateFinalRate(
                    10, cycle.attacker, Grade.Common,
                    cycle.attacker, Grade.Common,
                    10, cycle.defender, Grade.Common,
                    false);
                    
                float disadvantage = calculator.CalculateFinalRate(
                    10, cycle.defender, Grade.Common,
                    cycle.defender, Grade.Common,
                    10, cycle.attacker, Grade.Common,
                    false);
                
                bool passed = advantage > disadvantage;
                if (!passed) allPassed = false;
                
                if (showDetailedLogs)
                {
                    Debug.Log($"  {cycle.name}: 유리 {advantage:F1}% vs 불리 {disadvantage:F1}% (통과: {passed})");
                }
            }
            
            LogTestResult("속성 순환 상성", "모든 순환 상성 확인", allPassed);
        }
        
        private void TestHigherGradeAdvantage()
        {
            float legendary = calculator.CalculateFinalRate(
                10, Element.None, Grade.Legendary,
                Element.None, Grade.Legendary,
                10, Element.None, Grade.Common,
                false);
                
            float common = calculator.CalculateFinalRate(
                10, Element.None, Grade.Common,
                Element.None, Grade.Common,
                10, Element.None, Grade.Common,
                false);
            
            bool passed = legendary > common;
            LogTestResult("고급 등급 우위", 
                $"전설: {legendary:F1}%, 일반: {common:F1}%", passed);
        }
        
        private void TestHeroVsCustomer()
        {
            float heroRate = calculator.CalculateFinalRate(
                10, Element.None, Grade.Common,
                Element.None, Grade.Common,
                10, Element.None, Grade.Common,
                true);
                
            float customerRate = calculator.CalculateFinalRate(
                10, Element.None, Grade.Common,
                Element.None, Grade.Common,
                10, Element.None, Grade.Common,
                false);
            
            bool passed = heroRate > customerRate;
            LogTestResult("Hero vs Customer", 
                $"Hero: {heroRate:F1}%, Customer: {customerRate:F1}%", passed);
        }
        
        private void TestHeroBonusAccuracy()
        {
            float heroRate = calculator.CalculateFinalRate(
                10, Element.None, Grade.Common,
                Element.None, Grade.Common,
                10, Element.None, Grade.Common,
                true);
                
            float customerRate = calculator.CalculateFinalRate(
                10, Element.None, Grade.Common,
                Element.None, Grade.Common,
                10, Element.None, Grade.Common,
                false);
            
            float difference = heroRate - customerRate;
            bool passed = Mathf.Approximately(difference, 10f);
            LogTestResult("Hero 보정 정확도", 
                $"차이: {difference:F1}% (예상: 10%)", passed);
        }
        
        private void TestZeroLevels()
        {
            float result = calculator.CalculateFinalRate(
                0, Element.None, Grade.Common,
                Element.None, Grade.Common,
                0, Element.None, Grade.Common,
                false);
            
            bool passed = result >= 1f && result <= 100f;
            LogTestResult("0 레벨 처리", $"결과: {result:F1}%", passed);
        }
        
        private void TestNegativeLevels()
        {
            float negativeResult = calculator.CalculateFinalRate(
                -5, Element.None, Grade.Common,
                Element.None, Grade.Common,
                -3, Element.None, Grade.Common,
                false);
                
            float positiveResult = calculator.CalculateFinalRate(
                1, Element.None, Grade.Common,
                Element.None, Grade.Common,
                1, Element.None, Grade.Common,
                false);
            
            bool passed = Mathf.Approximately(negativeResult, positiveResult);
            LogTestResult("음수 레벨 처리", 
                $"음수: {negativeResult:F1}%, 양수: {positiveResult:F1}%", passed);
        }
        
        private void TestExtremeConditions()
        {
            // 극단적 조건에서도 안정적인지 테스트
            var extremeCases = new[]
            {
                new { adv = 1, dung = 9999, desc = "극단적 불리" },
                new { adv = 9999, dung = 1, desc = "극단적 유리" }
            };
            
            bool allPassed = true;
            foreach (var extreme in extremeCases)
            {
                float result = calculator.CalculateFinalRate(
                    extreme.adv, Element.Fire, Grade.Legendary,
                    Element.Fire, Grade.Legendary,
                    extreme.dung, Element.Ice, Grade.Common,
                    true);
                
                bool passed = result >= 1f && result <= 100f && !float.IsNaN(result) && !float.IsInfinity(result);
                if (!passed) allPassed = false;
                
                if (showDetailedLogs)
                {
                    Debug.Log($"  {extreme.desc}: {result:F1}% (안정성: {passed})");
                }
            }
            
            LogTestResult("극단적 조건 안정성", "범위 내 안정적 결과", allPassed);
        }
        
        private void TestOptimalScenario()
        {
            float result = calculator.CalculateFinalRate(
                50, Element.Fire, Grade.Legendary,    
                Element.Fire, Grade.Legendary,        
                10, Element.Ice, Grade.Common,        
                true);                                
            
            bool passed = result >= 90f;
            LogTestResult("최적 시나리오", $"결과: {result:F1}% (목표: ≥90%)", passed);
        }
        
        private void TestWorstScenario()
        {
            float result = calculator.CalculateFinalRate(
                1, Element.Ice, Grade.Common,         
                Element.Ice, Grade.Common,            
                100, Element.Fire, Grade.Legendary,   
                false);                               
            
            bool passed = result >= 1f && result <= 10f;
            LogTestResult("최악 시나리오", $"결과: {result:F1}% (목표: 1-10%)", passed);
        }
        
        private void TestBalancedScenario()
        {
            float result = calculator.CalculateFinalRate(
                25, Element.Water, Grade.Rare,
                Element.Thunder, Grade.Rare,
                25, Element.Fire, Grade.Rare,
                false);
            
            bool passed = result >= 30f && result <= 80f;
            LogTestResult("균형 시나리오", $"결과: {result:F1}% (목표: 30-80%)", passed);
        }
        
        private void TestNewPlayerScenario()
        {
            // 신규 플레이어: 낮은 레벨, 일반 등급
            float result = calculator.CalculateFinalRate(
                5, Element.None, Grade.Common,
                Element.None, Grade.Common,
                5, Element.None, Grade.Common,
                false);
            
            bool passed = result >= 40f && result <= 60f;
            LogTestResult("신규 플레이어 시나리오", $"결과: {result:F1}% (목표: 40-60%)", passed);
        }
        
        private void TestHighLevelScenario()
        {
            // 고레벨 플레이어: 높은 레벨, 희귀 등급
            float result = calculator.CalculateFinalRate(
                80, Element.Light, Grade.Epic,
                Element.Light, Grade.Epic,
                60, Element.Dark, Grade.Rare,
                true);
            
            bool passed = result >= 70f;
            LogTestResult("고레벨 플레이어 시나리오", $"결과: {result:F1}% (목표: ≥70%)", passed);
        }

        #endregion

        #region 유틸리티 메서드

        private void LogTestCategory(string category)
        {
            Debug.Log($"\n--- {category} ---");
        }
        
        private void LogTestResult(string testName, string details, bool passed)
        {
            string status = passed ? "✓ 통과" : "✗ 실패";
            string message = $"{status}: {testName} - {details}";
            
            if (passed)
                testsPassed++;
            else
                testsFailed++;
            
            testResults.Add(message);
            
            if (showDetailedLogs || !passed)
            {
                if (passed)
                    Debug.Log(message);
                else
                    Debug.LogError(message);
            }
        }
        
        private void LogTestSummary(System.TimeSpan? duration = null)
        {
            Debug.Log("\n=== 테스트 결과 요약 ===");
            Debug.Log($"통과: {testsPassed}개, 실패: {testsFailed}개, 총 {testsPassed + testsFailed}개");
            
            if (duration.HasValue)
            {
                Debug.Log($"실행 시간: {duration.Value.TotalMilliseconds:F0}ms");
            }
            
            if (testsFailed > 0)
            {
                Debug.LogError("일부 테스트가 실패했습니다. 위의 로그를 확인하세요.");
            }
            else
            {
                Debug.Log("모든 테스트가 성공했습니다!");
            }
        }
        
        private void ResetTestCounters()
        {
            testsPassed = 0;
            testsFailed = 0;
            testResults.Clear();
        }
        
        private Element GetRandomElement()
        {
            return (Element)Random.Range(0, System.Enum.GetValues(typeof(Element)).Length);
        }
        
        private Grade GetRandomGrade()
        {
            return (Grade)Random.Range(0, System.Enum.GetValues(typeof(Grade)).Length);
        }

        #endregion

        #region 에디터 전용 메서드

#if UNITY_EDITOR
        [Header("에디터 전용 기능")]
        [SerializeField] private bool showTestButtons = true;
        
        [Header("커스텀 성공률 계산기")]
        [Space(5)]
        [Header("모험가 정보")]
        [SerializeField] private int customAdventurerLevel = 10;
        [SerializeField] private Element customAdventurerElement = Element.Fire;
        [SerializeField] private Grade customAdventurerGrade = Grade.Common;
        
        [Header("무기 정보")]
        [SerializeField] private Element customWeaponElement = Element.Fire;
        [SerializeField] private Grade customWeaponGrade = Grade.Common;
        
        [Header("던전 정보")]
        [SerializeField] private int customDungeonLevel = 10;
        [SerializeField] private Element customDungeonElement = Element.Ice;
        [SerializeField] private Grade customDungeonGrade = Grade.Common;
        
        [Header("특수 설정")]
        [SerializeField] private bool customIsHero = false;
        [SerializeField] private bool showDetailedBreakdown = true;
        
        /// <summary>
        /// 인스펙터에서 설정한 커스텀 조건으로 성공률 계산
        /// </summary>
        [ContextMenu("커스텀 조건 성공률 계산")]
        public void CalculateCustomSuccessRate()
        {
            InitializeCalculator();
            
            Debug.Log("=== 커스텀 성공률 계산 ===");
            Debug.Log($"모험가: Lv.{customAdventurerLevel} {customAdventurerElement} {customAdventurerGrade}");
            Debug.Log($"무기: {customWeaponElement} {customWeaponGrade}");
            Debug.Log($"던전: Lv.{customDungeonLevel} {customDungeonElement} {customDungeonGrade}");
            Debug.Log($"타입: {(customIsHero ? "Hero" : "Customer")}");
            Debug.Log("---");
            
            float result = calculator.CalculateFinalRate(
                customAdventurerLevel, customAdventurerElement, customAdventurerGrade,
                customWeaponElement, customWeaponGrade,
                customDungeonLevel, customDungeonElement, customDungeonGrade,
                customIsHero);
            
            Debug.Log($"<color=yellow><b>최종 성공률: {result:F1}%</b></color>");
            
            if (showDetailedBreakdown)
            {
                var breakdown = calculator.GetDetailedBreakdown(
                    customAdventurerLevel, customAdventurerElement, customAdventurerGrade,
                    customWeaponElement, customWeaponGrade,
                    customDungeonLevel, customDungeonElement, customDungeonGrade,
                    customIsHero);
                
                Debug.Log("=== 상세 계산 과정 ===");
                Debug.Log($"기본 성공률: {breakdown.baseRate:F1}%");
                Debug.Log($"속성 보정: {breakdown.elementBonus:F2}배");
                Debug.Log($"등급 보정: {breakdown.gradeBonus:F2}배");
                Debug.Log($"특수 보정: +{breakdown.specialBonus:F1}%");
                Debug.Log($"계산식: {breakdown.baseRate:F1}% × {breakdown.elementBonus:F2} × {breakdown.gradeBonus:F2} + {breakdown.specialBonus:F1}% = {breakdown.finalRate:F1}%");
            }
            
            Debug.Log("========================");
        }
        
        /// <summary>
        /// 코드로 직접 호출할 수 있는 성공률 계산 메서드
        /// </summary>
        public float CalculateSuccessRate(int advLevel, Element advElement, Grade advGrade,
            Element weaponElement, Grade weaponGrade, int dungeonLevel, Element dungeonElement, 
            Grade dungeonGrade, bool isHero, bool logResult = true)
        {
            InitializeCalculator();
            
            float result = calculator.CalculateFinalRate(
                advLevel, advElement, advGrade,
                weaponElement, weaponGrade,
                dungeonLevel, dungeonElement, dungeonGrade,
                isHero);
            
            if (logResult)
            {
                Debug.Log($"성공률 계산 결과: {result:F1}% " +
                         $"(모험가: Lv.{advLevel} {advElement} {advGrade}, " +
                         $"무기: {weaponElement} {weaponGrade}, " +
                         $"던전: Lv.{dungeonLevel} {dungeonElement} {dungeonGrade}, " +
                         $"타입: {(isHero ? "Hero" : "Customer")})");
            }
            
            return result;
        }
        
        /// <summary>
        /// 상세 분석과 함께 성공률 계산
        /// </summary>
        public SuccessRateBreakdown CalculateWithBreakdown(int advLevel, Element advElement, Grade advGrade,
            Element weaponElement, Grade weaponGrade, int dungeonLevel, Element dungeonElement, 
            Grade dungeonGrade, bool isHero)
        {
            InitializeCalculator();
            
            return calculator.GetDetailedBreakdown(
                advLevel, advElement, advGrade,
                weaponElement, weaponGrade,
                dungeonLevel, dungeonElement, dungeonGrade,
                isHero);
        }
        
        /// <summary>
        /// 레벨 변화에 따른 성공률 비교 분석
        /// </summary>
        [ContextMenu("레벨별 성공률 분석")]
        public void AnalyzeLevelProgression()
        {
            InitializeCalculator();
            
            Debug.Log("=== 레벨별 성공률 분석 ===");
            Debug.Log($"기준: {customAdventurerElement} {customAdventurerGrade} 모험가 vs " +
                     $"Lv.{customDungeonLevel} {customDungeonElement} {customDungeonGrade} 던전");
            Debug.Log($"무기: {customWeaponElement} {customWeaponGrade}");
            Debug.Log("---");
            
            for (int level = 1; level <= 50; level += 5)
            {
                float result = calculator.CalculateFinalRate(
                    level, customAdventurerElement, customAdventurerGrade,
                    customWeaponElement, customWeaponGrade,
                    customDungeonLevel, customDungeonElement, customDungeonGrade,
                    customIsHero);
                
                Debug.Log($"Lv.{level:D2}: {result:F1}%");
            }
            
            Debug.Log("========================");
        }
        
        /// <summary>
        /// 속성 상성별 성공률 비교 분석
        /// </summary>
        [ContextMenu("속성 상성 분석")]
        public void AnalyzeElementAdvantages()
        {
            InitializeCalculator();
            
            Debug.Log("=== 속성 상성 분석 ===");
            Debug.Log($"기준: Lv.{customAdventurerLevel} {customAdventurerGrade} 모험가 vs " +
                     $"Lv.{customDungeonLevel} {customDungeonElement} {customDungeonGrade} 던전");
            Debug.Log($"무기 등급: {customWeaponGrade}");
            Debug.Log("---");
            
            var elements = new[] { Element.None, Element.Fire, Element.Water, Element.Thunder, 
                                 Element.Earth, Element.Air, Element.Ice, Element.Light, Element.Dark };
            
            foreach (Element element in elements)
            {
                float result = calculator.CalculateFinalRate(
                    customAdventurerLevel, element, customAdventurerGrade,
                    element, customWeaponGrade,
                    customDungeonLevel, customDungeonElement, customDungeonGrade,
                    customIsHero);
                
                string advantage = GetAdvantageDescription(element, customDungeonElement);
                Debug.Log($"{element,-8}: {result:F1}% {advantage}");
            }
            
            Debug.Log("========================");
        }
        
        /// <summary>
        /// 등급별 성공률 비교 분석
        /// </summary>
        [ContextMenu("등급별 성공률 분석")]
        public void AnalyzeGradeImpact()
        {
            InitializeCalculator();
            
            Debug.Log("=== 등급별 성공률 분석 ===");
            Debug.Log($"기준: Lv.{customAdventurerLevel} {customAdventurerElement} 모험가 vs " +
                     $"Lv.{customDungeonLevel} {customDungeonElement} {customDungeonGrade} 던전");
            Debug.Log($"무기 속성: {customWeaponElement}");
            Debug.Log("---");
            
            var grades = new[] { Grade.Common, Grade.Uncommon, Grade.Rare, Grade.Epic, Grade.Legendary };
            
            foreach (Grade grade in grades)
            {
                float result = calculator.CalculateFinalRate(
                    customAdventurerLevel, customAdventurerElement, grade,
                    customWeaponElement, grade,
                    customDungeonLevel, customDungeonElement, customDungeonGrade,
                    customIsHero);
                
                Debug.Log($"{grade,-10}: {result:F1}%");
            }
            
            Debug.Log("========================");
        }
        
        /// <summary>
        /// Hero vs Customer 비교 분석
        /// </summary>
        [ContextMenu("Hero vs Customer 비교")]
        public void CompareHeroVsCustomer()
        {
            InitializeCalculator();
            
            Debug.Log("=== Hero vs Customer 비교 ===");
            Debug.Log($"조건: Lv.{customAdventurerLevel} {customAdventurerElement} {customAdventurerGrade} + " +
                     $"{customWeaponElement} {customWeaponGrade} vs " +
                     $"Lv.{customDungeonLevel} {customDungeonElement} {customDungeonGrade}");
            Debug.Log("---");
            
            float customerResult = calculator.CalculateFinalRate(
                customAdventurerLevel, customAdventurerElement, customAdventurerGrade,
                customWeaponElement, customWeaponGrade,
                customDungeonLevel, customDungeonElement, customDungeonGrade,
                false);
            
            float heroResult = calculator.CalculateFinalRate(
                customAdventurerLevel, customAdventurerElement, customAdventurerGrade,
                customWeaponElement, customWeaponGrade,
                customDungeonLevel, customDungeonElement, customDungeonGrade,
                true);
            
            float difference = heroResult - customerResult;
            
            Debug.Log($"Customer: {customerResult:F1}%");
            Debug.Log($"Hero:     {heroResult:F1}%");
            Debug.Log($"차이:     +{difference:F1}%");
            
            Debug.Log("========================");
        }
        
        private string GetAdvantageDescription(Element attacker, Element defender)
        {
            if (attacker == Element.None || defender == Element.None)
                return "(중립)";
            
            if (attacker == defender)
                return "(같은 속성 - 불리)";
            
            if (attacker == Element.Light || attacker == Element.Dark)
                return "(Light/Dark - 유리)";
            
            var advantages = new Dictionary<(Element, Element), string>
            {
                {(Element.Fire, Element.Ice), "(유리)"},
                {(Element.Water, Element.Fire), "(유리)"},
                {(Element.Thunder, Element.Water), "(유리)"},
                {(Element.Earth, Element.Thunder), "(유리)"},
                {(Element.Air, Element.Earth), "(유리)"},
                {(Element.Ice, Element.Air), "(유리)"},
                
                {(Element.Ice, Element.Fire), "(불리)"},
                {(Element.Fire, Element.Water), "(불리)"},
                {(Element.Water, Element.Thunder), "(불리)"},
                {(Element.Thunder, Element.Earth), "(불리)"},
                {(Element.Earth, Element.Air), "(불리)"},
                {(Element.Air, Element.Ice), "(불리)"}
            };
            
            return advantages.TryGetValue((attacker, defender), out string result) ? result : "(중립)";
        }
#endif

        #endregion
    }
}