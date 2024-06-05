using MudBlazor;
using System.Reflection;
using System.Runtime.Serialization;

namespace MudBlazorCrmApp.Shared.Blazor;

internal static class ODataHelpers
{
    private static string GetDataMemberName<T>(string propertyName)
    {
        return (typeof(T)
            .GetProperty(propertyName)?
            .GetCustomAttribute<DataMemberAttribute>()?
            .Name
            ?? propertyName)
            .Replace(".", "/");
    }

    public static string? GetOrderBy<T>(ICollection<SortDefinition<T>> sortDefinitions)
    {
        if (!sortDefinitions.Any())
        {
            return null;
        }

        return string.Join(", ", sortDefinitions.Select(x => $"{GetDataMemberName<T>(x.SortBy)} {(x.Descending ? "desc" : "asc")}"));
    }

    public static string? GetFilter<T>(ICollection<IFilterDefinition<T>> filterDefinitions)
    {
        List<string> expressions = [];

        foreach (var filterDefinition in filterDefinitions)
        {
            var fieldType = filterDefinition.FieldType;
            var op = filterDefinition.Operator;
            var property = GetDataMemberName<T>(filterDefinition.Column!.PropertyName);
            var value = filterDefinition.Value;

            string? exp = null;

            if (fieldType.IsString)
            {
                var valueString = value == null ? "''" : $"'{value}'";

                exp = op switch
                {
                    FilterOperator.String.Contains => $"contains({property}, {valueString})",
                    FilterOperator.String.NotContains => $"not(contains({property}, {valueString}))",
                    FilterOperator.String.Equal => $"{property} eq {valueString}",
                    FilterOperator.String.NotEqual => $"{property} ne {valueString}",
                    FilterOperator.String.StartsWith => $"startsWith({property}, {valueString})",
                    FilterOperator.String.EndsWith => $"endsWith({property}, {valueString})",
                    FilterOperator.String.Empty => $"({property} eq null or {property} eq '')",
                    FilterOperator.String.NotEmpty => $"({property} ne null and {property} ne '')",
                    _ => null
                };
            }
            else if (fieldType.IsNumber)
            {
                if (op != FilterOperator.Number.Empty && op != FilterOperator.Number.NotEmpty && value == null)
                {
                    continue;
                }

                exp = op switch
                {
                    FilterOperator.Number.Equal => $"{property} eq {value}",
                    FilterOperator.Number.NotEqual => $"{property} ne {value}",
                    FilterOperator.Number.GreaterThan => $"{property} gt {value}",
                    FilterOperator.Number.GreaterThanOrEqual => $"{property} ge {value}",
                    FilterOperator.Number.LessThan => $"{property} lt {value}",
                    FilterOperator.Number.LessThanOrEqual => $"{property} le {value}",
                    FilterOperator.Number.Empty => $"{property} eq null",
                    FilterOperator.Number.NotEmpty => $"{property} ne null",
                    _ => null
                };
            }
            else if (fieldType.IsEnum)
            {
                exp = op switch
                {
                    FilterOperator.Enum.Is => $"{property} eq {value}",
                    FilterOperator.Enum.IsNot => $"{property} ne {value}",
                    _ => null
                };
            }
            else if (fieldType.IsBoolean)
            {
                exp = op switch
                {
                    FilterOperator.Boolean.Is => $"{property} eq {value}",
                    _ => null
                };
            }
            else if (fieldType.IsDateTime)
            {
                if (op != FilterOperator.DateTime.Empty && op != FilterOperator.DateTime.NotEmpty && value == null)
                {
                    continue;
                }

                DateTimeOffset dto;

                if (value is DateTimeOffset dtoValue)
                {
                    dto = dtoValue;
                }
                else if (value is DateTime dtValue)
                {
                    dto = new(dtValue);
                }
                else
                {
                    continue;
                }

                exp = op switch
                {
                    FilterOperator.DateTime.Is => $"{property} eq {dto:o}",
                    FilterOperator.DateTime.IsNot => $"{property} ne {dto:o}",
                    FilterOperator.DateTime.After => $"{property} gt {dto:o}",
                    FilterOperator.DateTime.OnOrAfter => $"{property} ge {dto:o}",
                    FilterOperator.DateTime.Before => $"{property} lt {dto:o}",
                    FilterOperator.DateTime.OnOrBefore => $"{property} le {dto:o}",
                    FilterOperator.DateTime.Empty => $"{property} eq null",
                    FilterOperator.DateTime.NotEmpty => $"{property} ne null",
                    _ => null
                };
            }
            else if (fieldType.IsGuid)
            {
                var valueString = value == null ? "null" : value.ToString();

                exp = op switch
                {
                    FilterOperator.Guid.Equal => $"{property} eq {valueString}",
                    FilterOperator.Guid.NotEqual => $"{property} ne {valueString}",
                    _ => null
                };
            }

            if (exp == null)
            {
                continue;
            }

            expressions.Add(exp);
        }

        if (!expressions.Any())
        {
            return null;
        }

        return string.Join(" and ", expressions);
    }
}
