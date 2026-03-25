# FocusStudy 

Aplicação web construída em ASP.NET Core para ajudar estudantes a manter o foco, acompanhar o tempo de estudo e visualizar o histórico de sessões por usuário.

O sistema oferece um cronômetro moderno com barra de progresso circular, metas de estudo, classificação por tipo de atividade (badges) e um layout responsivo com suporte a tema claro/escuro (dark mode com foco em preto + verde).

---

## Funcionalidades

### 1. Autenticação simples de usuário
- Cadastro de usuário com:
  - Nome
  - E-mail (único)
  - Senha (armazenada como hash SHA256)
- Login/Logout usando sessão.
- Cada usuário enxerga apenas **suas** sessões de estudo.

### 2. Cronômetro de estudo
- Cronômetro circular com barra de progresso (SVG):
  - Exibe tempo decorrido em `hh:mm:ss`.
  - Usa uma barra de progresso circular animada, com cor verde em destaque.
- Meta de tempo opcional:
  - Campo para informar **meta em minutos** (ex.: 25, 50, 90).
  - O progresso circular vai de 0% a 100% em função da meta.
- Tipos de atividade (badge):
  - Foco
  - Estudo
  - Exercícios
  - Videoaulas
  - Pesquisas
  - Documentações
- Campos adicionais:
  - Assunto / Matéria (texto livre, opcional).

Ao clicar em **Finalizar**, o sistema:
- Calcula a duração em minutos.
- Gera `StartTime` e `EndTime` com base na duração.
- Salva a sessão no banco de dados atrelada ao usuário logado.

### 3. Alarme de conclusão de meta

- Arquivo de áudio em `Sound/freesound_community-alarm-clock-short-6402.mp3`.
- Quando o cronômetro atinge 100% da meta preenchida:
  - O som do alarme toca automaticamente.
  - Um botão “Parar alarme” aparece, permitindo pausar/resetar o som.

### 4. Histórico de estudos

- Página de histórico com layout em **cards**:
  - Data da sessão.
  - Horário de início e fim.
  - Duração total em minutos.
  - Meta configurada (quando existir).
  - Badge/Tipo de atividade com cores diferentes (Foco, Estudo, Exercícios, etc).
- Cards com animações suaves (hover com elevação/zoom).
- Responsivo (grids em colunas para desktop e uma coluna só em mobile).

### 5. Pesquisa no histórico

- Barra de pesquisa por assunto/matéria.
- Busca parcial (`Contains`) – funciona mesmo digitando parte do nome.
- Ícone de limpar filtro (X) reposicionado ao lado do campo.

### 6. Tema claro/escuro (Dark Mode)

- **Dark mode** como padrão, com paleta preto+verde:
  - Fundo `#0b0c10`.
  - Cards `#1f2326`.
  - Destaques em verde neon `#20c997`.
- Theme switcher no topo:
  - Botão pequeno circular com ícone.
  - Preferência salva em `localStorage`.
- Usabilidade:
  - Inputs, bordas e focos adaptados para ótima legibilidade no escuro.
  - Cores ajustadas para textos secundários e ícones.

---

## Arquitetura

### Tecnologias principais

- **.NET 8** (ASP.NET Core MVC)
- **C# 12**
- **Entity Framework Core** com provider:
  - `Pomelo.EntityFrameworkCore.MySql` (8.x) – MySQL
- **MySQL** como banco de dados relacional
- **Bootstrap 5** + **Bootstrap Icons**
- JavaScript vanilla (para cronômetro, progress bar, alarme e dark mode)

### Estrutura de pastas relevante

