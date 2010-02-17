﻿//-----------------------------------------------------------------------
// <copyright file="OpenIdRelyingPartyElement.cs" company="Andrew Arnott">
//     Copyright (c) Andrew Arnott. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace DotNetOpenAuth.Configuration {
	using System;
	using System.Configuration;
	using System.Diagnostics.Contracts;
	using DotNetOpenAuth.OpenId;
	using DotNetOpenAuth.OpenId.RelyingParty;

	/// <summary>
	/// The section in the .config file that allows customization of OpenID Relying Party behaviors.
	/// </summary>
	[ContractVerification(true)]
	internal class OpenIdRelyingPartyElement : ConfigurationElement {
		/// <summary>
		/// The name of the custom store sub-element.
		/// </summary>
		private const string StoreConfigName = "store";

		/// <summary>
		/// Gets the name of the security sub-element.
		/// </summary>
		private const string SecuritySettingsConfigName = "security";

		/// <summary>
		/// The name of the &lt;behaviors&gt; sub-element.
		/// </summary>
		private const string BehaviorsElementName = "behaviors";

		/// <summary>
		/// The name of the &lt;discoveryServices&gt; sub-element.
		/// </summary>
		private const string DiscoveryServicesElementName = "discoveryServices";

		/// <summary>
		/// The built-in set of identifier discovery services.
		/// </summary>
		private static readonly TypeConfigurationCollection<IIdentifierDiscoveryService> defaultDiscoveryServices = new TypeConfigurationCollection<IIdentifierDiscoveryService>(new Type[] { typeof(UriDiscoveryService), typeof(XriDiscoveryProxyService) });

		/// <summary>
		/// Initializes a new instance of the <see cref="OpenIdRelyingPartyElement"/> class.
		/// </summary>
		public OpenIdRelyingPartyElement() {
		}

		/// <summary>
		/// Gets or sets the security settings.
		/// </summary>
		[ConfigurationProperty(SecuritySettingsConfigName)]
		public OpenIdRelyingPartySecuritySettingsElement SecuritySettings {
			get { return (OpenIdRelyingPartySecuritySettingsElement)this[SecuritySettingsConfigName] ?? new OpenIdRelyingPartySecuritySettingsElement(); }
			set { this[SecuritySettingsConfigName] = value; }
		}

		/// <summary>
		/// Gets or sets the special behaviors to apply.
		/// </summary>
		[ConfigurationProperty(BehaviorsElementName, IsDefaultCollection = false)]
		[ConfigurationCollection(typeof(TypeConfigurationCollection<IRelyingPartyBehavior>))]
		public TypeConfigurationCollection<IRelyingPartyBehavior> Behaviors {
			get { return (TypeConfigurationCollection<IRelyingPartyBehavior>)this[BehaviorsElementName] ?? new TypeConfigurationCollection<IRelyingPartyBehavior>(); }
			set { this[BehaviorsElementName] = value; }
		}

		/// <summary>
		/// Gets or sets the type to use for storing application state.
		/// </summary>
		[ConfigurationProperty(StoreConfigName)]
		public TypeConfigurationElement<IRelyingPartyApplicationStore> ApplicationStore {
			get { return (TypeConfigurationElement<IRelyingPartyApplicationStore>)this[StoreConfigName] ?? new TypeConfigurationElement<IRelyingPartyApplicationStore>(); }
			set { this[StoreConfigName] = value; }
		}

		/// <summary>
		/// Gets or sets the services to use for discovering service endpoints for identifiers.
		/// </summary>
		/// <remarks>
		/// If no discovery services are defined in the (web) application's .config file,
		/// the default set of discovery services built into the library are used.
		/// </remarks>
		[ConfigurationProperty(DiscoveryServicesElementName, IsDefaultCollection = false)]
		[ConfigurationCollection(typeof(TypeConfigurationCollection<IIdentifierDiscoveryService>))]
		internal TypeConfigurationCollection<IIdentifierDiscoveryService> DiscoveryServices {
			get {
				var configResult = (TypeConfigurationCollection<IIdentifierDiscoveryService>)this[DiscoveryServicesElementName];
				return configResult != null && configResult.Count > 0 ? configResult : defaultDiscoveryServices;
			}

			set {
				this[DiscoveryServicesElementName] = value;
			}
		}
	}
}
