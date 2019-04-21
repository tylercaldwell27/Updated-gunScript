using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using UnityStandardAssets.Characters.FirstPerson;
public class GunShot : MonoBehaviour
{

    public Slider SpSlider;
    public GameObject health;
    public Transform teleport;
    public Transform spawn;
    //text
    public Text ammoText;
    public Text clipText;
    public Text killText;
    //audio Sounds For the players
    public AudioSource gunfire;
    public AudioSource hitSoundEffect;

    public AudioSource healthPickUp;
    RaycastHit hit;//sets a rayCast to hit

    public int moreAmmo = 50;
    public float kills = 0;

    //this is for damaging the enemy
   
    public float damageEnemy;// the amount of damage the gun will do

    
    public Transform shootPoint;// where the ray cast is going to go from on screen

 
   public int currentAmmo = 20;//ammo left in the gun

    
    public int ClipSize;//the amount of ammo pere clip

  
   public int ClipsLeft = 8;//the amount of clips left to reload

    //Weapon effects
    public ParticleSystem muzzleFlash;

    
    public float rateOfFire;//rate of fire
    public float nextFire = 0;
    public float t = 8000;
    public float T = 8000;
    public float weaponRange;//how far the weapon can shoot
    public int uses = 1;//uses of the Sp
    // stuff for setting up the classes
    public FirstPersonController FPC;
    public MainMenu Menu;
    public PlayerHealth hp;
   
    bool setUp = true;

    //stamina bar
   public float stamina = 10;
    public float maxStamina = 10;
    public float Sprint;
     Rect staminaBar;
     Texture2D staminaTexture;
    

    void Start()
    {
       

        Debug.Log("start of the level");
        staminaBar = new Rect(Screen.width / 10, Screen.height * 9 / 10, Screen.width / 3, Screen.height / 50);
        staminaTexture = new Texture2D(1, 1);
        staminaTexture.SetPixel(0, 0, Color.white);
        staminaTexture.Apply();
  

        setUp = true;
        SetAmmoText();
        SetClipText();
        muzzleFlash.Stop();//turns of the muzzleFlash
        SetKillsText();
       

    }

    void Update(){
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("running");
            stamina -= Time.deltaTime;
            if(stamina < 0)
            {
                stamina = 0;
                FPC.m_RunSpeed = FPC.m_WalkSpeed;
               
            }
        }
        if (stamina < maxStamina && !Input.GetKey(KeyCode.LeftShift))
        {
            stamina += Time.deltaTime;
            FPC.m_RunSpeed = Sprint;
        }

        if (MainMenu.heavy == true && setUp == true)
        {
            t = 15000;
            T = 15000;
            FPC.m_WalkSpeed = 4;
            FPC.m_RunSpeed = 6;
            FPC.m_JumpSpeed = 6;
            hp.health = 200;
            damageEnemy = 10;
            rateOfFire = 0.05f;
            ClipsLeft = ClipsLeft + 25;
            currentAmmo = currentAmmo + 40;
            ClipSize = 60;
            SetAmmoText();
            SetClipText();
            SetKillsText();
            hp.SetHealthText();
            stamina = 5;
            maxStamina = 5;
            hp.AttackSpeed = 2;
            Sprint = FPC.m_RunSpeed;
            setUp = false;
            SpSlider.maxValue = 15000;
            SpSlider.value = 15000;
        }
       else if (MainMenu.balance == true && setUp == true)
        {
            T = 5000;
            t = 5000;
            FPC.m_WalkSpeed = 8;
            FPC.m_RunSpeed = 11;
            FPC.m_JumpSpeed = 8;
            hp.health = 150;
            damageEnemy = 50;
            rateOfFire = 0.2f;
            ClipsLeft = ClipsLeft + 10;
            currentAmmo = 20 ;
            ClipSize = 20;
            SetAmmoText();
            SetClipText();
            SetKillsText();
            hp.SetHealthText();
            stamina = 7.5f;
            maxStamina = 7.5f;
            Sprint = FPC.m_RunSpeed;
            hp.AttackSpeed = 2;
            setUp = false;
            SpSlider.maxValue = 5000;
            SpSlider.value = 5000;
        }
        else if (MainMenu.fast == true && setUp == true)
        {
            t = 12000;
            T = 12000;
            FPC.m_WalkSpeed = 10;
            FPC.m_RunSpeed = 15;
            FPC.m_JumpSpeed = 10;
            hp.health = 100;
            weaponRange = 30;
            damageEnemy = 100;
            rateOfFire = 0.6f;
            ClipsLeft = ClipsLeft + 25;
            currentAmmo = currentAmmo  - 10;
            ClipSize = 10;
            SetAmmoText();
            SetClipText();
            SetKillsText();
            hp.SetHealthText();
            stamina = 10;
            maxStamina = 10;
            Sprint = FPC.m_RunSpeed;
            hp.AttackSpeed = 2;
            setUp = false;
            SpSlider.maxValue = 12000;
            SpSlider.value = 12000;
        }

        else if (MainMenu.medic == true && setUp == true)
        {
            T = 8000;
            t = 8000;
            FPC.m_WalkSpeed = 8;
            FPC.m_RunSpeed = 12;
            FPC.m_JumpSpeed = 8;
            hp.health = 125;
            weaponRange = 200;
            damageEnemy = 20;
            rateOfFire = 0.2f;
            ClipsLeft = ClipsLeft + 15;
            currentAmmo = currentAmmo - 8;
            ClipSize = 12;
            SetAmmoText();
            SetClipText();
            SetKillsText();
            hp.SetHealthText();
            stamina = 8;
            maxStamina = 8;
            Sprint = FPC.m_RunSpeed;
            hp.AttackSpeed = 2;
            SpSlider.maxValue = 8000;
            SpSlider.value = 8000;
            setUp = false;
        }


