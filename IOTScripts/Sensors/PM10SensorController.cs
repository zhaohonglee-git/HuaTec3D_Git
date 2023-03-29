using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PM10SensorController : MonoBehaviour
{
    [Header("PM10传感器 [0,2000] μg/m3 step:1")]
    private int step = 1;
    
    [SerializeField] private int _pm10Value;
    public int Pm10Value { get => _pm10Value; private set => _pm10Value = value; }

    [Header("PM10传感器属性Key值")]
    public string Pm10_DataDicKey;
    public Dictionary<string, string> Pm10_DataDic = new Dictionary<string, string>();

    public bool IsRealValueModel;

    [Header("PM10传感器对接IOT云平台Json格式数据")]
    [SerializeField] private string _pm10_DataJson;
    public string Pm10_DataJson { get => _pm10_DataJson; private set => _pm10_DataJson = value; }

    private void Start()
    {
        SensorsBase _theSensorBase = transform.GetComponent<SensorsBase>();
        Pm10_DataDicKey = _theSensorBase.Identifier;
        GetValue();
    }

    public void GetValue() {
        if (IsRealValueModel)
        {
            Debug.Log("从IOT云平台请求真实传感器数据值并赋值给_pm10Value");
        }
        else {
            _pm10Value = DataGenerater.IntGenerater(1010, -400, 400);
            Pm10_DataDic[Pm10_DataDicKey] = _pm10Value.ToString("#0");
            Debug.Log("Unity自发虚拟数据");

            _pm10_DataJson = JsonConvert.SerializeObject(Pm10_DataDic);
            Debug.Log(_pm10_DataJson);
        }
    }

}
