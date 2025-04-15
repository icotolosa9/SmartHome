using ModeloValidador.Abstracciones;

namespace Validators
{
    public class OnlySixLetters : IModeloValidador
    {
        public bool EsValido(Modelo modelo)
        {
            if (string.IsNullOrEmpty(modelo.Value))
                return false;

            string modeloString = modelo.Value;
            return modeloString.Length == 6 && modeloString.All(char.IsLetter);
        }
    }
}
