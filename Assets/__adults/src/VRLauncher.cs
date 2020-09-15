using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using GEX.utils;

public enum ShootMode
{ 
    WITH_VIEW,
    WITH_HAND
}

public class VRLauncher : MonoBehaviour
{
    
    public class BoolMonitor
    {
        public delegate bool BoolGenerator();

        private string m_name = "";
        private BoolGenerator m_generator;
        private bool m_prevValue = false;
        private bool m_currentValue = false;
        private bool m_currentValueRecentlyChanged = false;
        private float m_displayTimeout = 0.0f;
        private float m_displayTimer = 0.0f;

        public BoolMonitor(string name, BoolGenerator generator, float displayTimeout = 0.5f)
        {
            m_name = name;
            m_generator = generator;
            m_displayTimeout = displayTimeout;
        }

        public void Update()
        {
            m_prevValue = m_currentValue;
            m_currentValue = m_generator();

            if (m_currentValue != m_prevValue)
            {
                m_currentValueRecentlyChanged = true;
                m_displayTimer = m_displayTimeout;
            }

            if (m_displayTimer > 0.0f)
            {
                m_displayTimer -= Time.deltaTime;

                if (m_displayTimer <= 0.0f)
                {
                    m_currentValueRecentlyChanged = false;
                    m_displayTimer = 0.0f;
                }
            }
        }

        public void AppendToStringBuilder(ref StringBuilder sb)
        {
            sb.Append(m_name);

            if (m_currentValue && m_currentValueRecentlyChanged)
                sb.Append(": *True*\n");
            else if (m_currentValue)
                sb.Append(":  True \n");
            else if (!m_currentValue && m_currentValueRecentlyChanged)
                sb.Append(": *False*\n");
            else if (!m_currentValue)
                sb.Append(":  False \n");
        }
    }

    public static VRLauncher me
    {
        get
        {
            if (_me == null)
                _me = GameObject.FindObjectOfType<VRLauncher>();

            return _me;
        }
    }

    private static VRLauncher _me = null;

    public MainMenuPanel MAIN_MENU;
    public GameObject ring;
    public GameObject beachBall;
    //public Text uiText;
    private List<BoolMonitor> monitors;
    private StringBuilder data;
    private BeachBall currBall;
    private Rigidbody rg;
    private bool LAUNCHING = false;
    private ShootMode shootMode = ShootMode.WITH_HAND;
    private Vector3 savedBallScale;
    private bool _ENABLE_ = false;
    private bool AUTO_RELAUNCH_MODE = false;
    private bool WAIT_FIRST_TRIGGER_AUTO_RELAUNCH = false;
    private XTimer autoRelaunchTimer;
    private XTimer holdTimer;
    //private Quaternion savedRotation;

    private const float AUTO_RELAUNCH_TIMER = 0.4f;
    
