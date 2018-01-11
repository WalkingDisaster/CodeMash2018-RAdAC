using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LittleIdea.Radac.Services
{
    public class RiskService : IRiskService
    {
        private readonly string _url;
        private readonly string _apiKey;
        private static readonly Dictionary<string, DateTime> LastAccessed = new Dictionary<string, DateTime>();

        private static readonly ConcurrentQueue<(string user, DateTime utcTime)> AccessLog =
            new ConcurrentQueue<(string user, DateTime utcTime)>();

        public RiskService(string url, string apiKey)
        {
            _url = url;
            _apiKey = apiKey;
        }

        public async Task<double> DetermineRisk(
            string user,
            string role,
            string mandatory,
            string discretionary
        )
        {
            using (var client = new HttpClient())
            {
                var now = DateTime.UtcNow;
                AccessLog.Enqueue((user, now));
                var secondsSinceLastAccessed = 0;
                if (LastAccessed.ContainsKey(user))
                {
                    var lastAccessed = LastAccessed[user];
                    secondsSinceLastAccessed = (int) now.Subtract(lastAccessed).TotalSeconds;
                }

                LastAccessed[user] = now;

                while (AccessLog.Count > 0 && AccessLog.TryPeek(out var result) &&
                       result.utcTime <= now.AddMinutes(-1)) AccessLog.TryDequeue(out var dequeued);

                var userRequestsInLastMinute = AccessLog.Count(x => x.user == user);
                var allRequestsInLastMinute = AccessLog.Count;

                var scoreRequest = new
                {
                    Inputs = new Dictionary<string, List<Dictionary<string, string>>>
                    {
                        {
                            "input1",
                            new List<Dictionary<string, string>>
                            {
                                new Dictionary<string, string>
                                {
                                    {
                                        "Label", "1"
                                    },
                                    {
                                        "User", user
                                    },
                                    {
                                        "Role", role
                                    },
                                    {
                                        "MandatoryAccess", mandatory
                                    },
                                    {
                                        "DiscretionaryAccess", discretionary
                                    },
                                    {
                                        "IpAddress", "10.10.5.3"
                                    },
                                    {
                                        "DayOfWeek", "4"
                                    },
                                    {
                                        "DayOfMonth", "11"
                                    },
                                    {
                                        "HourOfDay", "15"
                                    },
                                    {
                                        "IsWeekend", "false"
                                    },
                                    {
                                        "IsHoliday", "false"
                                    },
                                    {
                                        "UserSecondsSinceLastRequest",
                                        secondsSinceLastAccessed.ToString(CultureInfo.InvariantCulture)
                                    },
                                    {
                                        "UserRequestsInLastMinute", $"{userRequestsInLastMinute}"
                                    },
                                    {
                                        "UserRequestsInLastTenMinutes", $"{userRequestsInLastMinute + 1}"
                                    },
                                    {
                                        "AllRequestsInLastMinute", $"{allRequestsInLastMinute}"
                                    },
                                    {
                                        "AllRequestsInLastHour", $"{allRequestsInLastMinute + 43}"
                                    },
                                    {
                                        "AllRequestsInLastDay", $"{allRequestsInLastMinute + 350}"
                                    }
                                }
                            }
                        }
                    },
                    GlobalParameters = new Dictionary<string, string>()
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
                client.BaseAddress = new Uri(_url);
                var response = await client.PostAsJsonAsync("", scoreRequest);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var deserialized = JsonConvert.DeserializeObject<RootObject>(result);
                    if (deserialized.Results.output1.Any())
                    {
                        var mlResult = deserialized.Results.output1.First();
                        return mlResult.Risk;
                    }
                }

                return 1;
            }
        }
    }

    public class Output1
    {
        [JsonProperty(PropertyName = "Scored Probabilities")]
        public double Risk { get; set; }
    }

    public class Results
    {
        public List<Output1> output1 { get; set; }
    }

    public class RootObject
    {
        public Results Results { get; set; }
    }

}