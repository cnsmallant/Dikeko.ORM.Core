﻿using Dikeko.ORM.Core.DataBase.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static Dikeko.ORM.Core.DataBase.Enum.DataBaseEnum;

namespace Dikeko.ORM.Core.DataBase
{
    /// <summary>
    /// 获取数据库连接
    /// </summary>
    public class DBConnectionString
    {
        public static IConfigurationRoot Configuration { get; set; }

        public static string ConnectionString(string connname)
        {

            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            return Configuration.GetSection("ConnectionStrings")[connname];
        }
    }

    /// <summary>
    /// 数据库帮助类
    /// </summary>
    public class DikekoDataBase : IDikekoDataBase
    {
        string connectionString = string.Empty;
        public DikekoDataBase(string connname)
        {
            connectionString = DBConnectionString.ConnectionString(connname);
        }
        #region 数据库操作方法
        /// <summary>
        /// 执行插入
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="t">实体参数</param>
        /// <returns></returns>
        public dynamic Insert<T>(T t)
        {
            string name = string.Join(",", DataBaseAuxiliary.Current.GetColumns(t, OperationMethodEnum.Add));
            string nameserialization = string.Join(",", DataBaseAuxiliary.Current.GetPropertiesNameSerialization(t, OperationMethodEnum.Add));
            List<object> value = DataBaseAuxiliary.Current.GetPropertiesValue(t, OperationMethodEnum.Add);
            var args = value.ToArray();
            string sql = $"INSERT INTO {t.GetType().Name} ({name}) VALUES ({nameserialization})";
            return new DataBaseAuxiliary(connectionString).ExecuteNonQuery(sql, args);
        }

        /// <summary>
        /// 执行插入
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public dynamic Insert(string sql, params object[] args)
        {
            return new DataBaseAuxiliary(connectionString).ExecuteNonQuery(sql, args);
        }

