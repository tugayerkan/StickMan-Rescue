using UnityEngine.Rendering;

namespace SencanUtils
{
    public static class SUInfo
    {
        public enum PipeLineType
        {
            BuiltIn,
            URP,
            HDRP
        }
        
        public static PipeLineType PipelineType { get; private set; }
        
        static SUInfo()
        {
            PipelineType = GetPipeLineType();
        }
        
        private static PipeLineType GetPipeLineType()
        {
            string rpAsset = "";
            if (GraphicsSettings.renderPipelineAsset != null)
                rpAsset = GraphicsSettings.renderPipelineAsset.GetType().Name;

            PipeLineType type;
            if (rpAsset.Equals("HDRenderPipelineAsset"))
                type = PipeLineType.HDRP;
            else if (rpAsset.Equals("UniversalRenderPipelineAsset"))
                type = PipeLineType.URP;
            else
                type = PipeLineType.BuiltIn;

            return type;
        }
    }
}
