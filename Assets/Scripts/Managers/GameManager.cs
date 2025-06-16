using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Data Pool")]
    public List<WeaponData> testStartWeapons;
    public List<string> customerNamePool = new List<string> 
    {   
        "에린", "바츠", "세리아", "닐스", "루에린" 
    }; 
    public List<string> customerDescPool = new List<string> 
    {   
        "한때 유명했던 모험가", "초보 연금술사", "던전에서 길 잃은 생존자"
    }; 
    public List<Sprite> iconPool;

    [Header("Customer Data")]
    public List<CustomerData> commonCustomers;
    public List<CustomerData> uncommonCustomers;
    public List<CustomerData> rareCustomers;
    public List<CustomerData> epicCustomers;
    public List<CustomerData> legendaryCustomers;

    [Header("Game Settings")]
    public float baseSuccessRate = 0.5f;
    public int startingGold = 1000;
    public int maxDay = 30;

    [Header("Managers")]
    public UIManager uiManager;
    public DataManager dataManager;
    public CustomerManager customerManager;
    public WeaponManager weaponManager;
    public AdventureManager adventureManager;

    // 게임 상태
    private int currentGold;
    private int currentDay;
    private GamePhase currentPhase;
    private List<DayResult> dayResults = new List<DayResult>();

    // 이벤트
    public event System.Action<int> OnGoldChanged;
    public event System.Action<int> OnDayChanged;
    public event System.Action<GamePhase> OnPhaseChanged;
    public event System.Action<CustomerInstance> OnCustomerGenerated;
    public event System.Action<WeaponInstance> OnWeaponGenerated;
    public event System.Action<WeaponInstance> OnWeaponRented;
    public event System.Action<WeaponInstance> OnWeaponReturned;
    public event System.Action<WeaponInstance> OnWeaponRepaired;
    public event System.Action<DayResult> OnDayResultAdded;

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

    private void InitializeGame()
    {
        currentGold = startingGold;
        currentDay = 1;
        currentPhase = GamePhase.Morning;

        // 매니저 초기화
        weaponManager.Initialize();
        customerManager.InitializeDay();
        uiManager.ShowMorningUI();

        // 이벤트 발생
        OnGoldChanged?.Invoke(currentGold);
        OnDayChanged?.Invoke(currentDay);
        OnPhaseChanged?.Invoke(currentPhase);
    }

    void Start()
    {
        Invoke("GetTestWeapon", 0.1f);
        StartMorning();
    }

    void GetTestWeapon()
    {
        foreach (WeaponData weapon in testStartWeapons)
            AddWeapon(weapon, Random.Range(1, 4));
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        OnGoldChanged?.Invoke(currentGold);
    }

    public bool SpendGold(int amount)
    {
        if (currentGold < amount) return false;
        currentGold -= amount;
        OnGoldChanged?.Invoke(currentGold);
        return true;
    }

    public void NextDay()
    {
        if (currentDay >= maxDay) return;

        currentDay++;
        OnDayChanged?.Invoke(currentDay);
        customerManager.InitializeDay();
    }

    public void ChangePhase(GamePhase newPhase)
    {
        if (currentPhase == newPhase) return;

        currentPhase = newPhase;
        OnPhaseChanged?.Invoke(currentPhase);

        switch (currentPhase)
        {
            case GamePhase.Morning:
                customerManager.InitializeDay();
                uiManager.ShowMorningUI();
                break;
            case GamePhase.Day:
                adventureManager.ProcessDayEnd();
                uiManager.ShowDayUI();
                break;
            case GamePhase.Night:
                uiManager.ShowNightUI(dayResults);
                break;
        }
    }

    public void AddDayResult(DayResult result)
    {
        dayResults.Add(result);
        OnDayResultAdded?.Invoke(result);
    }

    // 프로퍼티
    public int Gold => currentGold;
    public int CurrentDay => currentDay;
    public GamePhase CurrentPhase => currentPhase;
    public List<DayResult> DayResults => dayResults;

    public enum GamePhase
    {
        Morning,
        Day,
        Night
    }
}

[System.Serializable]
public class DayResult
{
    public string customerName;
    public string dungeonName;
    public bool isSuccess;
    public int reward;
}

