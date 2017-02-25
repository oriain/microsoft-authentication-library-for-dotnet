﻿//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.Internal.Instance;
using Microsoft.Identity.Client.Internal.Requests;

namespace Microsoft.Identity.Client
{
    /// <summary>
    /// ConfidentialClientApplication
    /// </summary>
    public sealed class ConfidentialClientApplication : ClientApplicationBase
    {
        /// <summary>
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="redirectUri"></param>
        /// <param name="clientCredential"></param>
        /// <param name="userTokenCache"></param>
        /// <param name="appTokenCache"></param>
        public ConfidentialClientApplication(string clientId, string redirectUri,
            ClientCredential clientCredential, TokenCache userTokenCache, TokenCache appTokenCache)
            : this(clientId, DefaultAuthority, redirectUri, clientCredential, userTokenCache, appTokenCache)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="authority"></param>
        /// <param name="redirectUri"></param>
        /// <param name="clientCredential"></param>
        /// <param name="userTokenCache"></param>
        /// <param name="appTokenCache"></param>
        public ConfidentialClientApplication(string clientId, string authority, string redirectUri,
            ClientCredential clientCredential, TokenCache userTokenCache, TokenCache appTokenCache) : base(authority, clientId, redirectUri, true)
        {
            this.ClientCredential = clientCredential;
            this.UserTokenCache = userTokenCache;
            this.AppTokenCache = appTokenCache;
            this.AppTokenCache.TokenCacheAccessor.TokenCachePlugin = PlatformPlugin.NewTokenCachePluginInstance;
        }

        /// <summary>
        /// ClientCredential
        /// </summary>
        public ClientCredential ClientCredential { get; }

        /// <summary>
        /// AppTokenCache
        /// </summary>
        public TokenCache AppTokenCache { get; }

        /// <summary>
        /// AcquireTokenOnBehalfOfAsync
        /// </summary>
        public async Task<AuthenticationResult> AcquireTokenOnBehalfOfAsync(string[] scope, UserAssertion userAssertion)
        {
            Authority authority = Internal.Instance.Authority.CreateAuthority(this.Authority, this.ValidateAuthority);
            return
                await
                    this.AcquireTokenOnBehalfCommonAsync(authority, scope, userAssertion)
                        .ConfigureAwait(false);
        }

        /// <summary>
        /// AcquireTokenOnBehalfOfAsync
        /// </summary>
        public async Task<AuthenticationResult> AcquireTokenOnBehalfOfAsync(string[] scope, UserAssertion userAssertion,
            string authority)
        {
            Authority authorityInstance = Internal.Instance.Authority.CreateAuthority(authority, this.ValidateAuthority);
            return
                await
                    this.AcquireTokenOnBehalfCommonAsync(authorityInstance, scope, userAssertion)
                        .ConfigureAwait(false);
        }

        /// <summary>
        /// AcquireTokenByAuthorizationCodeAsync
        /// </summary>
        public async Task<AuthenticationResult> AcquireTokenByAuthorizationCodeAsync(string authorizationCode, string[] scope)
        {
            return
                await
                    this.AcquireTokenByAuthorizationCodeCommonAsync(authorizationCode, scope, new Uri(this.RedirectUri)).ConfigureAwait(false);
        }

        /// <summary>
        /// AcquireTokenByAuthorizationCodeAsync
        /// </summary>
        public async Task<AuthenticationResult> AcquireTokenByAuthorizationCodeAsync(
            string authorizationCode, string[] scope, string redirectUri)
        {
            return
                await
                    this.AcquireTokenByAuthorizationCodeCommonAsync(
                        authorizationCode, scope, new Uri(redirectUri)).ConfigureAwait(false);
        }

        /// <summary>
        /// AcquireTokenForClient
        /// </summary>
        public async Task<AuthenticationResult> AcquireTokenForClientAsync(string[] scope)
        {
            return
                await
                    this.AcquireTokenForClientCommonAsync(scope).ConfigureAwait(false);
        }

