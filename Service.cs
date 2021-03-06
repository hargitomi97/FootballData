﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FootballData
{
    public class Service
    {
        private readonly Uri serverUrl = new Uri("https://api-football-v1.p.rapidapi.com");

        /// <summary>
        /// A method which calls GET and deserializes result from JSON. Handles exceptions
        /// </summary>
        /// <typeparam name="T">Type of the returned result.</typeparam>
        /// <param name="uri">The uri to fetch from. </param>
        /// <returns>The needed data or null</returns>
        private T GetDataFromFootballApi<T>(Uri uri) where T : class
        {
            var client = new RestClient(uri);
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "api-football-v1.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "acd21cc6femsh5d6f180530ebd38p1b1101jsnf5eb2e9a6be5");
            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var json = response.Content;
                T r = JsonConvert.DeserializeObject<T>(json);
                return r;
            }
            else
            {
                return null;
            }
        }

        private T GetDataFromWikidata<T>(Uri uri) where T : class
        {
            var client = new RestClient(uri);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var json = response.Content;
                T r = JsonConvert.DeserializeObject<T>(json);/*, new JsonSerializerSettings()
                {
                    ContractResolver = new CustomResolver()
                }
                );*/
                return r;
            }
            else
            {
                return null;
            }
        }

        public dynamic GetIdentifiers(string id)
        {
            return GetDataFromWikidata<dynamic>(new Uri("http://www.wikidata.org/entity/{id}.json"));
        }

        public dynamic GetEntityNames(string allKeysSeparated = "")
        {
            return GetDataFromWikidata<dynamic>(new Uri($"https://www.wikidata.org/w/api.php?action=wbgetentities&ids={allKeysSeparated}&format=json&props=labels&languages=en"));
        }

        

        public dynamic GetEntityClaims(string id, string propertyId)
        {
            return GetDataFromFootballApi<dynamic>(new Uri($"https://www.wikidata.org/w/api.php?action=wbgetclaims&entity={id}&property={propertyId}&format=json"));
        }
        /// <summary>
        /// Get Teams from Football API
        /// </summary>
        /// <returns></returns>
        public RootTeamObject GetTeams(int league_id)
        {
            return GetDataFromFootballApi<RootTeamObject>(new Uri(serverUrl, $"/v2/teams/league/{league_id}"));
        }

        

        public RootFixtureObject GetFixtures(int league_id)
        {
            return GetDataFromFootballApi<RootFixtureObject>(new Uri(serverUrl, $"/v2/fixtures/league/{league_id}"));
        }

        

        public RootLeagueObject GetLeagues(int league_id)
        {
            return GetDataFromFootballApi<RootLeagueObject>(new Uri(serverUrl, $"/v2/leagues/league/{league_id}/"));
        }
        public RootLeagueObject FindLeagueByIdAndSeason(string league_name)
        {
            return GetDataFromFootballApi<RootLeagueObject>(new Uri(serverUrl, $"/v2/leagues/search/{league_name}"));
        }

        public RootLeagueObject GetLeaguesFromSeason(string season)
        {
            return GetDataFromFootballApi<RootLeagueObject>(new Uri(serverUrl, $"/v2/leagues/season/{season}"));
        }

        public RootLeagueObject GetSeasonsFromLeague(int league_id)
        {
            return GetDataFromFootballApi<RootLeagueObject>(new Uri(serverUrl, $"/v2/leagues/seasonsavailable/{league_id}"));
        }
    }
}
