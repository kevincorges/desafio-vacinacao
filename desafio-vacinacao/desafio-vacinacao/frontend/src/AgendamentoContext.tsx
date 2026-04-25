import React, { createContext, useState, useCallback, ReactNode } from 'react';
import { Agendamento } from './api';

interface AgendamentoContextType {
  agendamentos: Agendamento[];
  carregando: boolean;
  erro: string | null;
  mensagemSucesso: string | null;
  carregarAgendamentos: () => Promise<void>;
  adicionarAgendamento: (agendamento: Agendamento) => void;
  atualizarAgendamento: (id: number, agendamento: Partial<Agendamento>) => void;
  limparMensagem: () => void;
  definirErro: (erro: string | null) => void;
  definirSucesso: (mensagem: string | null) => void;
}

export const AgendamentoContext = createContext<AgendamentoContextType | undefined>(undefined);

interface AgendamentoProviderProps {
  children: ReactNode;
}

export const AgendamentoProvider: React.FC<AgendamentoProviderProps> = ({ children }) => {
  const [agendamentos, setAgendamentos] = useState<Agendamento[]>([]);
  const [carregando, setCarregando] = useState(false);
  const [erro, setErro] = useState<string | null>(null);
  const [mensagemSucesso, setMensagemSucesso] = useState<string | null>(null);

  const carregarAgendamentos = useCallback(async () => {
    setCarregando(true);
    setErro(null);
    try {
      const { api } = await import('./api');
      const dados = await api.listarAgendamentos();
      setAgendamentos(dados);
    } catch (err) {
      setErro('Erro ao carregar agendamentos');
      console.error(err);
    } finally {
      setCarregando(false);
    }
  }, []);

  const adicionarAgendamento = (agendamento: Agendamento) => {
    setAgendamentos(prev => [...prev, agendamento]);
  };

  const atualizarAgendamento = (id: number, updates: Partial<Agendamento>) => {
    setAgendamentos(prev =>
      prev.map(a => a.id === id ? { ...a, ...updates } : a)
    );
  };

  const limparMensagem = () => {
    setMensagemSucesso(null);
  };

  const definirErro = (novoErro: string | null) => {
    setErro(novoErro);
  };

  const definirSucesso = (mensagem: string | null) => {
    setMensagemSucesso(mensagem);
  };

  return (
    <AgendamentoContext.Provider value={{
      agendamentos,
      carregando,
      erro,
      mensagemSucesso,
      carregarAgendamentos,
      adicionarAgendamento,
      atualizarAgendamento,
      limparMensagem,
      definirErro,
      definirSucesso
    }}>
      {children}
    </AgendamentoContext.Provider>
  );
};

export const useAgendamento = () => {
  const context = React.useContext(AgendamentoContext);
  if (!context) {
    throw new Error('useAgendamento deve ser usado dentro de AgendamentoProvider');
  }
  return context;
};
