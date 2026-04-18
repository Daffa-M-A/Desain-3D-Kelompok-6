using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Settings")]
    public int maxBattery = 8;
    public int currentBattery = 0;

    // Fungsi untuk menambah baterai
    public bool AddBattery()
    {
        if (currentBattery < maxBattery)
        {
            currentBattery++;
            Debug.Log("Baterai disimpan! Total: " + currentBattery + "/" + maxBattery);
            return true; // Berhasil mengambil
        }
        else
        {
            Debug.LogWarning("Inventaris Penuh! Tidak bisa mengambil Battery Box.");
            return false; // Gagal karena penuh
        }
    }
}