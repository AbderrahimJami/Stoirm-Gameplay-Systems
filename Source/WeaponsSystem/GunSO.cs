using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using UnityEngine;
using UnityEngine.PlayerLoop;

[CreateAssetMenu(fileName = "Gun", menuName = "Guns/ GunSO", order = 0)]
public class GunSO : ScriptableObject
{
    public GunType Type;
    public string Name;
    public GameObject ModelPrefab;
    public Vector3 SpawnPoint;
    public Vector3 SpawnRotation;
    public LayerMask CollisionMask;
    

    public ShootConfigurationSO ShootConfig;
    public DamageConfiguration DamageConfig;
    public AmmoConfigScriptableObject AmmoConfig;


    private MonoBehaviour _activeMonoBehaviour;
    private GameObject _model;
    private float _lastShootTime;
    private ParticleSystem ShootSystem;
    //private ObjectPool TrailPool;

    //Instead of creating a TrailPool for now we'll create an object pool of Bullets PoolableObjects
    public Bullet BulletPrefab;
    private ObjectPool BulletPool;


    private Camera _followCamera;


    public void Spawn(Transform parent, MonoBehaviour activeMonobehaviour, Camera camera)
    {

        //Initialize the pool....

        this._activeMonoBehaviour = activeMonobehaviour;
        _lastShootTime = 0;

        //TrailPool = ObjectPool.CreateInstance();
        _model = Instantiate(ModelPrefab);
        _model.transform.SetParent(parent, false);
        _model.transform.localPosition = SpawnPoint;
        
        _followCamera = camera;

    }

    public void Shoot()
    {
        if(Time.time > ShootConfig.FireRate + _lastShootTime)
        {
            //Guard clause 
            //Check if we're out of ammo in mag
            if(AmmoConfig.CurrentClipAmmo == 0)
                //Idially play out of ammo sound
                return;

            //Trigger shooting animation
            GetAnimator().SetTrigger("OnLMBPressed");


            AmmoConfig.CurrentClipAmmo--;

            _lastShootTime = Time.time;
            
            Vector3 shootDirection = _followCamera.transform.forward
                + new Vector3(
                    Random.Range(-ShootConfig.Spread.x, ShootConfig.Spread.x),
                    Random.Range(- ShootConfig.Spread.y, ShootConfig.Spread.y),
                    Random.Range(-ShootConfig.Spread.z, ShootConfig.Spread.z));
            shootDirection.Normalize();

            

            if (Physics.Raycast(
                    _followCamera.transform.position, shootDirection,
                    out RaycastHit hit,
                    float.MaxValue, layerMask: CollisionMask))
            {

                //Check if collided object is damageable
                if (hit.collider.TryGetComponent(out IDamageable damageable))
                {
                    //If so, deal damage
                    damageable.TakeDamage(DamageConfig.GetDamage(hit.distance));
                }

            }
            else
                Debug.Log("<color=green>I didn't hit anything! </color>");

            Debug.DrawRay(_model.transform.position, shootDirection, Color.red, 3f);
        }   
    }



    /// <summary>
    /// Handle ammo after a reload animation.
    /// ScriptableObjects can't catch Animation Events, which is how we're determining when the
    /// reload has completed, instead of using a timer
    /// </summary>
    public void OnReloadingAnimationFinished()
    {
        //Compute new reloading stats
        AmmoConfig.Reload();
    }


    /// <summary>
    /// Whether or not this gun can be reloaded
    /// </summary>
    /// <returns>Whether or not this gun can be reloaded</returns>
    public bool CanReload()
    {
        return AmmoConfig.CanReload();
    }

    public Animator GetAnimator()
    {
        return _model.GetComponent<Animator>();
    }


    public void ResetAmmo()
    {
        AmmoConfig.CurrentAmmo = AmmoConfig.MaxAmmo;
        AmmoConfig.CurrentClipAmmo = 1;
    }


}
