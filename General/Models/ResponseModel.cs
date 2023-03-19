using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.Synthscribe.General.Models.Interrfaces
{
    public class ResponseModel: IResponseModel
    {
        public ResponseModel()
        {

        }
        public ResponseModel(bool succes, string message)
        {
            Succes = succes;
            _message = message;
        }
        private string _message;
        public bool Succes { get; set; }
        public string Message { get => _message ?? "Seomthing went wrong."; set => _message = value; }
    }
}
