namespace ProtectedDba.Models {

    public class UserAnonymized
    {
        public int Id {get; set;}
        public string? Name {get; set;}
        public string? Cpf {get; set;}

        public string? gender {get; set;}

        public string? HashKey {get; set;}
    }
}