using UnityEngine;
using System.Collections.Generic;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance { get; private set; }

    [Header("Customer Settings")]
    public int maxCustomersPerDay = 3;
    public float customerSpawnInterval = 5f;
    public float customerStayTime = 30f;

    private List<CustomerInstance> activeCustomers = new List<CustomerInstance>();
    private List<CustomerInstance> waitingCustomers = new List<CustomerInstance>();
    private GameManager gameManager;
    private DataManager dataManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            gameManager = GameManager.Instance;
            dataManager = DataManager.Instance;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitializeDay()
    {
        ClearAllCustomers();
        GenerateDailyCustomers();
    }

    private void ClearAllCustomers()
    {
        foreach (var customer in activeCustomers)
        {
            if (customer != null)
            {
                Destroy(customer.gameObject);
            }
        }
        activeCustomers.Clear();
        waitingCustomers.Clear();
    }

    private void GenerateDailyCustomers()
    {
        int customerCount = Random.Range(1, maxCustomersPerDay + 1);
        for (int i = 0; i < customerCount; i++)
        {
            GenerateCustomer();
        }
    }

    public void GenerateCustomer()
    {
        if (waitingCustomers.Count >= maxCustomersPerDay) return;

        // 고객 데이터 생성
        CustomerData customerData = dataManager.GetRandomCustomerData();
        if (customerData == null) return;

        // 던전 선택
        DungeonData dungeonData = dataManager.GetRandomDungeonData();
        if (dungeonData == null) return;

        // 고객 인스턴스 생성
        CustomerInstance customer = new CustomerInstance
        {
            customerData = customerData,
            dungeon = new DungeonInstance { dungeonData = dungeonData },
            level = Random.Range(1, 10),
            weapon = null
        };

        waitingCustomers.Add(customer);
        gameManager.OnCustomerGenerated?.Invoke(customer);
    }

    public CustomerInstance GetNextCustomer()
    {
        if (waitingCustomers.Count == 0) return null;

        CustomerInstance customer = waitingCustomers[0];
        waitingCustomers.RemoveAt(0);
        activeCustomers.Add(customer);
        return customer;
    }

    public void CompleteCustomerInteraction(CustomerInstance customer)
    {
        if (customer == null) return;

        activeCustomers.Remove(customer);
        if (customer.weapon != null)
        {
            gameManager.OnWeaponRented?.Invoke(customer.weapon);
        }
    }

    public void ReturnCustomerWeapon(CustomerInstance customer)
    {
        if (customer == null || customer.weapon == null) return;

        gameManager.OnWeaponReturned?.Invoke(customer.weapon);
        customer.weapon = null;
    }

    public bool HasWaitingCustomers()
    {
        return waitingCustomers.Count > 0;
    }

    public int GetWaitingCustomerCount()
    {
        return waitingCustomers.Count;
    }

    public List<CustomerInstance> GetActiveCustomers()
    {
        return activeCustomers;
    }
} 