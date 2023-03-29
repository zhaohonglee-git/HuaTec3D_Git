using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActuatorsBase : MonoBehaviour
{
    [Serializable]
    public struct ActuatorsBaseStruct {
        public string Identifier;
        public string Name;
        public string Unit;
        public string UnitName;
        public List<EnumFunctionsStruct> EnumList;
    }


    [Serializable]
    public struct EnumFunctionsStruct {
        public string FunctionCode;
        public string FunctionInfo;
    }

    public List<ActuatorsBaseStruct> ActuatorsBaseFuncs = new List<ActuatorsBaseStruct>();

    [SerializeField] private List<string> _jsonKeys = new List<string>();
    public List<string> JsonKeys { get => _jsonKeys; private set => _jsonKeys = value; }

    private void Start()
    {
        foreach (var item in ActuatorsBaseFuncs) {
            _jsonKeys.Add(item.Identifier);
        }
    }


}

public class LightControlItem
{
    List<string> strings = new List<string>()
    {
        "LightLuxValue_01","LightLuxValue_02"
    };
    List<Dictionary<string, object>> LightControl = new List<Dictionary<string, object>>();
        Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
    public LightControlItem() {
        keyValuePairs.Add("LightLuxValue_01", 200);
        keyValuePairs.Add("LightSwitch_01", 1);
        keyValuePairs.Add("LightState_01", 0);
        LightControl.Add(keyValuePairs);
    }
    
    /// <summary>
    /// 
    /// </summary>
    public int LightLuxValue_01 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int LightSwitch_01 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int LightState_01 { get; set; }

}

public class AirConditionerControlItem
{
    /// <summary>
    /// 
    /// </summary>
    public int AirConditionerControl_01 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int AirConditionerState_01 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int AirConditionerControlModelSet_01 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int AirConditionerTemperatureSet_01 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int AirConditionerWorkingGrade_01 { get; set; }
}

public class VMC_ControlModelSetItem
{
    /// <summary>
    /// 
    /// </summary>
    public int VMC_ControlModelSet_01 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int VMC_TemperatureSet_01 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int VMC_WorkingGrade_01 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int VMC_State_01 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int VMC_AlarmCode_01 { get; set; }
}

public class HumidityControllerControlModelSetItem
{
    /// <summary>
    /// 
    /// </summary>
    public int HumidityControllerControlModelSet_01 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int HumidityControllerControllSet_01 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int HumidityControllerTemperatureSet_01 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int HumidityControllerState_01 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int HumidityControllerAlarmCode_01 { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int HumidityControllerCurrentHumidity_01 { get; set; }
}

public class @params
{
    /// <summary>
    /// 
    /// </summary>
    public List<LightControlItem> LightControl { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<AirConditionerControlItem> AirConditionerControl { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<VMC_ControlModelSetItem> VMC_ControlModelSet { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<HumidityControllerControlModelSetItem> HumidityControllerControlModelSet { get; set; }
}

public class Root
{
    /// <summary>
    /// 
    /// </summary>
    public int id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public @params @params { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string version { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string method { get; set; }
}
