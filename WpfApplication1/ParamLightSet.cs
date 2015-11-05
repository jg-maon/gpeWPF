using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    /// <summary>
    /// LightSetパラメータモデル
    /// </summary>
    class ParamLightSet : ParameterBase
    {
        public class LightInfo
        {
            public string Name { get; set; }
            float[] angle;
            public float[] Angle { get { return angle; } set { angle = value; } }
            public float AngleX { get { return angle[0]; } set { angle[0] = value; } }
            public float AngleY { get { return angle[1]; } set { angle[1] = value; } }
            public ColorRGBI DiffColor { get; set; }
            public ColorRGBI SpecColor { get; set; }
            public float Sharpness { get; set; }
            public float SsaoWeight { get; set; }
            public LightInfo()
            {
                Name = null;
                angle = new float[2] { 0.0f, 0.0f };
                DiffColor = new ColorRGBI(1.0f);
                SpecColor = new ColorRGBI(1.0f);
                Sharpness = 50.0f;
                SsaoWeight = 1.0f;
            }
        }

        public LightInfo DirLight0 = new LightInfo() { Name = "DirLight0" };
        public LightInfo DirLight1 = new LightInfo() { Name = "DirLight1" };
        public LightInfo LocalLight1 = new LightInfo() { Name = "LocalLight1" };
        public LightInfo LocalLight2 = new LightInfo() { Name = "LocalLight2" };

        public bool EnableLocalLight1 = false;
        public bool EnableLocalLight2 = false;

        public ColorRGBI HemiColorUp = new ColorRGBI(1.0f);
        public ColorRGBI HemiColorDown = new ColorRGBI(1.0f);

    }
}
