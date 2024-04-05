using Microsoft.IdentityModel.Tokens;
using Simple.AspNetCore.Models;
using Simple.Utils;
using Simple.Utils.Helper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Simple.AspNetCore.Helper
{
    /// <summary>jwt 帮助类</summary>
    public class JWTHelper
    {
        /// <summary>JWT配置</summary>
        private static readonly JwtSetting settings;

        /// <summary>构造函数</summary>
        /// <param name="settings"></param>
        static JWTHelper()
        {
            settings = ConfigHelper.GetValue<JwtSetting>("Jwt");
        }

        /// <summary>生成Jwt</summary>
        /// <param name="claims"></param>
        /// <param name="Minutes">过期时间</param>
        /// <returns></returns>
        public static string CreateToken(IEnumerable<Claim> claims, int Minutes = 0)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var securityToken = new JwtSecurityToken(
                issuer: settings.Issuer,
                audience: settings.Audience,
                claims: claims,
                expires: Minutes > 0 ? DateTime.Now.AddMinutes(Minutes) : DateTime.Now.AddMinutes(settings.ExpMinutes),
                signingCredentials: creds);
            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            HostServiceExtension.httpContext?.Session.Set(ConfigHelper.GetValue("TokenHeadKey"), token);
            return "Bearer " + token;
        }

        /// <summary>生成Jwt</summary>
        /// <param name="userAccount">账户信息</param>
        /// <param name="userName"></param>
        /// <param name="roleId"></param>
        /// <param name="userId"></param>
        /// <param name="orgnizationId">组织结构ID</param>
        /// <param name="isAdmin">是否管理员</param>
        /// <returns></returns>
        public static string CreateToken(string userAccount, string userName, string roleId, string userId, string orgnizationId, bool SupperAdmin = false, bool AdminOrg = false)
        {
            if (roleId is null)
                roleId = "";
            if (orgnizationId is null)
                orgnizationId = "";
            //声明claim
            var claims = new Claim[] {
                new Claim("UserAccount", userAccount),
                new Claim("UserName", userName),
                new Claim("UserID", userId),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToUnix().ToString(), ClaimValueTypes.Integer64),//签发时间
                new Claim(JwtRegisteredClaimNames.Nbf, DateTime.UtcNow.ToUnix().ToString(), ClaimValueTypes.Integer64),//生效时间
                new Claim(JwtRegisteredClaimNames.Exp, DateTime.Now.AddMinutes(settings.ExpMinutes).ToUnix().ToString(), ClaimValueTypes.Integer64), //过期时间
                new Claim(JwtRegisteredClaimNames.Iss, settings.Issuer),
                new Claim(JwtRegisteredClaimNames.Aud, settings.Audience),
                new Claim(ClaimTypes.Name, userName),
                new Claim("RoleId", roleId),
                new Claim("OrgnizationId",orgnizationId),
                new Claim("SupperAdmin",SupperAdmin.ToString()),
                new Claim("AdminOrg", AdminOrg.ToString()),
                new Claim("ExpTime",DateTime.Now.AddMinutes(settings.ExpMinutes).ToString("yyyy-MM-dd HH:mm:ss"))
            };
            return CreateToken(claims);
        }

        /// <summary>刷新token</summary>
        /// <returns></returns>
        public static string RefreshToken(string oldToken)
        {
            var pl = GetPayload(oldToken);
            //声明claim
            var claims = new Claim[] {
                new Claim("UserName", pl?.UserName),
                new Claim("UserAccount", pl?.UserAccount),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToUnix().ToString(), ClaimValueTypes.Integer64),//签发时间
                new Claim(JwtRegisteredClaimNames.Nbf, DateTime.UtcNow.ToUnix().ToString(), ClaimValueTypes.Integer64),//生效时间
                new Claim(JwtRegisteredClaimNames.Exp, DateTime.Now.AddMinutes(settings.ExpMinutes).ToUnix().ToString(), ClaimValueTypes.Integer64), //过期时间
                new Claim(JwtRegisteredClaimNames.Iss, settings.Issuer),
                new Claim(JwtRegisteredClaimNames.Aud, settings.Audience),
                new Claim("UserName", pl?.UserName),
                new Claim("RoleId", pl?.RoleId),
                new Claim("UserID", pl?.UserID),
                new Claim("SupperAdmin",pl?.SupperAdmin.ToString()),
                new Claim("AdminOrg", pl?.AdminOrg.ToString()),
                new Claim("OrgnizationId",pl?.OrgnizationId),
                new Claim("ExpTime",pl?.ExpTime.ToString("yyyy-MM-dd HH:mm:ss"))
            };

            return CreateToken(claims);
        }

        /// <summary>从token中获取用户身份</summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static IEnumerable<Claim> GetClaims(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.ReadJwtToken(token);
            return securityToken?.Claims;
        }

        /// <summary>从Token中获取用户身份</summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static ClaimsPrincipal GetPrincipal(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            try
            {
                return handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecurityKey)),
                    ValidateLifetime = false
                }, out SecurityToken validatedToken);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>校验Token</summary>
        /// <param name="token">token</param>
        /// <returns></returns>
        public static bool CheckToken(string token)
        {
            var principal = GetPrincipal(token);
            if (principal is null)
            {
                return false;
            }
            return true;
        }

        /// <summary>获取Token中的载荷数据</summary>
        /// <param name="token">token</param>
        /// <returns></returns>
        public static Models.JwtPayload GetPayload(string token)
        {
            token = token.Replace("Bearer ", "");
            if (!CheckToken(token))
            {
                throw new Exception("token校验没有成功");
            }
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken securityToken = jwtHandler.ReadJwtToken(token);
            return new Models.JwtPayload
            {
                UserAccount = securityToken.Payload["UserAccount"]?.ToString(),
                UserID = securityToken.Payload["UserID"]?.ToString(),
                UserName = securityToken.Payload["UserName"]?.ToString(),
                OrgnizationId = securityToken.Payload["OrgnizationId"]?.ToString(),
                RoleId = securityToken.Payload["RoleId"].ToString(),
                SupperAdmin = securityToken.Payload["SupperAdmin"].ToBool(),
                AdminOrg = securityToken.Payload["AdminOrg"].ToBool(),
                ExpTime = securityToken.Payload[JwtRegisteredClaimNames.Exp]?.ToDateTime()
            };
        }

        /// <summary>获取Token中的载荷数据</summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="token">token</param>
        /// <returns></returns>
        public static T GetPayload<T>(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(token);
            return JsonHelper.FromJson<T>(jwtToken.Payload.SerializeToJson());
        }

        /// <summary>获取Token中的载荷数据</summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static Models.JwtPayload GetPayload(IEnumerable<Claim> claims)
        {
            var jwtPayload = new Models.JwtPayload();
            jwtPayload.UserAccount = claims.FirstOrDefault(p => p.Type == "UserAccount")?.Value;
            jwtPayload.UserID = claims.FirstOrDefault(p => p.Type == "UserID")?.Value;
            jwtPayload.UserName = claims.FirstOrDefault(p => p.Type == "UserName")?.Value;
            jwtPayload.RoleId = claims.FirstOrDefault(p => p.Type == "RoleId")?.Value;
            jwtPayload.ExpTime = claims.FirstOrDefault(p => p.Type == "ExpTime")?.Value.ToDateTime();
            jwtPayload.OrgnizationId = claims.FirstOrDefault(p => p.Type == "OrgnizationId")?.Value;
            jwtPayload.SupperAdmin = claims.First(p => p.Type == "SupperAdmin").Value.ToBool();
            jwtPayload.AdminOrg = claims.First(p => p.Type == "AdminOrg").Value.ToBool();
            jwtPayload.OrgnizationId = claims.FirstOrDefault(p => p.Type == "OrgnizationId")?.Value;
            return jwtPayload;
        }

        /// <summary>判断token是否过期</summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool IsExp(string token)
        {
            //return GetPrincipal(token)?.Claims.First(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value?.UnixToTime() < DateTime.Now;
            return GetPayload(token).ExpTime < DateTime.Now;
        }
    }
}