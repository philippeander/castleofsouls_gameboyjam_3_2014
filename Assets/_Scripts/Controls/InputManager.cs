using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour {
    static public InputManager Instance;

    private Vector2 m_Axis = new Vector2(); //Save the arrows keys input

    ////Overrides the GetAxes function in Update method
    private bool m_IsMobleInput; 
    private bool m_IsArrowKeysClicked;

    //Call Limiter for update
    private float X_isClicked;
    private float Y_isClicked;

    #region ACTIONS
    public delegate void ArrowKeys(Vector2 axis);
    public event ArrowKeys ArrowKeysAction;

    public delegate void A_Btn();
    public event A_Btn A_BtnAction;

    public delegate void B_Btn();
    public event B_Btn B_BtnAction;

    public delegate void Start_Btn();
    public event Start_Btn Start_BtnAction;

    public delegate void Select_Btn();
    public event Select_Btn Select_BtnAction;
#endregion

    public Vector2 GetAxis { //if necessary, take the keyboard input
        get {


            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        
    }
    public Vector2 Axis {
        get {
            return m_Axis;
        }
        set {
            m_Axis = value;
            ArrowKeysAction(m_Axis);
        }
    }
    public bool IsArrowKeysClicked {
        get {
            return m_IsArrowKeysClicked;
        }

        set {
            m_IsArrowKeysClicked = value;
        }
    }

    private void Awake() {
        Instance = this;
    }
    private void Start () {
        m_IsMobleInput = false;
        IsArrowKeysClicked = false;
    }
    private void OnDestroy() {
        
    }
    private void Update () {
        KeyboardInput();

    }

    //Methods for Ui buttons
    public void X_GetAxis(float val) {
        m_IsMobleInput = val != 0;
        Axis = new Vector2(val, 0);
    }
    public void Y_GetAxis(float val) {
        m_IsMobleInput = val != 0;
        Axis = new Vector2(0, val); 
    }
    public void A_Button() {
        A_BtnAction();
    }
    public void B_Button() {
        B_BtnAction();
    }
    public void Start_Button() {
        Start_BtnAction();
    }
    public void Select_Button() {
        Select_BtnAction();
    }

    //Method for Keyboard Input
    private void KeyboardInput() {
        if (!m_IsMobleInput) {

            float x = GetAxis.x;
            float y = GetAxis.y;
            
            if (x != X_isClicked) {
                Axis = new Vector2(x, Axis.y);
                X_isClicked = x;
            }
            if (y != Y_isClicked) {
                Axis = new Vector2(Axis.x, y);
                Y_isClicked = y;
            }

            if (Input.GetKeyDown(KeyCode.Z)) {
                A_BtnAction();
            }
            if (Input.GetKeyDown(KeyCode.X)) {
                B_BtnAction();
            }
            if (Input.GetKeyDown(KeyCode.Insert)) {
                Start_BtnAction();
            }
            if (Input.GetKeyDown(KeyCode.Space)) {
                Select_BtnAction();
            }
        }
    }
    
}