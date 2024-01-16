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

    public Vector3 CameraPosition;
    public Vector3 ADSCameraPosition;

    public AudioClip ShootSound;
    public AudioClip ClickSound;

    public ParticleSystem SmokeEffect;
    public ParticleSystem MuzzleFlash;

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

        if(Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            Reload();
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
        Camera.main.transform.localPosition = ADSCameraPosition;
    }

    private void unADS()
    {
        Camera.main.transform.localPosition = CameraPosition;
    }
}
