﻿// © 2022 Adrian Clark
// This file is licensed to you under the MIT license.

using System.Net;
using Aydsko.iRacingData.IntegrationTests.Member;
using Microsoft.Extensions.Configuration;

namespace Aydsko.iRacingData.IntegrationTests;

[Category("Integration")]
public class BaseIntegrationFixture : IDisposable
{
    private IConfigurationRoot _configuration;
    private CookieContainer _cookieContainer;
    private HttpClientHandler _handler;
    private HttpClient _httpClient;

    internal DataClient Client { get; private set; }
    protected IConfigurationRoot Configuration => _configuration;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _configuration = new ConfigurationBuilder()
                                .SetBasePath(TestContext.CurrentContext.TestDirectory)
                                .AddJsonFile("appsettings.json", false)
                                .AddUserSecrets(typeof(MemberInfoTest).Assembly)
                                .AddEnvironmentVariables("IRACINGDATA_")
                                .Build();

        _cookieContainer = new CookieContainer();
        _handler = new HttpClientHandler()
        {
            CookieContainer = _cookieContainer,
            UseCookies = true,
            CheckCertificateRevocationList = true
        };
        _httpClient = new HttpClient(_handler);

        var options = new iRacingDataClientOptions
        {
            UserAgentProductName = "Aydsko.iRacingData.IntegrationTests",
            UserAgentProductVersion = typeof(MemberInfoTest).Assembly.GetName().Version,
            Username = _configuration["iRacingData:Username"],
            Password = _configuration["iRacingData:Password"]
        };

        Client = new DataClient(_httpClient, new TestLogger<DataClient>(), options, _cookieContainer);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            (_httpClient as IDisposable)?.Dispose();
            _httpClient = null!;
            (_handler as IDisposable)?.Dispose();
            _handler = null!;
        }
    }
}