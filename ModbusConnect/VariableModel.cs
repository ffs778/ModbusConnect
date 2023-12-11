using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCConnect
{
    public abstract class VariableModel
    {
        public string VariableName { get; set; }
        public string DataType { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
    public class SiteTypeVarModel:VariableModel
    {
        public string StartAddress { get; set; }
        public string Length { get; set; }
    }
    public class LabelTypeVarModel:VariableModel
    {
        public string VariableNameInPLC { get; set; }
    }
}
