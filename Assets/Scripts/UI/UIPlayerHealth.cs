using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHealth : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private GameObject healthContainer;
    [SerializeField] private GameObject heartHolder;
    private List<GameObject> heartHolders = new List<GameObject>();
    [SerializeField] [Tooltip("This value defines how much HP the player has to lose/gain in order to show the next sprite. " +
                              "The amount of hearts that the player has at the start is defined by: maxHP / stages.length * hpPerStage")]
    private int hpPerStage;
    [SerializeField] private Sprite[] stages;
    [SerializeField] private Sprite emptyStage;
    
    void Start()
    {
        int totalHeartsAmount = (int)(health.MaxHp / stages.Length * hpPerStage);
        for (int i = 0; i < totalHeartsAmount; i++)
        {
            heartHolders.Add(Instantiate(heartHolder, healthContainer.transform));
        }

        health.OnDamaged += UpdateUI;
    }

    private void UpdateUI()
    {
        int currentHeartsAmount = (int)(health.CurrentHp / stages.Length * hpPerStage);
        float currentFractionalHeart = (health.CurrentHp / stages.Length * hpPerStage) - currentHeartsAmount;
        for (int i = 0; i < currentHeartsAmount; i++)
        {
            heartHolders[i].GetComponent<Image>().sprite = stages[stages.Length - 1];
        } 

        float stageSteps = 1f / stages.Length;
        int spriteIndex = (int)(currentFractionalHeart / stageSteps) - 1;
        if (spriteIndex == -1)
        {
            heartHolders[currentHeartsAmount].GetComponent<Image>().sprite = emptyStage;
        }
        else
        {
            heartHolders[currentHeartsAmount].GetComponent<Image>().sprite = stages[spriteIndex];
        }
        
 
        for (int i = currentHeartsAmount + 1; i < stages.Length - 1; i++)
        {
            heartHolders[i].GetComponent<Image>().sprite = emptyStage;
        }
    }
}
