// ReSharper disable CheckNamespace

using System;
using System.Net.Http;
using EventStore.Client;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

#nullable enable
namespace Microsoft.Extensions.DependencyInjection {
	/// <summary>
	/// A set of extension methods for <see cref="IServiceCollection"/> which provide support for an <see cref="EventStoreProjectionManagementClient"/>.
	/// </summary>
	public static class EventStoreProjectionManagementClientCollectionExtensions {
#if GRPC_CORE
		/// <summary>
		/// Adds an <see cref="EventStoreProjectionManagementClient"/> to the <see cref="IServiceCollection"/>.
		/// </summary>
		/// <param name="services"></param>
		/// <param name="address"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static IServiceCollection AddEventStoreProjectionManagementClient(this IServiceCollection services,
			Uri address)
			=> services.AddEventStoreProjectionManagementClient(options => {
				options.ConnectivitySettings.Address = address;
			});
#else
		/// <summary>
		/// Adds an <see cref="EventStoreProjectionManagementClient"/> to the <see cref="IServiceCollection"/>.
		/// </summary>
		/// <param name="services"></param>
		/// <param name="address"></param>
		/// <param name="createHttpMessageHandler"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static IServiceCollection AddEventStoreProjectionManagementClient(this IServiceCollection services,
			Uri address,
			Func<HttpMessageHandler>? createHttpMessageHandler = null)
			=> services.AddEventStoreProjectionManagementClient(options => {
				options.ConnectivitySettings.Address = address;
				options.CreateHttpMessageHandler = createHttpMessageHandler;
			});
#endif

#if NETCOREAPP3_1
		/// <summary>
		/// Adds an <see cref="EventStoreProjectionManagementClient"/> to the <see cref="IServiceCollection"/>.
		/// </summary>
		/// <param name="services"></param>
		/// <param name="address"></param>
		/// <param name="createHttpMessageHandler"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		[Obsolete]
		public static IServiceCollection AddEventStoreProjectionManagementClient(this IServiceCollection services,
			// ReSharper disable once MethodOverloadWithOptionalParameter
			Uri address, Func<HttpMessageHandler>? createHttpMessageHandler = null)
			=> services.AddEventStoreProjectionManagementClient(options => {
				options.ConnectivitySettings.Address = address;
				options.CreateHttpMessageHandler = createHttpMessageHandler;
			});
#endif
		/// <summary>
		/// Adds an <see cref="EventStoreProjectionManagementClient"/> to the <see cref="IServiceCollection"/>.
		/// </summary>
		/// <param name="services"></param>
		/// <param name="configureSettings"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static IServiceCollection AddEventStoreProjectionManagementClient(this IServiceCollection services,
			Action<EventStoreClientSettings>? configureSettings = null) =>
			services.AddEventStoreProjectionManagementClient(new EventStoreClientSettings(), configureSettings);

		/// <summary>
		/// Adds an <see cref="EventStoreProjectionManagementClient"/> to the <see cref="IServiceCollection"/>.
		/// </summary>
		/// <param name="services"></param>
		/// <param name="connectionString"></param>
		/// <param name="configureSettings"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static IServiceCollection AddEventStoreProjectionManagementClient(this IServiceCollection services,
			string connectionString, Action<EventStoreClientSettings>? configureSettings = null) =>
			services.AddEventStoreProjectionManagementClient(EventStoreClientSettings.Create(connectionString),
				configureSettings);

		private static IServiceCollection AddEventStoreProjectionManagementClient(this IServiceCollection services,
			EventStoreClientSettings settings, Action<EventStoreClientSettings>? configureSettings) {
			if (services == null) {
				throw new ArgumentNullException(nameof(services));
			}

			configureSettings?.Invoke(settings);

			services.TryAddSingleton(provider => {
				settings.LoggerFactory ??= provider.GetService<ILoggerFactory>();
				settings.Interceptors ??= provider.GetServices<Interceptor>();

				return new EventStoreProjectionManagementClient(settings);
			});

			return services;
		}
	}
}
// ReSharper restore CheckNamespace
