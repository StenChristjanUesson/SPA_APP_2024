namespace Backend.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string SaltedPassword { get; set; }
        public int UserType { get; set; }
        public List<PrivateNotes> PrivateNotes { get; set; } = [];
    }
}
