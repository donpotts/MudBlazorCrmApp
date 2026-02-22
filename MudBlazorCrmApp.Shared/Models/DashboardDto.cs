using System.Runtime.Serialization;

namespace MudBlazorCrmApp.Shared.Models;

[DataContract]
public class KpiSummaryDto
{
    [DataMember] public int NewLeads { get; set; }
    [DataMember] public decimal TotalRevenue { get; set; }
    [DataMember] public double ConversionRate { get; set; }
    [DataMember] public decimal PipelineValue { get; set; }
    [DataMember] public int DealsClosed { get; set; }
    [DataMember] public decimal AvgDealSize { get; set; }
    [DataMember] public int TotalCustomers { get; set; }
    [DataMember] public int OpenSupportCases { get; set; }
}

[DataContract]
public class SalesOverTimeDto
{
    [DataMember] public string? Month { get; set; }
    [DataMember] public decimal Total { get; set; }
    [DataMember] public int Count { get; set; }
}

[DataContract]
public class LeadSourceDto
{
    [DataMember] public string? Source { get; set; }
    [DataMember] public int Count { get; set; }
}

[DataContract]
public class PipelineStageDto
{
    [DataMember] public string? Stage { get; set; }
    [DataMember] public int Count { get; set; }
    [DataMember] public decimal TotalValue { get; set; }
}

[DataContract]
public class RecentActivityDto
{
    [DataMember] public long? Id { get; set; }
    [DataMember] public string? Type { get; set; }
    [DataMember] public string? Subject { get; set; }
    [DataMember] public string? EntityName { get; set; }
    [DataMember] public DateTime? Date { get; set; }
}

[DataContract]
public class TopOpportunityDto
{
    [DataMember] public long? Id { get; set; }
    [DataMember] public string? Name { get; set; }
    [DataMember] public decimal? Value { get; set; }
    [DataMember] public string? Stage { get; set; }
    [DataMember] public double? Probability { get; set; }
    [DataMember] public string? CustomerName { get; set; }
}

[DataContract]
public class TimelineEventDto
{
    [DataMember] public long? Id { get; set; }
    [DataMember] public string? EventType { get; set; }
    [DataMember] public string? Title { get; set; }
    [DataMember] public string? Description { get; set; }
    [DataMember] public DateTime? Date { get; set; }
    [DataMember] public string? Icon { get; set; }
}

[DataContract]
public class SearchResultDto
{
    [DataMember] public List<SearchResultItem>? Customers { get; set; }
    [DataMember] public List<SearchResultItem>? Contacts { get; set; }
    [DataMember] public List<SearchResultItem>? Leads { get; set; }
    [DataMember] public List<SearchResultItem>? Opportunities { get; set; }
}

[DataContract]
public class SearchResultItem
{
    [DataMember] public long? Id { get; set; }
    [DataMember] public string? Name { get; set; }
    [DataMember] public string? EntityType { get; set; }
    [DataMember] public string? Detail { get; set; }
}

[DataContract]
public class ActivityReportDto
{
    [DataMember] public int Total { get; set; }
    [DataMember] public List<ActivityTypeCount>? ByType { get; set; }
    [DataMember] public List<ActivityStatusCount>? ByStatus { get; set; }
}

[DataContract]
public class ActivityTypeCount
{
    [DataMember] public string? Type { get; set; }
    [DataMember] public int Count { get; set; }
}

[DataContract]
public class ActivityStatusCount
{
    [DataMember] public string? Status { get; set; }
    [DataMember] public int Count { get; set; }
}
