using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coralina : MonoBehaviour {
    

    private Rigidbody2D rb;    
    private Animator anim;

    // Used to disable the trash gameobject from sceen
    internal GameObject UnderCoralina;

    //************************************************************************
    //Game Controlling Boolean Variables
    internal bool OutBounds; //Coralina is outside of game bounds
    internal bool TouchEnemy; // Coralina step on Rock or Sea Ursin

    //Variables to know if the Corallina is on top of trash and what type.
    internal bool glass_bin;
    internal bool aluminum_bin;
    internal bool plastic_bin;


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

   
    public void MoveUp()
    {
        transform.Translate(0, 1f, 0);
    }
   public void MoveDown()
    {
        transform.Translate(0, -1f, 0);
    }
   public void MoveLeft()
    {
        transform.Translate(-1f, 0, 0);
    }
   public void MoveRight()
    {

        transform.Translate(1f, 0, 0);
    }
    public void PickUp()
    {
        anim.Play("pickup");
        if (UnderCoralina != null) { UnderCoralina.SetActive(false); UnderCoralina = null; }
    }

   public void Drop()
    {
        anim.Play("drop");
    }

    public void JumpUp()
    {

        transform.Translate(0, 2f, 0);
    }
    public void JumpDown()
    {

        transform.Translate(0, -2f, 0);
    }
    public void JumpLeft()
    {

        transform.Translate(-2f, 0, 0);

    }
    public void JumpRight()
    {

        transform.Translate(2f, 0, 0);
    }
    

    void OnTriggerStay2D(Collider2D col)
    {
        if ((col.gameObject.tag == "aluminum") || (col.gameObject.tag == "plastic") || (col.gameObject.tag == "glass"))
        {
            UnderCoralina = col.gameObject;
        }
        
        else if (col.gameObject.tag == "aluminum_bin")
        {
            aluminum_bin = true;
        }
        else if (col.gameObject.tag == "plastic_bin")
        {
            plastic_bin = true;
        }
        else if (col.gameObject.tag == "glass_bin")
        {
            glass_bin = true;
        }

    }
    void OnTriggerExit2D(Collider2D col)
    {
        UnderCoralina = null;
        aluminum_bin = false;
        plastic_bin = false;
        glass_bin = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "enemy")
        {
            TouchEnemy = true;
            anim.Play("hit");
        }
        else if (col.gameObject.tag == "collider")
        {
            OutBounds = true;
            anim.Play("hit");
        }
            UnderCoralina = null;

    }

}


