﻿namespace EntityProject
{
    public class Endereco
    {
        public int Numero { get; internal set; }
        public string Logadouro { get; internal set; }
        public string Complemento { get; internal set; }
        public string Bairro { get; internal set; }
        public string Cidade { get; internal set; }
        public Cliente Cliente { get; set; }
    }
}