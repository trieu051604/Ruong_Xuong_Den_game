using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int coinCount = 0;

    public void AddCoin(int amount)
    {
        coinCount += amount;
        Debug.Log("Coin: " + coinCount);
    }
}
