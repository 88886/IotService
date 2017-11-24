using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintX.Dev.Utils.ToolsKit
{
    public static class EntityFactory
    {
        public static TEntity GetEntity<TEntity, TData>(TData data, Func<TData, string, object> func)
            where TEntity : class, new()
            where TData : class
        {
            TEntity result;
            if (data == null)
            {
                result = default(TEntity);
            }
            else
            {
                result = (TEntity)((object)EntityFactory.GetEntity<TData>(data, typeof(TEntity), func));
            }
            return result;
        }

        public static System.Collections.Generic.IList<TEntity> GetEntities<TEntity, TData>(System.Collections.Generic.IList<TData> datas, Func<TData, string, object> func)
            where TEntity : class, new()
            where TData : class
        {
            System.Collections.Generic.IList<TEntity> result;
            if (datas == null)
            {
                result = null;
            }
            else
            {
                result = (EntityFactory.GetEntities<TData>(datas, typeof(TEntity), func) as System.Collections.Generic.IList<TEntity>);
            }
            return result;
        }

        public static System.Collections.Generic.IList<TEntity> GetEntities<TEntity, TData>(System.Collections.IEnumerable datas, Func<TData, string, object> func)
            where TEntity : class, new()
            where TData : class
        {
            System.Collections.Generic.IList<TEntity> result;
            if (datas == null)
            {
                result = null;
            }
            else
            {
                result = (EntityFactory.GetEntities<TData>(datas, typeof(TEntity), func) as System.Collections.Generic.IList<TEntity>);
            }
            return result;
        }

        private static object GetEntity<TData>(TData data, System.Type entityType, Func<TData, string, object> func) where TData : class
        {
            object result;
            if (data == null)
            {
                result = null;
            }
            else
            {
                EntityClassAttribute entityClassAttribute = JavaScriptSerializer.GetEntityClassAttribute(entityType);
                System.Reflection.PropertyInfo[] properties = entityType.GetProperties();
                object obj = System.Activator.CreateInstance(entityType);
                System.Reflection.PropertyInfo[] array = properties;
                for (int i = 0; i < array.Length; i++)
                {
                    System.Reflection.PropertyInfo propertyInfo = array[i];
                    System.Reflection.MethodInfo setMethod = propertyInfo.GetSetMethod();
                    if (!(setMethod == null))
                    {
                        string propertyKey = JavaScriptSerializer.GetPropertyKey(propertyInfo, entityClassAttribute);
                        System.Type propertyType = propertyInfo.PropertyType;
                        object value = EntityFactory.GetValue(func(data, propertyKey), propertyType);
                        if (value != null)
                        {
                            if (propertyType.IsGenericType)
                            {
                                if (EntityFactory.IsIList(propertyType))
                                {
                                    System.Type type = propertyType.GetGenericArguments()[0];
                                    propertyInfo.SetValue(obj, EntityFactory.GetEntities<TData>(value as System.Collections.IEnumerable, type, func), null);
                                }
                            }
                            else if (!EntityFactory.IsPrimitiveType(propertyType))
                            {
                                propertyInfo.SetValue(obj, EntityFactory.GetEntity<TData>(value as TData, propertyType, func), null);
                            }
                            else
                            {
                                setMethod.Invoke(obj, new object[]
								{
									value
								});
                            }
                        }
                    }
                }
                result = obj;
            }
            return result;
        }

        private static System.Collections.IList GetEntities<TData>(System.Collections.IEnumerable datas, System.Type type, Func<TData, string, object> func) where TData : class
        {
            System.Collections.IList result;
            if (datas == null)
            {
                result = null;
            }
            else
            {
                System.Type typeFromHandle = typeof(System.Collections.Generic.List<>);
                System.Type type2 = typeFromHandle.MakeGenericType(new System.Type[]
				{
					type
				});
                System.Collections.IList list = System.Activator.CreateInstance(type2) as System.Collections.IList;
                foreach (object current in datas)
                {
                    list.Add(EntityFactory.GetEntity<TData>(current as TData, type, func));
                }
                result = list;
            }
            return result;
        }

        public static TData GetEntityData<TEntity, TData>(TEntity entity, Action<TData, string, object> action)
            where TEntity : class
            where TData : class, new()
        {
            TData result;
            if (entity == null)
            {
                result = default(TData);
            }
            else
            {
                result = EntityFactory.GetEntityData<TData>(entity, entity.GetType(), action);
            }
            return result;
        }

        public static System.Collections.Generic.IList<TData> GetEntityDatas<TEntity, TData>(System.Collections.Generic.IList<TEntity> entites, Action<TData, string, object> action)
            where TEntity : class
            where TData : class, new()
        {
            System.Collections.Generic.IList<TData> result;
            if (entites == null)
            {
                result = null;
            }
            else
            {
                result = EntityFactory.GetEntityDatas<TData>(entites, typeof(TEntity), action);
            }
            return result;
        }

        private static TData GetEntityData<TData>(object entity, System.Type entityType, Action<TData, string, object> action) where TData : class, new()
        {
            TData result;
            if (entity == null)
            {
                result = default(TData);
            }
            else
            {
                TData tData = System.Activator.CreateInstance<TData>();
                EntityClassAttribute entityClassAttribute = JavaScriptSerializer.GetEntityClassAttribute(entityType);
                System.Reflection.PropertyInfo[] properties = entityType.GetProperties();
                System.Reflection.PropertyInfo[] array = properties;
                for (int i = 0; i < array.Length; i++)
                {
                    System.Reflection.PropertyInfo propertyInfo = array[i];
                    string propertyKey = JavaScriptSerializer.GetPropertyKey(propertyInfo, entityClassAttribute);
                    object value = propertyInfo.GetValue(entity, null);
                    if (value == null)
                    {
                        action(tData, propertyKey, value);
                    }
                    else
                    {
                        System.Type propertyType = propertyInfo.PropertyType;
                        if (propertyType.IsGenericType)
                        {
                            if (EntityFactory.IsIList(propertyType))
                            {
                                System.Type type = propertyType.GetGenericArguments()[0];
                                if (type.IsClass)
                                {
                                    System.Collections.IList entites = value as System.Collections.IList;
                                    System.Collections.Generic.IList<TData> entityDatas = EntityFactory.GetEntityDatas<TData>(entites, type, action);
                                    action(tData, propertyKey, entityDatas);
                                }
                            }
                        }
                        else if (!EntityFactory.IsPrimitiveType(propertyInfo.PropertyType))
                        {
                            action(tData, propertyKey, EntityFactory.GetEntityData<TData>(value, propertyInfo.PropertyType, action));
                        }
                        else
                        {
                            action(tData, propertyKey, value);
                        }
                    }
                }
                result = tData;
            }
            return result;
        }

        private static System.Collections.Generic.IList<TData> GetEntityDatas<TData>(System.Collections.IEnumerable entites, System.Type entityType, Action<TData, string, object> action) where TData : class, new()
        {
            System.Collections.Generic.IList<TData> result;
            if (entites == null)
            {
                result = null;
            }
            else
            {
                System.Collections.Generic.IList<TData> list = new System.Collections.Generic.List<TData>();
                foreach (object current in entites)
                {
                    TData entityData = EntityFactory.GetEntityData<TData>(current, entityType, action);
                    list.Add(entityData);
                }
                result = list;
            }
            return result;
        }

        private static bool IsIList(System.Type propType)
        {
            return typeof(System.Collections.Generic.IList<>) == propType.GetGenericTypeDefinition() || typeof(System.Collections.Generic.List<>) == propType.GetGenericTypeDefinition();
        }

        internal static object GetValue(object value, System.Type type)
        {
            if (value == System.DBNull.Value)
            {
                value = null;
            }
            System.Type type2 = type;
            object result;
            if (value != null && EntityFactory.IsPrimitiveType(type, out type2))
            {
                result = EntityFactory.ChangeType(value, type2);
            }
            else if (value != null && type2.IsEnum && !value.GetType().IsEnum)
            {
                result = System.Enum.ToObject(type2, value);
            }
            else if (value == null && type2.IsSubclassOf(typeof(System.ValueType)))
            {
                result = EntityFactory.GetDefaultValue(type);
            }
            else
            {
                result = value;
            }
            return result;
        }

        internal static bool IsPrimitiveType(System.Type type)
        {
            System.Type type2;
            return EntityFactory.IsPrimitiveType(type, out type2);
        }

        internal static bool IsPrimitiveType(System.Type type, out System.Type underlyingType)
        {
            underlyingType = type;
            bool flag = EntityFactory.DoGetIsPrimitiveType(type);
            bool result;
            if (flag)
            {
                result = true;
            }
            else if (type.IsValueType && type.IsGenericType)
            {
                underlyingType = System.Nullable.GetUnderlyingType(type);
                result = EntityFactory.DoGetIsPrimitiveType(underlyingType);
            }
            else
            {
                result = false;
            }
            return result;
        }

        private static bool DoGetIsPrimitiveType(System.Type type)
        {
            return type.IsPrimitive || type == typeof(string) || type == typeof(System.DateTime) || type == typeof(decimal);
        }

        private static object GetDefaultValue(System.Type type)
        {
            return type.IsValueType ? System.Activator.CreateInstance(type) : null;
        }

        internal static object ChangeType(object value, System.Type type)
        {
            object result;
            if (type == typeof(bool) && value.GetType() == typeof(string))
            {
                result = ((string)value == "1");
            }
            else
            {
                result = System.Convert.ChangeType(value, type, null);
            }
            return result;
        }
    }
}
