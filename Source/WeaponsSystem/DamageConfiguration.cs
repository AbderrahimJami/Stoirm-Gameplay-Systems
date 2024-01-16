using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//??? why static?
using static UnityEngine.ParticleSystem;



[CreateAssetMenu(fileName = "Damage Config", menuName = "Guns/Damage Config", order = 1)]
public class DamageConfiguration : ScriptableObject
{
    [Tooltip("Determines the amount of damage the weapon deals")]
    public MinMaxCurve DamageCurve;


    /// <summary>
    /// Resets the Damage curve mode to Curve... Whatever that means.. 
    /// </summary>
    public void Reset()
    {
        DamageCurve.mode = ParticleSystemCurveMode.Curve;
    }

    /// <summary>
    /// Returns amount of weapon damage based on distance 
    /// </summary>
    /// <param name="distance">used in the ceiltoint function to compute damage amount from MinMaxCurve</param>
    /// <returns></returns>
    public int GetDamage(float distance = 0)
    {
        return Mathf.CeilToInt(DamageCurve.Evaluate(distance, Random.value)); 
    }




}
