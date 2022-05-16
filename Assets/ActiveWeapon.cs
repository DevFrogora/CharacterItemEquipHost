using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

// which weapon the character is equiped 

public class ActiveWeapon : MonoBehaviour
{
    public enum WeaponSlot
    {
        Primary=0,
        Secondary=1,
        Pistol = 2
    }

    public enum PlayerState
    {
        Default=0,
        holdingWeapon=1,
        Punching=2,
        throwingGranade =3,
        afterThrowing = 4,
    }
    public static PlayerState playerState;
    

    public Transform crossHairTarget;
    public UnityEngine.Animations.Rigging.Rig handIk;
    public Transform[] weaponSlots;
    public Transform weaponLeftGrip;
    public Transform weaponRightGrip;

    public RaycastWeapon[] eqipped_weapon = new RaycastWeapon[3];
    int activeWeaponIndex;
    public Animator rigController;

    public bool isHolstered = false;
    public bool defaultState = true;

    //public RaycastWeapon[] weaponPrefab;
    public class WeaponSlotClass
    {
        public static int Primary = 0;
        public static int Secondary = 1;
        public static int Pistol = 2;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        defaultState = true;
        playerState = PlayerState.Default;
        RaycastWeapon existingWeapon = GetComponentInChildren<RaycastWeapon>();
        if (existingWeapon)
        {
            Equip(existingWeapon);
        }

    }


    RaycastWeapon GetWeapon(int index)
    {
        if(index < 0 || index >= eqipped_weapon.Length)
        {
            return null;
        }
        return eqipped_weapon[index];
    }



    // Update is called once per frame
    void Update()
    {
        if (playerState == PlayerState.throwingGranade) return;
        var weapon = GetWeapon(activeWeaponIndex);
        if (weapon && !isHolstered)
        {
            weapon.UpdateWeapon(Time.deltaTime);

        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            ToggleActiveWeapon();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetActiveWeapon(WeaponSlot.Primary);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetActiveWeapon(WeaponSlot.Secondary);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetActiveWeapon(WeaponSlot.Pistol);
        }


        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            swapWeapon();

        }
        //punch
        if (Input.GetMouseButtonDown(0))
        {
            if(playerState == PlayerState.Default)
            {
                StartCoroutine(punch());

            }
        }


    }

    IEnumerator punch()
    {
        playerState = PlayerState.Punching;
        rigController.Play("punch");
        rigController.SetBool("punching", true);
        //do
        //{
            yield return new WaitForEndOfFrame();

        //} while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);

        rigController.SetBool("punching", false);
        playerState = PlayerState.Default;
    }

    public void Equip(RaycastWeapon newWeapon)
    {

        int weaponSlotIndex = (int)newWeapon.weaponSlot;
        var weapon = GetWeapon(weaponSlotIndex);
        if(weapon)
        {
            Destroy(weapon.gameObject);
        }
        weapon = newWeapon;
        weapon.raycastDestination = crossHairTarget;
        weapon.transform.SetParent(weaponSlots[weaponSlotIndex],false);

        //weapon.transform.parent = weaponSlots[weaponSlotIndex];
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;


        eqipped_weapon[weaponSlotIndex] = weapon;
        SetActiveWeapon(newWeapon.weaponSlot);
    }


    public void ToggleActiveWeapon()
    {
        bool isHoistered = rigController.GetBool("hoister_weapon");
        if(isHoistered)
        {
            StartCoroutine(ActivateWeapon(activeWeaponIndex));
        }
        else
        {
            StartCoroutine(HolsterWeapon(activeWeaponIndex));

        }
    }


    void SetActiveWeapon(WeaponSlot weaponSlot)
    {
        int holsterIndex = activeWeaponIndex;
        int activateIndex = (int) weaponSlot;

        if(holsterIndex == activateIndex)
        {
            holsterIndex = -1;
        }

        StartCoroutine(SwitchWeapon(holsterIndex, activateIndex));
    }

    IEnumerator HolsterWeapon(int index)
    {
        isHolstered = true;
        defaultState = true;
        playerState = PlayerState.Default;
        var weapon = GetWeapon(index);
        if(weapon)
        {
           rigController.SetBool("hoister_weapon",true);
            do
            {
                yield return new WaitForEndOfFrame();

            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
    }

    IEnumerator ActivateWeapon(int index)
    {
        var weapon = GetWeapon(index);
        if (weapon)
        {
            rigController.SetBool("hoister_weapon", false);
            rigController.Play("equip_" + weapon.weaponName);

            do
            {
                yield return new WaitForEndOfFrame();

            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
            isHolstered = false;
            defaultState = false;
            playerState = PlayerState.holdingWeapon;
            Debug.Log("Rifle animation got triggered");
        }

    }

    IEnumerator SwitchWeapon(int holsterIndex,int activateIndex)
    {
        yield return StartCoroutine(HolsterWeapon(holsterIndex));
        yield return StartCoroutine(ActivateWeapon(activateIndex));
        activeWeaponIndex = activateIndex;

    }

    public void swapWeapon()
    {
        if(eqipped_weapon[0] == null && eqipped_weapon[1] == null)
        {
            return;

        }
        if(eqipped_weapon[0] != null && eqipped_weapon[1] == null)
        {
            RaycastWeapon newWeaponPP = Instantiate(eqipped_weapon[0]);
            newWeaponPP.weaponName = "secondary";
            newWeaponPP.weaponSlot = WeaponSlot.Secondary;
            Destroy(eqipped_weapon[0].gameObject);
            Equip(newWeaponPP);
            return;
        }
        if (eqipped_weapon[1] != null && eqipped_weapon[0] == null)
        {
            Debug.Log("Primary Is empty");
            RaycastWeapon newWeaponSS = Instantiate(eqipped_weapon[1]);
            newWeaponSS.weaponName = "primary";
            newWeaponSS.weaponSlot = WeaponSlot.Primary;
            Destroy(eqipped_weapon[1].gameObject);
            Equip(newWeaponSS);
            return;

        }
        RaycastWeapon newWeaponP = Instantiate(eqipped_weapon[0]);
        newWeaponP.weaponName = "secondary";
        newWeaponP.weaponSlot = WeaponSlot.Secondary;

        RaycastWeapon newWeaponS = Instantiate(eqipped_weapon[1]);
        newWeaponS.weaponName = "primary";
        newWeaponS.weaponSlot = WeaponSlot.Primary;

        Destroy(eqipped_weapon[0].gameObject);
        Destroy(eqipped_weapon[1].gameObject);


        Equip(newWeaponP);
        Equip(newWeaponS);
    }

    public void DropGun()
    {
        Debug.Log(eqipped_weapon.Length);
        for(int i  = 0; i < eqipped_weapon.Length; i++)
        {
            if(eqipped_weapon[i] != null)
            {
                Destroy(eqipped_weapon[i].gameObject);
            }
                
           
        }
    }

}
