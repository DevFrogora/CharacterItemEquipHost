using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeController : MonoBehaviour
{
    public float rotationSpeed = 1;
    public float BlastPower = 5;

    public GameObject granadeBall;
    public Transform ShotPoint;
    GameObject newGranadeBall;

    public ParticleSystem Explosion;
    //private void Update()
    //{


    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //         newGranadeBall = Instantiate(granadeBall, ShotPoint.position, ShotPoint.rotation);
    //        newGranadeBall.GetComponent<Rigidbody>().velocity = ShotPoint.transform.up * BlastPower;

    //        // Added explosion for added effect
    //        //Destroy(Instantiate(Explosion, ShotPoint.position, ShotPoint.rotation), 2);

    //        // Shake the screen for added effect
    //        //Screenshake.ShakeAmount = 5;
    //        Destroy(newGranadeBall, 1f);
    //    }

    //}

    bool executeOnce = false;
    public void Update()
    {
        if (ActiveWeapon.playerState == ActiveWeapon.PlayerState.afterThrowing)
        {
            //this.gameObject.transform = Instantiate(granadeBall, ShotPoint.position, ShotPoint.rotation);
            this.GetComponent<Rigidbody>().isKinematic = false;
            this.GetComponent<Rigidbody>().velocity = ShotPoint.transform.up * BlastPower;
            //Destroy(Instantiate(Explosion, ShotPoint.position, ShotPoint.rotation), 2);
        }


        positionOfGranadeBeforeThrowing();
        // Added explosion for added effect
        //Destroy(Instantiate(Explosion, ShotPoint.position, ShotPoint.rotation), 2);

        // Shake the screen for added effect
        //Screenshake.ShakeAmount = 5;
        //StartCoroutine(DestroyGrande());

    }
    Vector3 startingVelocity = new Vector3();
    public Transform handSlotTransform;


    public void positionOfGranadeBeforeThrowing()
    {
        if (ActiveWeapon.playerState == ActiveWeapon.PlayerState.throwingGranade)
        {

            //this.gameObject.transform = Instantiate(granadeBall, ShotPoint.position, ShotPoint.rotation);
            this.GetComponent<Rigidbody>().isKinematic = false;

            startingVelocity = ShotPoint.transform.up * BlastPower;
            //}
            this.GetComponent<Rigidbody>().velocity = startingVelocity;
            if (!executeOnce)
            {

                handSlotTransform = this.GetComponent<Transform>().transform;
            }
            this.gameObject.transform.position = handSlotTransform.localPosition;

            //Destroy(Instantiate(Explosion, ShotPoint.position, ShotPoint.rotation), 2);
            executeOnce = true;
         }
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        //if(collision.gameObject.layer == 13)
        //{
        //    
        //}
    }

    IEnumerator DestroyGrande()
    {
        yield return new WaitForSeconds(1);

        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }

}
