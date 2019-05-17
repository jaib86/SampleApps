﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BathanysPiShop.Services;
using BathanysPiShop.Views;

namespace BathanysPiShop
{
    public partial class App : Application
    {
        //TODO: Replace with *.azurewebsites.net url after deploying backend to Azure
        public static string AzureBackendUrl = "http://localhost:5000";
        public static bool UseMockDataStore = true;

        public App()
        {
            InitializeComponent();

            if (UseMockDataStore)
                DependencyService.Register<MockDataStore>();
            else
                DependencyService.Register<AzureDataStore>();
            //MainPage = new AppShell();
            //MainPage = new ContentPageView();
            //MainPage = new NavigationPage(new NavigationPageView());
            //MainPage = new TabbedPageView();
            //MainPage = new LoginPageView();

            //MainPage = new LoginView();
            //MainPage = new RegisterView();
            MainPage = new PieOverview();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
