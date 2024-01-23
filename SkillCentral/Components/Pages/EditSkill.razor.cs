using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SkillCentral.Dtos;
using Google.Protobuf.WellKnownTypes;

namespace SkillCentral.Components.Pages
{
    public partial class EditSkill
    {
        [SupplyParameterFromForm]
        public SkillCreateDto? NewSkill { get; set; }

        private SkillDto? skillDto { get; set; }

        private bool isFormValid;
        private MudForm form;
        string[] errors = { };
        protected override Task OnInitializedAsync()
        {
            NewSkill ??= new SkillCreateDto();
            return base.OnInitializedAsync();
        }

        private async Task OnValidFormSubmit(EditContext context)
        {
            if(!string.IsNullOrWhiteSpace(NewSkill.Name))
            {
                isFormValid = true;
                skillDto = await skillClient.CreateSkillAsync(NewSkill);
                if (skillDto != null)
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
                Snackbar.Add("Skill name cannot be empty or null", Severity.Error);
            }
        }
    }
}
