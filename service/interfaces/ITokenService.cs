using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecommerce.models;

namespace ecommerce.service
{
    public interface ITokenService
    {
       TokenInfo GetToken(LoginRequest request);

    }

}