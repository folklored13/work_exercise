using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Data;
using System.IO;

public class csvTest : MonoBehaviour
{
    private void Start()
    {
        // ����DataTable����
        DataTable dataTable = new DataTable();

        // �����
        dataTable.Columns.Add("�ȼ�", typeof(int));
        dataTable.Columns.Add("����", typeof(int));
        dataTable.Columns.Add("�ƶ��ٶ�", typeof(float));

        // ���������
        for (int i = 0; i < 10; i++)
        {
            DataRow row = dataTable.NewRow();
            row["�ȼ�"] = i;
            row["����"] = Random.Range(1, 101); // �������1��100������ֵ
            row["�ƶ��ٶ�"] = GetMoveSpeed(i); // ���ݵȼ������ƶ��ٶ�
            dataTable.Rows.Add(row);
        }

        // ��DataTable����ΪCSV�ļ�
        string filePath = Path.Combine(Application.dataPath, "Resources/PlayerPropertyData.csv");
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(string.Join(",", dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName))); // д���ͷ
            foreach (DataRow row in dataTable.Rows)
            {
                writer.WriteLine(string.Join(",", row.ItemArray)); // д��ÿһ������
            }
        }
    }

    // ���ݵȼ������ƶ��ٶȵķ���
    private float GetMoveSpeed(int level)
    {
        float baseSpeed = 5.0f; // �����ƶ��ٶ�
        return baseSpeed * level + 1 * 1.13f; // �����ƶ��ٶ�
    }
}
