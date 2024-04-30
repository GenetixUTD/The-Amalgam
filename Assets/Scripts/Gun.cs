using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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



/*    public AudioClip ShootSound;
    public AudioClip ClickSound;
    public AudioClip ReloadSound;*/

    private bool isReloading = false;

/*    private AudioSource AudioManager;*/

    public ParticleSystem SmokeEffect;
    public ParticleSystem MuzzleFlash;

    public TextMeshProUGUI ClipCountText;
    public TextMeshProUGUI ReserveCountText;

    public TextMeshProUGUI PauseReserveCountText;

    private float reloadAnimTimer;

    private void Start()
    {
        /*AudioManager = GetComponent<AudioSource>();*/
        UpdateClipCount();
        UpdateReserveCount();
    }

    private void Update()
    {
        reloadAnimTimer += Time.deltaTime;
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

        if(Input.GetMouseButtonDown(1) && !Input.GetKey(KeyCode.LeftShift))
        {
            ADS();
        }
        if(Input.GetMouseButtonUp(1) || Input.GetKey(KeyCode.LeftShift))
        {
            unADS();
        }
        
        if(reloadAnimTimer > 2.767f)
        {
            isReloading = false;
        }
    }

    private void Shoot()
    {
        if (!isReloading)
        {
            if (CurrentMag != 0 && LastShot >= FireRate)
            {
                GunAnimator.SetTrigger("Shooting");
                AkSoundEngine.PostEvent("play_gunshotevent", this.gameObject);
                SmokeEffect.Play();
                MuzzleFlash.Play();
                Instantiate(BulletPrefab, MuzzlePosition.position, this.gameObject.transform.rotation);
                CurrentMag -= 1;
                LastShot = 0;
                /*AudioManager.PlayOneShot(ShootSound);*/
            }
            else
            {
                //Play Click Sound;
            }

            UpdateClipCount();
            UpdateReserveCount();
        }
    }

    private void Reload()
    {
            if (CurrentMag != MaxMagCount && CurrentReserves != 0)
            {
                isReloading = true;
                reloadAnimTimer = 0;
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
                GunAnimator.SetTrigger("Reloading");
                ReloadSFX();
            }
        UpdateClipCount();
        UpdateReserveCount();
    }

    private void ADS()
    {
        this.gameObject.transform.parent.transform.localPosition = new Vector3(0f, -0.103f, 0.246000007f);

    }

    private void unADS()
    {
        this.gameObject.transform.parent.transform.localPosition = new Vector3(0.255939007f, -0.144660413f, 0.424523592f);
    }

    public void IncreaseReserves(int count)
    {
        CurrentReserves += count;
    }

    public void UpdateReserveCount()
    {
        ReserveCountText.SetText("/ " + CurrentReserves);
        PauseReserveCountText.SetText(""+CurrentReserves);
    }

    public void UpdateClipCount()
    {
        ClipCountText.SetText("" + CurrentMag);
    }

    public void ReloadSFX()
    {
        /* AudioManager.PlayOneShot(ReloadSound);*/
        AkSoundEngine.PostEvent("play_gunreloadevent", this.gameObject);
    }
}
