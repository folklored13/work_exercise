using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System;

public class SampelXML : MonoBehaviour
{
    public SampleScriptableObject Sample;
    void Start()
    {
        SaveXML();
        //LoadXML();

        Sample = ScriptableObject.CreateInstance<SampleScriptableObject>();
        Sample.Index++;
        Debug.Log(Sample.Index);
    }

    void SaveXML()
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlElement xmlRootElment = xmlDoc.CreateElement("今天上课到课学生");

        xmlRootElment.InnerText = "江西财经大学u3d";
        xmlRootElment.SetAttribute("时间", DateTime.Now.ToShortDateString());

        XmlElement xmlStudentElement = xmlDoc.CreateElement("学生");
        //for (int i = 0; i < 10; i++)
        //{
        //    XmlElement xmlStudentIndexElement = xmlDoc.CreateElement("学号");
        //    xmlStudentIndexElement.InnerText = i.ToString();

        //    xmlStudentElement.AppendChild(xmlStudentIndexElement);
        //}
        xmlRootElment.AppendChild(xmlStudentElement);

        XmlElement xmlStudentMale = xmlDoc.CreateElement("男学生");
        for(int i=0;i<5;i++)
        {
            XmlElement xmlStudentIndexElement = xmlDoc.CreateElement("学号");
            xmlStudentIndexElement.InnerText = i.ToString();
            xmlStudentMale.SetAttribute("性别", "男");
            xmlStudentMale.AppendChild(xmlStudentIndexElement);
        }
        
        xmlStudentElement.AppendChild(xmlStudentMale);
        
        XmlElement xmlStudentFemale = xmlDoc.CreateElement("女学生");
        for (int i = 0; i < 5; i++)
        {
            XmlElement xmlStudentIndexElement = xmlDoc.CreateElement("学号");
            xmlStudentIndexElement.InnerText = i.ToString();
            xmlStudentFemale.SetAttribute("性别", "女");
            xmlStudentFemale.AppendChild(xmlStudentIndexElement);
        }
        xmlStudentElement.AppendChild(xmlStudentFemale);

        //XmlElement xmlTeacherElement = xmlDoc.CreateElement("教师");
        //for (int i = 0; i < 10; i++)
        //{
        //    XmlElement xmlTeacherIndexElement = xmlDoc.CreateElement("工号");
        //    xmlTeacherIndexElement.InnerText = i.ToString();

        //    xmlTeacherElement.AppendChild(xmlTeacherIndexElement);
        //}
        //xmlRootElment.AppendChild(xmlTeacherElement);

        xmlDoc.AppendChild(xmlRootElment);

        string xmlPath = Path.Combine(Application.dataPath, "xmlDocSample.Sample");
        xmlDoc.Save(xmlPath);
    }

    void LoadXML()
    {
        string xmlPath = Path.Combine(Application.dataPath, "xmlDocSample.Sample");
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(xmlPath);

        //node是节点读取类型，但部分类型无法设置
        foreach (XmlNode node in xmlDoc.ChildNodes)
        {
            Debug.Log(node.Name);
            XmlElement element = (XmlElement)node;

            element.SetAttribute("动态添加", "运行时");
            foreach (XmlNode childNode in node.ChildNodes)
            {
                Debug.Log(childNode.Name);
            }
        }
        xmlDoc.Save(xmlPath);
    }

}
