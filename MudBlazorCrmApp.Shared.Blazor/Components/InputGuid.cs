using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace MudBlazorCrmApp.Shared.Blazor.Components;

public class InputGuid : InputBase<Guid?>
{
    [DisallowNull] public ElementReference? Element { get; protected set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "input");
        builder.AddMultipleAttributes(1, AdditionalAttributes);
        builder.AddAttributeIfNotNullOrEmpty(2, "class", CssClass);
        builder.AddAttribute(3, "value", BindConverter.FormatValue(CurrentValue));
        builder.AddAttribute(4, "onchange", EventCallback.Factory
            .CreateBinder<string?>(this, __value => CurrentValueAsString = __value, CurrentValueAsString));
        builder.AddElementReferenceCapture(5, __inputReference => Element = __inputReference);
        builder.CloseElement();
    }

    protected override bool TryParseValueFromString(
        string? value,
        out Guid? result,
        [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (value == null)
        {
            result = null;
            validationErrorMessage = null;
            return true;
        }
        else if (Guid.TryParse(value, out Guid guid))
        {
            result = guid;
            validationErrorMessage = null;
            return true;
        }
        else
        {
            result = null;
            validationErrorMessage
                = $"The {DisplayName ?? FieldIdentifier.FieldName} field must be a GUID.";
            return false;
        }
    }
}

internal static class RenderTreeBuilderExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AddAttributeIfNotNullOrEmpty(
        this RenderTreeBuilder builder,
        int sequence,
        string name,
        string? value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            builder.AddAttribute(sequence, name, value);
        }
    }
}
