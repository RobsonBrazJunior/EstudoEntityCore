using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
                var serviceProvider = contexto.GetInfrastructure<IServiceProvider>();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());

                var cliente = contexto.Clientes.Include(c => c.EnderecoDeEntrega).FirstOrDefault();

                Console.WriteLine($"Endereço de entrega: {cliente.EnderecoDeEntrega.Logadouro}");

                var produto = contexto.Produtos.Include(p => p.Compras).Where(p => p.Id == 2002).FirstOrDefault();

                Console.WriteLine($"Mostrando as compras do produto {produto.Nome}");
                foreach (var item in produto.Compras)
                {
                    Console.WriteLine(item);
                }
            }
        }

        private static void ExibeProdutosDaPromocao()
        {
            using (var contexto = new LojaContext())
            {
                var serviceProvider = contexto.GetInfrastructure<IServiceProvider>();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());

                var promocao = contexto.Promocoes.Include(p => p.Produtos).ThenInclude(pp => pp.Produto).FirstOrDefault();
                Console.WriteLine("\nMostrando os produtos da promoção...");
                foreach (var item in promocao.Produtos)
                {
                    Console.WriteLine(item.Produto);
                }
            }
        }

        private static void IncluirPromocao()
        {
            using (var contexto = new LojaContext())
            {
                var serviceProvider = contexto.GetInfrastructure<IServiceProvider>();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());

                var promocao = new Promocao();
                promocao.Descricao = "Queima total Setembro 2018";
                promocao.DataInicio = new DateTime(2018, 9, 17);
                promocao.DataTermino = new DateTime(2018, 9, 30);

                var produtos = contexto.Produtos.Where(p => p.Categoria == "Bebidas").ToList();

                foreach (var item in produtos)
                {
                    promocao.IncluiProduto(item);
                }

                contexto.Promocoes.Add(promocao);

                ExibeEntries(contexto.ChangeTracker.Entries());

                contexto.SaveChanges();
            }
        }

        private static void UmParaUm ()
        {
            var cliente = new Cliente();
            cliente.Nome = "Fulano de tal";
            cliente.EnderecoDeEntrega = new Endereco()
            {
                Numero = 12,
                Logadouro = "Rua A",
                Complemento = "Ap 02",
                Bairro = "Centro",
                Cidade = "Cidade"
            };

            using (var contexto = new LojaContext())
            {
                var serviceProvider = contexto.GetInfrastructure<IServiceProvider>();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());

                contexto.Clientes.Add(cliente);
                contexto.SaveChanges();
            }
        }

        private static void MuitosParaMuitos()
        {
            var p1 = new Produto()
            {
                Nome = "Suco de Laranja",
                Categoria = "Bebidas",
                PrecoUnitario = 8.76,
                Unidade = "Litros"
            };

            var p2 = new Produto()
            {
                Nome = "Café",
                Categoria = "Bebidas",
                PrecoUnitario = 12.50,
                Unidade = "Gramas"
            };

            var p3 = new Produto()
            {
                Nome = "Macarrão",
                Categoria = "Alimentos",
                PrecoUnitario = 4.25,
                Unidade = "Gramas"
            };

            var promocaoDePascoa = new Promocao();
            promocaoDePascoa.Descricao = "Feliz Páscoa";
            promocaoDePascoa.DataInicio = DateTime.Now;
            promocaoDePascoa.DataTermino = DateTime.Now.AddMonths(3);

            promocaoDePascoa.IncluiProduto(p1);
            promocaoDePascoa.IncluiProduto(p2);
            promocaoDePascoa.IncluiProduto(p3);


            using (var contexto = new LojaContext())
            {
                var serviceProvider = contexto.GetInfrastructure<IServiceProvider>();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());

                //contexto.Promocoes.Add(promocaoDePascoa);
                var promocao = contexto.Promocoes.Find(1);
                contexto.Promocoes.Remove(promocao);
                ExibeEntries(contexto.ChangeTracker.Entries());
                contexto.SaveChanges();
            }
        }

        private static void ExibeEntries(IEnumerable<EntityEntry> entries)
        {
            foreach (var e in entries)
            {
                Console.WriteLine(e.Entity.ToString() + " - " + e.State);
            }
        }
    }
}
