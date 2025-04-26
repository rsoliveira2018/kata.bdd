namespace Biblioteca.BDD.StepDefinitions;

[Binding]
public class AdicionarLivroSteps
{
    private List<string> carrinho = [];

    [Given(@"que o carrinho está vazio")]
    public void DadoQueOCarrinhoEstaVazio()
    {
        carrinho.Clear();
    }

    [Given(@"que o livro ""(.*)"" está disponível")]
    public void DadoQueOLivroEstaDisponivel(string titulo)
    {
        // Normalmente aqui você marcaria que o livro existe no catálogo
        // Para este exemplo, não precisamos fazer nada ainda
    }

    [When(@"eu adicionar o livro ""(.*)"" ao carrinho")]
    public void QuandoEuAdicionarOLivroAoCarrinho(string titulo)
    {
        carrinho.Add(titulo);
    }

    [Then(@"o carrinho deve conter (.*) item")]
    public void EntaoOCarrinhoDeveConterItem(int quantidade)
    {
        Assert.Equal(quantidade, carrinho.Count);
    }

    [Then(@"o carrinho deve conter o livro ""(.*)""")]
    public void EntaoOCarrinhoDeveConterOLivro(string titulo)
    {
        Assert.Contains(titulo, carrinho);
    }
}
