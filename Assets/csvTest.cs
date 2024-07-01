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
        // 创建DataTable对象
        DataTable dataTable = new DataTable();

        // 添加列
        dataTable.Columns.Add("等级", typeof(int));
        dataTable.Columns.Add("力量", typeof(int));
        dataTable.Columns.Add("移动速度", typeof(float));

        // 添加数据行
        for (int i = 0; i < 10; i++)
        {
            DataRow row = dataTable.NewRow();
            row["等级"] = i;
            row["力量"] = Random.Range(1, 101); // 随机生成1到100的力量值
            row["移动速度"] = GetMoveSpeed(i); // 根据等级计算移动速度
            dataTable.Rows.Add(row);
        }

        // 将DataTable保存为CSV文件
        string filePath = Path.Combine(Application.dataPath, "Resources/PlayerPropertyData.csv");
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(string.Join(",", dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName))); // 写入表头
            foreach (DataRow row in dataTable.Rows)
            {
                writer.WriteLine(string.Join(",", row.ItemArray)); // 写入每一行数据
            }
        }
    }

    // 根据等级计算移动速度的方法
    private float GetMoveSpeed(int level)
    {
        float baseSpeed = 5.0f; // 基础移动速度
        return baseSpeed * level + 1 * 1.13f; // 计算移动速度
    }
}
