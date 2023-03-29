using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class VMCController : MonoBehaviour
{
    [Header("1. 新风系统工做模式设置:制热、制冷、过滤")]
    public string[] VMC_ControlModelSet = new string[] { "0", "1", "2" };
    [Header("2. 新风系统送风温度设定[18-30]")]
    [Range(18.0f, 30.0f)]
    public float VMC_TemperatureSet;
    [Header("3. 新风系统工作强度设置:Low, Medium, High")]
    public string[] VMC_WorkingGrade = new string[] { "1", "2", "3" };

    [Header("4. 新风系统运行状态反馈: 0-停机，1-运行中，2-休眠， 3-离线， 4-故障")]
    public string[] VMC_RunningState = new string[] { "0", "1", "2", "3", "4"};

    [Header("5. 新风系统运行模式状态反馈: 0-制热模式， 1-制冷模式， 2-过滤模式")]
    public string[] VMC_RunningModelState = new string[] { "0", "1", "2"};

    [Header("6. 新风系统运行强度状态反馈: 1-工作强度Low， 2-工作强度Medium， 3-工作强度High")]
    public string[] VMC_RunningGradeState = new string[] { "1", "2", "3"};

    [Header("7. 新风系统故障代码反馈: 0-正常， 1-需更换滤网， 2-进风口气压异常， 3-出风口气压异常， 4-风机故障， 5-其他")]
    public string[] VMC_AlarmCode = new string[] { "0", "1", "2", "3", "4", "5"};

    [Space(10)]
    [Header("新风系统所有属性key的列表")]
    [SerializeField] private List<string> _vmc_DataKey = new List<string>();
    public List<string> _VMC_DataKey { get => _vmc_DataKey; private set => _vmc_DataKey = value; }

    public Dictionary<string, string> VMC_DataDic = new Dictionary<string, string>();
    public bool IsRealValueModel;

    [Header("新风系统对接IOT云平台Json格式数据")]
    [SerializeField] private string _vmc_DataJson;
    public string VMC_DataJson { get => _vmc_DataJson; private set => _vmc_DataJson = value; }
    

    private void Start()
    {
        ActuatorsBase _theActuatorsBase = transform.GetComponent<ActuatorsBase>();

        for (int i = 0; i < _theActuatorsBase.ActuatorsBaseFuncs.Count; i++) {
            _vmc_DataKey.Add(_theActuatorsBase.ActuatorsBaseFuncs[i].Identifier);
        };

        // 初始化设备
        GenerateVMCDate();

        GetData();
    }

    public void GenerateVMCDate(string controlModelSet = "0", string temperatureSet = "24.0", string workingGrade = "2", string runningState = "1", string runningModelState = "0", string runingGradeState = "2", string alarmCode = "0") {
        VMC_DataDic[_vmc_DataKey[0]] = controlModelSet;
        VMC_DataDic[_vmc_DataKey[1]] = temperatureSet;
        VMC_DataDic[_vmc_DataKey[2]] = workingGrade;
        VMC_DataDic[_vmc_DataKey[3]] = runningState;
        VMC_DataDic[_vmc_DataKey[4]] = runningModelState;
        VMC_DataDic[_vmc_DataKey[5]] = runingGradeState;
        VMC_DataDic[_vmc_DataKey[6]] = alarmCode;
    }


    // 新风系统控制（Unity模拟控制）
    public void ControlVMCByUnity() {
        string controlModelSet = VMC_ControlModelSet[Random.Range(0, VMC_ControlModelSet.Length)];
        string temperatureSet = Random.Range(18.0f, 30.0f).ToString("#0.0");
        string workingGrade = VMC_WorkingGrade[Random.Range(0, VMC_WorkingGrade.Length)];

        string runningState = VMC_RunningState[Random.Range(0, VMC_RunningState.Length)];

        //string runningModelState = VMC_RunningModelState[Random.Range(0, VMC_RunningModelState.Length)];
        //string runingGradeState = VMC_RunningGradeState[Random.Range(0, VMC_RunningGradeState.Length)];
        string runningModelState = controlModelSet;
        string runingGradeState = workingGrade;

        string alarmCode = VMC_AlarmCode[Random.Range(0, VMC_AlarmCode.Length)];

        VMC_TemperatureSet = float.Parse(temperatureSet);

        GenerateVMCDate(controlModelSet, temperatureSet, workingGrade, runningState, runningModelState, runingGradeState, alarmCode);
    }


    public void GetData() {
        if (IsRealValueModel)
        {
            //Debug.Log("从IOT云平台请求真实新风系统数据值并赋值给VMC_DataDic");
        }
        else {
            ControlVMCByUnity();

            _vmc_DataJson = JsonConvert.SerializeObject(VMC_DataDic);
            Debug.Log(_vmc_DataJson);
        }
    }

}