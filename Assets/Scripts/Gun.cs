using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private int CurrentMag = 12;
    [SerializeField]
    private int MaxMagCount = 12;
    [SerializeField]
    private int CurrentReserves = 5;
    private float FireRate = .1f;
    private float LastShot;
    public Transform MuzzlePosition;
    public GameObject BulletPrefab;

    public Animator GunAnimator;

    public Camera MainCamera;
    public Camera ADSCamera;

    public AudioClip ShootSound;
    public AudioClip ClickSound;

    public ParticleSystem SmokeEffect;
    public ParticleSystem MuzzleFlash;


    private void Update()
    {
        if(LastShot < FireRate)
        {
            LastShot += Time.deltaTime;
        }

        if(Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        if(Input.GetMouseButtonDown(2) && !Input.GetKeyDown(KeyCode.LeftShift))
        {
            ADS();
        }
        if(Input.GetMouseButtonUp(2))
        {
            unADS();
        }
    }

    private void Shoot()
    {
        if (CurrentMag != 0 && LastShot >= FireRate)
        {
            GunAnimator.SetTrigger("Shooting");
            SmokeEffect.Play();
            MuzzleFlash.Play();
            Instantiate(BulletPrefab, MuzzlePosition.position, this.gameObject.transform.rotation);
            CurrentMag -= 1;
            LastShot = 0;
            //Play Shoot Sound;
        }
        else
        {
            //Play Click Sound;
        }
    }

    private void Reload()
    {
            if (CurrentMag < MaxMagCount && CurrentReserves != 0)
            {
                int temp = MaxMagCount - CurrentMag;
                if (CurrentReserves >= temp)
                {
                    CurrentReserves -= temp;
                    CurrentMag = MaxMagCount;
                }
                else if (CurrentReserves < temp)
                {
                    CurrentMag += CurrentReserves;
                    CurrentReserves = 0;
                }
            }
    }

    private void ADS()
    {
        MainCamera.enabled = false;
        ADSCamera.enabled = true;
    }

    private void unADS()
    {
        MainCamera.enabled = true;
        ADSCamera.enabled = false;
    }
}
