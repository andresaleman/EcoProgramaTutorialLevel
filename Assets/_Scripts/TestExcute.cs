using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestExcute : MonoBehaviour
{

    //Variable to Start Execution
    public bool Play = false;

    public static bool Play2 = false;

    private bool check = false;

    public GameObject character;
    private Vector3 StartPosition;
    private Vector3 CurrentPosition;
    public Text Dialogue;

    //The player current score
    public int score = 0;

    //Count the Amount of trash the Player has collect of each type.
    int glass_amount = 0;
    int aluminum_amount = 0;
    int plastic_amount = 0;

    int totalAmountofTrash;

    // Use this for initialization
    void Start()
    {
        CurrentPosition = StartPosition = character.transform.position;
        totalAmountofTrash = GameObject.Find("Trash").transform.childCount;
       
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("alum: " + character.GetComponent<Coralina>().aluminum_bin);
        Debug.Log("plastic: " + character.GetComponent<Coralina>().plastic_bin);
        Debug.Log("glass: " + character.GetComponent<Coralina>().glass_bin);
        if (Play)
        {
            Play = false;
            if (DragAndDropCell.Tutorialstep == 9)
            {
                DragAndDropCell.Tutorialstep++;
                
            }
            AlgothimDevelopment.turnONToggle();
            StartCoroutine(ExecuteAlgorithim());

            if (totalAmountofTrash == 0)
            {
                Debug.Log("Recogio Toda la Basura");
                ResetScene();
            }

        }
        else if (Play2) {
            Play2 = false;
            if (DragAndDropCell.Tutorialstep == 9) //Tutorial -------------------------
            {
                DragAndDropCell.Tutorialstep++;
                Dialogue.text = "Dirígeme hacia dónde debo nadar para alcanzar el cristal.";
            }
            if (DragAndDropCell.Tutorialstep == 24)
            {
                DragAndDropCell.Tutorialstep++;
                Dialogue.text = "Has ganado puntos de Bono por haber recogido dos cristales.";
                Debug.Log("Level1");
            }
            //Tutorial -----------------------------------------------
            check = true;
            StartCoroutine(ExecuteAlgorithim());
            if (totalAmountofTrash == 0)
            {
                Debug.Log("Recogio Toda la Basura");
                ResetScene();
            }


        }

    }

    //Executes the command in the i position of the array.
    void Algorithim(int i)
    {
       
            if (AlgothimDevelopment.cellArray[i, 0] == "Swim")
            {
                switch (AlgothimDevelopment.cellArray[i, 1])
                {

                    case "Up":
                        character.GetComponent<Coralina>().MoveUp();
                        break;
                    case "Down":
                        character.GetComponent<Coralina>().MoveDown();
                        break;
                    case "Left":
                        character.GetComponent<Coralina>().MoveLeft();
                        break;
                    case "Right":
                        character.GetComponent<Coralina>().MoveRight();
                        break;
                }

            }

            else if (AlgothimDevelopment.cellArray[i, 0] == "Jump")
            {
                switch (AlgothimDevelopment.cellArray[i, 1])
                {

                    case "Up":
                        character.GetComponent<Coralina>().JumpUp();
                        break;
                    case "Down":
                        character.GetComponent<Coralina>().JumpDown();
                        break;
                    case "Left":
                        character.GetComponent<Coralina>().JumpLeft();
                        break;
                    case "Right":
                        character.GetComponent<Coralina>().JumpRight();
                        break;
                }
            }

            else if (AlgothimDevelopment.cellArray[i, 0] == "Drop")
                character.GetComponent<Coralina>().Drop();

            else if (AlgothimDevelopment.cellArray[i, 0] == "PickUp")
                character.GetComponent<Coralina>().PickUp();

     }

    //Waits a 1 seconds before calling the movement function.
    private IEnumerator ExecuteAlgorithim()
    {
        for (int i = 0; i < AlgothimDevelopment.size; i++)
        {
           if (character.GetComponent<Coralina>().OutBounds)
            {
                Debug.Log("Fuera de Tablero");
                yield return new WaitForSeconds(1);
                ResetScene();
                character.GetComponent<Coralina>().OutBounds = false;
            }

            else if (character.GetComponent<Coralina>().TouchEnemy)
            {
                Debug.Log("Toco Enemigo");
                yield return new WaitForSeconds(1);
                ResetScene();
                character.GetComponent<Coralina>().TouchEnemy = false;
            }
            //Need to call the same command multiple times. Waits before doing so.
            else if (AlgothimDevelopment.cellArray[i, 0] == "Swim")
            {

                int amount = int.Parse(AlgothimDevelopment.cellArray[i, 2]);

                while (amount != 0)
                {
                    Algorithim(i);
                    amount--;
                    yield return new WaitForSeconds(1);

                    if (character.GetComponent<Coralina>().OutBounds || character.GetComponent<Coralina>().TouchEnemy)
                    {
                        break;
                    }
                }
            }          

            else if (AlgothimDevelopment.cellArray[i, 0] == "Jump")
            {
                Algorithim(i);
                yield return new WaitForSeconds(1);
            }

            else if (AlgothimDevelopment.cellArray[i, 0] == "Drop")
            {
                //has not pickup Any trash. Nothing Bad Happends
                if(glass_amount == 0 && aluminum_amount == 0 && plastic_amount == 0)
                {
                    Algorithim(i);
                    yield return new WaitForSeconds(1);
                }
                //Has pick Up trash but is not in container . Reset Scene
               else if (!character.GetComponent<Coralina>().glass_bin && !character.GetComponent<Coralina>().aluminum_bin && !character.GetComponent<Coralina>().plastic_bin)
                {
                    Debug.Log("No esta en Bin");
                    yield return new WaitForSeconds(1);
                    ResetScene();
                }
                // Has Pick Up glass and is on top of glass bin
               else if (glass_amount > 0 && character.GetComponent<Coralina>().glass_bin)
                {
                    Algorithim(i);
                    glass_amount = 0;
                    yield return new WaitForSeconds(1);
                }
                // Has Pick Up plasic and is on top of plastic bin
                else if (plastic_amount > 0 && character.GetComponent<Coralina>().plastic_bin)
                {
                    Algorithim(i);
                    plastic_amount = 0;
                    yield return new WaitForSeconds(1);
                }
                // Has Pick Up aluminum and is on top of aluminum bin
                else if (aluminum_amount > 0 && character.GetComponent<Coralina>().aluminum_bin)
                {
                    Algorithim(i);
                    aluminum_amount = 0;
                    yield return new WaitForSeconds(1);
                }
              
            }
            else if (AlgothimDevelopment.cellArray[i, 0] == "PickUp")
            {
                //Nothing to PickUp.
                if (character.GetComponent<Coralina>().UnderCoralina == null)
                {
                    Algorithim(i);
                    yield return new WaitForSeconds(1);
                }
                //has not pickup Any trash. Nothing Bad Happends
                else if (glass_amount == 0 && aluminum_amount == 0 && plastic_amount == 0)
                {
                    IncreaseTrashCounter(character.GetComponent<Coralina>().UnderCoralina.tag);
                    Algorithim(i);
                    yield return new WaitForSeconds(1);
                }

                else if (character.GetComponent<Coralina>().UnderCoralina.tag == "aluminum" && glass_amount == 0 && plastic_amount == 0)
                {
                    IncreaseTrashCounter(character.GetComponent<Coralina>().UnderCoralina.tag);
                    Algorithim(i);
                    yield return new WaitForSeconds(1);
                }
                else if (character.GetComponent<Coralina>().UnderCoralina.tag == "plastic" && aluminum_amount == 0 && glass_amount == 0)
                {
                    IncreaseTrashCounter(character.GetComponent<Coralina>().UnderCoralina.tag);
                    Algorithim(i);
                    yield return new WaitForSeconds(1);
                }
                else if (character.GetComponent<Coralina>().UnderCoralina.tag == "glass" && aluminum_amount == 0 && plastic_amount == 0)
                {
                    IncreaseTrashCounter(character.GetComponent<Coralina>().UnderCoralina.tag);
                    Algorithim(i);
                    yield return new WaitForSeconds(1);
                }
                //Want to PickUp Trash but has alredy pickup a different type of trash
                else
                {
                    Debug.Log("Mezclar tipo de Basura");
                    yield return new WaitForSeconds(1);
                    ResetScene();

                }

            }        

        }
        if (check)
        {
            check = false;
            CurrentPosition = character.transform.position;
            CheckPoint();
            
        }
        else {
            ResetScene();
        }
    }

    private void IncreaseTrashCounter(string type) {

        switch (type)
        {
            case "aluminum":
                Debug.Log("alumino");
                aluminum_amount++;
                break;
            case "plastic":
                plastic_amount++;
                break;
            case "glass":
                glass_amount++;
                break;
        }
        totalAmountofTrash--;
    }

    private void CheckPoint() {
        //Reset Coralina to Start Position before the movements
        character.transform.position = CurrentPosition;
        //Deletes the previous blocks
        AlgothimDevelopment.resetCell();    
    }

//Reset Everything
    public void ResetScene() {

        //Reset Coralina to Start Position
        character.transform.position = StartPosition;

        //Show all the Trash of the Scene
        GameObject TrashParent = GameObject.Find("Trash");
        for (int i = 0; i < TrashParent.transform.childCount; i++)
        {
            TrashParent.transform.GetChild(i).gameObject.SetActive(true);
        }
        //Reset Points and Trash Counter to ZERO
        aluminum_amount = plastic_amount = glass_amount = score = 0;

        //Reset the Amount of Trash Left to Pick Up
        totalAmountofTrash = GameObject.Find("Trash").transform.childCount;

        //Emoty Algorthim Development Cells
        //  AlgothimDevelopment.resetCell();

        AlgothimDevelopment.deleteSelected();
        AlgothimDevelopment.turnOFFToggle();
    }

}