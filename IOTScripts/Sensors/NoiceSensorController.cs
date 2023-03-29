using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiceSensorController : MonoBehaviour
{
    [Header("���������� [30,130] dB step:0.1")]
    private float step = 0.1f;

    [SerializeField] private float _noiceValue;
    public float NoiceValue { get => _noiceValue; private set => _noiceValue = value; }

    [Header("��������������Keyֵ")]
    public string Noice_DataDicKey;
    public Dictionary<string, string> Noice_DataDic = new Dictionary<string, string>();

    public bool IsRealValueModel;

    [Header("�����������Խ�IOT��ƽ̨Json��ʽ����")]
    [SerializeField] private string _noice_DataJson;
    public string Noice_DataJson { get => _noice_DataJson; private set => _noice_DataJson = value; }

    private void Start()
    {
        SensorsBase _theSensorBase = transform.GetComponent<SensorsBase>(); 
        
        Noice_DataDicKey = _theSensorBase.Identifier;
        GetValue();
    }

    public void GetValue()
    {
        if (IsRealValueModel)
        {
            //Debug.Log("��IOT��ƽ̨������ʵ����������ֵ����ֵ��_noiceValue");
        }
        else
        {
            _noiceValue = DataGenerater.FloatGenerater(50.0f, -20.0f, 50.0f);
            Noice_DataDic[Noice_DataDicKey] = _noiceValue.ToString("#0.0");
            //Debug.Log("Unity�Է���������");

            _noice_DataJson = JsonConvert.SerializeObject(Noice_DataDic);
        }
    }
}
