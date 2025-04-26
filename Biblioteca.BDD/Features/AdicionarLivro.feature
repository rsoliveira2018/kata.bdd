Feature: AdicionarLivro
  Como um usuário
  Eu desejo colocar um livro no carrinho
  Para que eu possa comprá-lo posteriormente

  Scenario: Adicionar um livro ao carrinho com sucesso
    Given que o carrinho está vazio
    And que o livro "O Senhor dos Anéis" está disponível
    When eu adicionar o livro "O Senhor dos Anéis" ao carrinho
    Then o carrinho deve conter 1 item
    And o carrinho deve conter o livro "O Senhor dos Anéis"
