using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagInventory : MonoBehaviour
{
    public ActiveWeapon character;
    public GameObject crateBox;
    public List<WeaponPickup> allChildren;

    public GameObject Vest;
    public GameObject Helmet;

    public GameObject VestPrefab;
    public GameObject HelmePrfab;

    public void onDrop()
    {
        allChildren.AddRange(crateBox.GetComponentsInChildren<WeaponPickup>());
        character.GetComponent<ActiveWeapon>().ToggleActiveWeapon();

        // destroy Helmet aand Vest
        //Destroy(Helmet.GetComponentInChildren<Transform>().gameObject);
        //Destroy(Vest.GetComponentInChildren<Transform>().gameObject);

        int childs = Vest.gameObject.transform.childCount;
        for (var i = childs - 1; i >= 0; i--)
        {
            Destroy(Vest.transform.GetChild(i).gameObject);
        }

        int childsH = Helmet.gameObject.transform.childCount;
        for (var i = childsH - 1; i >= 0; i--)
        {
            Destroy(Helmet.transform.GetChild(i).gameObject);
        }

        character.GetComponent<ActiveWeapon>().DropGun();
        //character.GetComponent<ActiveWeapon>().weaponPrefab = null;
        foreach (WeaponPickup item in allChildren.ToArray())
        {
            if(item.nameText.text == "Helmet")
            {
                item.helmetPreb = HelmePrfab;
            }

            if (item.nameText.text == "Vest")
            {
                item.vestPreb = VestPrefab;
            }

            if (item.nameText.text == "Pistol")
            {
                WeaponPickup.totalPistol--;
            }

            if (item.nameText.text == "Rifle" || item.nameText.text == "Ak47")
            {
                WeaponPickup.totalRifle--;
            }

            item.transform.SetParent(character.transform.parent);
            item.GetComponent<Collider>().enabled = true;
            item.GetComponent<Rigidbody>().isKinematic = (true);
            item.itemBoxImage.transform.localScale = new Vector3(1, 1, 1);
            item.itemBoxImage.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            item.itemBoxImage.rectTransform.sizeDelta = new Vector2(76.43f, 39.6f);
            item.gameObject.SetActive(true);
            
        }
    }

}
