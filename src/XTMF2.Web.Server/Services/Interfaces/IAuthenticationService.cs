using System.Threading.Tasks;

namespace XTMF2.Web.Services.Interfaces {
    /// <summary>
    ///     Authentication service for clients. Associates a session with a backed XTMF2 user account.
    /// </summary>
    public interface IAuthenticationService {

        Task<string> SignIn (string userName, string password = null);

        Task SignOut (User user);
    }

}