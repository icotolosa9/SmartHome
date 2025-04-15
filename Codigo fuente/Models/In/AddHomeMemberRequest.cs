using Domain;

namespace Models.In
{
    public class AddHomeMemberRequest
    {
        public List<AddHomeMemberDto> Members { get; set; } = new List<AddHomeMemberDto>();
    }
}