```text
Projecttest/
  Controllers/
    HomeController.cs       # Cronômetro, histórico e fluxo principal
    AccountController.cs    # Cadastro, login e logout simples
  Data/
    ApplicationDbContext.cs # DbContext EF Core (Users, StudySessions)
  Models/
    User.cs                 # Entidade de usuário
    StudySession.cs         # Entidade de sessão de estudo
  Views/
    Home/
      Index.cshtml          # Cronômetro + meta + badge + alarme
      History.cshtml        # Cards do histórico + filtro
    Account/
      Login.cshtml
      Register.cshtml
    Shared/
      _Layout.cshtml        # Layout geral, navbar, dark mode, recursos estáticos
  Sound/
    freesound_community-alarm-clock-short-6402.mp3  # Som do alarme
  wwwroot/
    ...                     # Bootstrap, JS, CSS gerados
  appsettings.json
  appsettings.Development.json (IGNORADO no Git)
  Program.cs
  Projecttest.csproj
```

### Modelos

`Models/User.cs`:

```csharp
public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public ICollection<StudySession> StudySessions { get; set; } = new List<StudySession>();
}
```

`Models/StudySession.cs`:

```csharp
public class StudySession
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public int DurationMinutes { get; set; }

    public string? Subject { get; set; }

    public int? TargetMinutes { get; set; }

    public string? Badge { get; set; } // Foco, Estudo, Exercícios, Videoaulas, Pesquisas, Documentações
}
```

---

## Configuração e execução

### 1. Pré-requisitos

- .NET 8 SDK instalado
- MySQL Server em execução (local ou remoto)
- Acesso a um usuário MySQL (por exemplo, `root`)

### 2. Clonar o repositório

```bash
git clone https://github.com/<seu-usuario>/Projecttest.git
cd Projecttest
```

### 3. Banco de dados

Crie o banco:

```sql
CREATE DATABASE studyfocusdb
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_general_ci;
```

Aplique as migrations do Entity Framework Core:

```bash
cd Projecttest
dotnet ef database update
```

### 4. Configuração de conexão (segura)

- `appsettings.json` (versionado, exemplo):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;database=studyfocusdb;user=root;password=CHANGEME;"
  }
}
```

- `appsettings.Development.json` (local, ignorado pelo Git):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;database=studyfocusdb;user=root;password=SUA_SENHA_REAL_AQUI;"
  }
}
```

Certifique-se de:

1. Ajustar `SUA_SENHA_REAL_AQUI` com a senha correta do seu MySQL.
2. Garantir que `appsettings.Development.json` está listado no `.gitignore`.

### 5. Rodar o projeto

Na pasta `Projecttest/` (onde está o `.csproj`):

```bash
dotnet run
```

A aplicação deve subir em algo como:

```text
https://localhost:7067
```

---

## Fluxo de uso

1. Acessar `/Account/Register` e criar uma conta.
2. Fazer login em `/Account/Login`.
3. Ir para a página inicial (`/Home/Index`):
   - Escolher tipo de atividade (badge).
   - (Opcional) Definir assunto e meta em minutos.
   - Iniciar/Pausar/Finalizar o cronômetro.
4. Ao atingir a meta:
   - O círculo completa 100%.
   - O som de alarme é disparado.
   - Um botão “Parar alarme” permite silenciar o som.
5. Acessar `/Home/History` para ver o histórico em cards:
   - Filtrar por texto (assunto).
   - Visualizar badges coloridas por tipo de atividade.
   - Ver meta e duração real.

---

## Segurança e boas práticas

- Senhas de usuários são salvas como hash (não em texto plano).
- A string de conexão real do banco fica apenas em `appsettings.Development.json` (ignorado pelo Git).
- `Program.cs` lança exceção se a connection string não estiver configurada, evitando rodar em produção sem configuração adequada.

---

## Possíveis melhorias futuras

- Implementar ASP.NET Identity completo (roles, recuperação de senha, etc.).
- Adicionar gráficos (por exemplo, tempo de estudo por dia/semana usando Chart.js).
- Notificações de desktop quando a meta de estudo é alcançada.
- Suporte a múltiplos timers/projetos por usuário.
- Internacionalização (pt-BR / en-US).

---
