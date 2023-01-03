using GraduateThesis.ApplicationCore.Enums;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateThesis.ApplicationCore.File
{
    public class ContentTypeManager
    {
        public string GetContentType(ExportTypeOptions exportTypeOptions)
        {
            switch (exportTypeOptions)
            {
                case ExportTypeOptions.XLS: 
                    return ContentTypeConsts.XLS;
                case ExportTypeOptions.XLSX: 
                    return ContentTypeConsts.XLSX;
                default:
                    return ContentTypeConsts.XLS;
            }
        }

        public string GetExtensionName(ExportTypeOptions exportTypeOptions)
        {
            return exportTypeOptions.ToString().ToLower();
        }
    }
}
