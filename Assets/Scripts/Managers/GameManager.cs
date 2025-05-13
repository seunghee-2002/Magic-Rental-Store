using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("Data Pool")]
    public List<ItemData> testStartItems; // 테스트용 시작 아이템
    public List<string> customerNamePool = new List<string> 
    {   // 고객 이름 풀
        "에린", "바츠", "세리아", "닐스", "루에린" 
    }; 
    public List<string> customerDescPool = new List<string> 
    {   // 고객 설명 풀
        "한때 유명했던 모험가", "초보 연금술사", "던전에서 길 잃은 생존자"
    }; 
    public List<Sprite> iconPool; // 고객 아이콘 풀
    public Customer currentCustomer;
    [Header("Player State")]
    public int playerLevel = 1; // 플레이어 레벨
    public int gold = 1000000;
    public int currentDay = 1;
    [Header("Game State")]
    public DayPhase currentPhase = DayPhase.Morning; // 현재 시간대
    int customersPerDay = 5; // 하루에 생성되는 고객 수
    List<Customer> todayCustomers; // 오늘의 고객 리스트
    Dictionary<Customer, ItemData> assignedItems; // 고객과 대여 아이템 매핑
    List<DayResult> dayResults; // 오늘의 결과 리스트
    [Header("Tax Settings")]
    public int taxAmount = 2000; // 세금 
    int taxInterval = 7; // 세금 주기
    [Header("Unlock Settings")]
    public int blacksmithUnlockGold = 5000; // 대장장이 해금 금액
    public int heroUnlockGold = 8000; // 용사 제작 해금 금액
    [HideInInspector] public bool isBlacksmithUnlocked = false;
    [HideInInspector] public bool isHeroSystemUnlocked = false;

    [Range(0f, 1f)] public float baseSuccessRate = 0.5f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // 예시: 테스트용 초기 인벤토리
        foreach (ItemData item in testStartItems)
            InventoryManager.Instance.AddItem(item, Random.Range(1, 4));

        StartMorning(); // 아침 시작
        UIManager.Instance.UpdateGold(gold);
        UIManager.Instance.UpdatePhase(currentDay, currentPhase);
        UIManager.Instance.UpdateInventoryUI();
    }

    public bool SpendGold(int amount) // 골드 소비
    {
        if (gold < amount)
        {
            UIManager.Instance.DisplayResult("골드가 부족합니다!");
            return false;
        }
        gold -= amount;
        UIManager.Instance.UpdateGold(gold);
        return true;
    }

    public void AddGold(int amount) // 골드 획득
    {
        gold += amount;
        UIManager.Instance.UpdateGold(gold);
    }

    public void NextPhase()
    {
        switch (currentPhase)
        {
            case DayPhase.Morning: StartDay();   break;
            case DayPhase.Day:     StartNight(); break;
            case DayPhase.Night:
            {
                // 세금 체크
                if (currentDay % taxInterval == 0)
                {
                    if (!SpendGold(taxAmount))
                    {
                        UIManager.Instance.DisplayResult($"<color=red>월세 미납! 게임 오버</color>");
                        // TODO: 게임오버 처리 (리셋하거나 종료)
                        return;
                    }
                    UIManager.Instance.DisplayResult($"<color=yellow>월세 {taxAmount}G 납부 완료</color>");
                }
                currentDay++;
                StartMorning();
                break;
            }    
        }

        UIManager.Instance.UpdateGold(gold);
        UIManager.Instance.UpdatePhase(currentDay, currentPhase);
        UIManager.Instance.UpdateInventoryUI();
    }

    void StartMorning() // 아침 시작
    {
        currentPhase = DayPhase.Morning;
        // 초기화
        todayCustomers = new List<Customer>();
        assignedItems   = new Dictionary<Customer, ItemData>();
        dayResults      = new List<DayResult>();

        UIManager.Instance.ShowMorningUI();
    }

    void StartDay() // 낮 시작
    {
        currentPhase = DayPhase.Day;
        // 고객 생성
        for (int i = 0; i < customersPerDay; i++)
            todayCustomers.Add(GenerateCustomer());

        UIManager.Instance.ShowDayUI(todayCustomers);
    }

    void StartNight() // 밤 시작
    {
        currentPhase = DayPhase.Night;
        // 결과
        foreach (Customer c in todayCustomers)
        {
            ItemData item = assignedItems.ContainsKey(c) 
                            ? assignedItems[c] 
                            : null;
            DayResult result = SimulateRun(c, item);
            dayResults.Add(result);
        }
        UIManager.Instance.ShowNightUI(dayResults);
    }

    Customer GenerateCustomer() // 고객 생성
    {
        Customer customer = new Customer();
        customer.name = customerNamePool[Random.Range(0, customerNamePool.Count)];
        customer.desc = customerDescPool[Random.Range(0, customerDescPool.Count)];
        customer.level = SetCustomerLevel();
        customer.grade = SetCustomerGrade();
        customer.element = (Element)Random.Range(0, System.Enum.GetValues(typeof(Element)).Length);
        customer.icon = iconPool[Random.Range(0, iconPool.Count)];

        return customer;
    }

    int SetCustomerLevel() // 고객 레벨은 ±1~2 범위에서 조절
    {
        return Mathf.Clamp(playerLevel + Random.Range(-1, 2), 1, 99);
    }

    Grade SetCustomerGrade() // 고객 등급은 ±1~2 범위에서 조절
    {
        switch(playerLevel) {
            case 1:
                return Grade.Common;
            case 2:
                return Grade.Uncommon;
            case 3:
                return Grade.Rare;
            case 4:
                return Grade.Epic;
            case 5:
                return Grade.Legendary;
            default:
                return Grade.Common;
        }
    }

    DayResult SimulateRun(Customer c, ItemData item) // 모험 시뮬레이션
    {
        float baseRate = item == null ? 0 : 0.5f + (c.level - playerLevel)*0.05f;

        bool isSuccess = Random.value < Mathf.Clamp01(baseRate);
        int reward = isSuccess ? 300 : 0;

        if (isSuccess) AddGold(reward); // 보상

        return new DayResult { customer = c, item = item, isSuccess = isSuccess, reward = reward };
    }

    public bool TryAssignItem(Customer c, ItemData item)
    {
        if (!InventoryManager.Instance.UseItem(item)) return false;
        assignedItems[c] = item;
        return true;
    }

    public void PurchaseHeroSystemUnlock() // 용사 시스템 해금
    {
        if (!SpendGold(heroUnlockGold)) return;
        isHeroSystemUnlocked = true;

        UIManager.Instance.DisplayResult("용사 제작 시스템이 해금되었습니다!");
        UIManager.Instance.heroUnlockButton.gameObject.SetActive(false);

        // 이제 “영웅” 메뉴 버튼도 활성화
        UIManager.Instance.OnHeroSystemUnlocked();      
    }

    public void PurchaseBlacksmithSystemUnlock() // 대장장이 시스템 해금
    {
        if (!SpendGold(blacksmithUnlockGold)) return;
        
        isBlacksmithUnlocked = true;

        UIManager.Instance.DisplayResult("대장장이 시스템이 해금되었습니다!");
        UIManager.Instance.blacksmithUnlockButton.gameObject.SetActive(false);

        // 이제 “대장장이” 메뉴 버튼도 활성화
        UIManager.Instance.OnBlacksmithUnlocked();    
    }
}

