using AbsenDulu.BE.Models.MenuAccess;

namespace AbsenDulu.BE.Models.Identity;
    public class UserIdentityResponses
    {
        public bool IsAuthSuccessfull { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Token { get; set; }
        public List<JsonContentMenu>? Access { get; set; }

}