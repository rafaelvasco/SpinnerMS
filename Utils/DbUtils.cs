using System;
using System.Text.RegularExpressions;
using Microsoft.Azure.Cosmos;

namespace SpinnerMS.Utils
{
    public static class DbUtils
    {
        public static QueryDefinition BuildQueryDefinition(string query_str, params object[] query_params)
        {
            var query_def = new QueryDefinition(query_str);

            if (query_params != null && query_params.Length > 0)
            {
                var param_names = new string[query_params.Length];

                var param_name_match = new Regex(@"@\w+").Match(query_str);

                if (!param_name_match.Success)
                {
                    throw new Exception("Invalid query str: No params");
                }

                if (param_name_match.Groups.Count != param_names.Length)
                {
                    throw new Exception("Invalid query str: Unmatched params names and param values");
                }

                int idx = 0;

                while (param_name_match.Success)
                {
                    var param_name = param_name_match.Groups[0];

                    query_def = query_def.WithParameter(param_name.Value, query_params[idx++]);

                    param_name_match = param_name_match.NextMatch();
                }
            }

            return query_def;
        }
    }
}
