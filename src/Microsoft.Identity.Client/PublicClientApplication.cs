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
using Microsoft.Identity.Client.Internal.Interfaces;
using Microsoft.Identity.Client.Internal;
using Microsoft.Identity.Client.Internal.Instance;
using Microsoft.Identity.Client.Internal.Requests;
using System.Collections.Generic;

namespace Microsoft.Identity.Client
{
    /// <summary>
    /// Class to be used for native applications (Desktop/UWP/iOS/Android).
    /// </summary>
    public sealed partial class PublicClientApplication : ClientApplicationBase, IPublicClientApplication
    {
        internal const string DEFAULT_REDIRECT_URI = "urn:ietf:wg:oauth:2.0:oob";

        /// <summary>
        /// Default consutructor of the application. It will use https://login.microsoftonline.com/common as the default authority.
        /// </summary>
        /// <param name="clientId">Client id of the application</param>
        public PublicClientApplication(string clientId) : this(clientId, DefaultAuthority)
        {
        }

        /// <summary>
        /// Default consutructor of the application.
        /// </summary>
        /// <param name="clientId">Client id of the application</param>
        /// <param name="authority">Default authority to be used for the application</param>
        public PublicClientApplication(string clientId, string authority)
            : base(authority, clientId, DEFAULT_REDIRECT_URI, true)
        {
            UserTokenCache = new TokenCache()
            {
                ClientId = clientId
            };
        }


#if WINRT
/// <summary>
/// 
/// </summary>
        public bool UseCorporateNetwork { get; set; }
#endif

#if !ANDROID
        /// <summary>
        /// Interactive request to acquire token. 
        /// </summary>
        /// <param name="scope">Array of scopes requested for resource</param>
        /// <returns>Authentication result containing token of the user</returns>
        public async Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scope)
        {
            Authority authority = Internal.Instance.Authority.CreateAuthority(Authority, ValidateAuthority);
            return
                await
                    AcquireTokenCommonAsync(authority, scope, null, (string) null,
                        UIBehavior.SelectAccount, null, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Interactive request to acquire token. 
        /// </summary>
        /// <param name="scope">Array of scopes requested for resource</param>
        /// <param name="loginHint">Identifier of the user. Generally a UPN.</param>
        /// <returns>Authentication result containing token of the user</returns>
        public async Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scope, string loginHint)
        {
            Authority authority = Internal.Instance.Authority.CreateAuthority(Authority, ValidateAuthority);
            return
                await
                    AcquireTokenCommonAsync(authority, scope, null, loginHint,
                        UIBehavior.SelectAccount, null, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Interactive request to acquire token. 
        /// </summary>
        /// <param name="scope">Array of scopes requested for resource</param>
        /// <param name="loginHint">Identifier of the user. Generally a UPN.</param>
        /// <param name="behavior">Enumeration to control UI behavior.</param>
        /// <param name="extraQueryParameters">This parameter will be appended as is to the query string in the HTTP authentication request to the authority. The parameter can be null.</param>
        /// <returns>Authentication result containing token of the user</returns>
        public async Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scope, string loginHint,
            UIBehavior behavior, string extraQueryParameters)
        {
            Authority authority = Internal.Instance.Authority.CreateAuthority(Authority, ValidateAuthority);
            return
                await
                    AcquireTokenCommonAsync(authority, scope, null, loginHint,
                        behavior, extraQueryParameters, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Interactive request to acquire token. 
        /// </summary>
        /// <param name="scope">Array of scopes requested for resource</param>
        /// <param name="user">User object to enforce the same user to be authenticated in the web UI.</param>
        /// <param name="behavior">Enumeration to control UI behavior.</param>
        /// <param name="extraQueryParameters">This parameter will be appended as is to the query string in the HTTP authentication request to the authority. The parameter can be null.</param>
        /// <returns>Authentication result containing token of the user</returns>
        public async Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scope, IUser user,
            UIBehavior behavior, string extraQueryParameters)
        {
            Authority authority = Internal.Instance.Authority.CreateAuthority(Authority, ValidateAuthority);
            return
                await
                    AcquireTokenCommonAsync(authority, scope, null, user, behavior,
                        extraQueryParameters, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Interactive request to acquire token. 
        /// </summary>
        /// <param name="scope">Array of scopes requested for resource</param>
        /// <param name="loginHint">Identifier of the user. Generally a UPN.</param>
        /// <param name="behavior">Enumeration to control UI behavior.</param>
        /// <param name="extraQueryParameters">This parameter will be appended as is to the query string in the HTTP authentication request to the authority. The parameter can be null.</param>
        /// <param name="additionalScope">Array of scopes for which a developer can request consent upfront.</param>
        /// <param name="authority">Specific authority for which the token is requested. Passing a different value than configured does not change the configured value</param>
        /// <returns>Authentication result containing token of the user</returns>
        public async Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scope, string loginHint,
            UIBehavior behavior, string extraQueryParameters, IEnumerable<string> additionalScope, string authority)
        {
            Authority authorityInstance = Internal.Instance.Authority.CreateAuthority(authority, ValidateAuthority);
            return
                await
                    AcquireTokenCommonAsync(authorityInstance, scope, additionalScope,
                        loginHint, behavior, extraQueryParameters, null).ConfigureAwait(false);
        }

        /// <summary>
        /// Interactive request to acquire token. 
        /// </summary>
        /// <param name="scope">Array of scopes requested for resource</param>
        /// <param name="user">User object to enforce the same user to be authenticated in the web UI.</param>
        /// <param name="behavior">Enumeration to control UI behavior.</param>
        /// <param name="extraQueryParameters">This parameter will be appended as is to the query string in the HTTP authentication request to the authority. The parameter can be null.</param>
        /// <param name="additionalScope">Array of scopes for which a developer can request consent upfront.</param>
        /// <param name="authority">Specific authority for which the token is requested. Passing a different value than configured does not change the configured value</param>
        /// <returns>Authentication result containing token of the user</returns>
        public async Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scope, IUser user,
            UIBehavior behavior, string extraQueryParameters, IEnumerable<string> additionalScope, string authority)
        {
            Authority authorityInstance = Internal.Instance.Authority.CreateAuthority(authority, ValidateAuthority);
            return
                await
                    AcquireTokenCommonAsync(authorityInstance, scope, additionalScope, user,
                        behavior, extraQueryParameters, null).ConfigureAwait(false);
        }
#endif
        

        /// <summary>
        /// Interactive request to acquire token. 
        /// </summary>
        /// <param name="scope">Array of scopes requested for resource</param>
        /// <returns>Authentication result containing token of the user</returns>
        public async Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scope, UIParent parent)
        {
            Authority authority = Internal.Instance.Authority.CreateAuthority(Authority, ValidateAuthority);
            return
                await
                    AcquireTokenCommonAsync(authority, scope, null, (string)null,
                        UIBehavior.SelectAccount, null, parent).ConfigureAwait(false);
        }

        /// <summary>
        /// Interactive request to acquire token. 
        /// </summary>
        /// <param name="scope">Array of scopes requested for resource</param>
        /// <param name="loginHint">Identifier of the user. Generally a UPN.</param>
        /// <returns>Authentication result containing token of the user</returns>
        public async Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scope, string loginHint, UIParent parent)
        {
            Authority authority = Internal.Instance.Authority.CreateAuthority(Authority, ValidateAuthority);
            return
                await
                    AcquireTokenCommonAsync(authority, scope, null, loginHint,
                        UIBehavior.SelectAccount, null, parent).ConfigureAwait(false);
        }

        /// <summary>
        /// Interactive request to acquire token. 
        /// </summary>
        /// <param name="scope">Array of scopes requested for resource</param>
        /// <param name="loginHint">Identifier of the user. Generally a UPN.</param>
        /// <param name="behavior">Enumeration to control UI behavior.</param>
        /// <param name="extraQueryParameters">This parameter will be appended as is to the query string in the HTTP authentication request to the authority. The parameter can be null.</param>
        /// <returns>Authentication result containing token of the user</returns>
        public async Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scope, string loginHint,
            UIBehavior behavior, string extraQueryParameters, UIParent parent)
        {
            Authority authority = Internal.Instance.Authority.CreateAuthority(Authority, ValidateAuthority);
            return
                await
                    AcquireTokenCommonAsync(authority, scope, null, loginHint,
                        behavior, extraQueryParameters, parent).ConfigureAwait(false);
        }

