using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">値の型</typeparam>
    [Serializable]
    public class CustomPropertyDescriptor<T> : PropertyDescriptor, ISerializable
    {
        private Type m_propertyType;
        private Type m_componentType;
        T m_propertyValue;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value">値</param>
        /// <param name="componentType">プロパティを保持する型</param>
        public CustomPropertyDescriptor(string propertyName, T value, Type componentType)
            : base(propertyName, new Attribute[] { })
        {
            //this.m_propertyType = typeof(T);
            this.m_propertyType = value.GetType();
            this.m_componentType = componentType;
            m_propertyValue = value;
        }

        public override bool CanResetValue(object component) { return true; }
        public override Type ComponentType { get { return m_componentType; } }

        public override object GetValue(object component)
        {
            return m_propertyValue;
        }

        public override bool IsReadOnly { get { return false; } }
        public override Type PropertyType { get { return m_propertyType; } }
        public override void ResetValue(object component) { SetValue(component, default(T)); }
        public override void SetValue(object component, object value)
        {
            if (!value.GetType().IsAssignableFrom(m_propertyType))
            {
                throw new System.Exception("Invalid type to assign");
            }

            m_propertyValue = (T)value;
        }

        public override bool ShouldSerializeValue(object component) { return true; }

        #region ISerializable メンバー

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("m_propertyType", m_propertyType);
            info.AddValue("m_componentType", m_componentType);
            info.AddValue("m_propertyValue", m_propertyValue);
        }

        #endregion
    }
}
