using Our.Umbraco.Synthscribe.General.Models.Interrfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.Synthscribe.Features.DoctypeGeneration.Service
{
    public interface IGenerateDoctypeService
    {
        public Task<IResponseModel> GenerateDoctype(string context);
    }
}