        /// <summary>
        /// Interactive request to acquire token. 
        /// </summary>
        /// <param name="scope">Array of scopes requested for resource</param>
        /// <param name="user">User object to enforce the same user to be authenticated in the web UI.</param>
        /// <param name="behavior">Enumeration to control UI behavior.</param>
        /// <param name="extraQueryParameters">This parameter will be appended as is to the query string in the HTTP authentication request to the authority. The parameter can be null.</param>
        /// <returns>Authentication result containing token of the user</returns>
        public async Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scope, IUser user,
            UIBehavior behavior, string extraQueryParameters, UIParent parent)
        {
            Authority authority = Internal.Instance.Authority.CreateAuthority(Authority, ValidateAuthority);
            return
                await
                    AcquireTokenCommonAsync(authority, scope, null, user, behavior,
                        extraQueryParameters, parent).ConfigureAwait(false);
        }

        /// <summary>
        /// Interactive request to acquire token. 
        /// </summary>
        /// <param name="scope">Array of scopes requested for resource</param>
        /// <param name="loginHint">Identifier of the user. Generally a UPN.</param>
        /// <param name="behavior">Enumeration to control UI behavior.</param>
        /// <param name="extraQueryParameters">This parameter will be appended as is to the query string in the HTTP authentication request to the authority. The parameter can be null.</param>
        /// <param name="additionalScope">Array of scopes for which a developer can request consent upfront.</param>
        /// <param name="authority">Specific authority for which the token is requested. Passing a different value than configured does not change the configured value</param>
        /// <returns>Authentication result containing token of the user</returns>
        public async Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scope, string loginHint,
            UIBehavior behavior, string extraQueryParameters, IEnumerable<string> additionalScope, string authority, UIParent parent)
        {
            Authority authorityInstance = Internal.Instance.Authority.CreateAuthority(authority, ValidateAuthority);
            return
                await
                    AcquireTokenCommonAsync(authorityInstance, scope, additionalScope,
                        loginHint, behavior, extraQueryParameters, parent).ConfigureAwait(false);
        }

        /// <summary>
        /// Interactive request to acquire token. 
        /// </summary>
        /// <param name="scope">Array of scopes requested for resource</param>
        /// <param name="user">User object to enforce the same user to be authenticated in the web UI.</param>
        /// <param name="behavior">Enumeration to control UI behavior.</param>
        /// <param name="extraQueryParameters">This parameter will be appended as is to the query string in the HTTP authentication request to the authority. The parameter can be null.</param>
        /// <param name="additionalScope">Array of scopes for which a developer can request consent upfront.</param>
        /// <param name="authority">Specific authority for which the token is requested. Passing a different value than configured does not change the configured value</param>
        /// <returns>Authentication result containing token of the user</returns>
        public async Task<AuthenticationResult> AcquireTokenAsync(IEnumerable<string> scope, IUser user,
            UIBehavior behavior, string extraQueryParameters, IEnumerable<string> additionalScope, string authority, UIParent parent)
        {
            Authority authorityInstance = Internal.Instance.Authority.CreateAuthority(authority, ValidateAuthority);
            return
                await
                    AcquireTokenCommonAsync(authorityInstance, scope, additionalScope, user,
                        behavior, extraQueryParameters, parent).ConfigureAwait(false);
        }



        internal IWebUI CreateWebAuthenticationDialog(UIParent parent, UIBehavior behavior, RequestContext requestContext)
        {
            //create instance of UIParent and assign useCorporateNetwork to UIParent 
            if (parent == null)
            {
                parent = new UIParent();
            }

#if WINRT || DESKTOP
            //hidden webview can be used in both WinRT and desktop applications.
            parent.UseHiddenBrowser = behavior.Equals(UIBehavior.Never);
#if WINRT
            parent.UseCorporateNetwork = UseCorporateNetwork;
#endif
#endif

            return PlatformPlugin.WebUIFactory.CreateAuthenticationDialog(parent, requestContext);
        }

        private async Task<AuthenticationResult> AcquireTokenCommonAsync(Authority authority, IEnumerable<string> scope,
            IEnumerable<string> additionalScope, string loginHint, UIBehavior behavior,
            string extraQueryParameters, UIParent parent)
        {
            var requestParams = CreateRequestParameters(authority, scope, null, UserTokenCache);
            requestParams.ExtraQueryParameters = extraQueryParameters;
            var handler =
                new InteractiveRequest(requestParams, additionalScope, loginHint, behavior,
                    CreateWebAuthenticationDialog(parent, behavior, requestParams.RequestContext));
            return await handler.RunAsync().ConfigureAwait(false);
        }

        private async Task<AuthenticationResult> AcquireTokenCommonAsync(Authority authority, IEnumerable<string> scope,
            IEnumerable<string> additionalScope, IUser user, UIBehavior behavior, string extraQueryParameters, UIParent parent)
        {

            var requestParams = CreateRequestParameters(authority, scope, user, UserTokenCache);
            requestParams.ExtraQueryParameters = extraQueryParameters;

            var handler =
                new InteractiveRequest(requestParams, additionalScope, behavior,
                    CreateWebAuthenticationDialog(parent, behavior, requestParams.RequestContext));
            return await handler.RunAsync().ConfigureAwait(false);
        }

        internal override AuthenticationRequestParameters CreateRequestParameters(Authority authority,
            IEnumerable<string> scope, IUser user, TokenCache cache)
        {
            AuthenticationRequestParameters parameters = base.CreateRequestParameters(authority, scope, user, cache);
            parameters.ClientId = ClientId;
            return parameters;
        }
    }
}