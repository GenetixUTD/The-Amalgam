using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private int CurrentMag;
    private int MaxMagCount = 12;
    private int CurrentReserves;
    private float FireRate;
    private float LastShot;
    private Transform MuzzlePosition;
    private GameObject BulletPrefab;

    private Vector3 CameraPosition;
    private Vector3 ADSCameraPosition;

    private AudioClip ShootSound;
    private AudioClip ClickSound;

    private ParticleSystem SmokeEffect;
    private ParticleSystem MuzzleFlash;

    private void Start()
    {
        CameraPosition = Camera.main.transform.localPosition;
    }

    private void Update()
    {
        if(LastShot < FireRate)
        {
            LastShot += Time.deltaTime;
        }
    }

    private void Shoot()
    {
        if (CurrentMag != 0 && LastShot >= FireRate)
        {
            Instantiate(BulletPrefab, MuzzlePosition);
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
        if(CurrentMag < MaxMagCount && CurrentReserves != 0)
        {
            int temp = MaxMagCount - CurrentMag;
            if (CurrentReserves >= temp)
            {
                CurrentReserves -= temp;
                CurrentMag = MaxMagCount;
            }
            else if(CurrentReserves < temp)
            {
                CurrentMag += temp;
                CurrentReserves = 0;
            }
        }
    }

    private void ADS()
    {
        Camera.main.transform.localPosition = ADSCameraPosition;
    }

    private void unADS()
    {
        Camera.main.transform.localPosition = CameraPosition;
    }
}
