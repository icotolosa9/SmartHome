using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModeloValidador.Abstracciones;

namespace Validators
{
    public class ThreeLettersThreeNumbers : IModeloValidador
    {
        public bool EsValido(Modelo modelo)
        {
            if (string.IsNullOrEmpty(modelo.Value))
                return false;

            string modeloString = modelo.Value;
            return modeloString.Length == 6 &&
                   modeloString.Substring(0, 3).All(char.IsLetter) &&
                   modeloString.Substring(3, 3).All(char.IsDigit);
        }
    }
}