    void Start()
    {
        //savedRotation = this.transform.rotation;

        holdTimer = new XTimer();

        autoRelaunchTimer = new XTimer();
        autoRelaunchTimer.setDelay(AUTO_RELAUNCH_TIMER);
        autoRelaunchTimer.start();

#if UNITY_EDITOR
        shootMode = ShootMode.WITH_VIEW;
#else
        shootMode = ShootMode.WITH_HAND;
#endif
        Setup(shootMode);
        /*if (uiText != null)
        {
            uiText.supportRichText = false;
        }*/

        data = new StringBuilder(2048);

        monitors = new List<BoolMonitor>()
		{
			// virtual
			new BoolMonitor("WasRecentered",                    () => OVRInput.GetControllerWasRecentered()),
			new BoolMonitor("One",                              () => OVRInput.Get(OVRInput.Button.One)),
			new BoolMonitor("OneDown",                          () => OVRInput.GetDown(OVRInput.Button.One)),
			new BoolMonitor("OneUp",                            () => OVRInput.GetUp(OVRInput.Button.One)),
			new BoolMonitor("One (Touch)",                      () => OVRInput.Get(OVRInput.Touch.One)),
			new BoolMonitor("OneDown (Touch)",                  () => OVRInput.GetDown(OVRInput.Touch.One)),
			new BoolMonitor("OneUp (Touch)",                    () => OVRInput.GetUp(OVRInput.Touch.One)),
			new BoolMonitor("Two",                              () => OVRInput.Get(OVRInput.Button.Two)),
			new BoolMonitor("TwoDown",                          () => OVRInput.GetDown(OVRInput.Button.Two)),
			new BoolMonitor("TwoUp",                            () => OVRInput.GetUp(OVRInput.Button.Two)),
			new BoolMonitor("PrimaryIndexTrigger",              () => OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)),
			new BoolMonitor("PrimaryIndexTriggerDown",          () => OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)),
			new BoolMonitor("PrimaryIndexTriggerUp",            () => OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger)),
			new BoolMonitor("PrimaryIndexTrigger (Touch)",      () => OVRInput.Get(OVRInput.Touch.PrimaryIndexTrigger)),
			new BoolMonitor("PrimaryIndexTriggerDown (Touch)",  () => OVRInput.GetDown(OVRInput.Touch.PrimaryIndexTrigger)),
			new BoolMonitor("PrimaryIndexTriggerUp (Touch)",    () => OVRInput.GetUp(OVRInput.Touch.PrimaryIndexTrigger)),
			new BoolMonitor("PrimaryHandTrigger",               () => OVRInput.Get(OVRInput.Button.PrimaryHandTrigger)),
			new BoolMonitor("PrimaryHandTriggerDown",           () => OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger)),
			new BoolMonitor("PrimaryHandTriggerUp",             () => OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger)),
			new BoolMonitor("Up",                               () => OVRInput.Get(OVRInput.Button.Up)),
			new BoolMonitor("Down",                             () => OVRInput.Get(OVRInput.Button.Down)),
			new BoolMonitor("Left",                             () => OVRInput.Get(OVRInput.Button.Left)),
			new BoolMonitor("Right",                            () => OVRInput.Get(OVRInput.Button.Right)),
			new BoolMonitor("Touchpad (Click)",                 () => OVRInput.Get(OVRInput.Button.PrimaryTouchpad)),
			new BoolMonitor("TouchpadDown (Click)",             () => OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad)),
			new BoolMonitor("TouchpadUp (Click)",               () => OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad)),
			new BoolMonitor("Touchpad (Touch)",                 () => OVRInput.Get(OVRInput.Touch.PrimaryTouchpad)),
			new BoolMonitor("TouchpadDown (Touch)",             () => OVRInput.GetDown(OVRInput.Touch.PrimaryTouchpad)),
			new BoolMonitor("TouchpadUp (Touch)",               () => OVRInput.GetUp(OVRInput.Touch.PrimaryTouchpad)),

			// raw
			new BoolMonitor("Start",                            () => OVRInput.Get(OVRInput.RawButton.Start)),
			new BoolMonitor("StartDown",                        () => OVRInput.GetDown(OVRInput.RawButton.Start)),
			new BoolMonitor("StartUp",                          () => OVRInput.GetUp(OVRInput.RawButton.Start)),
			new BoolMonitor("Back",                             () => OVRInput.Get(OVRInput.RawButton.Back)),
			new BoolMonitor("BackDown",                         () => OVRInput.GetDown(OVRInput.RawButton.Back)),
			new BoolMonitor("BackUp",                           () => OVRInput.GetUp(OVRInput.RawButton.Back)),
			new BoolMonitor("A",                                () => OVRInput.Get(OVRInput.RawButton.A)),
			new BoolMonitor("ADown",                            () => OVRInput.GetDown(OVRInput.RawButton.A)),
			new BoolMonitor("AUp",                              () => OVRInput.GetUp(OVRInput.RawButton.A)),
		};
    }
    static string prevConnected = "";
    static BoolMonitor controllers = new BoolMonitor("Controllers Changed", () => { return OVRInput.GetConnectedControllers().ToString() != prevConnected; });


    private void Setup(ShootMode shootMode)
    {
        this.shootMode = shootMode;

        GameObject node = null;

        if (shootMode == ShootMode.WITH_HAND)
        {
            node = GameObject.Find("TrackedRemote");
            this.transform.SetParent(node.transform);
            this.transform.localPosition = new Vector3(0f, 0f, 0.25f);
            this.transform.localRotation = Quaternion.identity;
        }
        else if (shootMode == ShootMode.WITH_VIEW)
        {
           // node = GameObject.Find("CenterEyeAnchor");
           // this.transform.SetParent(node.transform);
            this.transform.localPosition = new Vector3(0f, -0.5f, 0.5f);
            this.transform.localRotation = Quaternion.Euler(-16, 0, 0);//savedRotation;
        }
    }

    public void SetAutoRelaunch(bool active)
    {
        autoRelaunchTimer.setDelay(AUTO_RELAUNCH_TIMER);
        autoRelaunchTimer.start();
        //AUTO_RELAUNCH_MODE = active;

        //
        // New change
        //
        if (active)
        {
            AUTO_RELAUNCH_MODE =                false;
            WAIT_FIRST_TRIGGER_AUTO_RELAUNCH =  true;
        }
        else
        {
            AUTO_RELAUNCH_MODE =                false;
            WAIT_FIRST_TRIGGER_AUTO_RELAUNCH =  false;
        }
    }

    private BeachBall CREATE_BALL()
    {
        ColorType colorType = RingManager.me.GetNextColorType();

        if (colorType == ColorType.CANT_CREATE_NOW)
            return null;

        GameObject gobj = GameObject.Instantiate(beachBall, this.transform.position, Quaternion.identity);
        BeachBall _ball = gobj.GetComponent<BeachBall>();
        _ball.Init(transform);
        _ball.SetType(colorType);
        
        rg = gobj.GetComponent<Rigidbody>();
        rg.useGravity = false;
        rg.detectCollisions = false;
        savedBallScale = gobj.transform.localScale;
        gobj.transform.localScale = new Vector3(0f, 0f, 0f);

        if(shootMode == ShootMode.WITH_VIEW)
            gobj.transform.DOScale(savedBallScale, 0.3f);
        else if (shootMode == ShootMode.WITH_HAND)
            gobj.transform.DOScale(savedBallScale / 3f, 0.3f);
        

        return _ball;
    }

    public void DisableAndKill()
    {
        if (currBall != null)
        {
            GameObject.Destroy(currBall.gameObject);
            currBall = null;
        }

        _ENABLE_ = false;
        LAUNCHING = false;
    }

    public void Enable()
    {
        _ENABLE_ = true;
    }

    void Update()
    {
        OVRInput.Controller activeController = OVRInput.GetActiveController();

        /*data.Length = 0;
        byte recenterCount = OVRInput.GetControllerRecenterCount();
        data.AppendFormat("RecenterCount: {0}\n", recenterCount);

        byte battery = OVRInput.GetControllerBatteryPercentRemaining();
        data.AppendFormat("Battery: {0}\n", battery);

        float framerate = OVRPlugin.GetAppFramerate();
        data.AppendFormat("Framerate: {0:F2}\n", framerate);

        string activeControllerName = activeController.ToString();
        data.AppendFormat("Active: {0}\n", activeControllerName);

        string connectedControllerNames = OVRInput.GetConnectedControllers().ToString();
        data.AppendFormat("Connected: {0}\n", connectedControllerNames);

        data.AppendFormat("PrevConnected: {0}\n", prevConnected);

        controllers.Update();
        controllers.AppendToStringBuilder(ref data);

        prevConnected = connectedControllerNames;

        Quaternion rot = OVRInput.GetLocalControllerRotation(activeController);
        data.AppendFormat("Orientation: ({0:F2}, {1:F2}, {2:F2}, {3:F2})\n", rot.x, rot.y, rot.z, rot.w);

        Vector3 angVel = OVRInput.GetLocalControllerAngularVelocity(activeController);
        data.AppendFormat("AngVel: ({0:F2}, {1:F2}, {2:F2})\n", angVel.x, angVel.y, angVel.z);

        Vector3 angAcc = OVRInput.GetLocalControllerAngularAcceleration(activeController);
        data.AppendFormat("AngAcc: ({0:F2}, {1:F2}, {2:F2})\n", angAcc.x, angAcc.y, angAcc.z);

        Vector3 pos = OVRInput.GetLocalControllerPosition(activeController);
        data.AppendFormat("Position: ({0:F2}, {1:F2}, {2:F2})\n", pos.x, pos.y, pos.z);

        Vector3 vel = OVRInput.GetLocalControllerVelocity(activeController);
        data.AppendFormat("Vel: ({0:F2}, {1:F2}, {2:F2})\n", vel.x, vel.y, vel.z);

        Vector3 acc = OVRInput.GetLocalControllerAcceleration(activeController);
        data.AppendFormat("Acc: ({0:F2}, {1:F2}, {2:F2})\n", acc.x, acc.y, acc.z);

        Vector2 primaryTouchpad = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
        data.AppendFormat("PrimaryTouchpad: ({0:F2}, {1:F2})\n", primaryTouchpad.x, primaryTouchpad.y);

        Vector2 secondaryTouchpad = OVRInput.Get(OVRInput.Axis2D.SecondaryTouchpad);
        data.AppendFormat("SecondaryTouchpad: ({0:F2}, {1:F2})\n", secondaryTouchpad.x, secondaryTouchpad.y);

        float indexTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
        data.AppendFormat("PrimaryIndexTriggerAxis1D: ({0:F2})\n", indexTrigger);

        float handTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger);
        data.AppendFormat("PrimaryHandTriggerAxis1D: ({0:F2})\n", handTrigger);

        for (int i = 0; i < monitors.Count; i++)
        {
            monitors[i].Update();
            monitors[i].AppendToStringBuilder(ref data);
        }

        if (uiText != null)
        {
            uiText.text = data.ToString();
        }*/



        /*bool pressed = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger);
        if (pressed)
            TriggerPressed();*/

        /*if (Input.GetMouseButtonDown(0))
            TriggerPressed();*/


        
        //else
        {

            /*if (WAIT_FIRST_TRIGGER_AUTO_RELAUNCH)
            {
                if (TriggerDown())
                    WAIT_FIRST_TRIGGER_AUTO_RELAUNCH = false;
                else
                    return;
            }*/


            if (LAUNCHING)
            {
                if (AUTO_RELAUNCH_MODE)
                {
                    if (autoRelaunchTimer.update())
                    {
                        autoRelaunchTimer.start();
                        currBall = null;
                        LAUNCHING = false;
                    }
                }
                else if (currBall.CanRelaunch())
                {
                    currBall = null;
                    LAUNCHING = false;
                }
            }
            else if (_ENABLE_)
            {

                if (currBall == null)
                {
                    currBall = CREATE_BALL();
                    
                    if (currBall == null) // Can't create any cause RingManager doesn't create any. Just wait
                        return;
                }

                Vector3 pos = this.transform.position;
                currBall.transform.position = Vector3.Lerp(currBall.transform.position, pos, Time.deltaTime * 10f);

                if (TriggerDown() || AUTO_RELAUNCH_MODE)
                {
                    if (WAIT_FIRST_TRIGGER_AUTO_RELAUNCH)
                    {
                        WAIT_FIRST_TRIGGER_AUTO_RELAUNCH = false;
                        AUTO_RELAUNCH_MODE = true;
                    }

                    TriggerPressed2();
                }
                


                /*else if (OVRInput.Get(OVRInput.Button.Left))
                {
                    Setup(ShootMode.WITH_VIEW);
                }
                else if (OVRInput.Get(OVRInput.Button.Right) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    Setup(ShootMode.WITH_HAND);
                }*/
                /*else if (OVRInput.GetDown(OVRInput.Button.Back) && OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
                //if(Input.GetMouseButtonDown(0))
                {
                    if (MAIN_MENU.gameObject.activeInHierarchy)
                    {
                        MAIN_MENU.gameObject.SetActive(false);
                    }
                    else
                    {
                        MAIN_MENU.gameObject.SetActive(true);
                    }
                }*/
            }
        }

    }

    private bool TriggerDown()
    {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
#else
        //if (Pvr_ControllerManager.controllerlink.Controller1.Trigger.PressedDown || Pvr_ControllerManager.controllerlink.Controller0.Trigger.PressedDown)
        if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        //if(OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
#endif
                return true;
        else
            return false;
    }

    private void TriggerPressed2()
    {
        //float force = 700;
        float force = 900f;
        float torque = 500f;

        if (shootMode == ShootMode.WITH_HAND)
        {
            //currBall.gameObject.transform.localScale = savedBallScale;//new Vector3(6, 6, 6);
            currBall.gameObject.transform.DOScale(savedBallScale, 0.3f);
        }

        rg.AddForce(this.transform.forward * force, ForceMode.Force);

        if (torque != 0f)
            rg.AddTorque((new Vector3(10, 20, 30).normalized) * torque, ForceMode.Acceleration);

        currBall.Launch();
        LAUNCHING = true;
    }

    /*private void TriggerPressed()
    {

        //GameObject gobj = GameObject.CreatePrimitive(PrimitiveType.Cube);

        GameObject element = null;
        float force = 200;
        float torque = 0;

        if (GameMode.MODE == GMode.MODE_A)
        {
            element = ring;
            force = 350;
        }
        else
        {
            element = beachBall;
            force = 600;
            torque = 500f;
        }

        GameObject gobj = GameObject.Instantiate(element, this.transform.position, Quaternion.identity);
        //gobj.transform.SetPositionAndRotation(this.transform.position, Quaternion.identity);

        Rigidbody rg = gobj.GetComponent<Rigidbody>();

        rg.AddForce(this.transform.forward * force, ForceMode.Force);

        if (torque != 0f)
            rg.AddTorque((new Vector3(10, 20, 30).normalized) * torque, ForceMode.Acceleration);

    }*/
}

