using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NearItemDetector : MonoBehaviour
{
    public List<WeaponPickup> itemObjects;
    public Image nearByImage;
    public Image FirstFixedNearByImage;
    public Image ScrollView;
    bool hideCurrentTrigger;
    public ActiveWeapon character;


    private void Start()
    {
        nearByImage.gameObject.SetActive( false);
        FirstFixedNearByImage.gameObject.SetActive(false);
        ScrollView.gameObject.SetActive(false);
    }




    private void LastUpdate()
    {
        if(itemObjects.Count > 0)
        {
            
            foreach(WeaponPickup item in itemObjects.ToArray())
            {
                Debug.Log(item.nameText.text);
                if(item.isEquiped)
                {
                    itemObjects.Remove(item);
                }
            }
            StartCoroutine(DisableTheNearbY());
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (PlayerCrate.crateOpening)
        {
            return;
        }
        Debug.Log("we are again able to find it");
        hideCurrentTrigger = false;
        WeaponPickup item = other.gameObject.GetComponent<WeaponPickup>();
        if (item)
        {
            item.activeWeapon = character;
            if (itemObjects.Count == 0)
            {
                item.activateOverLayerImage("F");
            }
            if (itemObjects.Count == 1)
            {
                item.activateOverLayerImage("G");
            }
            if (itemObjects.Count == 2)
            {
                item.activateOverLayerImage("H");
            }

            // if item present in inventory then set the color ECC44B of item info image
            itemObjects.Add(item);

            Debug.Log("trying to add in the list");
            item.AddInNearyByList();
        }
        if(itemObjects.Count >0)
        {
            if (!hideCurrentTrigger)
            {
                nearByImage.gameObject.SetActive(true);
                FirstFixedNearByImage.gameObject.SetActive(true);
                ScrollView.gameObject.SetActive(true);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        WeaponPickup item = other.gameObject.GetComponent<WeaponPickup>();
        if (item)
        {
            itemObjects.Remove(item);
            item.RemoveItem();
            item.deactivateOverLayerImage();
        }
        if (itemObjects.Count < 1)
        {
            if (hideCurrentTrigger)
            {
                nearByImage.gameObject.SetActive(false);
                FirstFixedNearByImage.gameObject.SetActive(false);
                ScrollView.gameObject.SetActive(false);
            }
            nearByImage.gameObject.SetActive(false);
            FirstFixedNearByImage.gameObject.SetActive(false);
            ScrollView.gameObject.SetActive(false);
        }

        if (itemObjects.Count == 1)
        {
            itemObjects[0].activateOverLayerImage("F");
        }
        if (itemObjects.Count == 2)
        {
            itemObjects[1].activateOverLayerImage("G");
        }
        if (itemObjects.Count == 3)
        {
            itemObjects[2].activateOverLayerImage("H");
        }
        removeAllItems();

    }

    public void  removeAllItems()
    {
        foreach(WeaponPickup item in itemObjects)
        {
            item.RemoveItem();
            StartCoroutine(DisableTheNearbY());
        }

        nearByImage.gameObject.SetActive(false);
        FirstFixedNearByImage.gameObject.SetActive(false);
        ScrollView.gameObject.SetActive(false);
        hideCurrentTrigger = true;

        foreach(WeaponPickup child in itemObjects.ToArray())
        {
            itemObjects.Remove(child);
        }
    }

    IEnumerator DisableTheNearbY()
    {
        yield return new WaitForEndOfFrame();
        if (itemObjects.Count < 1)
        {
            nearByImage.gameObject.SetActive(false);
            FirstFixedNearByImage.gameObject.SetActive(false);
            ScrollView.gameObject.SetActive(false);
        }

    }


}
