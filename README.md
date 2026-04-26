# Sistema de Agendamento de Vacinação - COVID-19

Um sistema simples e funcional para agendamento de vacinação contra COVID-19, desenvolvido com React + .NET 8.

## 📋 Estrutura do Projeto

```
desafio-vacinacao/
├── backend/
│   └── VacinacaoApi/          # API .NET 8
│       ├── Models/            # Entidades de domínio
│       ├── DTOs/              # Data Transfer Objects
│       ├── Services/          # Lógica de negócio
│       ├── Controllers/       # Endpoints da API
│       ├── Data/              # Contexto do banco de dados
│       └── Program.cs         # Configuração da aplicação
└── frontend/
    ├── src/
    │   ├── App.tsx            # Componente principal
    │   ├── FormAgendamento.tsx # Formulário de agendamento
    │   ├── ListaAgendamentos.tsx # Listagem de agendamentos
    │   ├── AgendamentoContext.tsx # Context API para estado
    │   ├── api.ts             # Serviço de comunicação com API
    │   └── main.tsx           # Entry point
    └── package.json           # Dependências
```

## 🚀 Como Executar

### Backend (.NET 8)

1. Navegue até a pasta do backend:
```bash
cd backend/VacinacaoApi
```

2. Execute a aplicação:
```bash
dotnet run
```

A API estará disponível em `http://localhost:5190`

### Frontend (React)

1. Navegue até a pasta do frontend:
```bash
cd frontend
```

2. Instale as dependências (se não estiverem instaladas):
```bash
npm install
```

3. Inicie o servidor de desenvolvimento:
```bash
npm run dev
```

A aplicação estará disponível em `http://localhost:5173`

## 📌 Regras de Negócio Implementadas

- ✅ **Máximo de 20 agendamentos por dia**
- ✅ **Máximo de 2 agendamentos por horário**
- ✅ **Intervalo de 1 hora entre agendamentos**
- ✅ **Validação de formulário**
- ✅ **Status de agendamento** (Pendente, Realizado, Cancelado)
- ✅ **Conclusão do atendimento**
- ✅ **Listagem agrupada por data e hora**
- ✅ **Notificação com total de agendamentos**

## 🏗️ Arquitetura

### Backend
- **Controllers**: Recebem as requisições HTTP
- **Services**: Contêm a lógica de negócio
- **Repositories**: Acesso aos dados (não implementado separadamente, integrado no Service)
- **DTOs**: Transferência de dados entre camadas
- **Models**: Entidades de domínio
- **Data**: Contexto do Entity Framework

### Frontend
- **Context API**: Gerenciamento de estado global
- **Componentes**: Separação de responsabilidades
- **API Service**: Comunicação com o backend
- **Tailwind CSS**: Estilização

## 🔧 Tecnologias Utilizadas

### Backend
- .NET 8
- Entity Framework Core (In-Memory)
- ASP.NET Core Web API
- Swagger/OpenAPI

### Frontend
- React 18
- TypeScript
- Vite
- Tailwind CSS
- Axios

## 📝 Endpoints da API

### Listar Agendamentos
```
GET /api/agendamentos
```

### Criar Agendamento
```
POST /api/agendamentos
Body: {
  "nome": "João Silva",
  "dataNascimento": "1990-01-15",
  "dataAgendamento": "2024-04-30",
  "horario": "14:00"
}
```

### Atualizar Status
```
PUT /api/agendamentos/{id}/status
Body: {
  "status": "Realizado",
  "conclusao": "Vacinação realizada com sucesso"
}
```

## 🎯 Decisões de Arquitetura

1. **In-Memory Database**: Escolhido para facilitar a execução sem necessidade de SQL Server
2. **Context API**: Gerenciamento de estado simples e direto, sem dependências externas
3. **Tailwind CSS**: Estilização rápida e responsiva
4. **Separação Frontend/Backend**: Repositórios separados para facilitar manutenção
5. **DTOs**: Separação entre modelos de domínio e transferência de dados

## 📚 Próximas Melhorias (Diferenciais)
- [ ] Autenticação JWT
- [ ] Controle de transações
- [ ] Validações com FluentValidation


---

**Desenvolvido como desafio técnico para processo seletivo de estágio.**