        /// <summary>
        /// 批量添加（最多一次性1000条数据）
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="t">实体参数</param>
        /// <param name="valuesArray">插入值组合，例如('a1','11','你好1'),('a2','12','你好2'),('a3','13','你好3'),('a4','14','你好4'),('a5','15','你好5')</param>
        /// <returns></returns>
        public dynamic BulkInsert<T>(T t, string valuesArray)
        {
            try
            {
                string name = string.Join(",", DataBaseAuxiliary.Current.GetColumns(t, OperationMethodEnum.Add));
                var sql = $"INSERT INTO {t.GetType().Name} ({name}) VALUES {valuesArray}";
                return new DataBaseAuxiliary(connectionString).ExecuteNonQuery(sql);
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// 批量插入（最多一次性1000条数据）
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="field">字段</param>
        /// <param name="valuesArray">插入值组合，例如('a1','11','你好1'),('a2','12','你好2'),('a3','13','你好3'),('a4','14','你好4'),('a5','15','你好5')</param>
        /// <returns></returns>
        public dynamic BulkInsert(string table, string field, string valuesArray)
        {
            try
            {
                var sql = $"INSERT INTO {table} ({field}) VALUES {valuesArray}";
                return new DataBaseAuxiliary(connectionString).ExecuteNonQuery(sql);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 执行修改
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="t">实体参数</param>
        /// <returns></returns>
        public dynamic Update<T>(T t)
        {
            string nameserialization = string.Join(",", DataBaseAuxiliary.Current.GetPropertiesNameSerialization(t, OperationMethodEnum.Update));
            List<object> value = DataBaseAuxiliary.Current.GetPropertiesValue(t, OperationMethodEnum.Update);
            var key = DataBaseAuxiliary.Current.GetPrimaryKey(t);
            var keyValue = $"{key.Name}=@{key.Name}";
            var args = value.ToArray();
            string sql = $"UPDATE {t.GetType().Name} SET {nameserialization} WHERE {keyValue}";
            return new DataBaseAuxiliary(connectionString).ExecuteNonQuery(sql, args);
        }
        /// <summary>
        /// 执行修改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public dynamic Update(string sql, params object[] args)
        {
            return new DataBaseAuxiliary(connectionString).ExecuteNonQuery(sql, args);
        }

        /// <summary>
        /// 执行删除
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="t">实体参数</param>
        /// <returns></returns>
        public dynamic Delete<T>(T t)
        {
            List<object> value = DataBaseAuxiliary.Current.GetPropertiesValue(t, OperationMethodEnum.Update);
            var key = DataBaseAuxiliary.Current.GetPrimaryKey(t);
            var keyValue = $"{key.Name}=@{key.Name}";
            var args = value.ToArray();
            string sql = $"DELETE FROM {t.GetType().Name}  WHERE {keyValue}";
            return new DataBaseAuxiliary(connectionString).ExecuteNonQuery(sql, args);
        }
        /// <summary>
        /// 执行删除
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public dynamic Delete(string sql, params object[] args)
        {
            return new DataBaseAuxiliary(connectionString).ExecuteNonQuery(sql, args);
        }

        /// <summary>
        /// 默认第一条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T FirstOrDefault<T>()
        {
            T t = Activator.CreateInstance<T>();
            string sql = DataBaseAuxiliary.Current.SelectSQL(t, SqlQueryTypeEnum.FirstOrDefault);
            return new DataBaseAuxiliary(connectionString).DataReaderSingle(t, sql);
        }

        /// <summary>
        /// 默认第一条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public T FirstOrDefault<T>(string sql, params object[] args)
        {
            T t = Activator.CreateInstance<T>();
            return new DataBaseAuxiliary(connectionString).DataReaderSingle(t, sql, args);
        }

        /// <summary>
        /// 默认一条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T SingleOrDefault<T>(T t)
        {
            string sql = DataBaseAuxiliary.Current.SelectSQL(t, SqlQueryTypeEnum.SingleOrDefault);
            return new DataBaseAuxiliary(connectionString).DataReaderSingle(t, sql);
        }

        /// <summary>
        /// 查询一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public T SingleOrDefault<T>(string sql, params object[] args)
        {
            T t = Activator.CreateInstance<T>();
            return new DataBaseAuxiliary(connectionString).DataReaderSingle(t, sql, args);
        }
        /// <summary>
        /// 查询总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public object CountOrDefault<T>()
        {
            T t = Activator.CreateInstance<T>();
            string sql = DataBaseAuxiliary.Current.SelectSQL(t, SqlQueryTypeEnum.CountOrDefault);
            return new DataBaseAuxiliary(connectionString).ExecuteScalar(sql);
        }

        /// <summary>
        /// 查询总数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public object CountOrDefault(string sql, params object[] args)
        {
            return new DataBaseAuxiliary(connectionString).ExecuteScalar(sql, args);
        }

        /// <summary>
        /// 查询总数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public object CountOrDefault<T>(string sql, params object[] args)
        {
            T t = Activator.CreateInstance<T>();
            return new DataBaseAuxiliary(connectionString).DataReaderMultiple(t, sql, args).Count;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IList<T> FetchOrDefault<T>()
        {
            T t = Activator.CreateInstance<T>();
            string sql = DataBaseAuxiliary.Current.SelectSQL(t, SqlQueryTypeEnum.FetchOrDefault);
            IList<T> list = new DataBaseAuxiliary(connectionString).DataReaderMultiple(t, sql);
            return list;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public IList<T> FetchOrDefault<T>(string sql, params object[] args)
        {
            T t = Activator.CreateInstance<T>();
            IList<T> list = new DataBaseAuxiliary(connectionString).DataReaderMultiple(t, sql, args);
            return list;
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="CurrentPage">当前页</param>
        /// <param name="PageSize">每页条数</param>
        /// <param name="sql">sql语句</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public Page<T> PageOrDefault<T>(int CurrentPage, int PageSize, string sql, params object[] args)
        {
            T t = Activator.CreateInstance<T>();
            int count = Convert.ToInt32(CountOrDefault<T>(sql, args));
            var list = new DataBaseAuxiliary(connectionString).DataReaderMultiple(t, sql, args).Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
            var result = new Page<T>
            {
                PageIndex = CurrentPage,
                PageSize = PageSize,
                TotalItems = count,
            };
            result.TotalPages = result.TotalItems / PageSize;

            if ((result.TotalItems % PageSize) != 0)
            {
                result.TotalPages++;
            }
            result.Items = list;
            return result;
        }

        /// <summary>
        /// 事务
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public dynamic Transaction(string sql, params object[] args)
        {
            return new DataBaseAuxiliary(connectionString).ExecuteTransaction(sql, args);
        }
        #endregion

    }

}
