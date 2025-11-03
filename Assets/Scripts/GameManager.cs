using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int currentEnergy;
    [SerializeField] private int energyThreshold = 3;
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject spawner;
    private bool bossCalled = false;
    [SerializeField] private Image energryBar;
    [SerializeField] private GameObject gameUi;

    void Start()
    {
        currentEnergy = 0;
        boss.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddEnergy()
    {
        if (bossCalled) return;
        currentEnergy += 1;
        UpdateEnergyBar();
        if (currentEnergy >= energyThreshold )
        {
            CallBoss(); 
        }
    }
    public void CallBoss()
    {
        bossCalled = true;
        boss.SetActive(true);
        spawner.SetActive(false);
        gameUi.SetActive(false);
    }
    private void UpdateEnergyBar()
    {
        if (energryBar != null)
        {
            float fillAmount = Mathf.Clamp01((float)currentEnergy / (float)energyThreshold);
            energryBar.fillAmount = fillAmount;
        }
    }
}
