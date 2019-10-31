﻿//    Copyright 2017-2019 University of Toronto
// 
//    This file is part of XTMF2.
// 
//    XTMF2 is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
// 
//    XTMF2 is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
// 
//    You should have received a copy of the GNU General Public License
//    along with XTMF2.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace XTMF2.Web.Services {
	/// <summary>
	///     Authentication service for clients. Associates a session with a backed XTMF2 user account.
	/// </summary>
	public class AuthenticationService {
		private readonly ILogger<AuthenticationService> _logger;

		private readonly SignInManager<XTMF2.User> _signInManager;

		private readonly UserManager<XTMF2.User> _userManager;

		private readonly IConfiguration _configuration;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userManager"></param>
		/// <param name="signInManager"></param>
		/// <param name="logger"></param>
		/// <param name="configuration"></param>
		public AuthenticationService (UserManager<XTMF2.User> userManager, SignInManager<XTMF2.User> signInManager,
			ILogger<AuthenticationService> logger, IConfiguration configuration) {
			_signInManager = signInManager;
			_userManager = userManager;
			_logger = logger;
			_configuration = configuration;
		}

		/// <summary>
		/// </summary>
		/// <param name="userName">The username to associate the session with.</param>
		/// <param name="password">Currently unused.</param>
		public async Task<string> SignIn (string userName, string password = null) {
			var user = await _userManager.FindByIdAsync (userName);
			await _signInManager.SignInAsync (user, true);
			var claims = new [] {
				new Claim (ClaimTypes.Name, userName)
			};
			var key = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (_configuration["JwtSecurityKey"]));
			var creds = new SigningCredentials (key, SecurityAlgorithms.HmacSha256);
			var expiry = DateTime.Now.AddDays (Convert.ToInt32 (_configuration["JwtExpiryInDays"]));

			var token = new JwtSecurityToken (
				_configuration["JwtIssuer"],
				_configuration["JwtAudience"],
				claims,
				expires : expiry,
				signingCredentials : creds
			);
			return new JwtSecurityTokenHandler ().WriteToken (token);
		}
	}
}