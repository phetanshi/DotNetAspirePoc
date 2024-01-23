using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using SkillCentral.Dtos;

namespace SkillCentral.Components.Pages;

public partial class EditEmployee
{
    [SupplyParameterFromForm]
    public EmployeeCreateDto? Employee { get; set; }

    private EmployeeDto? empDto { get; set; }

    private bool isFormValid;
    private MudForm form;
    string[] errors = { };
    protected override Task OnInitializedAsync()
    {
        Employee ??= new EmployeeCreateDto();
        return base.OnInitializedAsync();
    }

    private async Task OnValidFormSubmit(EditContext context)
    {
        isFormValid = true;
        empDto = await employeeApiClient.CreateEmployeeAsync(Employee);
        if(empDto != null)
        {
            StateHasChanged();
        }
        else
        {
            Snackbar.Add("Something went wrong!", Severity.Error);
        }
    }
}
