using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using FastMember;
using System.Linq;

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
        /// 执行查询，并返回查询结果集中的第一行第一列的内容
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
        /// 执行一条sql，返回一个SqlDataReader对象
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

                List<string> members = new List<string>();

                // get properties
                T entity = System.Activator.CreateInstance<T>();
                PropertyInfo[] propertyArr = entity.GetType().GetProperties();
                foreach (var property in propertyArr) {
                    if (property.MemberType == MemberTypes.Property) {
                        members.Add(property.Name);
                    }
                }
                // mapping source columns to target columns
                if (members.Count > 0) {
                    foreach(var member in members) {
                        bulkCopy.ColumnMappings.Add(member, member);
                    }
                }


                // batch insert
                try {
                    using (var reader = ObjectReader.Create<T>(entities, members.ToArray())) {
                        bulkCopy.WriteToServer(reader);
                    }
                }
                catch (Exception ex) {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// 将字典中的值转化为泛型T
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="dict">数据库查询中的行记录所对应的值集，key为字段，value为字段值</param>
        /// <returns></returns>
        private static T EntityConvert<T>(Dictionary<string,object> dict)
        {
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
    }
}
