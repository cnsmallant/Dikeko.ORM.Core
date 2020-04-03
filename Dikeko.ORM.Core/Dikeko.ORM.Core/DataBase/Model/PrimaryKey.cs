using System;
using System.Collections.Generic;
using System.Text;

namespace Dikeko.ORM.Core.DataBase.Model
{
    /// <summary>
    /// 主键
    /// </summary>
    public class PrimaryKey
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
        /// 值
        /// </summary>
        public object Value { get; set; }
    }
}
