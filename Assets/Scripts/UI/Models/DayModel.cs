using UnityEngine;
using System.Collections.Generic;

public class DayModel : DayPhaseModel
{
    public List<CustomerInstance> CurrentCustomers { get; private set; }
    public bool IsPanelActive { get; set; }
    public Dictionary<CustomerInstance, WeaponData> AssignedWeapons { get; private set; }

    public DayModel() : base()
    {
        CurrentCustomers = new List<CustomerInstance>();
        AssignedWeapons = new Dictionary<CustomerInstance, WeaponData>();
    }

    public void GenerateCustomers()
    {
        CurrentCustomers.Clear();
        for (int i = 0; i < GameManager.Instance.customersPerDay; i++)
        {
            Grade grade = GetRandomGrade();
            CustomerData data = GameManager.Instance.GetRandomCustomerByGrade(grade);
            if (data != null)
            {
                CurrentCustomers.Add(new CustomerInstance(data));
            }
        }
        GameManager.Instance.NotifyCustomersChanged();
    }

    private Grade GetRandomGrade()
    {
        float random = Random.value;
        if (random < 0.6f) return Grade.Common;
        if (random < 0.85f) return Grade.Uncommon;
        if (random < 0.95f) return Grade.Rare;
        if (random < 0.99f) return Grade.Epic;
        return Grade.Legendary;
    }

    public void ClearCurrentCustomer()
    {
        CurrentCustomers.Clear();
        GameManager.Instance.NotifyCustomersChanged();
    }

    public bool TryAssignWeapon(CustomerInstance customer, WeaponData weapon)
    {
        if (AssignedWeapons.ContainsKey(customer)) return false;
        AssignedWeapons[customer] = weapon;
        return true;
    }

    public void AssignWeapon(CustomerInstance customer, WeaponData weapon)
    {
        AssignedWeapons[customer] = weapon;
        customer.weapon = new WeaponInstance(weapon, 1);
        AdventureManager.Instance.StartAdventure(customer);
    }

    public void StartNewDay()
    {
        CurrentCustomers.Clear();
        AssignedWeapons.Clear();
        GenerateCustomers();
    }
} 
} 
} 