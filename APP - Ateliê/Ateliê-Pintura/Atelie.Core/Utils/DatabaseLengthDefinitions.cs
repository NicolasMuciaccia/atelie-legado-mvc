namespace Atelie.Core.Utils
{
    /// <summary>
    /// Centraliza as definições de tamanho padrão para os campos do banco de dados.
    /// </summary>
    public static class DatabaseLengthDefinitions
    {
        private const int LEN_VERY_LONG_500 = 500;
        private const int LEN_LONG_255 = 255;
        private const int LEN_MEDIUM_100 = 100;
        private const int LEN_SHORT_50 = 50;
        private const int LEN_CODE_10 = 10;

        public const int Nome = LEN_LONG_255;
        public const int Descricao = LEN_LONG_255;
    }
}
