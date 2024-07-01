using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using UnityEngine;

public class SampleCSV : MonoBehaviour
{
    public string CarriageReturn = "\r";
    public string LineFeed = "\n";
    string SeparataSymbol = ",";
    string LineFeedSymbol = "\r\n";

    // Start is called before the first frame update
    void Start()
    {
        SaveCSV();
        LoadCSV();
    }
    void LoadCSV()
    {
        string csvPath = Path.Combine(Application.dataPath, "Resources/PlayerPropertyData");
        string csvString = File.ReadAllText(csvPath);

        string[] csvRowDatas = csvString.Split(LineFeedSymbol);

        //��Ϊ��һ���Ǳ�ͷ�����Բ���Ҫ������������ﲻ��ȡ��ͷ
        for (int i = 1; i < csvRowDatas.Length; i++)
        {
            string[] csvColunmDatas = csvRowDatas[i].Split(SeparataSymbol);

            for (int j = 0; j < csvColunmDatas.Length; j++)
            {
                Debug.Log(csvColunmDatas[j]);
            }
        }
    }

    void SaveCSV()
    {
        //System.Data.DataTable csvSample = new System.Data.DataTable("ʾ��CSV");
        DataTable csvSample = new DataTable();
        csvSample.Columns.Add("�ȼ�");
        csvSample.Columns.Add("����");
        csvSample.Columns.Add("�ƶ��ٶ�");
        /*
        DataRow dataRow = csvSample.NewRow();
        dataRow["���"] = 0;
        dataRow["����"] = "����";
        dataRow["�ɼ�"] = Random.Range(60, 100);
        csvSample.Rows.Add(dataRow);

        dataRow = csvSample.NewRow();
        dataRow["���"] = 1;
        dataRow["����"] = "����";
        dataRow["�ɼ�"] = Random.Range(60, 100);
        csvSample.Rows.Add(dataRow);
        */

        DataRow dataRow = csvSample.NewRow();
        for (int i = 1; i < 10; i++)
        {
            dataRow = csvSample.NewRow();
            dataRow[0] = i;
            dataRow[1] = Random.Range(10, 100);
            dataRow[2] = 5;
            if (dataRow[2] != System.DBNull.Value)
            {
                dataRow[2] = (System.Convert.ToInt32(dataRow[2]) * i + 1) * 1.13;
            }
            else
            {
                // ��һ��Ĭ��ֵ���ߴ���ʽ
                dataRow[2] = 3; // ������������ʽ
            }
            csvSample.Rows.Add(dataRow);
        }

        StringBuilder csvString = new StringBuilder();

        //������ӱ�ͷ
        for (int j = 0; j < csvSample.Columns.Count; j++)
        {
            csvString.Append(csvSample.Columns[j].ColumnName);
            if (j < csvSample.Columns.Count - 1)
            {
                csvString.Append(SeparataSymbol);
            }
        }
        //����ÿһ�У�����ĳһ��ĳһ�е����ݴ����ַ�����
        for (int i = 0; i < csvSample.Rows.Count; i++)
        {
            csvString.Append(LineFeedSymbol);

            for (int j = 0; j < csvSample.Columns.Count; j++)
            {
                csvString.Append(csvSample.Rows[i][j].ToString());
                if (j < csvSample.Columns.Count - 1)
                {
                    csvString.Append(SeparataSymbol);
                }
            }
        }

        string csvPath = Path.Combine(Application.dataPath, "Resources/PlayerPropertyData");
        File.WriteAllText(csvPath, csvString.ToString());
    }

}
