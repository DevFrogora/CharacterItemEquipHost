using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
public class ActiveThrowable : MonoBehaviour
{

    public enum HandSlot
    {
        Granade = 0,
        Smoke = 1,
    }



    public UnityEngine.Animations.Rigging.Rig handIk;
    public Transform handSlot;
    public Transform weaponLeftGrip;
    public Transform weaponRightGrip;

    public RaycastThrowable[] eqipped_throwable = new RaycastThrowable[2];
    int activeWeaponIndex;
    public Animator rigController;

    public bool isHolstered = false;
    public bool defaultState = true;

    public GameObject character;

    // Start is called before the first frame update
    void Start()
    {

        defaultState = true;

    }



    RaycastThrowable GetThrowable(int index)
    {
        if (index < 0 || index >= eqipped_throwable.Length)
        {
            return null;
        }
        return eqipped_throwable[index];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetActiveThrowable(HandSlot.Granade);

        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            ToggleActiveGranade();

        }

        if (Input.GetMouseButtonDown(0) && isHolstered)
        {
            rigController.SetBool("beforeThrowing", true);
            ActiveWeapon.playerState = ActiveWeapon.PlayerState.throwingGranade;
            eqipped_throwable[0].gameObject.GetComponent<GranadeController>().positionOfGranadeBeforeThrowing();
        }


        if (Input.GetMouseButtonUp(0) && isHolstered)
        {
            rigController.SetBool("afterThrowing", true);

            ActiveWeapon.playerState = ActiveWeapon.PlayerState.afterThrowing;

            StartCoroutine(Throw());

            //eqipped_throwable[0].gameObject.GetComponent<Transform>()

        }





    }




    public void ToggleActiveGranade()
    {
        bool isHoistered = rigController.GetBool("hoister_throwing");
        if (isHoistered)
        {
            StartCoroutine(ActivateGranade(activeWeaponIndex));
        }
        else
        {
            StartCoroutine(HolsterGranade(activeWeaponIndex));

        }
    }


    IEnumerator Throw()
    {
        yield return new WaitForSeconds(0.4f);
        //eqipped_throwable[0].gameObject.GetComponent<GranadeController>().Throw();
        eqipped_throwable[0].gameObject.transform.SetParent(character.transform.parent);
        ActiveWeapon.playerState = ActiveWeapon.PlayerState.afterThrowing;
        yield return new WaitForSeconds(0.4f);
        ActiveWeapon.playerState = ActiveWeapon.PlayerState.Default;
    }

    public void Equip(RaycastThrowable throwableitem)
    {
        int weaponSlotIndex = (int)throwableitem.handSlot;
        var throwableObject = GetThrowable(weaponSlotIndex);
        if (throwableObject)
        {
            Destroy(throwableObject.gameObject);
        }

        throwableObject = throwableitem;
        throwableObject.transform.SetParent(handSlot, false);

        //weapon.transform.parent = weaponSlots[weaponSlotIndex];
        throwableObject.transform.localPosition = Vector3.zero;
        throwableObject.transform.localRotation = Quaternion.identity;


        eqipped_throwable[0] = throwableObject;
        SetActiveThrowable(throwableitem.handSlot);
    }



    void SetActiveThrowable(HandSlot handSlot)
    {
        int holsterIndex = activeWeaponIndex;
        int activateIndex = (int)handSlot;

        if (holsterIndex == activateIndex)
        {
            holsterIndex = -1;
        }

        StartCoroutine(SwitchItem(holsterIndex, activateIndex));

    }



    IEnumerator SwitchItem(int holsterIndex, int activateIndex)
    {
        yield return StartCoroutine(HolsterGranade(holsterIndex));
        yield return StartCoroutine(ActivateGranade(activateIndex));
        activeWeaponIndex = activateIndex;

    }




    IEnumerator ActivateGranade(int index)
    {
        var throwableObject = GetThrowable(index);
        if (throwableObject)
        {
            rigController.SetBool("hoister_throwing", false);
            rigController.Play("GranadeHolding");

            do
            {
                yield return new WaitForEndOfFrame();

            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
            isHolstered = false;
            defaultState = false;
            ActiveWeapon.playerState = ActiveWeapon.PlayerState.throwingGranade;
        }
        StartCoroutine(HolsterGranade(index));
    }

    IEnumerator HolsterGranade(int index)
    {
        isHolstered = true;
        
        var weapon = GetThrowable(index);
        if (weapon)
        {
            rigController.SetBool("hoister_throwing", true);
            do
            {
                yield return new WaitForEndOfFrame();

            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
        ActiveWeapon.playerState = ActiveWeapon.PlayerState.throwingGranade;
        Debug.Log("Grande Host ANimation got triggered");
    }





    public void DropGun()
    {
        Debug.Log(eqipped_throwable.Length);
        for (int i = 0; i < eqipped_throwable.Length; i++)
        {
            if (eqipped_throwable[i] != null)
            {
                Destroy(eqipped_throwable[i].gameObject);
            }


        }
    }

}


