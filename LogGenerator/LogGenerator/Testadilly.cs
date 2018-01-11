// This code requires the Nuget package Microsoft.AspNet.WebApi.Client to be installed.
// Instructions for doing this in Visual Studio:
// Tools -> Nuget Package Manager -> Package Manager Console
// Install-Package Microsoft.AspNet.WebApi.Client

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LogGenerator
{
    public class Testadilly
    {
        public static async Task InvokeRequestResponseService()
        {
            using (var client = new HttpClient())
            {
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
                                        "User", "Dr. Curly Howard"
                                    },
                                    {
                                        "Role", "Doctor"
                                    },
                                    {
                                        "MandatoryAccess", "Update::ePHI"
                                    },
                                    {
                                        "DiscretionaryAccess", "Patient Appointment"
                                    },
                                    {
                                        "IpAddress", "10.10.5.1"
                                    },
                                    {
                                        "DayOfWeek", "3"
                                    },
                                    {
                                        "TicksSinceLastRequestByUser", "864000000000"
                                    },
                                    {
                                        "RequestsByUserInLastMinute", "0"
                                    },
                                    {
                                        "RequestsByUserOfSameMandatoryInLastMinute", "0"
                                    },
                                    {
                                        "RequestsByUserOfSameDiscretionaryInLastMinute", "0"
                                    },
                                    {
                                        "RequestsByUserInLastTenMinutes", "0"
                                    },
                                    {
                                        "RequestsByUserOfSameMandatoryInLastTenMinutes", "0"
                                    },
                                    {
                                        "RequestsByUserOfSameDiscretionaryInLastTenMinutes", "0"
                                    },
                                    {
                                        "RequestsByUserInLastHour", "0"
                                    },
                                    {
                                        "RequestsByUserOfSameMandatoryInLastHour", "0"
                                    },
                                    {
                                        "RequestsByUserOfSameDiscretionaryInLastHour", "0"
                                    },
                                    {
                                        "RequestsByUserInLastDay", "121"
                                    },
                                    {
                                        "RequestsByUserOfSameMandatoryInLastDay", "0"
                                    },
                                    {
                                        "RequestsByUserOfSameDiscretionaryInLastDay", "120"
                                    },
                                    {
                                        "RequestsByAllInLastMinute", "0"
                                    },
                                    {
                                        "RequestsByAllOfSameMandatoryInLastMinute", "0"
                                    },
                                    {
                                        "RequestsByAllOfSameDiscretionaryInLastMinute", "0"
                                    },
                                    {
                                        "RequestsByAllInLastTenMinutes", "0"
                                    },
                                    {
                                        "RequestsByAllOfSameMandatoryInLastTenMinutes", "0"
                                    },
                                    {
                                        "RequestsByAllOfSameDiscretionaryInLastTenMinutes", "0"
                                    },
                                    {
                                        "RequestsByAllInLastHour", "0"
                                    },
                                    {
                                        "RequestsByAllOfSameMandatoryInLastHour", "0"
                                    },
                                    {
                                        "RequestsByAllOfSameDiscretionaryInLastHour", "0"
                                    },
                                    {
                                        "RequestsByAllInLastDay", "347"
                                    },
                                    {
                                        "RequestsByAllOfSameMandatoryInLastDay", "0"
                                    },
                                    {
                                        "RequestsByAllOfSameDiscretionaryInLastDay", "346"
                                    }
                                }
                            }
                        }
                    },
                    GlobalParameters = new Dictionary<string, string>()
                };

                const string apiKey = "+w+coB/P5d6sdZ5EEte/ggvT3R2sXCdMfk/JUTLma+aDB7ao3LUtLvfWRy9EXEVmux5ZF/clNZCXXMp5mzGGRg=="; // Replace this with the API key for the web service
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                client.BaseAddress =
                    new Uri(
                        "https://ussouthcentral.services.azureml.net/workspaces/95a1b35dc5a9468790f8c9f3f8e25f53/services/0cdb9f823886496c8c710ba6ce36e3f5/execute?api-version=2.0&format=swagger");

                // WARNING: The 'await' statement below can result in a deadlock
                // if you are calling this code from the UI thread of an ASP.Net application.
                // One way to address this would be to call ConfigureAwait(false)
                // so that the execution does not attempt to resume on the original context.
                // For instance, replace code such as:
                //      result = await DoSomeTask()
                // with the following:
                //      result = await DoSomeTask().ConfigureAwait(false)

                var response = await client.PostAsJsonAsync("", scoreRequest);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Result: {0}", result);
                }
                else
                {
                    Console.WriteLine("The request failed with status code: {0}", response.StatusCode);

                    // Print the headers - they include the requert ID and the timestamp,
                    // which are useful for debugging the failure
                    Console.WriteLine(response.Headers.ToString());

                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                }
            }
        }
    }
}