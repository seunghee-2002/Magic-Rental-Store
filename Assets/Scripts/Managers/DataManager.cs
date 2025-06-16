using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    [Header("Data Settings")]
    public TextAsset weaponDataJson;
    public TextAsset customerDataJson;
    public TextAsset dungeonDataJson;
    public TextAsset materialDataJson;

    private List<WeaponData> weaponDataPool;
    private List<CustomerData> customerDataPool;
    private List<DungeonData> dungeonDataPool;
    private List<MaterialData> materialDataPool;
    private GameManager gameManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            gameManager = GameManager.Instance;
            LoadAllData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadAllData()
    {
        LoadWeaponData();
        LoadCustomerData();
        LoadDungeonData();
        LoadMaterialData();
    }

    private void LoadWeaponData()
    {
        if (weaponDataJson == null) return;
        weaponDataPool = JsonUtility.FromJson<WeaponDataList>(weaponDataJson.text).weapons;
    }

    private void LoadCustomerData()
    {
        if (customerDataJson == null) return;
        customerDataPool = JsonUtility.FromJson<CustomerDataList>(customerDataJson.text).customers;
    }

    private void LoadDungeonData()
    {
        if (dungeonDataJson == null) return;
        dungeonDataPool = JsonUtility.FromJson<DungeonDataList>(dungeonDataJson.text).dungeons;
    }

    private void LoadMaterialData()
    {
        if (materialDataJson == null) return;
        materialDataPool = JsonUtility.FromJson<MaterialDataList>(materialDataJson.text).materials;
    }

    public WeaponData GetRandomWeaponData()
    {
        if (weaponDataPool == null || weaponDataPool.Count == 0) return null;
        return weaponDataPool[Random.Range(0, weaponDataPool.Count)];
    }

    public CustomerData GetRandomCustomerData()
    {
        if (customerDataPool == null || customerDataPool.Count == 0) return null;
        return customerDataPool[Random.Range(0, customerDataPool.Count)];
    }

    public DungeonData GetRandomDungeonData()
    {
        if (dungeonDataPool == null || dungeonDataPool.Count == 0) return null;
        return dungeonDataPool[Random.Range(0, dungeonDataPool.Count)];
    }

    public MaterialData GetRandomMaterialData()
    {
        if (materialDataPool == null || materialDataPool.Count == 0) return null;
        return materialDataPool[Random.Range(0, materialDataPool.Count)];
    }

    public WeaponData GetWeaponDataById(string id)
    {
        return weaponDataPool?.Find(w => w.id == id);
    }

    public CustomerData GetCustomerDataById(string id)
    {
        return customerDataPool?.Find(c => c.id == id);
    }

    public DungeonData GetDungeonDataById(string id)
    {
        return dungeonDataPool?.Find(d => d.id == id);
    }

    public MaterialData GetMaterialDataById(string id)
    {
        return materialDataPool?.Find(m => m.id == id);
    }

    public List<WeaponData> GetAllWeaponData()
    {
        return weaponDataPool;
    }

    public List<CustomerData> GetAllCustomerData()
    {
        return customerDataPool;
    }

    public List<DungeonData> GetAllDungeonData()
    {
        return dungeonDataPool;
    }

    public List<MaterialData> GetAllMaterialData()
    {
        return materialDataPool;
    }

    [System.Serializable]
    private class WeaponDataList
    {
        public List<WeaponData> weapons;
    }

    [System.Serializable]
    private class CustomerDataList
    {
        public List<CustomerData> customers;
    }

    [System.Serializable]
    private class DungeonDataList
    {
        public List<DungeonData> dungeons;
    }

    [System.Serializable]
    private class MaterialDataList
    {
        public List<MaterialData> materials;
    }
} 