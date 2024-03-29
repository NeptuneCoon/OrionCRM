﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Linq;
using System.ComponentModel;

namespace Orion.CRM.Toolkit
{
    public class SqlHelper
    {
        /// <summary>
        /// 执行一个不带事务的ExecuteNonQuery，返回受影响的行数
        /// </summary>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="cmdType">SqlCommand命令类型(存储过程、T-SQL语句等)</param>
        /// <param name="cmdText">存储过程的名称或T-SQL语句</param>
        /// <param name="parameters">以数组形式提供的SqlCommand命令中用到的参数列表</param>
        /// <returns>执行SqlCommand命令后影响的行数</returns>
        public static int ExecuteNonQuery(string connString, CommandType cmdType, string cmdText, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connString)) {
                if (conn.State != ConnectionState.Open) {
                    conn.Open();
                }

                SqlCommand cmd = new SqlCommand(cmdText, conn);
                cmd.CommandTimeout = 10;
                cmd.CommandType = cmdType;

                if (parameters != null) {
                    cmd.Parameters.AddRange(parameters);
                }

                int val = cmd.ExecuteNonQuery();
                return val;
            }
        }

        /// <summary>
        /// 执行一个带事务的ExecuteNonQuery，返回受影响的行数
        /// </summary>
        /// <param name="trans">Sql事务</param>
        /// <param name="cmdType">SqlCommand命令类型(存储过程、T-SQL语句等)</param>
        /// <param name="cmdText">存储过程的名称或T-SQL语句</param>
        /// <param name="parameters">以数组形式提供的SqlCommand命令中用到的参数列表</param>
        /// <returns>执行SqlCommand命令后影响的行数</returns>
        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] parameters)
        {
            SqlCommand cmd = new SqlCommand(cmdText, trans.Connection);
            cmd.CommandTimeout = 10;
            cmd.CommandType = cmdType;

            if (parameters != null) {
                cmd.Parameters.AddRange(parameters);
            }

            if (trans.Connection.State != ConnectionState.Open) {
                trans.Connection.Open();
            }

            int val = cmd.ExecuteNonQuery();
            return val;
        }

        /// <summary>
        /// 执行SQL语句，并返回结果集中的第一行第一列的内容
        /// </summary>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="cmdType">SqlCommand命令类型(存储过程、T-SQL语句等)</param>
        /// <param name="cmdText">存储过程的名称或T-SQL语句</param>
        /// <param name="parameters">以数组形式提供的SqlCommand命令中用到的参数列表</param>
        /// <returns>结果集中的第一行第一列的内容</returns>
        public static object ExecuteScalar(string connString, CommandType cmdType, string cmdText, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connString)) {
                if (conn.State != ConnectionState.Open) {
                    conn.Open();
                }

                SqlCommand cmd = new SqlCommand(cmdText, conn);
                cmd.CommandTimeout = 10;
                cmd.CommandType = cmdType;

                if (parameters != null) {
                    cmd.Parameters.AddRange(parameters);
                }

                object obj = cmd.ExecuteScalar();
                return obj;
            }
        }

        /// <summary>
        /// 执行带事务的SQL语句，并返回结果集中的第一行第一列的内容
        /// </summary>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="cmdType">SqlCommand命令类型(存储过程、T-SQL语句等)</param>
        /// <param name="cmdText">存储过程的名称或T-SQL语句</param>
        /// <param name="parameters">以数组形式提供的SqlCommand命令中用到的参数列表</param>
        /// <returns>结果集中的第一行第一列的内容</returns>
        public static Object ExecuteScalar(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] parameters)
        {
            SqlCommand cmd = new SqlCommand(cmdText, trans.Connection);
            cmd.CommandTimeout = 10;
            cmd.CommandType = cmdType;

            if (parameters != null) {
                cmd.Parameters.AddRange(parameters);
            }

            if (trans.Connection.State != ConnectionState.Open) {
                trans.Connection.Open();
            }

            object obj = cmd.ExecuteScalar();
            return obj;
        }

        /// <summary>
        /// 执行一条查询sql，返回结果集
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IEnumerable<T> ExecuteDataQuery<T>(string connString, CommandType cmdType, string cmdText, params SqlParameter[] parameters)
        {
            IList<T> entities = null;

            using (SqlConnection conn = new SqlConnection(connString)) {
                if (conn.State != ConnectionState.Open) {
                    conn.Open();
                }

                SqlCommand cmd = new SqlCommand(cmdText, conn);
                cmd.CommandTimeout = 10;
                cmd.CommandType = cmdType;

                if (parameters != null) {
                    cmd.Parameters.AddRange(parameters);
                }

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows) {
                    entities = new List<T>();

                    // 获取SqlDataReader返回的所有字段名称
                    List<string> fieldNames = new List<string>();
                    for (var i = 0; i < reader.FieldCount; i++) {
                        fieldNames.Add(reader.GetName(i));
                    }

                    while (reader.Read()) {
                        // 读取该行记录的值，并将其存入字典dict中
                        Dictionary<string, object> dict = new Dictionary<string, object>();
                        foreach (var name in fieldNames) {
                            dict.Add(name, reader[name]);
                        }
                        // 将该dict转化为泛型T
                        T entity = EntityConvert<T>(dict);

                        entities.Add(entity);
                    }
                }            

                reader.Dispose();
            }
            return entities;
        }

        /// <summary>
        /// 批量插入实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体集合</param>
        /// <param name="tableName">表名</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <returns></returns>
        public static bool ExecuteBatchInsert<T>(IEnumerable<T> entities, string tableName, string connString)
        {
            bool result = true;
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connString, SqlBulkCopyOptions.Default)) {
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.BulkCopyTimeout = 30;

                DataTable dataTable = ListToDataTable(entities);
                // 执行批量插入
                try {
                    bulkCopy.WriteToServer(dataTable);
                    
                }
                catch (Exception ex) {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// 将字典中的值转化为泛型T
        /// 该方法是因为.net core 1.1没有实现DataTable对象，所以只能使用SqlDataReader将数据读到字典中，再将字典转化为实体
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="dict">数据库查询中的行记录所对应的值集，key为字段，value为字段值</param>
        /// <returns></returns>
        private static T EntityConvert<T>(Dictionary<string, object> dict)
        {
            Type type = typeof(T);
            if(type == typeof(int) || type == typeof(long) || type == typeof(short) || type == typeof(string) || type == typeof(float) || type == typeof(double) 
                || type == typeof(char) || type == typeof(bool)) {
                return (T)dict.First().Value;
            }

            T entity = System.Activator.CreateInstance<T>();

            foreach (string provertyName in dict.Keys) {
                // 依次遍历每一个字段
                PropertyInfo propertyInfo = entity.GetType().GetProperty(provertyName);

                // 如果指定的实体entity中包含该字段
                if (propertyInfo != null) {

                    // 并且查询返回的字段dict.Keys中也同样包含该实体字段，则进行填充
                    if (dict.ContainsKey(propertyInfo.Name)) {
                        if (dict[propertyInfo.Name] != DBNull.Value) {
                            propertyInfo.SetValue(entity, dict[propertyInfo.Name]);
                        }
                        else {
                            propertyInfo.SetValue(entity, null);
                        }
                    }
                }
            }

            return entity;
        }

        /// <summary>
        /// 将泛型集合转化为DataTable对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        private static DataTable ListToDataTable<T>(IEnumerable<T> list)
        {
            DataTable table = new DataTable();

            foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(T))) {
                table.Columns.Add(dp.Name);
            }

            foreach (T item in list) {
                var Row = table.NewRow();
                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(T))) {
                    Row[dp.Name] = dp.GetValue(item);
                }
                table.Rows.Add(Row);
            }

            return table;
        }
    }
}
