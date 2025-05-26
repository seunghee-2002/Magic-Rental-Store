using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance { get; private set; }

    public List<string> customerNamePool = new List<string>
    {
        "에린", "바츠", "세리아", "닐스", "루에린"
    };
    public List<string> customerDescPool = new List<string>
    {
        "한때 유명했던 모험가", "초보 연금술사", "던전에서 길 잃은 생존자"
    };
    public List<Sprite> iconPool;
    int customersPerDay = 5;
    public List<CustomerInstance> todayCustomers = new List<CustomerInstance>();
    public List<DayResult> dayResults;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void InitCustomer()
    {
        todayCustomers = new List<CustomerInstance>();
        dayResults = new List<DayResult>();
    }

    public void SpawnCustomer()
    {
        for (int i = 0; i < customersPerDay; i++)
            todayCustomers.Add(GenerateCustomer());

        UIManager.Instance.dayUI.ShowDayUI(todayCustomers);
    }

    CustomerInstance GenerateCustomer()
    {
        CustomerData customer = new CustomerData
        {
            name = customerNamePool[Random.Range(0, customerNamePool.Count)],
            desc = customerDescPool[Random.Range(0, customerDescPool.Count)],
            level = SetCustomerLevel(),
            grade = SetCustomerGrade(),
            element = (Element)Random.Range(0, System.Enum.GetValues(typeof(Element)).Length),
            icon = iconPool[Random.Range(0, iconPool.Count)]
        };

        (DungeonData dungeonData, int dungeonLevel) = AdventureManager.Instance.SetDungeon(customer.grade, customer.level);

        CustomerInstance customerInstance = new CustomerInstance
        {
            customerData = customer,
            rentedWeapon = null,
            dungeonData = dungeonData,
            dungeonLevel = dungeonLevel
        };

        return customerInstance;
    }

    int SetCustomerLevel()
    {
        return Mathf.Clamp(GameManager.Instance.playerLevel + Random.Range(-1, 2), 1, 99);
    }

    Grade SetCustomerGrade()
    {
        switch (GameManager.Instance.playerLevel)
        {
            case 1: return Grade.Common;
            case 2: return Grade.Uncommon;
            case 3: return Grade.Rare;
            case 4: return Grade.Epic;
            case 5: return Grade.Legendary;
            default: return Grade.Common;
        }
    }

    public void Result()
    {
        foreach (CustomerInstance c in todayCustomers)
        {
            DayResult result = AdventureManager.Instance.SimulateRun(c);
            dayResults.Add(result);
        }

        UIManager.Instance.nightUI.ShowNightUI(dayResults);
    }

    public bool TryAssignWeapon(WeaponData weapon)
    {
        return InventoryManager.Instance.GetWeaponQuantity(weapon) != 0;
    }

    public void AssignWeapon(CustomerInstance customer, WeaponData weapon)
    {
        if (customer.rentedWeapon != null)
        {
            CommonUI.Instance.ShowMessage($"이미 {customer.rentedWeapon.weaponName}을 대여했습니다.");
            return;
        }
        customer.rentedWeapon = weapon;
        InventoryManager.Instance.UseWeapon(weapon);
    }
}
