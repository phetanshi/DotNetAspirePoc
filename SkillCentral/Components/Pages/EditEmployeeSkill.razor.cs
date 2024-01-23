using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using SkillCentral.Dtos;

namespace SkillCentral.Components.Pages
{
    public partial class EditEmployeeSkill
    {
        [Parameter]
        public string? UserId { get; set; }
        [Parameter]
        public int? SkillId { get; set; } = 0;

        [SupplyParameterFromForm]
        public EmployeeSkillCreateDto? EmployeeSkill { get; set; }

        private EmployeeSkillDto? empSkillDto;
        private string empName;
        private bool isFormValid;
        private MudForm form;
        string[] errors = { };
        private List<SkillDto> skillList;
        private SkillDto selectedSkill;

        protected override async Task OnInitializedAsync()
        {
            EmployeeSkill ??= new EmployeeSkillCreateDto();
            EmployeeSkill.UserId = UserId;
            //skillList = await skillClient.GetSkillsAsync();
            skillList = new List<SkillDto>();
            skillList.Add(new SkillDto { Id = 1, Name = "One" });
            skillList.Add(new SkillDto { Id = 2, Name = "Two" });
            skillList.Add(new SkillDto { Id = 3, Name = "Three" });
            skillList.Add(new SkillDto { Id = 4, Name = "Four" });
        }

        private async Task OnValidFormSubmit(EditContext context)
        {
            if(!string.IsNullOrWhiteSpace(EmployeeSkill.UserId) && selectedSkill is not null && selectedSkill.Id > 0)
            {
                isFormValid = true;

                if(selectedSkill is not null)
                {
                    EmployeeSkill.SkillId = selectedSkill.Id;
                }

                empSkillDto = await employeeSkillClient.AddEmployeeSkillAsync(EmployeeSkill);
                if (empSkillDto != null)
                {
                    StateHasChanged();
                }
                else
                {
                    Snackbar.Add("Something went wrong!", Severity.Error);
                }
            }
            else
            {
                Snackbar.Add("User id or skill id or both are empty or null", Severity.Error);
            }
        }
    }
}
