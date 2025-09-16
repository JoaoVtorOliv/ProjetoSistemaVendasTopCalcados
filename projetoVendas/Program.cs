using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Listacomstruct
{
    internal class Program
    {
        // ===================================================================
        // VARIÁVEIS GLOBAIS E ESTÁTICAS
        // ===================================================================
        // Define os caminhos dos arquivos de dados. 
        // São 'static' para que possam ser acessados de qualquer método da classe sem precisar de uma instância.
        static string arquivocalcados = @"C:\Users\joaov\OneDrive\Documentos\pastaProjeto\softwareVendas\estoquecalcados.txt";
        static string arquivoacessorios = @"C:\Users\joaov\OneDrive\Documentos\pastaProjeto\softwareVendas\acessorios.txt";
        static string arquivovendas = @"C:\Users\joaov\OneDrive\Documentos\pastaProjeto\softwareVendas\registrodevendas.txt";
        static string arquivousuarios = @"C:\Users\joaov\OneDrive\Documentos\pastaProjeto\softwareVendas\usuarios.txt";

        // ===================================================================
        // STRUCTS (Estruturas de Dados)
        // ===================================================================

        // Define a estrutura para armazenar dados de um calçado.
        public struct Calcado
        {
            public string nome;
            public string marca;
            public int tamanho;
            public int quant;
            public float preco;

            // Construtor para inicializar um novo objeto Calcado com todos os seus dados.
            public Calcado(string nome, string marca, int tamanho, int quantidade, float preco)
            {
                this.nome = nome;
                this.marca = marca;
                this.tamanho = tamanho;
                this.quant = quantidade;
                this.preco = preco;
            }

            // Sobrescreve o método ToString para formatar a exibição do objeto Calcado de forma legível.
            public override string ToString()
            {
                return $"{"Nome:",-15} {nome}\n" +
                       $"{"Marca:",-15} {marca}\n" +
                       $"{"Tamanho:",-15} {tamanho}\n" +
                       $"{"Quantidade:",-15} {quant}\n" +
                       $"{"Preço:",-15} R$ {preco:N2}\n" + // N2 formata o preço com 2 casas decimais.
                       $"----------------------------------\n";
            }
        }

        // Define a estrutura para armazenar dados de um acessório.
        public struct Acessorio
        {
            public string nome;
            public string tipo; // Ex: Meia, Cinto, Carteira
            public int quant;
            public float preco;

            // Construtor para inicializar um novo objeto Acessorio.
            public Acessorio(string nome, string tipo, int quantidade, float preco)
            {
                this.nome = nome;
                this.tipo = tipo;
                this.quant = quantidade;
                this.preco = preco;
            }

            // Sobrescreve o método ToString para uma exibição formatada do acessório.
            public override string ToString()
            {
                return $"{"Nome:",-15} {nome}\n" +
                       $"{"Tipo:",-15} {tipo}\n" +
                       $"{"Quantidade:",-15} {quant}\n" +
                       $"{"Preço:",-15} R$ {preco:N2}\n" +
                       $"----------------------------------\n";
            }
        }

        // Define a estrutura para registrar uma venda, sendo genérica para qualquer tipo de produto.
        public struct Venda
        {
            public string nomeProduto;
            public string categoria; // "Calçado" ou "Acessório"
            public int quantidade;
            public float precoUnitario;
            public string data;
            public string vendedor;

            // Construtor para registrar uma nova venda.
            public Venda(string nomeProduto, string categoria, int quantidade, float precoUnitario, string data, string vendedor)
            {
                this.nomeProduto = nomeProduto;
                this.categoria = categoria;
                this.quantidade = quantidade;
                this.precoUnitario = precoUnitario;
                this.data = data;
                this.vendedor = vendedor;
            }

            // Sobrescreve o método ToString para exibir detalhes da venda de forma organizada.
            public override string ToString()
            {
                return $"{"Produto:",-20} {nomeProduto}\n" +
                       $"{"Categoria:",-20} {categoria}\n" +
                       $"{"Quantidade Vendida:",-20} {quantidade}\n" +
                       $"{"Preço Unitário:",-20} R$ {precoUnitario:N2}\n" +
                       $"{"Total da Venda:",-20} R$ {(quantidade * precoUnitario):N2}\n" + // Calcula o total na hora de exibir.
                       $"{"Data:",-20} {data}\n" +
                       $"{"Vendedor:",-20} {vendedor}\n" +
                       $"------------------------------------------------\n";
            }
        }

        // Define a estrutura de um usuário do sistema, com login, senha e função (Role).
        public struct Usuario
        {
            public string Username;
            public string Password;
            public string Role; // Ex: "Admin", "Vendedor"

            // Construtor para criar um novo usuário.
            public Usuario(string u, string p, string r)
            {
                Username = u;
                Password = p;
                Role = r;
            }

            // Sobrescreve o ToString para exibir os dados do usuário (sem a senha).
            public override string ToString() => $"{"Username:",-15} {Username}\n{"Função:",-15} {Role}\n----------------------------------\n";
        }

        // ===================================================================
        // FUNÇÕES AUXILIARES DE INTERFACE (UI)
        // ===================================================================

        // Limpa a tela e imprime um cabeçalho padronizado e estilizado.
        static void ImprimirCabecalho(string titulo)
        {
            Console.Clear(); // Limpa o console.
            Console.ForegroundColor = ConsoleColor.DarkYellow; // Muda a cor do texto.
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
            // Centraliza o título dentro da borda.
            Console.WriteLine($"║{titulo.PadLeft((78 + titulo.Length) / 2).PadRight(78)                       }║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
            Console.ResetColor(); // Restaura a cor padrão do texto.
            Console.WriteLine();
        }

        // Imprime um texto centralizado na largura atual da janela do console.
        static void ImprimirCentralizado(string texto)
        {
            // Calcula o espaço à esquerda necessário para centralizar o texto.
            int espacoEsquerda = (Console.WindowWidth - texto.Length) / 2;
            // Garante que o espaço não seja negativo e aplica o preenchimento.
            Console.WriteLine(texto.PadLeft((espacoEsquerda > 0 ? espacoEsquerda : 0) + texto.Length));
        }

        // Lê uma string do usuário, garantindo que não seja vazia (a menos que permitido).
        static string LerString(string prompt, bool permiteVazio = false)
        {
            // Loop infinito que só é quebrado quando uma entrada válida é fornecida.
            while (true)
            {
                Console.Write($"{prompt,-15}"); // Exibe o texto de instrução (prompt).
                string input = Console.ReadLine();
                // Verifica se a entrada não está em branco ou se campos vazios são permitidos.
                if (!string.IsNullOrWhiteSpace(input) || permiteVazio)
                {
                    return input; // Retorna a entrada válida.
                }
                // Caso contrário, exibe uma mensagem de erro.
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Este campo não pode ser vazio.");
                Console.ResetColor();
            }
        }

        // Lê um número inteiro do usuário, garantindo que a entrada seja um número válido.
        static int LerInteiro(string prompt)
        {
            int valor;
            while (true)
            {
                Console.Write($"{prompt,-15}");
                // Tenta converter a string lida para um inteiro. 'TryParse' é seguro e não causa erro se a conversão falhar.
                if (int.TryParse(Console.ReadLine(), out valor))
                {
                    return valor; // Se a conversão for bem-sucedida, retorna o valor.
                }
                // Se falhar, exibe uma mensagem de erro e o loop continua.
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Entrada inválida. Digite um número inteiro.");
                Console.ResetColor();
            }
        }

        // Lê um número de ponto flutuante (float) do usuário, validando a entrada.
        static float LerFloat(string prompt)
        {
            float valor;
            while (true)
            {
                Console.Write($"{prompt,-15}");
                // Tenta converter a string para float.
                if (float.TryParse(Console.ReadLine(), out valor))
                {
                    return valor; // Retorna o valor se a conversão for bem-sucedida.
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Entrada inválida. Digite um número (ex: 59,90).");
                Console.ResetColor();
            }
        }

        // Pausa a execução do programa e espera o usuário pressionar a tecla ENTER.
        static void AguardarEnter()
        {
            Console.WriteLine("\nPressione ENTER para voltar...");
            Console.ReadLine();
        }

        // Lê uma senha do console sem exibir os caracteres digitados, mostrando '*' em seu lugar.
        static string LerSenha()
        {
            string senha = "";
            ConsoleKeyInfo keyInfo;
            do
            {
                // Lê a tecla pressionada sem exibi-la no console (graças ao 'true').
                keyInfo = Console.ReadKey(true);
                // Se a tecla não for Backspace nem Enter, adiciona o caractere à senha e imprime um asterisco.
                if (keyInfo.Key != ConsoleKey.Backspace && keyInfo.Key != ConsoleKey.Enter)
                {
                    senha += keyInfo.KeyChar;
                    Console.Write("*");
                }
                // Se for Backspace e a senha não estiver vazia, remove o último caractere.
                else if (keyInfo.Key == ConsoleKey.Backspace && senha.Length > 0)
                {
                    senha = senha[0..^1]; // Remove o último caractere da string.
                    Console.Write("\b \b"); // Apaga o asterisco visualmente do console.
                }
            } while (keyInfo.Key != ConsoleKey.Enter); // O loop continua até o usuário pressionar Enter.
            Console.WriteLine();
            return senha;
        }

        // ===================================================================
        // FUNÇÕES DE GERENCIAMENTO DE ARQUIVOS (Leitura e Escrita)
        // ===================================================================

        // Carrega a lista de calçados a partir do arquivo de texto.
        static List<Calcado> CarregarCalcados()
        {
            // Verifica se o arquivo existe; se não, cria um arquivo vazio para evitar erros.
            if (!File.Exists(arquivocalcados))
            {
                File.Create(arquivocalcados).Close(); // .Close() é importante para liberar o arquivo após a criação.
            }
            // Usa LINQ para processar o arquivo.
            return File.ReadAllLines(arquivocalcados) // 1. Lê todas as linhas do arquivo.
                .Where(linha => !string.IsNullOrWhiteSpace(linha)) // 2. Filtra linhas em branco ou nulas.
                .Select(linha => linha.Split(';')) // 3. Divide cada linha pelo delimitador ';' para obter os campos.
                .Select(partes => new Calcado(partes[0], partes[1], int.Parse(partes[2]), int.Parse(partes[3]), float.Parse(partes[4]))) // 4. Cria um objeto Calcado com os dados, convertendo os tipos.
                .ToList(); // 5. Converte o resultado em uma lista.
        }

        // Salva a lista de calçados no arquivo de texto, sobrescrevendo o conteúdo anterior.
        static void SalvarCalcados(List<Calcado> lista)
        {
            // Para cada objeto 'c' na lista, formata uma string com seus dados separados por ';'.
            var linhasParaSalvar = lista.Select(c => $"{c.nome};{c.marca};{c.tamanho};{c.quant};{c.preco}");
            // Escreve todas as linhas geradas no arquivo.
            File.WriteAllLines(arquivocalcados, linhasParaSalvar);
        }

        // Carrega a lista de acessórios do arquivo (lógica idêntica à de CarregarCalcados).
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

        // Salva a lista de acessórios no arquivo.
        static void SalvarAcessorios(List<Acessorio> lista)
        {
            var linhas = lista.Select(a => $"{a.nome};{a.tipo};{a.quant};{a.preco}");
            File.WriteAllLines(arquivoacessorios, linhas);
        }

        // Carrega o histórico de vendas do arquivo.
        static List<Venda> CarregarVendas()
        {
            if (!File.Exists(arquivovendas))
            {
                File.Create(arquivovendas).Close();
            }
            return File.ReadAllLines(arquivovendas)
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => l.Split(';'))
                // Verifica se a linha tem o campo vendedor (compatibilidade com versões antigas do arquivo).
                .Select(p => new Venda(p[0], p[1], int.Parse(p[2]), float.Parse(p[3]), p[4], p.Length > 5 ? p[5] : "N/A"))
                .ToList();
        }

        // Salva o histórico de vendas no arquivo.
        static void SalvarVendas(List<Venda> lista)
        {
            var linhas = lista.Select(v => $"{v.nomeProduto};{v.categoria};{v.quantidade};{v.precoUnitario};{v.data};{v.vendedor}");
            File.WriteAllLines(arquivovendas, linhas);
        }

        // Carrega os usuários do sistema.
        static List<Usuario> CarregarUsuarios()
        {
            // Se o arquivo de usuários não existe, cria um com um usuário "admin" padrão.
            if (!File.Exists(arquivousuarios))
            {
                var adminPadrao = new List<Usuario> { new Usuario("admin", "admin", "Admin") };
                SalvarUsuarios(adminPadrao);
                return adminPadrao;
            }

            // Lê e processa o arquivo de usuários.
            var usuarios = File.ReadAllLines(arquivousuarios)
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => l.Split(';'))
                .Select(p => new Usuario(p[0], p[1], p[2]))
                .ToList();

            // Se o arquivo existir mas estiver vazio, adiciona o admin padrão.
            if (!usuarios.Any())
            {
                usuarios.Add(new Usuario("admin", "admin", "Admin"));
                SalvarUsuarios(usuarios);
            }
            return usuarios;
        }

        // Salva a lista de usuários no arquivo.
        static void SalvarUsuarios(List<Usuario> lista)
        {
            var linhas = lista.Select(u => $"{u.Username};{u.Password};{u.Role}");
            File.WriteAllLines(arquivousuarios, linhas);
        }

        // ===================================================================
        // MENUS DE ESCOLHA DE CATEGORIA
        // ===================================================================
        // Estes menus servem para direcionar o fluxo, perguntando ao usuário qual tipo de produto ele quer manipular.

        // Menu para escolher se quer cadastrar um calçado ou um acessório.
        static void MenuCadastro(List<Calcado> lc, List<Acessorio> la)
        {
            ImprimirCabecalho("MENU DE CADASTRO");
            Console.WriteLine("[1] Cadastrar Calçado\n[2] Cadastrar Acessório\n\n[0] Voltar");
            string op = LerString("Escolha o tipo:");
            // Direciona para a função correta com base na escolha.
            if (op == "1") CadastrarCalcado(lc);
            else if (op == "2") CadastrarAcessorio(la);
        }

        // Menu para escolher o tipo de produto a ser vendido.
        static void MenuVenda(List<Calcado> lc, List<Acessorio> la, List<Venda> lv, Usuario u)
        {
            ImprimirCabecalho("MENU DE VENDA");
            Console.WriteLine("[1] Vender Calçado\n[2] Vender Acessório\n\n[0] Voltar");
            string op = LerString("Escolha o tipo:");
            if (op == "1") RealizarVendaCalcado(lc, lv, u);
            else if (op == "2") RealizarVendaAcessorio(la, lv, u);
        }

        // Menu para escolher qual tipo de produto editar.
        static void MenuEdicao(List<Calcado> lc, List<Acessorio> la)
        {
            ImprimirCabecalho("MENU DE EDIÇÃO");
            Console.WriteLine("[1] Editar Calçado\n[2] Editar Acessório\n\n[0] Voltar");
            string op = LerString("Escolha o tipo:");
            if (op == "1") EditarCalcado(lc);
            else if (op == "2") EditarAcessorio(la);
        }

        // Menu para escolher qual tipo de produto consultar ou excluir.
        static void MenuConsultaExclusao(List<Calcado> lc, List<Acessorio> la)
        {
            ImprimirCabecalho("MENU DE CONSULTA/EXCLUSÃO");
            Console.WriteLine("[1] Consultar/Excluir Calçado\n[2] Consultar/Excluir Acessório\n\n[0] Voltar");
            string op = LerString("Escolha o tipo:");
            if (op == "1") ConsultarExcluirCalcado(lc);
            else if (op == "2") ConsultarExcluirAcessorio(la);
        }

        // ===================================================================
        // FUNÇÕES PRINCIPAIS DO SISTEMA (CRUD - Create, Read, Update, Delete)
        // ===================================================================

        // Pede os dados, cria um novo calçado, adiciona à lista e salva no arquivo.
        static void CadastrarCalcado(List<Calcado> l)
        {
            ImprimirCabecalho("CADASTRAR CALÇADO");
            // Cria um novo objeto Calcado diretamente na chamada do método Add.
            l.Add(new Calcado(LerString("Nome:"), LerString("Marca:"), LerInteiro("Tamanho:"), LerInteiro("Quantidade:"), LerFloat("Preço:")));
            SalvarCalcados(l); // Persiste a alteração no arquivo.
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nCalçado cadastrado com sucesso!");
            Console.ResetColor();
            AguardarEnter();
        }

        // Pede os dados, cria um novo acessório, adiciona à lista e salva no arquivo.
        static void CadastrarAcessorio(List<Acessorio> l)
        {
            ImprimirCabecalho("CADASTRAR ACESSÓRIO");
            l.Add(new Acessorio(LerString("Nome:"), LerString("Tipo:"), LerInteiro("Quantidade:"), LerFloat("Preço:")));
            SalvarAcessorios(l); // Salva a lista atualizada no arquivo.
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nAcessório cadastrado com sucesso!");
            Console.ResetColor();
            AguardarEnter();
        }

        // Exibe todos os produtos em estoque, separados por categoria.
        static void ImprimirEstoque(List<Calcado> lc, List<Acessorio> la)
        {
            ImprimirCabecalho("RELAÇÃO DE PRODUTOS NO ESTOQUE");
            Console.WriteLine("--- CALÇADOS ---");
            // Verifica se a lista não está vazia antes de tentar iterar sobre ela.
            if (lc.Any())
                foreach (var item in lc) Console.Write(item); // Usa o método ToString() de cada item.
            else
                Console.WriteLine("Estoque de calçados vazio.\n");

            Console.WriteLine("\n--- ACESSÓRIOS ---");
            if (la.Any())
                foreach (var item in la) Console.Write(item);
            else
                Console.WriteLine("Estoque de acessórios vazio.\n");
            AguardarEnter();
        }

        // Lógica para realizar a venda de um calçado.
        static void RealizarVendaCalcado(List<Calcado> lc, List<Venda> lv, Usuario u)
        {
            ImprimirCabecalho("VENDA DE CALÇADO");
            string nome = LerString("Nome do calçado:");
            // Procura o índice do produto na lista, ignorando maiúsculas/minúsculas.
            int index = lc.FindIndex(c => c.nome.Equals(nome, StringComparison.OrdinalIgnoreCase));

            // Se o produto foi encontrado (índice diferente de -1).
            if (index != -1)
            {
                int quantidadeVendida = LerInteiro("Quantidade:");
                var item = lc[index]; // Pega uma cópia do item para trabalhar.

                // Valida se a quantidade pedida é positiva e se há estoque suficiente.
                if (quantidadeVendida > 0 && item.quant >= quantidadeVendida)
                {
                    item.quant -= quantidadeVendida; // Abate a quantidade do estoque.
                    lc[index] = item; // Atualiza o item na lista com a nova quantidade.

                    // Adiciona um registro na lista de vendas.
                    lv.Add(new Venda(item.nome, "Calçado", quantidadeVendida, item.preco, DateTime.Now.ToString("dd/MM/yyyy"), u.Username));

                    SalvarCalcados(lc); // Salva a lista de calçados atualizada.
                    SalvarVendas(lv); // Salva a lista de vendas atualizada.

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nVenda realizada com sucesso!");
                    Console.ResetColor();
                }
                else
                {
                    // Mensagem de erro se a quantidade for inválida ou o estoque insuficiente.
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nQuantidade inválida ou insuficiente! Estoque: {item.quant}");
                    Console.ResetColor();
                }
            }
            else
            {
                // Mensagem de erro se o produto não for encontrado.
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nProduto não encontrado!");
                Console.ResetColor();
            }
            AguardarEnter();
        }

        // Lógica para realizar a venda de um acessório (idêntica à de calçado).
        static void RealizarVendaAcessorio(List<Acessorio> la, List<Venda> lv, Usuario u)
        {
            ImprimirCabecalho("VENDA DE ACESSÓRIO");
            string n = LerString("Nome do acessório:");
            int i = la.FindIndex(a => a.nome.Equals(n, StringComparison.OrdinalIgnoreCase));
            if (i != -1)
            {
                int q = LerInteiro("Quantidade:");
                var item = la[i];
                if (q > 0 && item.quant >= q)
                {
                    item.quant -= q;
                    la[i] = item;
                    lv.Add(new Venda(item.nome, "Acessório", q, item.preco, DateTime.Now.ToString("dd/MM/yyyy"), u.Username));
                    SalvarAcessorios(la);
                    SalvarVendas(lv);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nVenda realizada com sucesso!");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nQuantidade inválida ou insuficiente! Estoque: {item.quant}");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nProduto não encontrado!");
                Console.ResetColor();
            }
            AguardarEnter();
        }

        // Permite a edição dos dados de um calçado existente.
        static void EditarCalcado(List<Calcado> l)
        {
            ImprimirCabecalho("EDITAR CALÇADO");
            string n = LerString("Nome do calçado a editar:");
            int i = l.FindIndex(c => c.nome.Equals(n, StringComparison.OrdinalIgnoreCase));

            // Se o produto não for encontrado, exibe erro e retorna.
            if (i == -1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nProduto não encontrado!");
                Console.ResetColor();
                AguardarEnter();
                return; // Sai da função.
            }

            var p = l[i]; // Pega o produto encontrado.
            Console.WriteLine("\nEditando o produto. Pressione ENTER para manter o valor atual.");

            // Pede o novo nome. Se o usuário digitar algo, atualiza; se pressionar Enter, mantém o antigo.
            string novoNome = LerString($"Nome ({p.nome}):", true);
            if (!string.IsNullOrWhiteSpace(novoNome)) p.nome = novoNome;

            // Repete a lógica para os outros campos.
            string novaMarca = LerString($"Marca ({p.marca}):", true);
            if (!string.IsNullOrWhiteSpace(novaMarca)) p.marca = novaMarca;

            Console.Write($"{"Tamanho:",-15}({p.tamanho}): ");
            if (int.TryParse(Console.ReadLine(), out int novoTamanho)) p.tamanho = novoTamanho;

            Console.Write($"{"Quantidade:",-15}({p.quant}): ");
            if (int.TryParse(Console.ReadLine(), out int novaQuant)) p.quant = novaQuant;

            Console.Write($"{"Preço:",-15}({p.preco:N2}): ");
            if (float.TryParse(Console.ReadLine(), out float novoPreco)) p.preco = novoPreco;

            l[i] = p; // Atualiza o produto na lista.
            SalvarCalcados(l); // Salva a lista com as alterações.
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nProduto atualizado com sucesso!");
            Console.ResetColor();
            AguardarEnter();
        }

        // Permite a edição dos dados de um acessório (lógica idêntica à de calçado).
        static void EditarAcessorio(List<Acessorio> l)
        {
            ImprimirCabecalho("EDITAR ACESSÓRIO");
            string n = LerString("Nome do acessório a editar:");
            int i = l.FindIndex(a => a.nome.Equals(n, StringComparison.OrdinalIgnoreCase));
            if (i == -1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nProduto não encontrado!");
                Console.ResetColor();
                AguardarEnter();
                return;
            }
            var p = l[i];
            Console.WriteLine("\nEditando o produto. Pressione ENTER para manter o valor atual.");
            string nn = LerString($"Nome ({p.nome}):", true);
            if (!string.IsNullOrWhiteSpace(nn)) p.nome = nn;
            string nt_str = LerString($"Tipo ({p.tipo}):", true);
            if (!string.IsNullOrWhiteSpace(nt_str)) p.tipo = nt_str;
            Console.Write($"{"Quantidade:",-15}({p.quant}): ");
            if (int.TryParse(Console.ReadLine(), out int nq)) p.quant = nq;
            Console.Write($"{"Preço:",-15}({p.preco:N2}): ");
            if (float.TryParse(Console.ReadLine(), out float npr)) p.preco = npr;
            l[i] = p;
            SalvarAcessorios(l);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nProduto atualizado com sucesso!");
            Console.ResetColor();
            AguardarEnter();
        }

        // Busca um calçado pelo nome, exibe seus dados e oferece a opção de excluí-lo.
        static void ConsultarExcluirCalcado(List<Calcado> l)
        {
            ImprimirCabecalho("CONSULTAR/EXCLUIR CALÇADO");
            string n = LerString("Nome do calçado:");
            int i = l.FindIndex(c => c.nome.Equals(n, StringComparison.OrdinalIgnoreCase));

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
                // Pede confirmação para a exclusão.
                if (Console.ReadKey().KeyChar.ToString().ToLower() == "s")
                {
                    l.RemoveAt(i); // Remove o item da lista.
                    SalvarCalcados(l); // Salva a lista sem o item.
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

        // Lógica de consultar e excluir para acessórios.
        static void ConsultarExcluirAcessorio(List<Acessorio> l)
        {
            ImprimirCabecalho("CONSULTAR/EXCLUIR ACESSÓRIO");
            string n = LerString("Nome do acessório:");
            int i = l.FindIndex(a => a.nome.Equals(n, StringComparison.OrdinalIgnoreCase));
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

        // Gera e exibe um relatório de vendas, totalizando por categoria e geral.
        static void ImprimirRelatorioVendas(List<Venda> lv)
        {
            ImprimirCabecalho("RELATÓRIO DE VENDAS");
            // Filtra as vendas por categoria usando LINQ.
            var vendasCalcados = lv.Where(v => v.categoria == "Calçado").ToList();
            var vendasAcessorios = lv.Where(v => v.categoria == "Acessório").ToList();

            // Calcula o subtotal de cada categoria e o total geral.
            float totalCalcados = vendasCalcados.Sum(v => v.quantidade * v.precoUnitario);
            float totalAcessorios = vendasAcessorios.Sum(v => v.quantidade * v.precoUnitario);
            float totalGeral = totalCalcados + totalAcessorios;

            Console.WriteLine("--- VENDAS DE CALÇADOS ---");
            if (vendasCalcados.Any())
                foreach (var v in vendasCalcados) Console.Write(v);
            else
                Console.WriteLine("Nenhuma venda de calçado registrada.\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"SUBTOTAL CALÇADOS: R$ {totalCalcados:N2}\n");
            Console.ResetColor();

            Console.WriteLine("--- VENDAS DE ACESSÓRIOS ---");
            if (vendasAcessorios.Any())
                foreach (var v in vendasAcessorios) Console.Write(v);
            else
                Console.WriteLine("Nenhuma venda de acessório registrada.\n");
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

        // Menu para as funções de administração de usuários.
        static void GerenciarUsuarios(List<Usuario> lu)
        {
            ImprimirCabecalho("GESTÃO DE USUÁRIOS");
            Console.WriteLine("[1] Cadastrar Novo Usuário\n[2] Listar Usuários\n\n[0] Voltar");
            string op = LerString("Escolha uma opção:");
            if (op == "1") CadastrarUsuario(lu);
            else if (op == "2") ImprimirUsuarios(lu);
        }

        // Cadastra um novo usuário no sistema.
        static void CadastrarUsuario(List<Usuario> lu)
        {
            ImprimirCabecalho("CADASTRAR NOVO USUÁRIO");
            string username = LerString("Username:");
            // Verifica se o nome de usuário já existe para evitar duplicatas.
            if (lu.Any(usr => usr.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Este nome de usuário já existe!");
                Console.ResetColor();
                AguardarEnter();
                return;
            }
            string password = LerString("Senha:");
            // Define a função com base na entrada (1 para Admin, qualquer outra coisa para Vendedor).
            string role = LerInteiro("Função (1=Admin, 2=Vendedor):") == 1 ? "Admin" : "Vendedor";

            lu.Add(new Usuario(username, password, role));
            SalvarUsuarios(lu);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nUsuário cadastrado com sucesso!");
            Console.ResetColor();
            AguardarEnter();
        }

        // Imprime a lista de todos os usuários cadastrados.
        static void ImprimirUsuarios(List<Usuario> lu)
        {
            ImprimirCabecalho("LISTA DE USUÁRIOS");
            foreach (var u in lu)
            {
                Console.Write(u); // Usa o ToString() da struct Usuario.
            }
            AguardarEnter();
        }

        // Exibe a tela de login e valida as credenciais do usuário.
        static Usuario? TelaDeLogin(List<Usuario> lu)
        {
            int tentativas = 0;
            // Permite um máximo de 3 tentativas de login.
            while (tentativas < 3)
            {
                ImprimirCabecalho("SISTEMA DE GESTÃO DE ESTOQUE E VENDAS - TOP CALÇADOS");
                Console.ForegroundColor = ConsoleColor.Cyan;
                ImprimirCentralizado("BEM-VINDO! FAÇA O LOGIN PARA CONTINUAR");
                Console.ResetColor();
                Console.WriteLine("\n");

                string username = LerString("Usuário: ");
                Console.Write($"{"Senha:",-15}");
                string password = LerSenha();

                // Procura na lista por um usuário que corresponda ao username (ignorando caso) E à senha.
                Usuario? usuarioEncontrado = lu.FirstOrDefault(usr => usr.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && usr.Password == password);

                // Se um usuário foi encontrado (não é nulo).
                if (usuarioEncontrado.HasValue)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    ImprimirCentralizado("\nLogin realizado com sucesso!");
                    Console.ResetColor();
                    ImprimirCentralizado("Pressione ENTER para continuar...");
                    Console.ReadLine();
                    return usuarioEncontrado; // Retorna o usuário logado.
                }

                tentativas++; // Incrementa o contador de tentativas.
                Console.ForegroundColor = ConsoleColor.Red;
                ImprimirCentralizado($"\nUsuário ou senha inválidos! Tentativas restantes: {3 - tentativas}");
                Console.ResetColor();
                Console.ReadLine();
            }
            // Se exceder as tentativas, exibe mensagem e retorna nulo.
            Console.ForegroundColor = ConsoleColor.Red;
            ImprimirCentralizado("\nNúmero máximo de tentativas excedido.");
            Console.ResetColor();
            Console.ReadLine();
            return null;
        }

        // ===================================================================
        // MÉTODO MAIN (Ponto de Entrada do Programa)
        // ===================================================================
        static void Main(string[] args)
        {
            Console.Title = "Top Calçados - Sistema de Gestão"; // Define o título da janela do console.

            // Carrega todos os dados dos arquivos para listas em memória no início da execução.
            List<Calcado> listaDeCalcados = CarregarCalcados();
            List<Acessorio> listaDeAcessorios = CarregarAcessorios();
            List<Venda> listaDeVendas = CarregarVendas();
            List<Usuario> listaDeUsuarios = CarregarUsuarios();

            // Chama a tela de login e armazena o usuário que efetuou login.
            Usuario? usuarioLogado = TelaDeLogin(listaDeUsuarios);

            // O programa principal só executa se o login for bem-sucedido.
            if (usuarioLogado.HasValue)
            {
                string escolha;
                // Loop principal do menu, continua executando até que o usuário escolha '0' para sair.
                do
                {
                    ImprimirCabecalho($"MENU PRINCIPAL (Usuário: {usuarioLogado.Value.Username})");
                    Console.WriteLine("[1] Cadastrar Produto");
                    Console.WriteLine("[2] Realizar Venda");
                    Console.WriteLine("[3] Editar Produto");
                    Console.WriteLine("[4] Consultar ou Excluir Produto");
                    Console.WriteLine("[5] Imprimir Estoque Completo");
                    Console.WriteLine("[6] Imprimir Relatório de Vendas");

                    // Verifica se o usuário logado é 'Admin' para exibir a opção de gerenciamento de usuários.
                    if (usuarioLogado.Value.Role == "Admin")
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("[7] Gerenciar Usuários (Admin)");
                        Console.ResetColor();
                    }

                    Console.WriteLine("\n[0] Sair do Sistema");
                    Console.Write("\nEscolha a operação desejada: ");
                    escolha = Console.ReadLine();

                    // Estrutura switch para direcionar a escolha do usuário para a função correspondente.
                    switch (escolha)
                    {
                        case "1": MenuCadastro(listaDeCalcados, listaDeAcessorios); break;
                        case "2": MenuVenda(listaDeCalcados, listaDeAcessorios, listaDeVendas, usuarioLogado.Value); break;
                        case "3": MenuEdicao(listaDeCalcados, listaDeAcessorios); break;
                        case "4": MenuConsultaExclusao(listaDeCalcados, listaDeAcessorios); break;
                        case "5": ImprimirEstoque(listaDeCalcados, listaDeAcessorios); break;
                        case "6": ImprimirRelatorioVendas(listaDeVendas); break;
                        case "7":
                            // Segurança: Mesmo que um não-admin digite '7', esta verificação impede o acesso.
                            if (usuarioLogado.Value.Role == "Admin")
                                GerenciarUsuarios(listaDeUsuarios);
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Acesso negado!");
                                Console.ResetColor();
                                AguardarEnter();
                            }
                            break;
                        case "0": break; // Quebra o switch, e o loop do-while terminará.
                        default: // Caso o usuário digite uma opção que não existe.
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nOpção inválida!");
                            Console.ResetColor();
                            AguardarEnter();
                            break;
                    }
                } while (escolha != "0");
            }
            // Mensagem de despedida ao sair do programa.
            ImprimirCentralizado("\nObrigado por utilizar o sistema Top Calçados!");
            AguardarEnter();
        }
    }
}