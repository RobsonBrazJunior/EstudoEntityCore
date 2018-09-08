using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityProject
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var contexto = new LojaContext())
            {
                var produtos = contexto.Produtos.ToList();
                foreach (var p in produtos)
                {
                    Console.WriteLine(p);
                }

                Console.WriteLine("======================");
                foreach(var e in contexto.ChangeTracker.Entries())
                {
                    Console.WriteLine(e);
                }

                var p1 = produtos.Last();
                p1.Nome = "Animais Fantáticos e Onde Habitam";

                Console.WriteLine("======================");
                foreach (var e in contexto.ChangeTracker.Entries())
                {
                    Console.WriteLine(e);
                }

                //contexto.SaveChanges();

                //Console.WriteLine("====================");
                //produtos = contexto.Produtos.ToList();
                //foreach(var p in produtos)
                //{
                //    Console.WriteLine(p);
                //}
            }
        }
    }
}
