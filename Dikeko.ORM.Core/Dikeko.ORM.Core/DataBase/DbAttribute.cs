using System;
using System.Collections.Generic;
using System.Text;

namespace Dikeko.ORM.Core
{
    /// <summary>
    /// 表名
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class TableNameAttribute : Attribute
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        /// <param name="tableName">表名</param>
        public TableNameAttribute(string tableName)
        {
            Name = tableName;
        }
    }

    /// <summary>
    /// 主键
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class PrimaryKeyAttribute : Attribute
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否是自增列
        /// </summary>
        public bool AutoIncrement { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        /// <param name="primaryKey">主键名称</param>
        public PrimaryKeyAttribute(string primaryKey, bool autoIncrement)
        {
            Name = primaryKey;
            AutoIncrement = autoIncrement;
        }
    }

    /// <summary>
    /// 列
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class ColumnAttribute : Attribute
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 列
        /// </summary>
        /// <param name="columnName">列名</param>
        public ColumnAttribute(string columnName)
        {
            Name = columnName;
        }
    }
}
