using UnityEngine;
using System.Collections;


public class caron : MonoBehaviour
{

    //操纵前轮，用于转向
    public WheelCollider FrontLeftWheel;
    public WheelCollider FrontRightWheel;

    public WheelCollider BackLeftWheel;
    public WheelCollider BackRightWheel;

    //齿轮数组
    public float[] GearRatio;
    //当前档位
    public int CurrentGear = 0;

    public float EngineTorgue = 600.0f;
    public float MaxEngineRPM = 3000.0f;
    public float MinEngineRPM = 1000.0f;
    private float EngineRPM = 0.0f;


    private Rigidbody rb;
    // Use this for initialization

    void Start()
    {


        rb = GetComponent<Rigidbody>();
        //设置车的重心，使车更稳定 

        Vector3 centerOfMass = rb.centerOfMass;
        centerOfMass.y = -1.5f;
        rb.centerOfMass = centerOfMass;
    }

    // Update is called once per frame
    void Update()
    {
        //限制车的最大速度，调整阻力可能不是最好的做法。但它很简单，而且不会干扰物理系统的运行。
        rb.drag = rb.velocity.magnitude / 250;
        //通过两个轮子的平均rpm，计算引擎rpm，然后切换档位
        EngineRPM = (FrontLeftWheel.rpm + FrontRightWheel.rpm) / 2 * GearRatio[CurrentGear];
        ShiftGears();


        //设置换档的声音
        /*
        audio.pitch=Mathf.Abs(EngineRPM/MaxEngineRPM)+1.0f;
        if(audio.pitch>2.0)
        {
        audio.pitch=2.0f;
        }*/
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        Debug.Log("V" + v);
        Debug.Log("H" + h);
        //最后设置轮子转动力矩。引擎力矩除以当前档位，乘以用户输入值。
        //轮子力矩提供一个汽车前进的力。轮子的转动又会提高档位。
        FrontLeftWheel.motorTorque = EngineTorgue / GearRatio[CurrentGear] * Input.GetAxis("Vertical");
        Debug.Log(FrontLeftWheel.motorTorque);
        FrontRightWheel.motorTorque = EngineTorgue / GearRatio[CurrentGear] * Input.GetAxis("Vertical");

        //转动角度是任意数乘以用户输入值


        FrontLeftWheel.steerAngle = 20 * Input.GetAxis("Horizontal");
        FrontRightWheel.steerAngle = 20 * Input.GetAxis("Horizontal");
    }
    void ShiftGears()
    {
        int AppropriateGear = 0;
        if (EngineRPM >= MaxEngineRPM)
        {
            AppropriateGear = CurrentGear;
            for (int i = 0; i < GearRatio.Length; i++)
            {
                if (FrontLeftWheel.rpm * GearRatio[i] < MaxEngineRPM)
                {
                    AppropriateGear = i;
                    break;
                }
            }
            CurrentGear = AppropriateGear;
        }

        if (EngineRPM <= MaxEngineRPM)
        {
            AppropriateGear = CurrentGear;
            for (int j = GearRatio.Length - 1; j >= 0; j--)
            {
                if (FrontLeftWheel.rpm * GearRatio[j] > MinEngineRPM)
                {
                    AppropriateGear = j;
                    break;
                }
            }
            CurrentGear = AppropriateGear;
        }
    }
}