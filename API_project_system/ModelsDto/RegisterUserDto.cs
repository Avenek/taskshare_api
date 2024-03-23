namespace API_project_system.ModelsDto
{
    public class RegisterUserDto
    {
        public string Email { get; set; }
        public string Nickname {  get; set; }
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }
        public int RoleId { get; set; } = 2;
    }
}
