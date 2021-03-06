﻿using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using YoutubeExplode.Models.MediaStreams;

namespace YTApp.Classes
{
    static class YoutubeMethodsStatic
    {
        static async public Task<YouTubeService> GetServiceAsync()
        {
            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets
            {
                ClientId = Constants.ClientID,
                ClientSecret = Constants.ClientSecret
            }, new[] { Google.Apis.Oauth2.v2.Oauth2Service.Scope.UserinfoProfile, YouTubeService.Scope.YoutubeForceSsl }, "user", CancellationToken.None);
        
            // Create the service.
            return new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Unofficial Youtube Client",
            });
        }

        static public YouTubeService GetServiceNoAuth()
        {
            return new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = Constants.ApiKey,
                ApplicationName = "Unofficial Youtube Client"
            });
        }

        static public async Task<bool> IsUserAuthenticated()
        {
            GoogleAuthorizationCodeFlow.Initializer initializer = new GoogleAuthorizationCodeFlow.Initializer();
            var secrets = new ClientSecrets
            {
                ClientSecret = Constants.ClientSecret,
                ClientId = Constants.ClientID
            };
            initializer.ClientSecrets = secrets;
            initializer.DataStore = new PasswordVaultDataStore();
            var test = new AuthorizationCodeFlow(initializer);
            var token = await test.LoadTokenAsync("user", CancellationToken.None);
            if (token == null)
            {
                return false;
            }
            else
            {
                Constants.Token = token;
                return true;
            }
        }

        static public string ViewCountShortner(long viewCount, int decimals = 1)
        {
            if (viewCount > 1000000000)
                return Convert.ToString(Math.Round(Convert.ToDouble(viewCount) / 1000000000, decimals)) + "B";
            else if (viewCount > 1000000)
                return Convert.ToString(Math.Round(Convert.ToDouble(viewCount) / 1000000, decimals)) + "M";
            else if (viewCount > 1000)
                return Convert.ToString(Math.Round(Convert.ToDouble(viewCount) / 1000, decimals)) + "K";
            else
                return Convert.ToString(viewCount);
        }

        static public string ViewCountShortner(ulong? viewCount, int decimals = 1)
        {
            if (viewCount > 1000000000)
                return Convert.ToString(Math.Round(Convert.ToDouble(viewCount) / 1000000000, decimals)) + "B";
            else if (viewCount > 1000000)
                return Convert.ToString(Math.Round(Convert.ToDouble(viewCount) / 1000000, decimals)) + "M";
            else if (viewCount > 1000)
                return Convert.ToString(Math.Round(Convert.ToDouble(viewCount) / 1000, decimals)) + "K";
            else
                return Convert.ToString(viewCount);
        }

        static public string GetVideoQuality(VideoQuality quality, bool GetMuxed)
        {
            if (GetMuxed)
            {
                foreach (var video in Constants.videoInfo.Muxed)
                {
                    if (video.VideoQuality == quality)
                        return video.Url;
                }
            }
            else
            {
                foreach (var video in Constants.videoInfo.Video)
                {
                    if (video.VideoQuality == quality)
                        return video.Url;
                }
            }

            return null;
        }

        static public List<VideoQuality> GetVideoQualityList()
        {
            var qualitiesString = Constants.videoInfo.GetAllVideoQualities().ToList();
            qualitiesString.Sort();
            return qualitiesString;
        }


    }
}
