# 🔴 Problema: Duplicidade de Usuários

## 📊 Situação Atual

```
ApplicationUser (ASP.NET Core Identity)
├── NomeCompleto ✓
├── Email ✓
├── CPF ✓
├── Telefone ✓
├── Perfil ✓
└── DataCriacao ✓

Usuario (Domain Entity)
├── Nome ✓
├── Email ✓
├── CPF ✓
├── Telefone ✓
├── Perfil ✓
├── DataExclusao
└── Emprestimos (relação)

⚠️ DUPLICAÇÃO TOTAL! Dados espalhados em dois lugares
```

---

## 💡 3 Soluções (escolha uma)

### **SOLUÇÃO 1: Remover `Usuario` e Usar Apenas `ApplicationUser`** ⭐ RECOMENDADO

**Vantagens:**
- ✅ Única fonte de verdade
- ✅ Sem duplicação de dados
- ✅ Menos código
- ✅ Melhor performance (menos JOINs)
- ✅ Mais seguro (um único banco de usuários)

**Desvantagens:**
- ❌ ApplicationUser fica "pesado"
- ❌ Precisa adicionar navegação para Emprestimos

**Implementação:**
```csharp
// ApplicationUser.cs - Adicionar relação
public class ApplicationUser : IdentityUser
{
    // ... campos existentes ...
    public ICollection<Emprestimo> Emprestimos { get; set; } = new List<Emprestimo>();
}

// Remover classe Usuario.cs
// Atualizar EmprestimoRepository para usar ApplicationUser
```

---

### **SOLUÇÃO 2: Manter `Usuario` e Remover Dados Duplicados de `ApplicationUser`**

**Vantagens:**
- ✅ Separa autenticação (Identity) de negócio (Domain)
- ✅ Clean Architecture mais pura
- ✅ Fácil manter histórico de usuários

**Desvantagens:**
- ⚠️ Mais complexo (JOINs necessários)
- ⚠️ Risco de inconsistência entre tabelas
- ⚠️ Precisa sincronizar dados

**Implementação:**
```csharp
// ApplicationUser.cs - Manter apenas autenticação
public class ApplicationUser : IdentityUser
{
    // Remover: NomeCompleto, Cpf, Telefone, Perfil, DataCriacao, DataExclusao

    // Adicionar FK
    public int? UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }
}

// Usuario.cs - Manter como está (todos os dados de negócio)
```

---

### **SOLUÇÃO 3: Manter Duplicação Mas Sincronizar Dados** ⚠️ NÃO RECOMENDADO

**Vantagens:**
- ✅ Sem mudanças estruturais (mantém código existente)
- ✅ Cada tabela tem seus dados

**Desvantagens:**
- ❌ Risco ALTO de inconsistência
- ❌ Mais código para manter sincronizado
- ❌ Difícil de debugar
- ❌ Impacto de performance

---

## 🎯 RECOMENDAÇÃO: Solução 1 (Unificar em `ApplicationUser`)

**Razão**: Você está usando Clean Architecture mas tem dados espalhados. 
`ApplicationUser` JÁ CONTÉM TUDO que você precisa!

---

## 📋 Plano de Implementação (Solução 1)

### **Fase 1: Preparação**
- [ ] Backup do banco de dados
- [ ] Verificar todos os usos de `Usuario`
- [ ] Criar migration para adicionar Emprestimos a ApplicationUser

### **Fase 2: Refatoração de Código**
- [ ] Adicionar navegação Emprestimos em ApplicationUser
- [ ] Atualizar Emprestimo.cs para referenciar ApplicationUser
- [ ] Atualizar repositórios (remover UsuarioRepository)
- [ ] Atualizar TokenController para retornar ApplicationUser

### **Fase 3: Testes**
- [ ] Testes de registro (CreateUser)
- [ ] Testes de login
- [ ] Testes de empréstimos (relação)
- [ ] Verificar dados no banco

### **Fase 4: Limpeza**
- [ ] Remover classe Usuario.cs
- [ ] Remover UsuarioRepository.cs
- [ ] Remover UsuarioService.cs
- [ ] Remover migrações antigas (opcional)

---

## 🔍 Checklist: Antes de Começar

Que solução você prefere?

1. **Solução 1** (Unificar em ApplicationUser) - 70% das mudanças, muito ganho
2. **Solução 2** (Separar dados) - 50% das mudanças, mais complexo
3. **Solução 3** (Sincronizar) - 10% das mudanças, mas muito risco

Qual você escolhe? 👇
