using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public class ColorRGBI
    {
        float[] m_value = null;
        public float R { get { return m_value[0]; } set { m_value[0] = value; } }
        public float G { get { return m_value[1]; } set { m_value[1] = value; } }
        public float B { get { return m_value[2]; } set { m_value[2] = value; } }
        public float I { get { return m_value[3]; } set { m_value[3] = value; } }
        public float[] Value { 
            get { return m_value; }
            set
            {
                m_value = value;
            }
        }
        public ColorRGBI():
            this(0.0f)            
        {
        }
        public ColorRGBI(float v) :
            this(v, v, v, v)
        {
        }
        public ColorRGBI(params float[] rgbi) :
            this(rgbi[0], rgbi[1], rgbi[2], rgbi[3])
        {
        }

        public ColorRGBI(float r, float g, float b, float i)
        {
            m_value = new float[4] { r, g, b, i };
        }
    }
    public class ColorRGBA
    {
        float[] m_value = null;
        public float R { get { return m_value[0]; } set { m_value[0] = value; } }
        public float G { get { return m_value[1]; } set { m_value[1] = value; } }
        public float B { get { return m_value[2]; } set { m_value[2] = value; } }
        public float A { get { return m_value[3]; } set { m_value[3] = value; } }
        public float[] Value { 
            get { return m_value; }
            set
            {
                m_value = value;
            }
        }
        public ColorRGBA():
            this(0.0f)            
        {
        }
        public ColorRGBA(float v) :
            this(v, v, v, v)
        {
        }
        public ColorRGBA(params float[] rgba) :
            this(rgba[0], rgba[1], rgba[2], rgba[3])
        {
        }

        public ColorRGBA(float r, float g, float b, float a)
        {
            m_value = new float[4] { r, g, b, a };
        }
    }
}
