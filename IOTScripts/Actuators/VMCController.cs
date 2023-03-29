using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class VMCController : MonoBehaviour
{
    [Header("1. �·�ϵͳ����ģʽ����:���ȡ����䡢����")]
    public string[] VMC_ControlModelSet = new string[] { "0", "1", "2" };
    [Header("2. �·�ϵͳ�ͷ��¶��趨[18-30]")]
    [Range(18.0f, 30.0f)]
    public float VMC_TemperatureSet;
    [Header("3. �·�ϵͳ����ǿ������:Low, Medium, High")]
    public string[] VMC_WorkingGrade = new string[] { "1", "2", "3" };

    [Header("4. �·�ϵͳ����״̬����: 0-ͣ����1-�����У�2-���ߣ� 3-���ߣ� 4-����")]
    public string[] VMC_RunningState = new string[] { "0", "1", "2", "3", "4"};

    [Header("5. �·�ϵͳ����ģʽ״̬����: 0-����ģʽ�� 1-����ģʽ�� 2-����ģʽ")]
    public string[] VMC_RunningModelState = new string[] { "0", "1", "2"};

    [Header("6. �·�ϵͳ����ǿ��״̬����: 1-����ǿ��Low�� 2-����ǿ��Medium�� 3-����ǿ��High")]
    public string[] VMC_RunningGradeState = new string[] { "1", "2", "3"};

    [Header("7. �·�ϵͳ���ϴ��뷴��: 0-������ 1-����������� 2-�������ѹ�쳣�� 3-�������ѹ�쳣�� 4-������ϣ� 5-����")]
    public string[] VMC_AlarmCode = new string[] { "0", "1", "2", "3", "4", "5"};

    [Space(10)]
    [Header("�·�ϵͳ��������key���б�")]
    [SerializeField] private List<string> _vmc_DataKey = new List<string>();
    public List<string> _VMC_DataKey { get => _vmc_DataKey; private set => _vmc_DataKey = value; }

    public Dictionary<string, string> VMC_DataDic = new Dictionary<string, string>();
    public bool IsRealValueModel;

    [Header("�·�ϵͳ�Խ�IOT��ƽ̨Json��ʽ����")]
    [SerializeField] private string _vmc_DataJson;
    public string VMC_DataJson { get => _vmc_DataJson; private set => _vmc_DataJson = value; }
    

    private void Start()
    {
        ActuatorsBase _theActuatorsBase = transform.GetComponent<ActuatorsBase>();

        for (int i = 0; i < _theActuatorsBase.ActuatorsBaseFuncs.Count; i++) {
            _vmc_DataKey.Add(_theActuatorsBase.ActuatorsBaseFuncs[i].Identifier);
        };

        // ��ʼ���豸
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


    // �·�ϵͳ���ƣ�Unityģ����ƣ�
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
            //Debug.Log("��IOT��ƽ̨������ʵ�·�ϵͳ����ֵ����ֵ��VMC_DataDic");
        }
        else {
            ControlVMCByUnity();

            _vmc_DataJson = JsonConvert.SerializeObject(VMC_DataDic);
            Debug.Log(_vmc_DataJson);
        }
    }

}