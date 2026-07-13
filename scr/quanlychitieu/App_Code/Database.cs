using System;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;

public class Database
{
    private static string GetConnectionString()
    {
        return ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString;
    }

    public static MySqlConnection GetConnection()
    {
        return new MySqlConnection(GetConnectionString());
    }

    // Thêm, sửa hoặc xóa dữ liệu
    public static int ExecuteNonQuery(string query, params MySqlParameter[] parameters)
    {
        using (MySqlConnection conn = GetConnection())
        {
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
    }

    // Trả về một giá trị
    public static object ExecuteScalar(string query, params MySqlParameter[] parameters)
    {
        using (MySqlConnection conn = GetConnection())
        {
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                conn.Open();
                return cmd.ExecuteScalar();
            }
        }
    }

    // Trả về bảng dữ liệu
    public static DataTable GetData(string query, params MySqlParameter[] parameters)
    {
        using (MySqlConnection conn = GetConnection())
        {
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }
    }

    // Lấy dữ liệu để xuất XML
    public static DataSet GetDataSet(string query, params MySqlParameter[] parameters)
    {
        using (MySqlConnection conn = GetConnection())
        {
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet("QuanLyChiTieu");
                    da.Fill(ds, "GiaoDich");
                    return ds;
                }
            }
        }
    }
}
