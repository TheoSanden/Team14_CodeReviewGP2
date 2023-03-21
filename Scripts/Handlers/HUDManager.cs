using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [Header("Player data")]
    public IntVariable PlayerMaxHealth;
    public float UtilityBlinkDuration;

    [Header("Player 1 UI Objects")]
    public SkinnedMeshRenderer Player1Renderer;
    public Image p1HealthFill;

    [Header("Player 2 UI Objects")]
    public SkinnedMeshRenderer Player2Renderer;
    public Image p2HealthFill;
    public void UpdateUtilityCooldownPlayer1(float amount, float maxAmount) => Player1Renderer.material.SetFloat("_DiegeticLerp", amount / maxAmount);
    public void UpdateUtilityCooldownPlayer2(float amount, float maxAmount)=> Player2Renderer.material.SetFloat("_DiegeticLerp", amount/ maxAmount);

    public void FlashUtilityPlayer1() => StartCoroutine(Blink(1));
    public void FlashUtilityPlayer2() => StartCoroutine(Blink(2));

    IEnumerator Blink(int player)
    {
        if (player == 1)
            Player1Renderer.material.SetFloat("_Blink", 1);
        else
            Player2Renderer.material.SetFloat("_Blink", 1);

        yield return new WaitForSeconds(UtilityBlinkDuration);

        if (player == 1)
            Player1Renderer.material.SetFloat("_Blink", 0);
        else
            Player2Renderer.material.SetFloat("_Blink", 0);
    }

    public void UpdateHealthPlayer1(int amount) => p1HealthFill.fillAmount = ((float)amount / PlayerMaxHealth.Value);
    public void UpdateHealthPlayer2(int amount) => p2HealthFill.fillAmount = ((float)amount / PlayerMaxHealth.Value);

    /* [Header("Player1")]
     //health
     public Image health1;
     public IntVariable player1_Maxhealth;
     //fireball
     public Ability_Fireball fireball;
     public FloatVariable fireCD;

     //radiation
     public Ability_Radiation radiation;
     public FloatVariable radCD;

     [Header("Player2")]
     //health
     public Image health2;

     public IntVariable player2_Maxhealth;
     //wind
     public Ability_Wind wind;
     public FloatVariable windCD;

     //sludge
     public Ability_SludgeTrail sludge;
     public FloatVariable sludgeCD;

     //dash 1
     public Ability_Dash dash;
     public FloatVariable dashCD;

     //dash 2
     public Ability_Dash dash2;
     public FloatVariable dashCD2;


     public Image fireFill, radFill, windFill, sludgeFill, dashFill, dashFill2;

     //REMOVE, MOve to seperate health ui scripts
     public void UpdateHealth1(int health)
     {
         health1.fillAmount = (float)health/player1_Maxhealth.Value;
     }

     public void UpdateHealth2(int health)
     {
         health2.fillAmount = (float)health/player2_Maxhealth.Value;
     }
     private void Update()
     {
         fireFill.fillAmount = fireball._cdTimer / fireCD.Value;
         radFill.fillAmount = radiation._cdTimer / radCD.Value;
         windFill.fillAmount = wind._cdTimer / windCD.Value;
         sludgeFill.fillAmount = sludge._cdTimer / sludgeCD.Value;
         dashFill.fillAmount = dash._cdTimer / dashCD.Value;
         dashFill2.fillAmount = dash2._cdTimer / dashCD2.Value;

     }*/
}