        private async Task<AuthenticationResult> AcquireTokenForClientCommonAsync(string[] scope)
        {
            Authority authority = Internal.Instance.Authority.CreateAuthority(this.Authority, this.ValidateAuthority);
            AuthenticationRequestParameters parameters = this.CreateRequestParameters(authority, scope, null,
                this.AppTokenCache);
            var handler = new ClientCredentialRequest(parameters);
            return await handler.RunAsync().ConfigureAwait(false);
        }

        private async Task<AuthenticationResult> AcquireTokenOnBehalfCommonAsync(Authority authority,
            string[] scope, UserAssertion userAssertion)
        {
            var requestParams = this.CreateRequestParameters(authority, scope, null, this.UserTokenCache);
            requestParams.UserAssertion = userAssertion;
            var handler = new OnBehalfOfRequest(requestParams);
            return await handler.RunAsync().ConfigureAwait(false);
        }

        private async Task<AuthenticationResult> AcquireTokenByAuthorizationCodeCommonAsync(string authorizationCode,
            string[] scope, Uri redirectUri)
        {
            Authority authority = Internal.Instance.Authority.CreateAuthority(this.Authority, this.ValidateAuthority);
            var requestParams = this.CreateRequestParameters(authority, scope, null, this.UserTokenCache);
            requestParams.AuthorizationCode = authorizationCode;
            requestParams.RedirectUri = redirectUri;
            var handler =
                new AuthorizationCodeRequest(requestParams);
            return await handler.RunAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Gets URL of the authorize endpoint including the query parameters.
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="loginHint"></param>
        /// <param name="extraQueryParameters"></param>
        /// <returns>URL of the authorize endpoint including the query parameters.</returns>
        public async Task<Uri> GetAuthorizationRequestUrlAsync(string[] scope, string loginHint,
            string extraQueryParameters)
        {
            Authority authority = Internal.Instance.Authority.CreateAuthority(this.Authority, this.ValidateAuthority);
            var requestParameters =
                this.CreateRequestParameters(authority, scope, null, this.UserTokenCache);
            requestParameters.ClientKey = new ClientKey(this.ClientId);
            requestParameters.ExtraQueryParameters = extraQueryParameters;

            var handler =
                new InteractiveRequest(requestParameters, null, loginHint, null, null);
            return await handler.CreateAuthorizationUriAsync(CreateCallState(CorrelationId)).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets URL of the authorize endpoint including the query parameters.
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="redirectUri"></param>
        /// <param name="loginHint"></param>
        /// <param name="extraQueryParameters"></param>
        /// <param name="additionalScope"></param>
        /// <param name="authority"></param>
        /// <returns>URL of the authorize endpoint including the query parameters.</returns>
        public async Task<Uri> GetAuthorizationRequestUrlAsync(string[] scope, string redirectUri, string loginHint,
            string extraQueryParameters, string[] additionalScope, string authority)
        {
            Authority authorityInstance = Internal.Instance.Authority.CreateAuthority(authority, this.ValidateAuthority);
            var requestParameters = this.CreateRequestParameters(authorityInstance, scope, null,
                this.UserTokenCache);
            requestParameters.RedirectUri = new Uri(redirectUri);
            requestParameters.ClientKey = new ClientKey(this.ClientId);
            requestParameters.ExtraQueryParameters = extraQueryParameters;

            var handler =
                new InteractiveRequest(requestParameters, additionalScope, loginHint, null, null);
            return await handler.CreateAuthorizationUriAsync(CreateCallState(CorrelationId)).ConfigureAwait(false);
        }

        internal override AuthenticationRequestParameters CreateRequestParameters(Authority authority, string[] scope, User user, TokenCache cache)
        {
            AuthenticationRequestParameters parameters = base.CreateRequestParameters(authority, scope, user, cache);
            parameters.ClientKey = new ClientKey(this.ClientId, this.ClientCredential, authority);

            return parameters;
        }
    }
}