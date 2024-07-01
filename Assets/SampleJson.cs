using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using Newtonsoft.Json;
using System.IO;



public class Student
{
    public string Name;
    public int Index;
}

public class SampleJson : MonoBehaviour
{
    
    void Start()
    {
        //Dictionary<string, string> newDic = new Dictionary<string, string>();
        //newDic.Add("KEY","Value");

        Student sampleStudent = new Student();
        sampleStudent.Name = "zhangsan";
        sampleStudent.Index = 1;

        SaveJson(sampleStudent);
        LoadJson();
    }

    void SaveJson(object targetObject)
    {
        string jsonString = JsonMapper.ToJson(targetObject);
        string jsonPath = Path.Combine(Application.dataPath, "LitJson.Sample");
        File.WriteAllText(jsonPath, jsonString);

        jsonString = JsonConvert.SerializeObject(targetObject);
        jsonPath = Path.Combine(Application.dataPath, "NewtonJson.Sample");
        File.WriteAllText(jsonPath, jsonString);

        jsonString = JsonUtility.ToJson(targetObject);
        jsonPath = Path.Combine(Application.dataPath, "JsonUtility.Sample");
        File.WriteAllText(jsonPath, jsonString);
    }

    void LoadJson()
    {
        string jsonPath = Path.Combine(Application.dataPath, "LitJson.Sample");
        string jsonString = File.ReadAllText(jsonPath);
        Student sampleStudent = JsonMapper.ToObject<Student>(jsonString);
        Debug.Log(sampleStudent.Name);
        Debug.Log(jsonPath);

        jsonPath = Path.Combine(Application.dataPath, "NewtonJson.Sample");
        jsonString = File.ReadAllText(jsonPath);
        sampleStudent = JsonConvert.DeserializeObject<Student>(jsonString);
        Debug.Log(sampleStudent.Name);
        Debug.Log(jsonPath);

        jsonPath = Path.Combine(Application.dataPath, "JsonUtility.Sample");
        jsonString = File.ReadAllText(jsonPath);
        sampleStudent = JsonUtility.FromJson<Student>(jsonString);
        Debug.Log(sampleStudent.Name);
        Debug.Log(jsonPath);

    }
    
}
