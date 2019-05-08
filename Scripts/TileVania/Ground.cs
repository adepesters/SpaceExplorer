using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    //bool feetAreTouchingGround = false;
    //bool feetAreCloseToTheGround = false; //added this big collider to account for the delay that occurs between the time the player presses the button to jump and the fact that the character's feet may 
    //// currently jumping and not on the ground

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.name.Contains("Feet"))
    //    {
    //        feetAreTouchingGround = true;
    //        FindObjectOfType<PlayerTileVania>().IsOnATree = false;
    //    }
    //    if (collision.gameObject.name.Contains("Extended Legs"))
    //    {
    //        feetAreCloseToTheGround = true;
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.name.Contains("Feet"))
    //    {
    //        feetAreTouchingGround = false;
    //    }
    //    if (collision.gameObject.name.Contains("Extended Legs"))
    //    {
    //        feetAreCloseToTheGround = false;
    //    }
    //}

    //public bool AreFeetOnTheGround()
    //{
    //    return feetAreTouchingGround;
    //}

    //public bool AreFeetCloseToTheGround()
    //{
    //    return feetAreCloseToTheGround;
    //}
}


