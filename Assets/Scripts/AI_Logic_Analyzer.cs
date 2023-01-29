using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;


public class AI_Logic_Analyzer : MonoBehaviour {

    public int max = 100;
    public float MaxSpeed = 10;
    public int Max_Fresh_hold = 0;
    public float max_Unit_Damage = 100;
    public float max_Unit_Armor = 10;


    public float DisPriority = 0; // Priorities how much distance in between characters and torgets is importent.
    
    public float StrPriority; // Priorities how much Strength between characters is important.
   
    public float PowPriority; // Prioritise how much the Army Power has an offect on decition.

    public float logPriority; // Prioritise how much the Army logistice has an offect on decition.
   
       
    public float func1(float x, float max, float a)
    {
        if (x >= 0)
            return (float)(max * (1 - Math.Pow(Math.E, -x / a)));
        else
            return (float)(-max * (1 - Math.Pow(Math.E, x / a)));
    }

    public float func2(float x, float max, float a)
    {
        if (x >= 0)
            return (float)(-max * (1 - Math.Pow(Math.E, -x / a)));
        else
            return (float)(max * (1 - Math.Pow(Math.E, x / a)));
    }


    public float AnalyzeDis(GameObject sorce ,GameObject drain)
    {
        float sum = 0;
        float speed = sorce.GetComponent<NavMeshAgent>().speed;
        float var_1 = max / Vector3.Distance(sorce.transform.position, drain.transform.position);
        float health = sorce.GetComponent<UnitCharateristics>().Health;
        float maxHealth = sorce.GetComponent<UnitCharateristics>().Max_Health;

        DisPriority = var_1 > DisPriority ? var_1 : DisPriority ;
        MaxSpeed = speed > MaxSpeed  ? speed : MaxSpeed ;

        sum += func1(var_1, max, DisPriority/2);
        sum += func1(speed, max, MaxSpeed / 2);
        sum += func1(health, max, maxHealth / 3);
        sum += UnityEngine.Random.Range(0, 100);
        return sum/4;
    }

    public float AnalyzePri(GameObject main ,GameObject target)
    {
        UnitCharateristics main_st = main.GetComponent<UnitCharateristics>();
        UnitCharateristics target_st = target.GetComponent<UnitCharateristics>();
        float var_1 = 0;
        float dis = Vector3.Distance(main.transform.position, target.transform.position);

        var_1 += func1(main_st.Health, max, main_st.Max_Health * 2);
        var_1 += func1(target_st.Health, max, target_st.Max_Health * 2);
       
        var_1 += UnityEngine.Random.Range(0, max);

        return var_1 / 3;
    }

    public float AnalyzeBiulding(Manager AI_manger, GameObject Biulding, GameObject sponer)
    {
  
        int Cradits = AI_manger.Credit;
        int Energy = AI_manger.Energy;
        UnitCharateristics sponer_Char = sponer.GetComponent<UnitCharateristics>();
        int Army_Power = AI_manger.Army_Power;
        int max_Army_Power = AI_manger.Game_Manger.Max_Army_Power;
        float sum = 0;

        if (Army_Power + sponer_Char.Power > max_Army_Power) // if the overall power of the Army excids the max Power limit set by the game manager. it will return a path value of zero.
            return 0;

        // chack if the AI have enough Cradits to buy this Unit. and if sow is it worthwhile
        if (sponer_Char.Credits <= Cradits) 
            sum += max - func1(sponer_Char.Credits, max, Cradits / 5);

        // chack if the AI have enough Energy to buy this Unit. and if Energy is it worthwhile
        if (sponer_Char.Energy <= Energy)
            sum += max - func1(sponer_Char.Energy, max, Energy / 5);

        max_Unit_Damage = sponer_Char.Damage > max_Unit_Damage ? sponer_Char.Damage : max_Unit_Damage; // if the Unit sponing Damage is biger then the last max damage the the value gets updated
        sum += func1(sponer_Char.Damage, max, max_Unit_Damage/2); // how mutch damage can the sponing unit can produs.

        max_Unit_Armor = sponer_Char.Armor > max_Unit_Armor ? sponer_Char.Armor : max_Unit_Armor; // if the Unit sponing Armor is biger then the last max Armor the the value gets updated
        sum += func1(sponer_Char.Armor, max, max_Unit_Armor / 2); // how mutch Armor can the sponing unit can produs.

        sum += UnityEngine.Random.Range(0, max);

        return sum / 5;
    }
}
