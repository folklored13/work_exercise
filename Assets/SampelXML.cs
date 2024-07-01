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
        XmlElement xmlRootElment = xmlDoc.CreateElement("�����Ͽε���ѧ��");

        xmlRootElment.InnerText = "�����ƾ���ѧu3d";
        xmlRootElment.SetAttribute("ʱ��", DateTime.Now.ToShortDateString());

        XmlElement xmlStudentElement = xmlDoc.CreateElement("ѧ��");
        //for (int i = 0; i < 10; i++)
        //{
        //    XmlElement xmlStudentIndexElement = xmlDoc.CreateElement("ѧ��");
        //    xmlStudentIndexElement.InnerText = i.ToString();

        //    xmlStudentElement.AppendChild(xmlStudentIndexElement);
        //}
        xmlRootElment.AppendChild(xmlStudentElement);

        XmlElement xmlStudentMale = xmlDoc.CreateElement("��ѧ��");
        for(int i=0;i<5;i++)
        {
            XmlElement xmlStudentIndexElement = xmlDoc.CreateElement("ѧ��");
            xmlStudentIndexElement.InnerText = i.ToString();
            xmlStudentMale.SetAttribute("�Ա�", "��");
            xmlStudentMale.AppendChild(xmlStudentIndexElement);
        }
        
        xmlStudentElement.AppendChild(xmlStudentMale);
        
        XmlElement xmlStudentFemale = xmlDoc.CreateElement("Ůѧ��");
        for (int i = 0; i < 5; i++)
        {
            XmlElement xmlStudentIndexElement = xmlDoc.CreateElement("ѧ��");
            xmlStudentIndexElement.InnerText = i.ToString();
            xmlStudentFemale.SetAttribute("�Ա�", "Ů");
            xmlStudentFemale.AppendChild(xmlStudentIndexElement);
        }
        xmlStudentElement.AppendChild(xmlStudentFemale);

        //XmlElement xmlTeacherElement = xmlDoc.CreateElement("��ʦ");
        //for (int i = 0; i < 10; i++)
        //{
        //    XmlElement xmlTeacherIndexElement = xmlDoc.CreateElement("����");
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

        //node�ǽڵ��ȡ���ͣ������������޷�����
        foreach (XmlNode node in xmlDoc.ChildNodes)
        {
            Debug.Log(node.Name);
            XmlElement element = (XmlElement)node;

            element.SetAttribute("��̬���", "����ʱ");
            foreach (XmlNode childNode in node.ChildNodes)
            {
                Debug.Log(childNode.Name);
            }
        }
        xmlDoc.Save(xmlPath);
    }

}
