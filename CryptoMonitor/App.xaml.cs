﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;
using CryptoMonitor.ViewModels;
using CryptoMonitor.Services;

namespace CryptoMonitor
{
    public partial class App : Application
    {
        private readonly IHost host;
        public App()
        {
            host = Host.CreateDefaultBuilder()
                   .ConfigureServices((context, services) =>
                   {
                       ConfigureServices(context.Configuration, services);
                   })
                   .Build();
        }

        private void ConfigureServices(IConfiguration configuration,
            IServiceCollection services)
        {
            services.AddSingleton<Func<Type, ViewModelBase>>(provider => 
                viewModelType => (ViewModelBase)provider.GetRequiredService(viewModelType));
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<HomePageViewModel>();
            services.AddTransient<ErrorPageViewModel>();
            services.AddScoped<CoinDataViewModel>();
            services.AddSingleton<ScrollService>();
            services.AddSingleton<CoinGeckoApiService>();
            services.AddSingleton(provider => 
            new MainWindow 
            { 
                DataContext = provider.GetRequiredService<MainWindowViewModel>() 
            });
            services.AddSingleton<Services.INavigationService, Services.NavigationService>();
            services.AddSingleton<IThemeService,ThemeService>();
            services.AddSingleton<ErrorService>();
            services.AddSingleton<CoinCapApiService>();
            services.AddHttpClient();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await host.StartAsync();

            var mainWindow = host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (host)
            {
                await host.StopAsync(TimeSpan.FromSeconds(5));
            }

            base.OnExit(e);
        }
    }
}
