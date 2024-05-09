using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace WebApi.Systems.Extensions
{
    static class CustomType
    {
        /// <summary>
        /// 获取枚举Description属性
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum val)
        {
            var field = val.GetType().GetField(val.ToString());
            if (field is null)
            {
                return "NoDescription";
            }
            var customAttribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return customAttribute is null ? val.ToString() : ((DescriptionAttribute)customAttribute).Description;
        }
        /// <summary>
        /// NPOI类型转数据集
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        public static DataSet ToDataSet(this IWorkbook workbook)
        {
            DataSet ds = new DataSet();
            for (int s = 0; workbook.NumberOfSheets > s && workbook.GetSheetAt(s).GetRow(0) != null; s++)
            {
                DataTable dt = new DataTable();
                ISheet sheet = workbook.GetSheetAt(s);
                dt.TableName = sheet.SheetName;
                int cell = sheet.GetRow(0).Cells.Count;
                for (int i = 0; sheet.GetRow(i) != null; i++)
                {
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < cell; j++)
                    {
                        ICell value = sheet.GetRow(i).GetCell(j);
                        if (i == 0)
                        {
                            dt.Columns.Add(value.ToString());
                        }
                        else if (value == null)
                        {
                            dr[dt.Columns[j].ToString()] = "";
                        }
                        else
                        {
                            dr[dt.Columns[j].ToString()] = value.ToString();
                        }
                    }
                    if (i > 0)
                    {
                        dt.Rows.Add(dr);
                    }
                }
                ds.Tables.Add(dt);
            }
            return ds;
        }
        /// <summary>
        /// 数据集转可枚举类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static IEnumerable<T> ToEnumerable<T>(this DataSet ds) where T : class, new()
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                T model = dr.ToModel<T>();
                yield return model;
            }
        }
        /// <summary>
        /// 数据表转可枚举类
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<T> ToEnumerable<T>(this DataTable dt) where T : class, new()
        {
            foreach (DataRow dr in dt.Rows)
            {
                T model = dr.ToModel<T>();
                yield return model;
            }
        }
        /// <summary>
        /// 只转第一张表的第一行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static T ToModel<T>(this DataSet ds) where T : class, new()
        {
            if (ds.Tables.Count == 0)
            {
                return new T { };
            }
            return ds.Tables[0].ToModel<T>();
        }
        /// <summary>
        /// 只转第一行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static T ToModel<T>(this DataTable dt) where T : class, new()
        {
            if (dt.Rows.Count == 0)
            {
                return new T { };
            }
            return dt.Rows[0].ToModel<T>();
        }
        /// <summary>
        /// 数据列转泛型类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <returns></returns>
        public static T ToModel<T>(this DataRow row) where T : class, new()
        {
            T model = new T();

            foreach (DataColumn column in row.Table.Columns)
            {
                var property = typeof(T).GetProperty(column.ColumnName);
                if (property != null && row[column] != DBNull.Value)
                {
                    if (property.PropertyType.IsEnum)
                    {
                        var enumValue = Enum.Parse(property.PropertyType, row[column].ToString());
                        property.SetValue(model, enumValue);
                    }
                    else if (property.PropertyType == typeof(DateTime?))
                    {
                        property.SetValue(model, Convert.ChangeType(row[column], typeof(DateTime)));
                    }
                    else
                    {
                        property.SetValue(model, Convert.ChangeType(row[column], property.PropertyType));
                    }
                }
            }

            return model;
        }
        /// <summary>
        /// 类型转数据类型
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>

        public static DbType ToDbType(this Type t)
        {
            if (Enum.TryParse(t.Name, out DbType dbt))
            {
                return dbt;
            }
            else
            {
                return DbType.String;
            }
        }
        /// <summary>
        /// 数据类型转类型
        /// </summary>
        /// <param name="dbt"></param>
        /// <returns></returns>

        public static Type ToType(this DbType dbt)
        {
            Dictionary<DbType, Type> typeMap = new Dictionary<DbType, Type>()
            {
                { DbType.UInt64, typeof(ulong) },
                { DbType.Int64, typeof(long) },
                { DbType.Int32, typeof(int) },
                { DbType.UInt32, typeof(uint) },
                { DbType.Single, typeof(float) },
                { DbType.Date, typeof(DateTime) },
                { DbType.DateTime, typeof(DateTime) },
                { DbType.Time, typeof(DateTime) },
                { DbType.String, typeof(string) },
                { DbType.StringFixedLength, typeof(string) },
                { DbType.AnsiString, typeof(string) },
                { DbType.AnsiStringFixedLength, typeof(string) },
                { DbType.UInt16, typeof(ushort) },
                { DbType.Int16, typeof(short) },
                { DbType.SByte, typeof(byte) },
                { DbType.Object, typeof(object) },
                { DbType.VarNumeric, typeof(decimal) },
                { DbType.Decimal, typeof(decimal) },
                { DbType.Currency, typeof(double) },
                { DbType.Binary, typeof(byte[]) },
                { DbType.Double, typeof(double) },
                { DbType.Guid, typeof(Guid) },
                { DbType.Boolean, typeof(bool) }
            };
            return typeMap.ContainsKey(dbt) ? typeMap[dbt] : typeof(DBNull);
        }

        /// <summary>
        /// 首字母小写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToFirstLower(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            string str = input.First().ToString().ToLower() + input.Substring(1);
            return str;
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToFirstUpper(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            string str = input.First().ToString().ToUpper() + input.Substring(1);
            return str;
        }

        /// <summary>
        /// default:false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool ToBoolean<T>(this T text)
        {
            bool.TryParse(text.ToString(), out var result);
            return result;
        }

        /// <summary>
        /// default:0001-01-01
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text"></param>
        /// <returns></returns>
        public static DateTime ToDateTime<T>(this T text)
        {
            if (typeof(T).Equals(typeof(string))&&text.ToString()== "now()")
            {
                return DateTime.Now;
            }
            DateTime.TryParse(text.ToString(), out var result);
            return result;
        }

        /// <summary>
        /// default:0m
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text"></param>
        /// <returns></returns>
        public static decimal ToDecimal<T>(this T text)
        {
            decimal.TryParse(text.ToString(), out var result);
            return result;
        }

        /// <summary>
        /// default:0d
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text"></param>
        /// <returns></returns>
        public static double ToDouble<T>(this T text)
        {
            double.TryParse(text.ToString(), out var result);
            return result;
        }

        /// <summary>
        /// default:0f
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text"></param>
        /// <returns></returns>
        public static float ToFloat<T>(this T text)
        {
            float.TryParse(text.ToString(), out var result);
            return result;
        }

        /// <summary>
        /// default:0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int ToInt<T>(this T text)
        {
            int.TryParse(text.ToString(), out var result);
            return result;
        }

        /// <summary>
        /// default:00000000-0000-0000-0000-000000000000
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Guid ToGuid<T>(this T text)
        {
            Guid.TryParse(text.ToString(), out var result);
            return result;
        }
    }

}

