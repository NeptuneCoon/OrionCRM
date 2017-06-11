using Orion.CRM.Toolkit;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace Orion.CRM.Core
{
    public class SqlMapFactory
    {
        /// <summary>
        /// SqlMap配置的内存映射
        /// </summary>
        private static Dictionary<string, SqlMap> SqlMapDict = new Dictionary<string, SqlMap>();
        /// <summary>
        /// 数据库连接配置的内存映射
        /// </summary>
        private static DBProviderConfig DBProviderConfig = new DBProviderConfig();
        private static readonly object StartLock = new object();

        /// <summary>
        /// 获取SqlMap
        /// </summary>
        /// <param name="sqlmapName">SqlMap名称(SqlMap文件名)</param>
        /// <returns></returns>
        public static SqlMap GetSqlMap(string sqlmapName)
        {
            SqlMap sqlmap = null;
            string sqlMapContent = GetSqlMapFileContent(sqlmapName);
            if(!string.IsNullOrEmpty(sqlMapContent)) {
                if(SqlMapDict.ContainsKey(sqlmapName)) {
                    sqlmap = SqlMapDict[sqlmapName];
                    return sqlmap;
                }
                else {
                    sqlmap = XmlSerializeHelper.XmlDeserialize<SqlMap>(sqlMapContent, Encoding.UTF8);
                    if (sqlmap != null) { 
                        SqlMapDict.Add(sqlmapName, sqlmap);
                        return sqlmap;
                    }
                    else {
                        throw new Exception("未能将sqlmap的xml内容序列化为指定的SqlMap对象。");
                    }
                }
            }
            else {
                throw new Exception("sqlmap文件的内容为空。");
            }
        }

        /// <summary>
        /// 获取SqlMapDetail对象
        /// </summary>
        /// <param name="sqlmapName">SqlMap名称</param>
        /// <param name="sqlName">SQL名称</param>
        /// <returns></returns>
        public static SqlMapDetail GetSqlMapDetail(string sqlmapName, string sqlName)
        {
            SqlMapDetail mapDetail = null;
            SqlMap sqlmap = GetSqlMap(sqlmapName);
            if(sqlmap != null) {
                if (sqlmap.SqlMapConfigurations != null) {
                    foreach(var sqlmapDetail in sqlmap.SqlMapConfigurations) {
                        if(sqlmapDetail.SqlName == sqlName) {
                            mapDetail = sqlmapDetail;
                            break;
                        }
                    }
                }
            }

            return mapDetail;
        }

        /// <summary>
        /// 从数据库连接配置中获取连接字符串
        /// </summary>
        /// <param name="dbConnectionName">数据库名称</param>
        /// <param name="category">数据库类型(SqlServer,MySql,MongoDB等)</param>
        /// <returns></returns>
        public static string GetDBConnectionString(string dbConnectionName, string category = "SqlServer")
        {
            string connectionString = string.Empty;

            if(DBProviderConfig.DBProviders == null || DBProviderConfig.DBProviders.Count <= 0) {
                string path = Directory.GetCurrentDirectory() +  @"\wwwroot\MappingFiles\Config\" + "DBProvider.config";
                string providerConfigFiles = File.ReadAllText(path, Encoding.UTF8);

                if (File.Exists(path)) {
                    string fileText = File.ReadAllText(path, Encoding.UTF8);
                    DBProviderConfig providerConfig = XmlSerializeHelper.XmlDeserialize<DBProviderConfig>(fileText, Encoding.UTF8);
                    connectionString = GetDBConnectionStringFromObj(providerConfig, dbConnectionName, category);

                    DBProviderConfig = providerConfig;//存入数据库连接配置的内存映射中
                }
            }
            else {
                connectionString = GetDBConnectionStringFromObj(DBProviderConfig, dbConnectionName, category);
            }

            return connectionString;
        }

        /// <summary>
        /// 从指定的DBProviderConfig对象中根据数据库名称获取连接字符串
        /// </summary>
        /// <param name="providerConfig">指定的DBProviderConfig对象</param>
        /// <param name="dbConnectionName">数据库名称</param>
        /// <param name="category">数据库类型(SqlServer,MySql,MongoDB等)</param>
        /// <returns></returns>
        private static string GetDBConnectionStringFromObj(DBProviderConfig providerConfig, string dbConnectionName, string category)
        {
            string connectionString = string.Empty;

            var providerForCategory = providerConfig.DBProviders.FirstOrDefault(x => x.Category.ToLower() == category.ToLower());
            if (providerForCategory != null) {
                if (providerForCategory.DBConnections != null) {
                    var dbConnection = providerForCategory.DBConnections.FirstOrDefault(x => x.Name == dbConnectionName);
                    if (dbConnection != null) {
                        connectionString = dbConnection.Connection;
                    }
                    else {
                        throw new Exception("未找到名为" + dbConnectionName + "的数据库连接配置。");
                    }
                }
                else {
                    throw new Exception(category + "类型的DBProvider下还没有数据库连接。");
                }
            }
            else {
                throw new Exception("未找到" + category + "类型的DBProvider，请检查配置文件。");
            }

            return connectionString;
        }


        /// <summary>
        /// 读取SqlMap文件内容
        /// </summary>
        /// <param name="sqlmapName"></param>
        /// <returns></returns>
        private static string GetSqlMapFileContent(string sqlmapName)
        {
            string fileText = "";

            lock(StartLock) { 
                string path = Directory.GetCurrentDirectory() + @"\wwwroot\MappingFiles\SqlMap\" + sqlmapName + ".sqlmap";

                if (File.Exists(path)) {
                    fileText = File.ReadAllText(path, Encoding.UTF8);
                }
                else {
                    throw new Exception("未找到名为" + sqlmapName + "的SqlMap文件，请仔细检查代码中指定的SqlMap文件是否存在。");
                }
            }
            return fileText;
        }

    }
}
