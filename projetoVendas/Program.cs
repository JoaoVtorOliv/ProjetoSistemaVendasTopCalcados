using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Listacomstruct
{
    internal class Program
    {
        // Define os caminhos dos arquivos de dados.
        static string arquivocalcados = @"C:\Users\joaov\OneDrive\Documentos\pastaProjeto\softwareVendas\estoquecalcados.txt";
        static string arquivoacessorios = @"C:\Users\joaov\OneDrive\Documentos\pastaProjeto\softwareVendas\acessorios.txt";
        static string arquivovendas = @"C:\Users\joaov\OneDrive\Documentos\pastaProjeto\softwareVendas\registrodevendas.txt";
        static string arquivousuarios = @"C:\Users\joaov\OneDrive\Documentos\pastaProjeto\softwareVendas\usuarios.txt";

        // ===================================================================
        // STRUCTS (Estruturas de Dados)
        // ===================================================================
        public struct Calcado
        {
            public string nome;
            public string marca;
            public int tamanho;
            public int quant;
            public float preco;

            public Calcado(string nome, string marca, int tamanho, int quantidade, float preco)
            {
                this.nome = nome;
                this.marca = marca;
                this.tamanho = tamanho;
                this.quant = quantidade;
                this.preco = preco;
            }

            public override string ToString()
            {
                return $"{"Nome:",-15} {nome}\n" +
                       $"{"Marca:",-15} {marca}\n" +
                       $"{"Tamanho:",-15} {tamanho}\n" +
                       $"{"Quantidade:",-15} {quant}\n" +
                       $"{"Preço:",-15} R$ {preco:N2}\n" +
                       $"----------------------------------\n";
            }
        }

        public struct Acessorio
        {
            public string nome;
            public string tipo;
            public int quant;
            public float preco;

            public Acessorio(string nome, string tipo, int quantidade, float preco)
            {
                this.nome = nome;
                this.tipo = tipo;
                this.quant = quantidade;
                this.preco = preco;
            }

            public override string ToString()
            {
                return $"{"Nome:",-15} {nome}\n" +
                       $"{"Tipo:",-15} {tipo}\n" +
                       $"{"Quantidade:",-15} {quant}\n" +
                       $"{"Preço:",-15} R$ {preco:N2}\n" +
                       $"----------------------------------\n";
            }
        }

        public struct Venda
        {
            public string nomeProduto;
            public string categoria;
            public int quantidade;
            public float precoUnitario;
            public string data;
            public string vendedor;

            public Venda(string nomeProduto, string categoria, int quantidade, float precoUnitario, string data, string vendedor)
            {
                this.nomeProduto = nomeProduto;
                this.categoria = categoria;
                this.quantidade = quantidade;
                this.precoUnitario = precoUnitario;
                this.data = data;
                this.vendedor = vendedor;
            }

            public override string ToString()
            {
                return $"{"Produto:",-20} {nomeProduto}\n" +
                       $"{"Categoria:",-20} {categoria}\n" +
                       $"{"Quantidade Vendida:",-20} {quantidade}\n" +
                       $"{"Preço Unitário:",-20} R$ {precoUnitario:N2}\n" +
                       $"{"Total da Venda:",-20} R$ {(quantidade * precoUnitario):N2}\n" +
                       $"{"Data:",-20} {data}\n" +
                       $"{"Vendedor:",-20} {vendedor}\n" +
                       $"------------------------------------------------\n";
            }
        }

        public struct Usuario
        {
            public string Username;
            public string Password;
            public string Role;

            public Usuario(string u, string p, string r)
            {
                Username = u;
                Password = p;
                Role = r;
            }

            public override string ToString() => $"{"Username:",-15} {Username}\n{"Função:",-15} {Role}\n----------------------------------\n";
        }

        public struct ItemDeEstoque
        {
            public int ID;
            public string Nome;
            public string Categoria;
        }

        // ===================================================================
        // FUNÇÕES AUXILIARES DE INTERFACE (UI)
        // ===================================================================
        static void ImprimirCabecalho(string titulo)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            int larguraTotal = Console.WindowWidth - 1;
            if (titulo.Length > larguraTotal - 4)
            {
                titulo = titulo.Substring(0, larguraTotal - 4);
            }
            Console.WriteLine("╔" + new string('═', larguraTotal - 2) + "╗");
            string linhaTitulo = $"║{titulo.PadLeft((larguraTotal - 2 + titulo.Length) / 2).PadRight(larguraTotal - 2)}║";
            Console.WriteLine(linhaTitulo);
            Console.WriteLine("╚" + new string('═', larguraTotal - 2) + "╝");
            Console.ResetColor();
            Console.WriteLine();
        }

        static void ImprimirCentralizado(string texto)
        {
            int espacoEsquerda = (Console.WindowWidth - texto.Length) / 2;
            Console.WriteLine(texto.PadLeft((espacoEsquerda > 0 ? espacoEsquerda : 0) + texto.Length));
        }

        static string LerString(string prompt, bool permiteVazio = false)
        {
            while (true)
            {
                Console.Write($"{prompt,-15}");
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input) || permiteVazio)
                {
                    return input;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Este campo não pode ser vazio.");
                Console.ResetColor();
            }
        }

        static int LerInteiro(string prompt)
        {
            int valor;
            while (true)
            {
                Console.Write($"{prompt,-15}");
                if (int.TryParse(Console.ReadLine(), out valor))
                {
                    return valor;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Entrada inválida. Digite um número inteiro.");
                Console.ResetColor();
            }
        }

        static float LerFloat(string prompt)
        {
            float valor;
            while (true)
            {
                Console.Write($"{prompt,-15}");
                if (float.TryParse(Console.ReadLine(), out valor))
                {
                    return valor;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Entrada inválida. Digite um número (ex: 59,90).");
                Console.ResetColor();
            }
        }

        static void AguardarEnter()
        {
            Console.WriteLine("\nPressione ENTER para voltar...");
            Console.ReadLine();
        }

        static string LerSenha()
        {
            string senha = "";
            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key != ConsoleKey.Backspace && keyInfo.Key != ConsoleKey.Enter)
                {
                    senha += keyInfo.KeyChar;
                    Console.Write("*");
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && senha.Length > 0)
                {
                    senha = senha.Substring(0, senha.Length - 1);
                    Console.Write("\b \b");
                }
            } while (keyInfo.Key != ConsoleKey.Enter);
            return senha;
        }

        // ===================================================================
        // FUNÇÕES DE GERENCIAMENTO DE ARQUIVOS
        // ===================================================================
        static List<Calcado> CarregarCalcados()
        {
            if (!File.Exists(arquivocalcados))
            {
                File.Create(arquivocalcados).Close();
            }
            return File.ReadAllLines(arquivocalcados)
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => l.Split(';'))
                .Select(p => new Calcado(p[0], p[1], int.Parse(p[2]), int.Parse(p[3]), float.Parse(p[4])))
                .ToList();
        }

        static void SalvarCalcados(List<Calcado> lista)
        {
            File.WriteAllLines(arquivocalcados, lista.Select(c => $"{c.nome};{c.marca};{c.tamanho};{c.quant};{c.preco}"));
        }

        static List<Acessorio> CarregarAcessorios()
        {
            if (!File.Exists(arquivoacessorios))
            {
                File.Create(arquivoacessorios).Close();
            }
            return File.ReadAllLines(arquivoacessorios)
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => l.Split(';'))
                .Select(p => new Acessorio(p[0], p[1], int.Parse(p[2]), float.Parse(p[3])))
                .ToList();
        }

        static void SalvarAcessorios(List<Acessorio> lista)
        {
            File.WriteAllLines(arquivoacessorios, lista.Select(a => $"{a.nome};{a.tipo};{a.quant};{a.preco}"));
        }

        static List<Venda> CarregarVendas()
        {
            if (!File.Exists(arquivovendas))
            {
                File.Create(arquivovendas).Close();
            }
            return File.ReadAllLines(arquivovendas)
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => l.Split(';'))
                .Select(p => new Venda(p[0], p[1], int.Parse(p[2]), float.Parse(p[3]), p[4], p.Length > 5 ? p[5] : "N/A"))
                .ToList();
        }

        static void SalvarVendas(List<Venda> lista)
        {
            File.WriteAllLines(arquivovendas, lista.Select(v => $"{v.nomeProduto};{v.categoria};{v.quantidade};{v.precoUnitario};{v.data};{v.vendedor}"));
        }

        static List<Usuario> CarregarUsuarios()
        {
            if (!File.Exists(arquivousuarios))
            {
                var adminPadrao = new List<Usuario> { new Usuario("admin", "admin", "Admin") };
                SalvarUsuarios(adminPadrao);
                return adminPadrao;
            }

            var usuarios = File.ReadAllLines(arquivousuarios)
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => l.Split(';'))
                .Select(p => new Usuario(p[0], p[1], p[2]))
                .ToList();

            if (!usuarios.Any())
            {
                usuarios.Add(new Usuario("admin", "admin", "Admin"));
                SalvarUsuarios(usuarios);
            }
            return usuarios;
        }

        static void SalvarUsuarios(List<Usuario> lista)
        {
            File.WriteAllLines(arquivousuarios, lista.Select(u => $"{u.Username};{u.Password};{u.Role}"));
        }

        // ===================================================================
        // MENUS DE ESCOLHA
        // ===================================================================
        static void MenuCadastro(List<Calcado> lc, List<Acessorio> la)
        {
            Console.Clear();
            ImprimirCabecalho("MENU DE CADASTRO");
            Console.WriteLine("[1] Cadastrar Calçado\n[2] Cadastrar Acessório\n\n[0] Voltar");
            string op = LerString("Escolha o tipo:");
            if (op == "1")
            {
                CadastrarCalcado(lc);
            }
            else if (op == "2")
            {
                CadastrarAcessorio(la);
            }
        }

        static ItemDeEstoque? SelecionarProdutoDoEstoque(List<Calcado> lc, List<Acessorio> la, string titulo)
        {
            Console.Clear();
            ImprimirCabecalho(titulo);

            var listaUnificada = new List<ItemDeEstoque>();
            int contadorId = 1;

            foreach (var calcado in lc)
            {
                listaUnificada.Add(new ItemDeEstoque { ID = contadorId++, Nome = calcado.nome, Categoria = "Calçado" });
            }
            foreach (var acessorio in la)
            {
                listaUnificada.Add(new ItemDeEstoque { ID = contadorId++, Nome = acessorio.nome, Categoria = "Acessório" });
            }

            if (!listaUnificada.Any())
            {
                Console.WriteLine("Nenhum produto cadastrado no estoque.");
                AguardarEnter();
                return null;
            }

            Console.WriteLine($"{"ID",-5} {"NOME",-40} {"CATEGORIA",-15}");
            Console.WriteLine(new string('-', 65));
            foreach (var item in listaUnificada)
            {
                string nomeExibicao = item.Nome.Length > 38 ? item.Nome.Substring(0, 38) + ".." : item.Nome;
                Console.WriteLine($"{item.ID,-5} {nomeExibicao,-40} {item.Categoria,-15}");
            }
            Console.WriteLine(new string('-', 65));

            Console.WriteLine("\n[0] Voltar ao Menu Principal");
            while (true)
            {
                int escolhaId = LerInteiro("Escolha o item pelo ID:");
                if (escolhaId == 0) return null;

                var itemEscolhido = listaUnificada.FirstOrDefault(item => item.ID == escolhaId);
                if (itemEscolhido.Nome != null)
                {
                    return itemEscolhido;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ID inválido. Tente novamente.");
                Console.ResetColor();
            }
        }

        static void MenuEdicao(List<Calcado> lc, List<Acessorio> la)
        {
            var itemSelecionado = SelecionarProdutoDoEstoque(lc, la, "EDITAR PRODUTO");
            if (!itemSelecionado.HasValue) return;

            if (itemSelecionado.Value.Categoria == "Calçado")
            {
                EditarCalcado(lc, itemSelecionado.Value.Nome);
            }
            else
            {
                EditarAcessorio(la, itemSelecionado.Value.Nome);
            }
        }

        static void MenuConsultaExclusao(List<Calcado> lc, List<Acessorio> la)
        {
            var itemSelecionado = SelecionarProdutoDoEstoque(lc, la, "CONSULTAR / EXCLUIR PRODUTO");
            if (!itemSelecionado.HasValue) return;

            if (itemSelecionado.Value.Categoria == "Calçado")
            {
                ConsultarExcluirCalcado(lc, itemSelecionado.Value.Nome);
            }
            else
            {
                ConsultarExcluirAcessorio(la, itemSelecionado.Value.Nome);
            }
        }

        // ===================================================================
        // FUNÇÕES PRINCIPAIS DO SISTEMA (CRUD)
        // ===================================================================
        static void CadastrarCalcado(List<Calcado> l)
        {
            Console.Clear();
            ImprimirCabecalho("CADASTRAR CALÇADO");
            l.Add(new Calcado(
                LerString("Nome:"),
                LerString("Marca:"),
                LerInteiro("Tamanho:"),
                LerInteiro("Quantidade:"),
                LerFloat("Preço:")
            ));
            SalvarCalcados(l);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nCalçado cadastrado com sucesso!");
            Console.ResetColor();
            AguardarEnter();
        }

        static void CadastrarAcessorio(List<Acessorio> l)
        {
            Console.Clear();
            ImprimirCabecalho("CADASTRAR ACESSÓRIO");
            l.Add(new Acessorio(
                LerString("Nome:"),
                LerString("Tipo:"),
                LerInteiro("Quantidade:"),
                LerFloat("Preço:")
            ));
            SalvarAcessorios(l);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nAcessório cadastrado com sucesso!");
            Console.ResetColor();
            AguardarEnter();
        }

        static void ImprimirRelatorioEstoque(List<Calcado> lc, List<Acessorio> la)
        {
            Console.Clear();
            ImprimirCabecalho("RELAÇÃO DE ESTOQUE");

            // Tabela de Calçados
            int colNomeCalcado = 25;
            int colMarca = 15;
            int colTamanho = 10;
            int colQuantCalcado = 15;
            int colPrecoCalcado = 20;
            int larguraTotalCalcados = colNomeCalcado + colMarca + colTamanho + colQuantCalcado + colPrecoCalcado + 16;
            string tituloCalcados = "--- [ CALÇADOS ] ";
            Console.WriteLine(tituloCalcados + new string('-', larguraTotalCalcados - tituloCalcados.Length));
            Console.WriteLine($"| {"NOME".PadRight(colNomeCalcado)} | {"MARCA".PadRight(colMarca)} | {"TAMANHO".PadRight(colTamanho)} | {"QUANTIDADE".PadRight(colQuantCalcado)} | {"PREÇO UNITÁRIO".PadRight(colPrecoCalcado)} |");
            Console.WriteLine($"|{new string('-', colNomeCalcado + 2)}|{new string('-', colMarca + 2)}|{new string('-', colTamanho + 2)}|{new string('-', colQuantCalcado + 2)}|{new string('-', colPrecoCalcado + 2)}|");

            if (lc.Any())
            {
                foreach (var c in lc)
                {
                    string nome = c.nome.Length > colNomeCalcado ? c.nome.Substring(0, colNomeCalcado) : c.nome.PadRight(colNomeCalcado);
                    string marca = c.marca.Length > colMarca ? c.marca.Substring(0, colMarca) : c.marca.PadRight(colMarca);
                    string tamanho = c.tamanho.ToString().PadRight(colTamanho);
                    string quant = $"{c.quant} un.".PadRight(colQuantCalcado);
                    string preco = $"R$ {c.preco:N2}".PadRight(colPrecoCalcado);
                    Console.WriteLine($"| {nome} | {marca} | {tamanho} | {quant} | {preco} |");
                }
            }
            else
            {
                Console.WriteLine($"| {"Nenhum calçado em estoque.".PadRight(larguraTotalCalcados - 4)} |");
            }
            Console.WriteLine($"+{new string('-', colNomeCalcado + 2)}+{new string('-', colMarca + 2)}+{new string('-', colTamanho + 2)}+{new string('-', colQuantCalcado + 2)}+{new string('-', colPrecoCalcado + 2)}+");

            // Tabela de Acessórios
            int colNomeAcessorio = 25;
            int colTipo = 25;
            int colQuantAcessorio = 15;
            int colPrecoAcessorio = 20;
            int larguraTotalAcessorios = colNomeAcessorio + colTipo + colQuantAcessorio + colPrecoAcessorio + 13;
            string tituloAcessorios = "--- [ ACESSÓRIOS ] ";
            Console.WriteLine("\n" + tituloAcessorios + new string('-', larguraTotalAcessorios - tituloAcessorios.Length));
            Console.WriteLine($"| {"NOME".PadRight(colNomeAcessorio)} | {"TIPO".PadRight(colTipo)} | {"QUANTIDADE".PadRight(colQuantAcessorio)} | {"PREÇO UNITÁRIO".PadRight(colPrecoAcessorio)} |");
            Console.WriteLine($"|{new string('-', colNomeAcessorio + 2)}|{new string('-', colTipo + 2)}|{new string('-', colQuantAcessorio + 2)}|{new string('-', colPrecoAcessorio + 2)}|");

            if (la.Any())
            {
                foreach (var a in la)
                {
                    string nome = a.nome.Length > colNomeAcessorio ? a.nome.Substring(0, colNomeAcessorio) : a.nome.PadRight(colNomeAcessorio);
                    string tipo = a.tipo.Length > colTipo ? a.tipo.Substring(0, colTipo) : a.tipo.PadRight(colTipo);
                    string quant = $"{a.quant} un.".PadRight(colQuantAcessorio);
                    string preco = $"R$ {a.preco:N2}".PadRight(colPrecoAcessorio);
                    Console.WriteLine($"| {nome} | {tipo} | {quant} | {preco} |");
                }
            }
            else
            {
                Console.WriteLine($"| {"Nenhum acessório em estoque.".PadRight(larguraTotalAcessorios - 4)} |");
            }
            Console.WriteLine($"+{new string('-', colNomeAcessorio + 2)}+{new string('-', colTipo + 2)}+{new string('-', colQuantAcessorio + 2)}+{new string('-', colPrecoAcessorio + 2)}+");

            AguardarEnter();
        }

        static void MenuVenda(List<Calcado> lc, List<Acessorio> la, List<Venda> lv, Usuario u)
        {
            while (true)
            {
                Console.Clear();
                ImprimirCabecalho("MENU DE VENDA - ESTOQUE DISPONÍVEL");

                var itensParaVenda = new List<(int id, string nome, string tipo, float preco, int quant)>();
                int contadorId = 1;
                foreach (var calcado in lc.Where(c => c.quant > 0))
                {
                    itensParaVenda.Add((contadorId++, calcado.nome, "Calçado", calcado.preco, calcado.quant));
                }
                foreach (var acessorio in la.Where(a => a.quant > 0))
                {
                    itensParaVenda.Add((contadorId++, acessorio.nome, "Acessório", acessorio.preco, acessorio.quant));
                }

                if (!itensParaVenda.Any())
                {
                    Console.WriteLine("Nenhum produto com estoque disponível para venda.");
                    AguardarEnter();
                    return;
                }

                Console.WriteLine($"{"ID",-5} {"NOME",-30} {"PREÇO",-15} {"ESTOQUE",-10}");
                Console.WriteLine(new string('-', 65));
                foreach (var item in itensParaVenda)
                {
                    Console.WriteLine($"{item.id,-5} {item.nome,-30} {$"R$ {item.preco:N2}",-15} {item.quant,-10}");
                }
                Console.WriteLine(new string('-', 65));

                Console.WriteLine("\n[0] Voltar ao Menu Principal");
                int escolhaId = LerInteiro("Escolha o item pelo ID:");
                if (escolhaId == 0)
                {
                    break;
                }

                var itemEscolhido = itensParaVenda.FirstOrDefault(item => item.id == escolhaId);
                if (itemEscolhido == default)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nID de produto inválido!");
                    Console.ResetColor();
                    AguardarEnter();
                    continue;
                }

                Console.WriteLine($"\nProduto selecionado: {itemEscolhido.nome}");
                int quantidadeVendida = LerInteiro("Quantidade a vender:");

                if (quantidadeVendida <= 0 || quantidadeVendida > itemEscolhido.quant)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nQuantidade inválida ou insuficiente! Estoque disponível: {itemEscolhido.quant}");
                    Console.ResetColor();
                    AguardarEnter();
                    continue;
                }

                if (itemEscolhido.tipo == "Calçado")
                {
                    int index = lc.FindIndex(c => c.nome == itemEscolhido.nome);
                    if (index != -1)
                    {
                        var calcado = lc[index];
                        calcado.quant -= quantidadeVendida;
                        lc[index] = calcado;
                        lv.Add(new Venda(calcado.nome, "Calçado", quantidadeVendida, calcado.preco, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), u.Username));
                        SalvarCalcados(lc);
                    }
                }
                else if (itemEscolhido.tipo == "Acessório")
                {
                    int index = la.FindIndex(a => a.nome == itemEscolhido.nome);
                    if (index != -1)
                    {
                        var acessorio = la[index];
                        acessorio.quant -= quantidadeVendida;
                        la[index] = acessorio;
                        lv.Add(new Venda(acessorio.nome, "Acessório", quantidadeVendida, acessorio.preco, DateTime.Now.ToString("dd/MM/yyyy"), u.Username));
                        SalvarAcessorios(la);
                    }
                }

                SalvarVendas(lv);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nVenda realizada com sucesso!");
                Console.ResetColor();
                AguardarEnter();
            }
        }

        static void EditarCalcado(List<Calcado> l, string nomeParaEditar)
        {
            Console.Clear();
            ImprimirCabecalho("EDITAR CALÇADO");

            int i = l.FindIndex(c => c.nome.Equals(nomeParaEditar, StringComparison.OrdinalIgnoreCase));
            if (i == -1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nProduto não encontrado!");
                Console.ResetColor();
                AguardarEnter();
                return;
            }

            var p = l[i];
            Console.WriteLine($"\nEditando o produto: {p.nome}.\nPressione ENTER para manter o valor atual.");

            string novoNome = LerString($"Nome ({p.nome}):", true);
            if (!string.IsNullOrWhiteSpace(novoNome)) p.nome = novoNome;

            string novaMarca = LerString($"Marca ({p.marca}):", true);
            if (!string.IsNullOrWhiteSpace(novaMarca)) p.marca = novaMarca;

            Console.Write($"{"Tamanho:",-15}({p.tamanho}): ");
            string tamanhoInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(tamanhoInput) && int.TryParse(tamanhoInput, out int novoTamanho)) p.tamanho = novoTamanho;

            Console.Write($"{"Quantidade:",-15}({p.quant}): ");
            string quantInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(quantInput) && int.TryParse(quantInput, out int novaQuant)) p.quant = novaQuant;

            Console.Write($"{"Preço:",-15}({p.preco:N2}): ");
            string precoInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(precoInput) && float.TryParse(precoInput, out float novoPreco)) p.preco = novoPreco;

            l[i] = p;
            SalvarCalcados(l);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nProduto atualizado com sucesso!");
            Console.ResetColor();
            AguardarEnter();
        }

        static void EditarAcessorio(List<Acessorio> l, string nomeParaEditar)
        {
            Console.Clear();
            ImprimirCabecalho("EDITAR ACESSÓRIO");

            int i = l.FindIndex(a => a.nome.Equals(nomeParaEditar, StringComparison.OrdinalIgnoreCase));
            if (i == -1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nProduto não encontrado!");
                Console.ResetColor();
                AguardarEnter();
                return;
            }

            var p = l[i];
            Console.WriteLine($"\nEditando o produto: {p.nome}.\nPressione ENTER para manter o valor atual.");

            string nn = LerString($"Nome ({p.nome}):", true);
            if (!string.IsNullOrWhiteSpace(nn)) p.nome = nn;

            string nt_str = LerString($"Tipo ({p.tipo}):", true);
            if (!string.IsNullOrWhiteSpace(nt_str)) p.tipo = nt_str;

            Console.Write($"{"Quantidade:",-15}({p.quant}): ");
            string quantInput = Console.ReadLine();
            if (int.TryParse(quantInput, out int nq)) p.quant = nq;

            Console.Write($"{"Preço:",-15}({p.preco:N2}): ");
            string precoInput = Console.ReadLine();
            if (float.TryParse(precoInput, out float npr)) p.preco = npr;

            l[i] = p;
            SalvarAcessorios(l);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nProduto atualizado com sucesso!");
            Console.ResetColor();
            AguardarEnter();
        }

        static void ConsultarExcluirCalcado(List<Calcado> l, string nomeParaConsultar)
        {
            Console.Clear();
            ImprimirCabecalho("CONSULTAR/EXCLUIR CALÇADO");

            int i = l.FindIndex(c => c.nome.Equals(nomeParaConsultar, StringComparison.OrdinalIgnoreCase));
            if (i == -1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nProduto não encontrado!");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("\n--- Produto Encontrado ---\n" + l[i]);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Deseja EXCLUIR este produto? (s/n): ");
                if (Console.ReadKey().KeyChar.ToString().ToLower() == "s")
                {
                    l.RemoveAt(i);
                    SalvarCalcados(l);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n\nProduto excluído com sucesso!");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("\n\nOperação cancelada.");
                }
            }
            AguardarEnter();
        }

        static void ConsultarExcluirAcessorio(List<Acessorio> l, string nomeParaConsultar)
        {
            Console.Clear();
            ImprimirCabecalho("CONSULTAR/EXCLUIR ACESSÓRIO");

            int i = l.FindIndex(a => a.nome.Equals(nomeParaConsultar, StringComparison.OrdinalIgnoreCase));
            if (i == -1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nProduto não encontrado!");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("\n--- Produto Encontrado ---\n" + l[i]);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Deseja EXCLUIR este produto? (s/n): ");
                if (Console.ReadKey().KeyChar.ToString().ToLower() == "s")
                {
                    l.RemoveAt(i);
                    SalvarAcessorios(l);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n\nProduto excluído com sucesso!");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("\n\nOperação cancelada.");
                }
            }
            AguardarEnter();
        }

        static void ImprimirRelatorioVendas(List<Venda> lv)
        {
            Console.Clear();
            ImprimirCabecalho("RELATÓRIO DE VENDAS");

            var vendasCalcados = lv.Where(v => v.categoria == "Calçado").ToList();
            var vendasAcessorios = lv.Where(v => v.categoria == "Acessório").ToList();
            float totalCalcados = vendasCalcados.Sum(v => v.quantidade * v.precoUnitario);
            float totalAcessorios = vendasAcessorios.Sum(v => v.quantidade * v.precoUnitario);
            float totalGeral = totalCalcados + totalAcessorios;

            Console.WriteLine("--- VENDAS DE CALÇADOS ---");
            if (vendasCalcados.Any())
            {
                foreach (var v in vendasCalcados) Console.Write(v);
            }
            else
            {
                Console.WriteLine("Nenhuma venda de calçado registrada.\n");
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"SUBTOTAL CALÇADOS: R$ {totalCalcados:N2}\n");
            Console.ResetColor();

            Console.WriteLine("--- VENDAS DE ACESSÓRIOS ---");
            if (vendasAcessorios.Any())
            {
                foreach (var v in vendasAcessorios) Console.Write(v);
            }
            else
            {
                Console.WriteLine("Nenhuma venda de acessório registrada.\n");
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"SUBTOTAL ACESSÓRIOS: R$ {totalAcessorios:N2}\n");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("================================================");
            Console.WriteLine($"TOTAL GERAL DE VENDAS: R$ {totalGeral:N2}");
            Console.WriteLine("================================================");
            Console.ResetColor();

            AguardarEnter();
        }

        static void GerenciarUsuarios(List<Usuario> lu)
        {
            Console.Clear();
            ImprimirCabecalho("GESTÃO DE USUÁRIOS");
            Console.WriteLine("[1] Cadastrar Novo Usuário\n[2] Listar Usuários\n\n[0] Voltar");
            string op = LerString("Escolha uma opção:");
            if (op == "1")
            {
                CadastrarUsuario(lu);
            }
            else if (op == "2")
            {
                ImprimirUsuarios(lu);
            }
        }

        static void CadastrarUsuario(List<Usuario> lu)
        {
            Console.Clear();
            ImprimirCabecalho("CADASTRAR NOVO USUÁRIO");

            string username = LerString("Username:");
            if (lu.Any(usr => usr.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Este nome de usuário já existe!");
                Console.ResetColor();
                AguardarEnter();
                return;
            }

            string password = LerString("Senha:");
            string role = LerInteiro("Função (1=Admin, 2=Vendedor):") == 1 ? "Admin" : "Vendedor";

            lu.Add(new Usuario(username, password, role));
            SalvarUsuarios(lu);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nUsuário cadastrado com sucesso!");
            Console.ResetColor();
            AguardarEnter();
        }

        static void ImprimirUsuarios(List<Usuario> lu)
        {
            Console.Clear();
            ImprimirCabecalho("LISTA DE USUÁRIOS");
            foreach (var u in lu)
            {
                Console.Write(u);
            }
            AguardarEnter();
        }

        static Usuario? TelaDeLogin(List<Usuario> lu)
        {
            int tentativas = 0;
            while (tentativas < 3)
            {
                Console.Clear();
                int larguraAnterior = Console.WindowWidth;
                int alturaAnterior = Console.WindowHeight;

                ImprimirCabecalho("SISTEMA DE GESTÃO DE ESTOQUE E VENDAS - TOP CALÇADOS");
                Console.ForegroundColor = ConsoleColor.Cyan;
                ImprimirCentralizado("BEM-VINDO! FAÇA O LOGIN PARA CONTINUAR");
                Console.ResetColor();
                Console.WriteLine();

                int larguraPainel = 50;
                int alturaPainel = 5;
                int margemEsquerda = (Console.WindowWidth - larguraPainel) / 2;
                margemEsquerda = Math.Max(0, margemEsquerda);
                int topoPainel = Console.CursorTop + 1;

                try
                {
                    Console.SetCursorPosition(margemEsquerda, topoPainel);
                    Console.WriteLine($"╔{new string('═', larguraPainel - 2)}╗");
                    Console.SetCursorPosition(margemEsquerda, topoPainel + 1);
                    Console.WriteLine($"║{new string(' ', larguraPainel - 2)}║");
                    Console.SetCursorPosition(margemEsquerda, topoPainel + 2);
                    Console.WriteLine($"║{new string(' ', larguraPainel - 2)}║");
                    Console.SetCursorPosition(margemEsquerda, topoPainel + 3);
                    Console.WriteLine($"╚{new string('═', larguraPainel - 2)}╝");

                    Console.SetCursorPosition(margemEsquerda + 4, topoPainel + 1);
                    Console.Write("Usuário: ");
                }
                catch (Exception) { continue; }

                string username = Console.ReadLine();
                if (Console.WindowWidth != larguraAnterior || Console.WindowHeight != alturaAnterior)
                {
                    continue;
                }

                try
                {
                    Console.SetCursorPosition(margemEsquerda + 4, topoPainel + 2);
                    Console.Write("Senha:   ");
                }
                catch (Exception) { continue; }

                string password = LerSenha();
                if (Console.WindowWidth != larguraAnterior || Console.WindowHeight != alturaAnterior)
                {
                    continue;
                }

                Console.SetCursorPosition(0, topoPainel + alturaPainel);

                Usuario? usuarioEncontrado = lu.FirstOrDefault(usr => usr.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && usr.Password == password);

                if (usuarioEncontrado.HasValue)
                {
                    Console.Clear();
                    ImprimirCabecalho("LOGIN BEM-SUCEDIDO");
                    Console.ForegroundColor = ConsoleColor.Green;
                    ImprimirCentralizado($"\nLogin realizado com sucesso! Bem-vindo(a), {usuarioEncontrado.Value.Username}!");
                    Console.ResetColor();
                    ImprimirCentralizado("\nPressione ENTER para continuar...");
                    Console.ReadLine();
                    return usuarioEncontrado;
                }

                tentativas++;
                Console.Clear();
                ImprimirCabecalho("ERRO DE LOGIN");
                Console.ForegroundColor = ConsoleColor.Red;
                ImprimirCentralizado($"\nUsuário ou senha inválidos! Tentativas restantes: {3 - tentativas}");
                Console.ResetColor();
                ImprimirCentralizado("\nPressione ENTER para tentar novamente...");
                Console.ReadLine();
            }

            Console.Clear();
            ImprimirCabecalho("ACESSO BLOQUEADO");
            Console.ForegroundColor = ConsoleColor.Red;
            ImprimirCentralizado("\nNúmero máximo de tentativas excedido.");
            Console.ResetColor();
            ImprimirCentralizado("\nPressione ENTER para fechar...");
            Console.ReadLine();
            return null;
        }

        static void Main(string[] args)
        {
            Console.Title = "Top Calçados - Sistema de Gestão";
            List<Calcado> listaDeCalcados = CarregarCalcados();
            List<Acessorio> listaDeAcessorios = CarregarAcessorios();
            List<Venda> listaDeVendas = CarregarVendas();
            List<Usuario> listaDeUsuarios = CarregarUsuarios();
            Usuario? usuarioLogado = TelaDeLogin(listaDeUsuarios);

            if (usuarioLogado.HasValue)
            {
                string escolha;
                do
                {
                    Console.Clear();
                    ImprimirCabecalho($"MENU PRINCIPAL (Usuário: {usuarioLogado.Value.Username})");
                    Console.WriteLine("[1] Cadastrar Produto");
                    Console.WriteLine("[2] Realizar Venda");
                    Console.WriteLine("[3] Editar Produto");
                    Console.WriteLine("[4] Consultar ou Excluir Produto");
                    Console.WriteLine("[5] Imprimir Estoque Completo");
                    Console.WriteLine("[6] Imprimir Relatório de Vendas");
                    if (usuarioLogado.Value.Role == "Admin")
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("[7] Gerenciar Usuários (Admin)");
                        Console.ResetColor();
                    }
                    Console.WriteLine("\n[0] Sair do Sistema");
                    Console.Write("\nEscolha a operação desejada: ");
                    escolha = Console.ReadLine();

                    switch (escolha)
                    {
                        case "1":
                            MenuCadastro(listaDeCalcados, listaDeAcessorios);
                            break;
                        case "2":
                            MenuVenda(listaDeCalcados, listaDeAcessorios, listaDeVendas, usuarioLogado.Value);
                            break;
                        case "3":
                            MenuEdicao(listaDeCalcados, listaDeAcessorios);
                            break;
                        case "4":
                            MenuConsultaExclusao(listaDeCalcados, listaDeAcessorios);
                            break;
                        case "5":
                            ImprimirRelatorioEstoque(listaDeCalcados, listaDeAcessorios);
                            break;
                        case "6":
                            ImprimirRelatorioVendas(listaDeVendas);
                            break;
                        case "7":
                            if (usuarioLogado.Value.Role == "Admin")
                            {
                                GerenciarUsuarios(listaDeUsuarios);
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Acesso negado!");
                                Console.ResetColor();
                                AguardarEnter();
                            }
                            break;
                        case "0":
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nOpção inválida!");
                            Console.ResetColor();
                            AguardarEnter();
                            break;
                    }
                } while (escolha != "0");
            }
            ImprimirCentralizado("\nObrigado por utilizar o sistema Top Calçados!");
            AguardarEnter();
        }
    }
}