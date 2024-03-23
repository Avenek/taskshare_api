namespace API_project_system.ModelsDto
{
    public class RegisterUserDto
    {
        public string Email { get; set; }
        public string Name {  get; set; }
        public string Lastname { get; set; }
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }
        public int RoleId { get; set; } = 3;
    }
}
