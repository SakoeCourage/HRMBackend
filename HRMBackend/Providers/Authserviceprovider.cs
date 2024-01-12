using HRMBackend.DB;
using HRMBackend.Model.Applicant;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using System.Security.Claims;

namespace HRMBackend.Providers
{
    public class Authserviceprovider
    {
        private readonly Context _context;
        private readonly HttpContext _httpContext;

        public Authserviceprovider(Context context, IHttpContextAccessor httpContextAccessor)
        {
             _context = context;
             _httpContext = httpContextAccessor.HttpContext;
        }

    public async Task<Applicant> GetAuthenticatedApplicant()
        {
            ClaimsIdentity identity = _httpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                Claim userIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
                Claim authorizationDecision = identity.FindFirst(ClaimTypes.AuthorizationDecision);
                Console.WriteLine($"applicant id is {userIdClaim?.Value}  & authorization reason is {authorizationDecision?.Value}");
                var _applicantId = userIdClaim.Value;
                var _authorizationDecision = authorizationDecision.Value;
                if (_applicantId != null && _authorizationDecision != null)
                {
                    if (_authorizationDecision.ToString() == AuthorizationDecisionType.Applicant.ToString()) {
                        var applicant = await FindApplicantAsync(int.Parse(_applicantId));
                        return applicant;  
                    }
                }
            }
            return null;
        }

        public async Task<Applicant> FindApplicantAsync(int Id)
        {
            var applicant = await _context.Applicant.FirstOrDefaultAsync(a => a.id == Id);
            return applicant;
        }

        public  Task FindUserAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) { return null; };

            //TO DO LATER
             
            return null;
        }

   
    }
}