        if (kills == 50 &&  SceneManager.GetActiveScene().buildIndex == 2)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);// goes to the next scene
          
        }
        
        if (moreAmmo == 0)
        {
            ClipsLeft = ClipsLeft + 30;
            moreAmmo = 50;
        }

        //if the left mouse button is pressed and the gun has ammo in it
        if (Input.GetButton("Fire1") && currentAmmo > 0){
            Shoot();//shoots the gun
            if (currentAmmo == 0)
            {// if the gun is out of ammo

            }
        }
        //MAKING THE CLASS ablites
        if (Input.GetButton("Fire2") && MainMenu.medic == true && t >= 8000)
        {
           
                Instantiate(health, spawn.position, Quaternion.identity);
                t = 0;
  
        }
        if (Input.GetButton("Fire2") && MainMenu.heavy == true && t >= 15000)
        {
            
            rateOfFire = 0.02f;
            ClipSize = 200;
            damageEnemy = 50;
            t = 0;

        }
        if (Input.GetButton("Fire2") && MainMenu.fast == true && t >= 12000)
        {
            FPC.m_WalkSpeed = 20;
            FPC.m_RunSpeed = 25;
            FPC.m_JumpSpeed = 15;

            t = 0;
        }
        if (Input.GetButton("Fire2") && MainMenu.balance == true && t >= 5000)
        {
           
            FPC.m_JumpSpeed = 25;
            t = 0;
        }

        if (t < T)
        {
            t += 2;
            Debug.Log(t);
            SpSlider.value = t;
        }
        if(t > 5000 && t < 14999 && MainMenu.heavy == true)
        {
            rateOfFire = 0.05f;
            ClipSize = 60;
            damageEnemy = 10;
        }
        if (t > 3000 && t < 11999 && MainMenu.fast == true)
        {
            FPC.m_WalkSpeed = 10;
            FPC.m_RunSpeed = 15;
            FPC.m_JumpSpeed = 10;
           
        }
        if (t > 1000 && t < 4999 && MainMenu.balance == true)
        {
           
            FPC.m_JumpSpeed = 8;

        }


        //if the r key is pressed and theres less then 12 bullets in the gun and the player still has more clips for reloading
        if (Input.GetButton("Reload") && currentAmmo < ClipSize && ClipsLeft > 0){
            ClipsLeft = ClipsLeft - 1;//subtracts one clip from ClipsLeft
            SetClipText();
            
            // if the number of ClipsLeft is greater than or equal to zero
            if (ClipsLeft >= 0){
                currentAmmo = ClipSize;//reloads the gun
                Debug.Log("reload");
                SetAmmoText();
            }

            //if the number of ClipsLeft is less than zero
            else if (ClipsLeft < 0){
                Debug.Log("out of ammo");
            }

           
            
        }


    }
    //when the shoot gun methood is called
    void Shoot()
    {
        //if Time is greater than nextFire
        if (Time.time > nextFire)
        {
            nextFire = Time.time + rateOfFire;// sets next fire equal to the time and rate of fire
            currentAmmo--;//and uses one ammo
            GunFire();// calls the GunFire method
            StartCoroutine(WeaponEffects());// calls the StartCoroutine method
            SetAmmoText();

            //checking for what the ray cast hits
            //if  the position of the ray cast has hit somthing and that it is still in the weapon range
            if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, weaponRange))
            {
                //if the object hit was the enemy
                if (hit.transform.tag == "Enemy")
                {
                    HitSound();
                    Debug.Log("hit enemy");//prints to debug log that enemy was hit
                    EnemyHealth enemyHealthScript = hit.transform.GetComponent<EnemyHealth>();//sets the enemy health script the remaining health from the enemyhealth var
                    enemyHealthScript.DeductHealth(damageEnemy);//calls the deductHealth methood for class  the enemy health script to deduct the health
                    if(enemyHealthScript.enemyHealth <= 0){
                         kills = kills + 1;
                        moreAmmo = moreAmmo - 1;// how many kills till you get more ammo
                        SetKillsText();
                       
                    }
                }
                else
                {
                    Debug.Log("hit Something Else");//prints to the de bug log saying somthing else was hit
                }
            }
        }
    }

    //GunFire sound  method
    public void GunFire()
    {
        gunfire.Play();// plays the gun fire sound effect
    }

    public void HitSound()
    {
        hitSoundEffect.Play();// plays the gun fire sound effect
    }

    // for usign weapon effects
    IEnumerator WeaponEffects()
    {
        muzzleFlash.Play();// plays the muzzleFlash
        yield return new WaitForEndOfFrame();//wait for the frame to end
        muzzleFlash.Stop();// stops playing the muzzleFlash
    }

    void SetAmmoText()
    {
       ammoText.text = "Ammo " + currentAmmo.ToString();
    }

    void SetClipText()
    {
        clipText.text = "Clips Left: " + ClipsLeft.ToString();
    }

    void SetKillsText(){
        killText.text = "Kills: " + kills.ToString();
    }


    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Health")
        {
            healthPickUp.Play();

        }

    }
private void OnGUI()
    {
        float ratio = stamina / maxStamina;
        float rectWidth = ratio * Screen.width / 3;
        staminaBar.width = rectWidth;
        GUI.DrawTexture(staminaBar, staminaTexture);

      

    }
}


    
