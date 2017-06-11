using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using System.Net.Http;
using System.Linq;
using System.Data;
using Orion.CRM.Toolkit;

namespace Orion.CRM.Core
{
    public class SqlMapHelper
    {
        /// <summary>
        /// 执行指定的用于查询的SqlMap，返回结果集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlmapName"></param>
        /// <param name="sqlName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetSqlMapResult<T>(string sqlmapName, string sqlName, params SqlParameter[] parameters)
        {
            SqlMapDetail mapDetail = SqlMapFactory.GetSqlMapDetail(sqlmapName, sqlName);
            return GetSqlMapResult<T>(mapDetail, parameters);
        }

        /// <summary>
        /// 执行指定的用于查询的SqlMap，返回结果集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="map"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetSqlMapResult<T>(SqlMapDetail mapDetail, params SqlParameter[] parameters)
        {
            if (mapDetail != null) {
                string connString = SqlMapFactory.GetDBConnectionString(mapDetail.DBConnectionName);
                IEnumerable<T> entities = SqlHelper.ExecuteDataQuery<T>(connString, CommandType.Text, mapDetail.OriginalSqlString, parameters);
                return entities;
            }

            return null;
        }

        /// <summary>
        /// 执行指定的用于查询的SqlMap，返回结果集中的第一条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlmapName"></param>
        /// <param name="sqlName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T GetSqlMapSingleResult<T>(string sqlmapName, string sqlName, params SqlParameter[] parameters)
        {
            SqlMapDetail mapDetail = SqlMapFactory.GetSqlMapDetail(sqlmapName, sqlName);
            return GetSqlMapSingleResult<T>(mapDetail, parameters);
        }

        /// <summary>
        /// 执行指定的用于查询的SqlMap，返回结果集中的第一条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="map"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T GetSqlMapSingleResult<T>(SqlMapDetail mapDetail, params SqlParameter[] parameters)
        {
            if (mapDetail != null) {
                // 调用GetSqlMapResult方法，返回集合中的第一个元素
                IEnumerable<T> entities = GetSqlMapResult<T>(mapDetail, parameters);
                if (entities == null) {
                    return default(T);
                }
                else { 
                    T entity = entities.FirstOrDefault();
                    return entity;
                }
            }
            return default(T);
        }

        /// <summary>
        /// 执行增删改类型SqlMap，并返回受影响的行数
        /// </summary>
        /// <param name="sqlmapName"></param>
        /// <param name="sqlName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int ExecuteSqlMapNonQuery(string sqlmapName, string sqlName, params SqlParameter[] parameters)
        {
            SqlMapDetail mapDetail = SqlMapFactory.GetSqlMapDetail(sqlmapName, sqlName);
            return ExecuteSqlMapNonQuery(mapDetail, parameters);
        }

        /// <summary>
        /// 执行增删改类型SqlMap，并返回受影响的行数
        /// </summary>
        /// <param name="map"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int ExecuteSqlMapNonQuery(SqlMapDetail mapDetail, params SqlParameter[] parameters)
        {
            if (mapDetail != null) {
                string connString = SqlMapFactory.GetDBConnectionString(mapDetail.DBConnectionName);
                int count = SqlHelper.ExecuteNonQuery(connString, CommandType.Text, mapDetail.OriginalSqlString, parameters);
                return count;
            }

            return 0;
        }

        /// <summary>
        /// 执行指定SqlMap，并返回结果集中的第一行第一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlmapName"></param>
        /// <param name="sqlName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T ExecuteSqlMapScalar<T>(string sqlmapName, string sqlName, params SqlParameter[] parameters)
        {
            SqlMapDetail mapDetail = SqlMapFactory.GetSqlMapDetail(sqlmapName, sqlName);
            return ExecuteSqlMapScalar<T>(mapDetail, parameters);
        }

        /// <summary>
        /// 执行指定SqlMap，并返回结果集中的第一行第一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="map"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T ExecuteSqlMapScalar<T>(SqlMapDetail mapDetail, params SqlParameter[] parameters)
        {
            if(mapDetail != null) {
                string connString = SqlMapFactory.GetDBConnectionString(mapDetail.DBConnectionName);
                object obj = SqlHelper.ExecuteScalar(connString, CommandType.Text, mapDetail.OriginalSqlString, parameters);

                return (T)obj;                
            }
            return default(T);
        }

        /// <summary>
        /// 批量插入实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sqlmapName"></param>
        /// <param name="sqlName"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static bool ExecuteBatchInsert<T>(string sqlmapName, string sqlName, IEnumerable<T> entities)
        {
            SqlMapDetail mapDetail = SqlMapFactory.GetSqlMapDetail(sqlmapName, sqlName);
            return ExecuteBatchInsert<T>(mapDetail, entities);
        }

        /// <summary>
        /// 批量插入实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mapDetail"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static bool ExecuteBatchInsert<T>(SqlMapDetail mapDetail,IEnumerable<T> entities)
        {
            if (mapDetail != null) {
                string connString = SqlMapFactory.GetDBConnectionString(mapDetail.DBConnectionName);
                bool result = SqlHelper.ExecuteBatchInsert<T>(entities, mapDetail.TableName, connString);

                return result;
            }

            return false;
        }
    }
}
