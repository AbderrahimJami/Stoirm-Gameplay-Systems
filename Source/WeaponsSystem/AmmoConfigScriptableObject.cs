using UnityEngine;


[CreateAssetMenu(fileName = "ReloadConfig", menuName = "Guns/Ammo Config")]
public class AmmoConfigScriptableObject : ScriptableObject
{
    public int MaxAmmo = 120;
    public int ClipSize = 20;

    public int CurrentAmmo = 120;
    public int CurrentClipAmmo = 20;


    /// <summary>
    /// Reloads Ammo using conserving algorithm, very popular. Not realistic though
    /// </summary>
    public void Reload()
    {
        //Find the maximum amount of ammo that you can reload based on clipsize and amount of total ammo available
        int maxReloadAmount = Mathf.Min(ClipSize, CurrentAmmo);
        //Find how much bullets from the current clip you shot 
        int availableBulletInCurrentClip = ClipSize - CurrentClipAmmo;
        //Get the amount of ammo to reload by finding the smalles between the amount of ammo you
        //can reload with and the amount of ammo you need to fill current clip
        int reloadAmount = Mathf.Min(maxReloadAmount, availableBulletInCurrentClip);
        //Add the above to the amount of bullets in the current clip
        CurrentClipAmmo += reloadAmount;
        //Reduce the amount of ammo available 
        CurrentAmmo -= reloadAmount;
    }


    /// <summary>
    /// Returns whether the player can actually reload or not based on amount of ammo in the current clip and 
    /// amount of ammo in total left
    /// </summary>
    /// <returns></returns>
    public bool CanReload()
    {
        return CurrentClipAmmo < ClipSize && CurrentAmmo > 0;
    }

    /// <summary>
    /// Adds the provided amount to the Current Weapon total Ammo
    /// </summary>
    /// <param name="amountToAdd">Amount to add to the Current Weapon total ammo</param>
    public void AddAmmo(int amountToAdd)
    {
        if (CurrentAmmo + amountToAdd > MaxAmmo)
            CurrentAmmo = MaxAmmo;
        else
            CurrentAmmo += amountToAdd;
    }



}
