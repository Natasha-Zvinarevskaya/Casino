using Casino.Services.Request.GoogleAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Interfaces
{
    public interface IGoogleService
    {
        string GoogleProvider(GetAuthUrlRequest request);

    }
}
