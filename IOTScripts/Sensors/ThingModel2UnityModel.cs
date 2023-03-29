using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using LitJson;
using System.Collections.Generic;
using static ActuatorsBase;
using Unity.VisualScripting;

public class ThingModel2UnityModel : MonoBehaviour
{
    [Header("ͨ����ȡ����StreamingAssets�ļ����µ���ģ��Json�ļ����Զ�����Unityģ��")]
    public List<string> AllActuatorsKeys = new List<string>();
    public List<string> AllSensorsKeys = new List<string>();
    private JsonData _thingModeJsonData;
    [Header("�ֶ�������ģ�����֣��磺'01_thing_model_test.json'")]
    public string ThingModelJsonFileName;

    private void Awake()
    {
        _thingModeJsonData = ReadJsonFromStreamingAssetsPath(ThingModelJsonFileName);
        Debug.Log(_thingModeJsonData["Sensors"][0]["name"].ToString());

        
        foreach (var key in _thingModeJsonData.Keys) {
            if (key != "Sensors")
            {
                AllActuatorsKeys.Add(key.ToString());
            }
            else if (key == "Sensors")
            {
                for (int i = 0; i < _thingModeJsonData["Sensors"].Count; i++) {
                    AllSensorsKeys.Add(_thingModeJsonData["Sensors"][i]["identifier"].ToString());
                }
            }
        }
    }

    private void Start()
    {
        // ����Sensors Actuator ����������
        GenerateAllSensorBase();
        GenerateAllActuatorBase();

        GenerateAllActuatorsBaseInfo();
        GenerateAllSensorsBaseInfo();

        GenerateDeviceControllerBase(transform);
    }


    // ����Device��Ӧ��DeviceControllerBase
    public void GenerateDeviceControllerBase(Transform theDeviceTransform) {
        theDeviceTransform.AddComponent<DeviceControllerBase>();
        Transform theSensorsTransform = theDeviceTransform.GetChild(0);
        Transform theActuatorsTransform = theDeviceTransform.GetChild(1);
        theDeviceTransform.GetComponent<DeviceControllerBase>().Sensors = theSensorsTransform;
        theDeviceTransform.GetComponent<DeviceControllerBase>().Actuators = theActuatorsTransform;
    }

    // ��ȡ���ص�json�ļ�����returnһ��JsonData
    public JsonData ReadJsonFromStreamingAssetsPath(string jsonName)
    {
        string url = Application.streamingAssetsPath + "/" + jsonName;
        Encoding endoning = Encoding.UTF8;
        StreamReader streamReader = new StreamReader(url, endoning);
        string jsonData = streamReader.ReadToEnd();
        JsonData jd = JsonMapper.ToObject(jsonData);
        return jd;
    }


    public void GenerateAllSensorBase() {
        GameObject sensors = new GameObject("Sensors");

        foreach (string sensor in AllSensorsKeys) {
            GameObject newGameObject =  new GameObject(sensor);
            newGameObject.transform.AddComponent<SensorsBase>();            
            newGameObject.transform.parent = sensors.transform;
        }
        sensors.transform.parent = transform;
    }

    public void GenerateAllActuatorBase() {
        GameObject actuators = new GameObject("Actuators");

        foreach (string actuator in AllActuatorsKeys)
        {
            GameObject newGameObject = new GameObject(actuator);
            newGameObject.transform.AddComponent<ActuatorsBase>();       
            newGameObject.transform.parent = actuators.transform;
        }

        actuators.transform.parent = transform;
    }

    // ������ģ��Json���ݣ���Sensor��Ϣչʾ��Unity�����
    public void GenerateAllSensorsBaseInfo() {
        Transform sensors = transform.GetChild(0);

        for (int i = 0; i < AllSensorsKeys.Count; i++) {

            sensors.GetChild(i).GetComponent<SensorsBase>().Identifier = _thingModeJsonData["Sensors"][i]["identifier"].ToString();
            sensors.GetChild(i).GetComponent<SensorsBase>().Name = _thingModeJsonData["Sensors"][i]["name"].ToString();

            if (_thingModeJsonData["Sensors"][i]["dataType"]["specs"].Keys.Contains("unit")) {
                sensors.GetChild(i).GetComponent<SensorsBase>().Unit = _thingModeJsonData["Sensors"][i]["dataType"]["specs"]["unit"].ToString();
            }

            if (_thingModeJsonData["Sensors"][i]["dataType"]["specs"].Keys.Contains("unitName")) { 
                sensors.GetChild(i).GetComponent<SensorsBase>().UnitName = _thingModeJsonData["Sensors"][i]["dataType"]["specs"]["unitName"].ToString();
            }
        }
    }


    // ������ģ��Json���ݣ���ִ�л����Ŀ�����Ϣչʾ��Unity�����
    public void ParshJson2ActuatorsInfo(JsonData jsonArray, ActuatorsBase theActuatorsBase) {
        
        for (int i = 0; i < jsonArray.Count; i++) {

            ActuatorsBaseStruct newActuatorsBaseStruct;

            newActuatorsBaseStruct.Identifier = jsonArray[i]["identifier"].ToString();
            newActuatorsBaseStruct.Name = jsonArray[i]["name"].ToString();

            if (jsonArray[i]["dataType"]["specs"].Keys.Contains("unit"))
            {
                newActuatorsBaseStruct.Unit = jsonArray[i]["dataType"]["specs"]["unit"].ToString();
            }
            else {
                newActuatorsBaseStruct.Unit = "";
            }

            if (jsonArray[i]["dataType"]["specs"].Keys.Contains("unitName"))
            {
                newActuatorsBaseStruct.UnitName = jsonArray[i]["dataType"]["specs"]["unitName"].ToString();
            }
            else {
                newActuatorsBaseStruct.UnitName = "";
            }


            List<EnumFunctionsStruct> theEnumFunctionsStructList = new List<EnumFunctionsStruct>();
            List<EnumFunctionsStruct> theEnumFunctionsStructList1 = new List<EnumFunctionsStruct>();

            if (jsonArray[i]["dataType"]["type"].ToString() == "enum")
            {
                IDictionary theEnumDic = jsonArray[i]["dataType"]["specs"] as IDictionary;

                for (int j = 0; j < jsonArray[i]["dataType"]["specs"].Count; j++)
                {
                    theEnumFunctionsStructList.Clear();
                    foreach (var key in theEnumDic.Keys) {
                        EnumFunctionsStruct theInfo;
                        theInfo.FunctionCode = key.ToString();
                        theInfo.FunctionInfo = theEnumDic[key].ToString();
                        theEnumFunctionsStructList.Add(theInfo);
                    }    
                }
                newActuatorsBaseStruct.EnumList = theEnumFunctionsStructList;
            }
            else
            {
                newActuatorsBaseStruct.EnumList = theEnumFunctionsStructList1;
            }
            theActuatorsBase.ActuatorsBaseFuncs.Add(newActuatorsBaseStruct);
        }
    }


    // �Զ�������ģ��Json�������е�ִ�л���ģ����Unity
    public void GenerateAllActuatorsBaseInfo() {
        for (int i = 0; i < transform.GetChild(1).childCount; i++ ){
            ParshJson2ActuatorsInfo(_thingModeJsonData[AllActuatorsKeys[i]], transform.GetChild(1).GetChild(i).GetComponent<ActuatorsBase>());
        }
    }

}
