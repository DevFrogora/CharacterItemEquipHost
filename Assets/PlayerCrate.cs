using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Player Dead Crate

public class PlayerCrate : MonoBehaviour
{
    // Start is called before the first frame update
    public Image crateOpenImage;
    public Image crateCloseImage;
    public List<WeaponPickup> allChildren;
    public Image CrateUiImage;
    public Text crateText;
    public static bool crateOpening;

    public GameObject crateUiContent;
    public ActiveWeapon character;

    public List<WeaponPickup> itemObjects;

    public bool isPlayerInsideTrigger;

    public GameObject PlayerNearDetector;

    void Start()
    {
        allChildren.AddRange(GetComponentsInChildren<WeaponPickup>());
        deactiveAllChildren();
        crateOpenImage.gameObject.SetActive(false);
        crateCloseImage.gameObject.SetActive(false);
        CrateUiImage.gameObject.SetActive(false);
        crateText.gameObject.SetActive(false);
        crateLight.SetActive(false);

    }

    private void LateUpdate()
    {
        if (itemObjects.Count > 0 && isPlayerInsideTrigger)
        {

            foreach (WeaponPickup item in itemObjects.ToArray())
            {
                Debug.Log(item.nameText.text);
                if (item.isEquiped)
                { 
                    itemObjects.Remove(item);
                    Debug.Log("Someone is equipped");


                }
            }
            StartCoroutine(DisableCrateNearBy());
        }
        else
        {
            //Debug.Log(itemObjects.Count);
        }
        

    }
    public ParticleSystem crateSmokeEffect;
    public GameObject crateLight;
    IEnumerator onCrateStart()
    {
        // before start
        crateLight.SetActive(false);
        yield return new WaitForSeconds(1);
        crateSmokeEffect.Play();
        for(int i =0; i < 5; i++)
        {
            yield return new WaitForSeconds(1);
            crateLight.SetActive(true);
            yield return new WaitForSeconds(1);
            crateLight.SetActive(false);
        }
    }

IEnumerator DisableCrateNearBy()
    {
        yield return new WaitForEndOfFrame();
        if (itemObjects.Count < 1)
        {
            crateUiContent.gameObject.SetActive(false);
            crateText.gameObject.SetActive(false);
            CrateUiImage.gameObject.SetActive(false);
        }
        allChildren.Clear();
        allChildren.AddRange(GetComponentsInChildren<WeaponPickup>());
        AddInCrateist();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        isPlayerInsideTrigger = true;
        if (other.gameObject.layer == 13)
        {
            this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        
        if(other.gameObject .layer == 12)
        {
            crateOpenImage.gameObject.SetActive(true);
        }


    }

    private void OnTriggerExit(Collider other)
    {

        isPlayerInsideTrigger = false;
        CloseCrate();
        crateOpenImage.gameObject.SetActive(false);
        removeAllItems();
        foreach (WeaponPickup item in itemObjects.ToArray())
        {
            Debug.Log(item.nameText.text);
            item.RemoveItem();
            //itemObjects.Remove(item);
            
        }

    }

    public void CrateClicked()
    {
        // crate opened
        Debug.Log("clicked");
        activeAllChildren();
        crateCloseImage.gameObject.SetActive(true);

    }

    void activeAllChildren()
    {
        foreach (WeaponPickup child in allChildren)
        {
            Debug.Log("1");
            child.gameObject.SetActive(true);
        }
    }

    void deactiveAllChildren()
    {
        foreach (WeaponPickup child in allChildren)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void OpenCrate()
    {
        StartCoroutine(onCrateStart());
        CrateClicked();
        crateText.gameObject.SetActive(true);
        crateOpening = true;
        CrateUiImage.gameObject.SetActive(true);
        AddInCrateist();
        PlayerNearDetector.SetActive(false);
    }

    public void CloseCrate()
    {
        removeAllItems();
        crateCloseImage.gameObject.SetActive(false);
        deactiveAllChildren();
        crateText.gameObject.SetActive(false);
        CrateUiImage.gameObject.SetActive(false);
        crateOpening = false;
        PlayerNearDetector.SetActive(true);
        

    }

    public void AddInCrateist()
    {
        if (allChildren.Count > 0)
        {
            foreach (WeaponPickup child in allChildren)
            {
                Image itemImage = child.GetComponentInChildren<WeaponPickup>().itemBoxImage;
                itemImage.gameObject.transform.SetParent(crateUiContent.transform);
                itemImage.gameObject.transform.localScale = new Vector3(1, 1, 1);
                Debug.Log("Added item in crate List");
                child.activeWeapon = character;

                itemObjects.Add(child);
            }

        }
        
        //itemBoxImage.transform.SetParent(nearbyItem.transform);

    }

    public void removeAllItems()
    {
        foreach (WeaponPickup item in itemObjects.ToArray())
        {
            Image itemImage = item.GetComponentInChildren<WeaponPickup>().itemBoxImage;
            //itemImage.gameObject.transform.SetParent(item.originalTransform);
            item.RemoveItem();
            //item.deactivateOverLayerImage();
            itemObjects.Remove(item);

        }
        foreach (WeaponPickup child in allChildren.ToArray())
        {
            child.GetComponentInChildren<WeaponPickup>().RemoveItem();
        }
    }



    }
