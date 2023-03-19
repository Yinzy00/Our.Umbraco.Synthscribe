using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.Synthscribe.General.Models.Interrfaces
{
    public interface IResponseModel
    {
        public bool Succes { get; set; }
        public string Message { get; set; }
    }
}
