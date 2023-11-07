using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using AbsenDulu.BE.Models.User;
using AbsenDulu.BE.DTO.Credential;
using AbsenDulu.BE.Interfaces.IServices;
using AbsenDulu.BE.Interfaces.IServices.CompanyData;

namespace AbsenDulu.BE.Token.Helper
{
    public class TokenConfiguration
    {
        private readonly IConfiguration _configuration;
        private readonly ICompanyService _companies;
        private JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        private TokenValidationParameters? validationParameters = new TokenValidationParameters();

        private  readonly TokenValidate _tokenval;
        private readonly IEmployeeService _employeeService;
        public TokenConfiguration(ICompanyService companyService, IEmployeeService employeeService, IConfiguration configuration,TokenValidate tokenValidate)
        {
            _configuration = configuration;
            _tokenval=tokenValidate;
            _employeeService=employeeService;
            _companies = companyService;
        }

        public string CreateToken(UserAccount user,string url)
        {
            try
            {
                string dept = "";
                var signingCredentials = new SigningCredentials(GetSecurityKey(GetJwtTokenSigningKey()), SecurityAlgorithms.HmacSha256);
                if(user.Role.ToLower()!="admin")
                {
                    var departmentCode = _employeeService.GetEmployeeDepartmentCode(user.CompanyId, user.EmployeeId);
                    dept=departmentCode.DepartmentCode;
                }

                var company = _companies.GetCompanies(user.CompanyId);
                var claims = new[]
                {
                    new Claim("username", user.UserName.ToString()),
                    new Claim("role", user.Role.ToString()),
                    new Claim("id", user.Id.ToString()),
                    new Claim("employee_id",user.Role.ToLower()!="admin" ? user.EmployeeId.ToString():"Admin"),
                    new Claim("icon_url", url),
                    new Claim("company_id", user.CompanyId.ToString()),
                    new Claim("company_name", user.CompanyName),
                    new Claim("department_code", user.Role.ToLower()!="admin" ? dept:"Admin"),
                    new Claim("company_city", company.FirstOrDefault().CompanyCity)
                    // new Claim("access", company.FirstOrDefault().CompanyCity)
                    // Add more claims as needed
                };

                    var token = new JwtSecurityToken(
                        issuer: _configuration.GetSection("JwtToken:Issuer").Value,
                        audience: "",
                        claims: claims,
                        expires: DateTime.UtcNow.AddDays(1),
                        signingCredentials: signingCredentials);

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var tokenString = tokenHandler.WriteToken(token);
                    return tokenString;
            }
            catch
            {
                throw;
            }
        }

        public bool ValidateToken(string token)
        {
            try
            {
                validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = GetSecurityKey(GetJwtTokenSigningKey()),
                    ValidateIssuer = false,
                    // ValidIssuer = "https://localhost:5013/",
                    ValidateAudience = false
                };

                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                if (validatedToken.ValidTo < DateTime.UtcNow)
                {
                    throw new TokenException("Token Expired");
                }

                if (validatedToken is JwtSecurityToken jwtToken)
                {
                    var username = jwtToken.Claims.FirstOrDefault(c => c.Type == "username")?.Value;
                    var userid = jwtToken.Claims.FirstOrDefault(c => c.Type ==  "id")?.Value;
                    var employeeid = jwtToken.Claims.FirstOrDefault(c => c.Type ==  "employee_id")?.Value;
                    var role = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
                    var companyId = jwtToken.Claims.FirstOrDefault(c => c.Type == "company_id")?.Value;
                    var companyName = jwtToken.Claims.FirstOrDefault(c => c.Type == "company_name")?.Value;
                    var departmentCode = jwtToken.Claims.FirstOrDefault(c => c.Type == "department_code")?.Value;
                    var companycity = jwtToken.Claims.FirstOrDefault(c => c.Type == "company_city")?.Value;

                    var userCredential = new UserCredentialDTO
                    {
                        UserName = username,
                        Id = userid,
                        CompanyId = companyId,
                        CompanyName = companyName,
                        Role = role,
                        EmployeeId = employeeid,
                        DepartmentCode = departmentCode,
                        CompanyCity = companycity
                    };
                }
                else
                {
                    throw new TokenException("Token Invalid");
                }
                return true;
            }
            catch
            {
                throw;
            }
        }

        public TokenValidate? GetTokenClaim(string? token)
        {
            try
            {
                validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = GetSecurityKey(GetJwtTokenSigningKey()),
                    ValidateIssuer = false,
                    // ValidIssuer = "https://localhost:5013/",
                    ValidateAudience = false
                };

                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                if (validatedToken is JwtSecurityToken jwtToken)
                {
                    var username = jwtToken.Claims.FirstOrDefault(c => c.Type == "username")?.Value;
                    var userid = jwtToken.Claims.FirstOrDefault(c => c.Type ==  "id")?.Value;
                    var employeeid = jwtToken.Claims.FirstOrDefault(c => c.Type ==  "employee_id")?.Value;
                    var role = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
                    var companyid = jwtToken.Claims.FirstOrDefault(c => c.Type == "company_id")?.Value;
                    var companyname = jwtToken.Claims.FirstOrDefault(c => c.Type == "company_name")?.Value;
                    var departmentcode = jwtToken.Claims.FirstOrDefault(c => c.Type == "department_code")?.Value;
                    var iconurl = jwtToken.Claims.FirstOrDefault(c => c.Type == "icon_url")?.Value;
                    var companycity = jwtToken.Claims.FirstOrDefault(c => c.Type == "company_city")?.Value;
                    Guid guid = new Guid(companyid);
                    _tokenval.UserName=username;
                    _tokenval.Id=userid;
                    _tokenval.EmployeeId=employeeid.ToString();
                    _tokenval.IconUrl = iconurl;
                    _tokenval.Role=role;
                    _tokenval.CompanyId= guid;
                    _tokenval.CompanyName=companyname;
                    _tokenval.isValidToken=true;
                    _tokenval.DepartmentCode=departmentcode;
                    _tokenval.CompanyCity=companycity;
                    return  _tokenval;
                }
                else
                {
                    throw new TokenException("Token Invalid");
                }
            }
            catch
            {
                throw;
            }
        }

        private SymmetricSecurityKey GetSecurityKey(string secretKey)
        {
            try
            {
                return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            }
            catch
            {
                throw;
            }
        }

        public string? GetJwtTokenSigningKey()
        {
            try
            {
                return _configuration.GetSection("JwtToken:SigningKey").Value;
            }
            catch
            {
                throw;
            }
        }
    }
}