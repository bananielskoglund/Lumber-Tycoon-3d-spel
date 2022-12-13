using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TempAxeScript : MonoBehaviour
{
    public int playerBalance = 0;

    [Header("RayCast")]
    public float range = 5;
    public GameObject ShopObject;
    public GameObject ShopCanvas;
    public int HitsToKill = 5;

    [Header("Axe Charge")]
    public float chargeTime = 1;

    bool shopIsOpen = false;
    float chargeLevel = 0f;
    int chargeCount = 0;




    public Button Axe1_btn, Axe2_btn, Axe3_btn;

    public TextMeshProUGUI Axe1_priceTag;

    public class Axe
    {
        public int power;
        public int cost;
        public int speed;

        public Axe(int power, int cost, int speed)
        {
            power = power;
            cost = cost;
            speed = speed;
        }
    }
    
    public Axe Axe1 = new Axe(10, 10, 1);
    public Axe Axe2 = new Axe(50, 100, 2);
    public Axe Axe3 = new Axe(200, 1000, 5);
 
    void Start() {
        ShopObject.GetComponent<Outline>().enabled = false;
        Axe1_priceTag.text = "$"+playerBalance;
        // Axe1.onClick.AddListener(() => )        
    }

    void Update() {

        Vector3 direction = Vector3.forward;

        Ray myRayCast = new Ray(transform.position, transform.TransformDirection(direction * range));
        Debug.DrawRay(transform.position, transform.TransformDirection(direction * range));
        ShopObject.GetComponent<Outline>().enabled = false;

        if (Physics.Raycast(myRayCast, out RaycastHit hit, range))
        {
            if (hit.collider.tag == "Shop"){
                ShopObject.GetComponent<Outline>().enabled = true;
                if (Input.GetKeyDown(KeyCode.F) && shopIsOpen == false)
                {
                    Cursor.visible = true;
                    ShopCanvas.SetActive(true);
                    shopIsOpen = true;
                    GetComponent<SC_FPSController>().canMove = false;
                    Cursor.lockState = CursorLockMode.None;
                }
                else if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Escape)  && shopIsOpen == true)
                {
                    Cursor.visible = false;
                    ShopCanvas.SetActive(false);
                    shopIsOpen = false;
                    GetComponent<SC_FPSController>().canMove = true;
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }


    }

    void HandleButtonClicked(int playerBalance) {

    }

}
