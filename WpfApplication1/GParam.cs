﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WpfApplication1
{
    
    public enum GX_META_INFO_TYPE
    {
        UNKNOWN = 0,
        INT8,
        INT16,
        INT32,
        INT64,
        UINT8,
        UINT16,
        UINT32,
        UINT64,
        FLOAT32,
        FLOAT64,
        BOOL,
        VECTOR2AL,
        VECTOR3AL,
        VECTOR4AL,
        COLOR32,
        STRINGW,
        NUM,
    }

    [XmlRoot("root")]
    public class GparamRoot
    {
        public class _ParamSet
        {
            [XmlAttribute("dispname")]
            public string DispName { get; set; }

            [XmlAttribute("name")]
            public string Name { get; set; }

            public class _Edited
            {
                [XmlElement("id")]
                public List<int> Id { get; set; }
            }
            [XmlElement("edited")]
            public _Edited Edited { get; set; }

            
            public class _Value
            {
                [XmlAttribute(AttributeName = "id")]
                public int Id { get; set; }
                [XmlAttribute(AttributeName = "index")]
                public int Index { get; set; }
                [XmlText]
                public string Text { get; set; }
            }

            public class _Comment
            {
                [XmlElement(ElementName = "value")]
                public List<_Value> Value { get; set; }
            }
            [XmlElement("comments")]
            public _Comment Comment { get; set; }


            public class _Slot
            {
                [XmlAttribute("dispname")]
                public string DispName { get; set; }
                [XmlAttribute("type")]
                public int Type { get; set; }

                [XmlAttribute("name")]
                public string Name { get; set; }

                [XmlElement(ElementName = "value")]
                public List<_Value> Value { get; set; }
            }


            [XmlElement(ElementName = "slot")]
            public List<_Slot> Slot { get; set; }
        }


        public class _IdSets
        {
            [XmlElement(ElementName = "idset")]
            public string Idset { get; set; }
        }

        [XmlElement(ElementName = "ParamSet")]
        public List<_ParamSet> ParamSet { get; set; }
        [XmlElement(ElementName = "idSets")]
        public _IdSets IdSets { get; set; }
    }
}
