using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private WeaponManager weapon_Manager;

    public float fireRate = 15f;
    private float nextTimeToFire;
    public float damage = 20f;

    private Animator zoomCameraAnim;
    private bool zoomed;

    private Camera mainCam;
    private GameObject crosshair;

    private bool is_Aiming;

    [SerializeField]
    private GameObject arrow_prefab, spear_prefab;

    [SerializeField]
    private Transform arrow_Bow_StartPosition;

    void Awake() {
        weapon_Manager = GetComponent<WeaponManager>();
        zoomCameraAnim = transform.Find(Tags.LOOK_ROOT).transform.Find(Tags.ZOOM_CAMERA).GetComponent<Animator>();
        crosshair = GameObject.FindWithTag(Tags.CROSSHAIR);
        mainCam = Camera.main;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WeaponShot();
        ZoomInAndOut();
    }

    void WeaponShot()
    {
        //IF ASSULT RIFLE
        if(weapon_Manager.GetCurrentSelectedWeapon().fireType == WeaponFireType.MULTIPLE)
        {
            //press left mouse click and time to fire is greater than next time fire
            if(Input.GetMouseButtonDown(0) && Time.time > nextTimeToFire){
                nextTimeToFire = Time.time + 1f / fireRate;
                weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();
                BulletFired();
            }
        } else {
            //single fire
            if(Input.GetMouseButtonDown(0)){
                 Debug.Log(weapon_Manager.GetCurrentSelectedWeapon().tag);
                //handle axe
                if(weapon_Manager.GetCurrentSelectedWeapon().tag == Tags.AXE_TAG){
                    weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();
                }

                //handle shot
                if(weapon_Manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.BULLET){
                    weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();
                    BulletFired();
                } else {
                    //handle spear and bow
                    if(is_Aiming){
                        weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();
                        if(weapon_Manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.ARROW){
                            ThrowArrowOrSpear(true);
                        } else if(weapon_Manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.SPEAR){
                            ThrowArrowOrSpear(false);
                        }
                    }
                }

            }
        }
    }

    void ZoomInAndOut() 
    {
        if(weapon_Manager.GetCurrentSelectedWeapon().weapon_Aim == WeaponAim.AIM){
            if(Input.GetMouseButtonDown(1)){
                zoomCameraAnim.Play(AnimationTags.ZOOM_IN_ANIM);
                crosshair.SetActive(false);
            }

            if(Input.GetMouseButtonDown(1)){
                zoomCameraAnim.Play(AnimationTags.ZOOM_OUT_ANIM);
                crosshair.SetActive(true);
            }
        }

        if(weapon_Manager.GetCurrentSelectedWeapon().weapon_Aim == WeaponAim.SELF_AIM){
            if(Input.GetMouseButtonDown(1)){
                weapon_Manager.GetCurrentSelectedWeapon().Aim(true);
                is_Aiming = true;
                Debug.Log("here");
            }

            if(Input.GetMouseButtonUp(1)){
                weapon_Manager.GetCurrentSelectedWeapon().Aim(false);
                is_Aiming = false;
            }
        }
    }

    void ThrowArrowOrSpear(bool throwArrow) {
        if(throwArrow){
            GameObject arrow = Instantiate(arrow_prefab);
            arrow.transform.position = arrow_Bow_StartPosition.position;

            arrow.GetComponent<ArrowBowScript>().launch(mainCam);
        } else {
            GameObject spear = Instantiate(spear_prefab);
            spear.transform.position = arrow_Bow_StartPosition.position;

            spear.GetComponent<ArrowBowScript>().launch(mainCam);

        }
    }

    void BulletFired() {
        //make a raycast distance
        RaycastHit hit;
        if(Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit)){

            //bug in cannibal
            if(hit.transform.tag == Tags.ENEMY_TAG){
                hit.transform.GetComponent<HealthScript>().ApplyDamage(damage);
            }

            //will hit a object name in world
            print("We hit :" + hit.transform.gameObject.name);
        }        
    }
}
