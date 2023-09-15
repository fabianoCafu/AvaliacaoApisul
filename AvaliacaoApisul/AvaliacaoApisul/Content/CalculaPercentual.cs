using System;

namespace AvaliacaoApisul
{
    public class CalculaPercentual
    {
        public float Percentual(int totalEntrevistados, int totalUso)
        {
            return Convert.ToSingle((totalEntrevistados * totalUso) / 100.00);
        }
    }
}