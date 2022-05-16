using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPickup : MonoBehaviour
{
    public RaycastWeapon weaponPreFab;
    public RaycastThrowable throwablePrefab;

    public GameObject helmetPreb;
    public GameObject helmetHolder;

    public GameObject vestPreb;
    public GameObject vestHolder;

    public Image nearbyContainer;
    public Image itemBoxImage;

    public Image overlayImage;
    public Text OverLayText;

    public Text nameText;
    public Text countText;
    public Text textButtonKey;

    public string WeaponName;
    public static int totalRifle;
    public static int totalPistol;

    public GameObject crateBox;




    public enum Armor
    {
        helmet = 0,
        vest = 1,
        none = 2,
    }

    public enum Throwable
    {
        none = 0,
        Smoke = 1,
        Grenade = 2,
    }

    public bool isHelmetWeared = false;
    public bool isVestWeared = false;


    public Armor armor;
    public Throwable throwable;

    public ActiveWeapon activeWeapon;
    public ActiveThrowable activeThrowable;
    public static bool isThrown = false;

    public Transform originalTransform;

    public string pressedKey;

    public bool isEquiped = false;
    private void Start()
    {

        if (originalTransform == null)
        {
            originalTransform = itemBoxImage.gameObject.transform.parent;
        }
        overlayImage.gameObject.SetActive(false);
    }

    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.H))
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                pressedKey = "F";
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                pressedKey = "G";
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                pressedKey = "H";
            }
            if (activeWeapon)
            {

                Debug.Log("inside active ");
                if (armor == Armor.none)
                {

                    if (activeWeapon)
                    {

                        if (throwable == Throwable.Grenade)
                        {
                            if ( OverLayText.text == pressedKey && nameText.text == "Grenade")
                            {
                                pressedKey = "";
                                EquipGrenade();
                            }
                        }
                        else if(throwable == Throwable.Smoke)
                        {
                            if (OverLayText.text == pressedKey && nameText.text == "Smoke")
                            {
                                pressedKey = "";
                                EquipSmoke();
                            }
                        }
                        else {

                            if (totalRifle < 2)
                            {
                                if (nameText.text == WeaponName && OverLayText.text == pressedKey && nameText.text != "Pistol")
                                {
                                    pressedKey = "";
                                    EquipWeapon();
                                    totalRifle++;

                                }


                            }

                            if (totalPistol < 1)
                            {
                                if (nameText.text == "Pistol" && OverLayText.text == pressedKey)
                                {
                                    pressedKey = "";
                                    EquipWeapon();
                                    totalPistol++;
                                }


                            }
                        }
                        //itemBoxImage.gameObject.transform.SetParent(nearbyItem.transform);
                    }
                }
                else
                {

                    if (armor == Armor.helmet)
                    {
                        Debug.Log("its helmet");

                        if (activeWeapon)
                        {
                            if (!isHelmetWeared)
                            {
                                if (nameText.text == "Helmet" && OverLayText.text == pressedKey )
                                {
                                    pressedKey = "";
                                    EquipHelmet();
                                }
                                //itemBoxImage.gameObject.transform.SetParent(nearbyItem.transform);
                            }

                        }
                    }

                    if (armor == Armor.vest)
                    {
                        Debug.Log("its vest");
                        if (activeWeapon)
                        {
                            if (!isVestWeared)
                            {
                                if (nameText.text == "Vest" && OverLayText.text == pressedKey)
                                {
                                    pressedKey = "";
                                    EquipVest();
                                    //itemBoxImage.gameObject.transform.SetParent(nearbyItem.transform);
                                }

                            }

                        }
                    }

                }
            }
        }

    }

    //private void OnTriggerEnter(Collider other)
    //{

    //    activeWeapon = other.gameObject.GetComponent<ActiveWeapon>();

    //}

    //private void OnTriggerExit(Collider other)
    //{

    //    activeWeapon = null;

    //}

    public void onItemClicked()
    {
        Debug.Log("Item clicked");
        if(nameText.text == "Helmet")
        {
            EquipHelmet();
        }
        if (nameText.text == "Vest")
        {
            EquipVest();
        }
        if(nameText.text == "Pistol")
        {
            
            if (totalPistol < 1)
            {
                totalPistol++;
                EquipWeapon();
                Debug.Log("Equipped Pistol");
            }
        }
        if (nameText.text == WeaponName && nameText.text != "Pistol")
        {
            if (totalRifle < 2)
            {
                EquipWeapon();
                totalRifle++;
            }

        }
        if (throwable == Throwable.Grenade)
        {
            if (OverLayText.text == pressedKey && nameText.text == "Grenade")
            {
                pressedKey = "";
                EquipGrenade();
            }
        }
        else if (throwable == Throwable.Smoke)
        {
            if (OverLayText.text == pressedKey && nameText.text == "Smoke")
            {
                pressedKey = "";
                EquipSmoke();
            }
        }
    }


    void EquipSmoke()
    {
        Debug.Log("smoke");
    }
    void EquipGrenade()
    {
        Debug.Log("Grande");
        this.GetComponent<Collider>().enabled = false;
        this.GetComponent<Rigidbody>().isKinematic = true;
        RemoveItem();
        deactivateOverLayerImage();
        isEquiped = true;
        this.transform.SetParent(crateBox.transform);
        this.transform.localPosition = new Vector3(0f, 0f, 0f);


        // change status
        ActiveWeapon.playerState = ActiveWeapon.PlayerState.throwingGranade;

        RaycastThrowable newThrowable = throwablePrefab;
        activeThrowable.Equip(newThrowable);

    }

    public void EquipHelmet()
    {
        isHelmetWeared = true;
        helmetPreb = Instantiate(helmetPreb);
        helmetPreb.transform.SetParent(helmetHolder.transform);
        helmetPreb.transform.localPosition = new Vector3(-0.02000002f, -0.08111232f, 0.0128394f);
        helmetPreb.transform.localRotation = Quaternion.identity;

        this.GetComponent<Collider>().enabled = false;
        this.GetComponent<Rigidbody>().isKinematic = true;
        RemoveItem();
        deactivateOverLayerImage();
        isEquiped = true;
        this.transform.SetParent(crateBox.transform);
        this.transform.localPosition = new Vector3(0f, 0f, 0f);
    }

    void EquipVest()
    {
        isVestWeared = true;
        vestPreb = Instantiate(vestPreb);
        vestPreb.transform.SetParent(vestHolder.transform);
        vestPreb.transform.localPosition = new Vector3(-0.031f, 0.035f, 0);
        vestPreb.transform.localRotation = Quaternion.Euler(new Vector3(-90f, 67.895f, 21.958f));

        this.GetComponent<Collider>().enabled = false;
        this.GetComponent<Rigidbody>().isKinematic = true;
        RemoveItem();
        deactivateOverLayerImage();
        isEquiped = true;
        this.transform.SetParent(crateBox.transform);
        this.transform.localPosition = new Vector3(0f, 0f, 0f);
    }

    void EquipWeapon()
    {

        this.GetComponent<Collider>().enabled = false;
        this.GetComponent<Rigidbody>().isKinematic = true;
        RemoveItem();
        deactivateOverLayerImage();
        isEquiped = true;
        this.transform.SetParent(crateBox.transform);
        this.transform.localPosition = new Vector3(0f, 0f, 0f);


        RaycastWeapon newWeapon = Instantiate(weaponPreFab);
        activeWeapon.Equip(newWeapon);



    }


    public void AddInNearyByList()
    {
        
        itemBoxImage.gameObject.transform.SetParent(nearbyContainer.transform);
        itemBoxImage.gameObject.transform.localScale = new Vector3(1, 1, 1);
        itemBoxImage.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        //itemBoxImage.transform.SetParent(nearbyItem.transform);

    }




    public void RemoveItem()
    {
        itemBoxImage.gameObject.transform.SetParent(originalTransform);
        //itemBoxImage.transform.SetParent(originalTransform);

    }

    public void activateOverLayerImage(string text)
    {
        overlayImage.gameObject.SetActive(true);
        OverLayText.text = text;
        StartCoroutine(falseTheOverWriteOfCanvas());

    }

    IEnumerator falseTheOverWriteOfCanvas()
    {
        yield return new WaitForEndOfFrame();
        if (overlayImage.gameObject.activeInHierarchy && overlayImage.enabled)
            overlayImage.GetComponent<Canvas>().overrideSorting = false;
    }

     public void deactivateOverLayerImage()
    {
        overlayImage.gameObject.SetActive(false);
    }

    public void dropItem()
    {
        //crateBox.gameObject.transform.parent
    }

    public void Destroy()
    {
        Destroy(throwablePrefab);
        Destroy(this.gameObject);

    }
}
