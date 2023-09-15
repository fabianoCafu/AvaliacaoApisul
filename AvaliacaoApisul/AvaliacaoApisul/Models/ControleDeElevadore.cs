using Newtonsoft.Json;

namespace AvaliacaoApisul.Models
{
    public class ControleDeElevadore
    {
        [JsonProperty("andar")]
        public int Andar { get; set; }
        [JsonProperty("elevador")]
        public char Elevador { get; set; }
        [JsonProperty("turno")]
        public char Turno { get; set; } 
    }
}